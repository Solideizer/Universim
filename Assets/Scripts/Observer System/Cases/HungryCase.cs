﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryCase : MonoBehaviour, ICase
{
    [SerializeField] LayerMask targetMask;
    [SerializeField, Range(10f, 30f)] float hungerTreshold = 0f;
    [SerializeField, Range(45f, 65f)] float deathTreshold = 1f;
    [SerializeField, Range(25f, 45f)] float criticalTreshold = 30f;
    [SerializeField, Range(5f, 20f)] float targetRange = 10f;
    [SerializeField, Range(30f, 60f)] float vision = 40f;
    [SerializeField] bool isRunning;

    public float hunger = 0;
    public bool alerted;

    public bool isVFXUsed;

    Transform target;
    AnimalAI ai;

    VFXScript vfx;

    private void Start()
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;
        ai.caseDatas.Add(new CaseContainer(Case.HUNGER, hunger, hungerTreshold, criticalTreshold, CasePriority.LOW));

        isRunning = false;
        alerted = false;
        isVFXUsed = false;
    }

    private void Update()
    {
        hunger += Time.deltaTime;

        if (isRunning)
        {
            if (!isVFXUsed)
            {
                isVFXUsed = true;
                vfx = VFXManager.Instance.GetHunger(transform.position, ai);
            }
            if (target != null)
            {
                ai.Move(target.position);
                if (Vector3.Distance(target.position, transform.position) < targetRange)
                {
                    if (target.tag == "Chicken")
                        target.GetComponent<AnimalAI>().OnCaseChanged(new CaseChangedEventArgs(null, Case.DEATH));

                    hunger = 0;
                    isRunning = false;
                    alerted = false;
                    isVFXUsed = false;
                    target = null;
                    VFXManager.Instance.hungerPool.Push(vfx);

                    ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDLE));
                }
            }
            else
            {
                isRunning = false;
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDLE));
            }
        }

        if (!alerted && hunger > hungerTreshold)
        {
            alerted = true;
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
        }

        if (hunger > deathTreshold)
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.DEATH));
    }

    private Transform FindFood()
    {
        Transform t = ai.FindClosestThing(ai.transform.position, targetMask, vision);

        if (targetMask == LayerMask.GetMask("Herbivore"))
            return t;
        else
        {
            if (t != null)
                ai.Memory.FillMemory(t, Memory.MemoryType.FOOD);
            else
                t = ai.Memory.GetPoint(transform.position, Memory.MemoryType.FOOD);

            return t;
        }
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.HUNGER)
        {
            target = FindFood();

            if (target != null)
            {
                ai.currentState = Case.HUNGER;
                Run();
            }
            else
            {
                ai.HandleSpeed(SpeedPhase.WALK);
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.WANDER));
            }
        }
        else if (e.state == Case.IDENTITY_UPDATE)
        {
            vision = ai.Identity.Vision;
        }
        else if (e.state == Case.AVAILABLE)
            CaseContainer.Adjust(ai.caseDatas, Case.HUNGER, hunger);
        else if (e.state == Case.RESET)
        {
            isRunning = false;
            hunger = 0;
            alerted = false;
        }
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

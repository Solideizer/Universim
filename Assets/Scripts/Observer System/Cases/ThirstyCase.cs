using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstyCase : MonoBehaviour, ICase
{
    [SerializeField] LayerMask targetMask;
    [SerializeField, Range(10f, 30f)] float thirstTreshold = 0f;
    [SerializeField, Range(45f, 65f)] float deathTreshold = 1f;
    [SerializeField, Range(25f, 50f)] float criticalTreshold = 30f;
    [SerializeField, Range(5f, 20f)] float targetRange = 10f;
    [SerializeField, Range(30f, 60f)] float vision = 40f;
    [SerializeField] bool isRunning;
    public bool isVFXUsed;


    public float thirst = 0;
    public bool alerted;

    Transform thisTransform;
    Transform target;
    AnimalAI ai;
    VFXScript vfx;

    private void Start()
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;
        ai.caseDatas.Add(new CaseContainer(Case.THIRST, thirst, thirstTreshold, criticalTreshold, CasePriority.MID));
        thisTransform = transform;

        isRunning = false;
        alerted = false;
        isVFXUsed = false;
    }

    private void Update()
    {
        thirst += Time.deltaTime;

        if (isRunning)
        {
            if (!isVFXUsed)
            {
                isVFXUsed = true;
                vfx = VFXManager.Instance.GetThirst(transform.position, ai);
            }
            if (target != null && Vector3.Distance(target.position, thisTransform.position) < targetRange)
            {
                thirst = 0;
                isRunning = false;
                alerted = false;
                target = null;
                isVFXUsed = false;
                VFXManager.Instance.thirstPool.Push(vfx);

                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDLE));
            }
        }

        if (!alerted && thirst > thirstTreshold)
        {
            alerted = true;
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
        }

        if (thirst > deathTreshold)
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.DEATH));
    }

    private Transform FindWater()
    {
        Transform t = ai.FindClosestThing(ai.transform.position, targetMask, vision);
        if (t != null)
            ai.Memory.FillMemory(t, Memory.MemoryType.WATER);
        else
            t = ai.Memory.GetPoint(transform.position, Memory.MemoryType.WATER);

        return t;
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.THIRST)
        {
            target = FindWater();

            if (target != null)
            {
                ai.currentState = Case.THIRST;
                Run();
                ai.Move(target.position);
            }
            else
            {
                ai.HandleSpeed(SpeedPhase.WALK);
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.WANDER));
            }
        }
        else if (e.state == Case.IDENTITY_UPDATE)
            vision = ai.Identity.Vision;
        else if (e.state == Case.AVAILABLE)
            CaseContainer.Adjust(ai.caseDatas, Case.THIRST, thirst);
        else if (e.state == Case.RESET)
        {
            thirst = 0;
            alerted = false;
            isRunning = false;
            target = null;
        }
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

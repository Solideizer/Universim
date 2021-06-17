using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryCase : MonoBehaviour, ICase
{
#pragma warning disable 0649
    [SerializeField] LayerMask targetMask;
    [SerializeField, Range(10f, 30f)] float hungerTreshold = 0f;
    [SerializeField, Range(45f, 65f)] float deathTreshold = 1f;
    [SerializeField, Range(25f, 45f)] float criticalTreshold = 30f;
    [SerializeField, Range(5f, 20f)] float targetRange = 10f;
    [SerializeField, Range(30f, 60f)] float vision = 40f;
    [SerializeField] bool isRunning;
#pragma warning restore 0649

    public float hunger = 0;
    public bool alerted;
    public bool isVFXUsed;
    [HideInInspector] public Transform reportedTarget;

    Transform target;
    AnimalAI ai;
    VFXScript vfx;
    AnimationManager _animationManager;

    private void Start()
    {
        ai = GetComponent<AnimalAI>();
        _animationManager = GetComponent<AnimationManager>();
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
                vfx = VFXManager.Instance.GetStateVFX(transform.position, ai, VFXType.HUNGER);
            }

            if (target != null)
            {
                _animationManager.SetState(AnimationType.Walk);
                ai.Move(target.position);
                if (Vector3.Distance(target.position, transform.position) < targetRange)
                {
                    if (target.tag == "Chicken")
                    {
                        AnimalAI targetAi = target.GetComponent<AnimalAI>();
                        // vfx = VFXManager.Instance.GetDeadVFX(transform.position, targetAi, VFXType.EATDEAD);
                        // VFXManager.Instance.WaitPush(vfx, VFXType.EATDEAD);
                        target.GetComponent<AnimalAI>().OnCaseChanged(new CaseChangedEventArgs(null, Case.DEATH));

                    }

                    hunger = 0;
                    isRunning = false;
                    alerted = false;
                    target = null;

                    isVFXUsed = false;
                    VFXManager.Instance.Push(vfx, VFXType.HUNGER);

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
        {
            // vfx = VFXManager.Instance.GetDeadVFX(transform.position, ai, VFXType.HUNGERDEAD);
            // VFXManager.Instance.WaitPush(vfx, VFXType.HUNGERDEAD);
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.DEATH));

        }
    }

    private Transform FindFood()
    {
        Transform food = ai.FindClosestThing(ai.transform.position, targetMask, vision);

        // Otçulların yemek hafızası varken etçillerin yok.
        if (targetMask == LayerMask.GetMask("Food"))
        {
            if (food != null)
                ai.Memory.CompareLocations(transform.position, food, Memory.MemoryType.FOOD);
            else
                food = ai.Memory.GetNearest(transform.position, Memory.MemoryType.FOOD);
        }

        if (food != null)
            Inform(food);

        return food;
    }

    private void Inform(Transform food)
    {
        //print("food informed");

        LayerMask targets = ai.ownMask;
        Collider[] hits;
        int hitCount = AnimalAI.GetColliders(transform.position, vision, targets, out hits);

        for (var i = 0; i < hitCount; i++)
        {
            if (AnimalManager.Instance.animals.ContainsKey(hits[i].gameObject.GetInstanceID()))
                AnimalManager.Instance.animals[hits[i].gameObject.GetInstanceID()].
            OnCaseChanged(new CaseChangedEventArgs(new HungerCaseData(food), Case.HUNGER));
        }
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.HUNGER)
        {
            if (e.data != null)
            {
                SetReportedData(e.data);
                return;
            }

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

    private void SetReportedData(CaseData data)
    {
        data.SetData(this);

        reportedTarget = null;
        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}
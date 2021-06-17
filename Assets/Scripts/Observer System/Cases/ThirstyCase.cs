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

    public float thirst = 0;
    public bool alerted;
    public bool isVFXUsed;
    [HideInInspector] public Transform reportedTarget;

    Transform thisTransform;
    Transform target;
    AnimalAI ai;
    VFXScript vfx;
    AnimationManager _animationManager;

    private void Start()
    {
        ai = GetComponent<AnimalAI>();
        _animationManager = GetComponent<AnimationManager>();
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
                vfx = VFXManager.Instance.GetStateVFX(transform.position, ai, VFXType.THIRST);
            }

            if (target != null && Vector3.Distance(target.position, thisTransform.position) < targetRange)
            {
                thirst = 0;
                isRunning = false;
                alerted = false;
                target = null;

                isVFXUsed = false;
                VFXManager.Instance.Push(vfx, VFXType.THIRST);

                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDLE));
            }
        }

        if (!alerted && thirst > thirstTreshold)
        {
            alerted = true;
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
        }

        if (thirst > deathTreshold)
        {
            vfx = VFXManager.Instance.GetDeadVFX(transform.position, ai, VFXType.THIRSTDEAD);
            StartCoroutine(VFXManager.Instance.WaitAndPush(vfx, VFXType.THIRSTDEAD));
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.DEATH));

        }
    }

    private Transform FindWater()
    {
        Transform water = ai.FindClosestThing(ai.transform.position, targetMask, vision);
        if (water != null)
            ai.Memory.CompareLocations(transform.position, water, Memory.MemoryType.WATER);
        else
            water = ai.Memory.GetNearest(transform.position, Memory.MemoryType.WATER);

        Inform(water);

        return water;
    }

    private void Inform(Transform water)
    {
        //print("water informed");

        LayerMask targets = ai.ownMask;
        Collider[] hits;
        int hitCount = AnimalAI.GetColliders(transform.position, vision, targets, out hits);

        for (var i = 0; i < hitCount; i++)
        {
            if (AnimalManager.Instance.animals.ContainsKey(hits[i].gameObject.GetInstanceID()))
                AnimalManager.Instance.animals[hits[i].gameObject.GetInstanceID()].
            OnCaseChanged(new CaseChangedEventArgs(new ThirstCaseData(water), Case.THIRST));
        }
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.THIRST)
        {
            if (e.data != null)
            {
                SetReportedData(e.data);
                return;
            }

            target = FindWater();
            if (target != null)
            {
                ai.currentState = Case.THIRST;
                Run();
                _animationManager.SetState(AnimationType.Walk);
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

    private void SetReportedData(CaseData data)
    {
        data.SetData(this);

        reportedTarget = null;
        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}
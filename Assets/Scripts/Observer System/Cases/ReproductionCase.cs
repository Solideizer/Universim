using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionCase : MonoBehaviour, ICase
{
#pragma warning disable 0649
    [SerializeField] LayerMask targetMask;
    [SerializeField, Range(5f, 75f)] float reproductionTreshold = 40f;
    [SerializeField, Range(5f, 20f)] float targetRange = 10f;
    [SerializeField, Range(30f, 60f)] float vision = 40f;
#pragma warning restore 0649

    public float reproductionUrge = 0;
    public bool alerted;
    public bool isRunning;
    public bool isVFXUsed;

    private AnimalAI ai;
    public Sex sex;
    private bool canReproduce;
    VFXScript vfx;
    AnimationManager _animationManager;

    [HideInInspector] public AnimalAI target;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<AnimalAI>();
        _animationManager = GetComponent<AnimationManager>();
        ai.CaseChanged += OnCaseChanged;
        ai.caseDatas.Add(new CaseContainer(Case.REPRODUCTION, reproductionUrge, reproductionTreshold, 1000f, CasePriority.TOO_LOW));

        isRunning = false;
        alerted = false;
        isVFXUsed = false;

        UpdateData();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canReproduce) return;

        reproductionUrge += Time.deltaTime;
        if (isRunning && target != null)
        {
            if (!isVFXUsed)
            {
                isVFXUsed = true;
                vfx = VFXManager.Instance.GetStateVFX(transform.position, ai, VFXType.LOVE);
            }
            _animationManager.SetState(AnimationType.Walk);
            ai.Move(target.transform.position);
            if (Vector3.Distance(target.transform.position, transform.position) < targetRange)
            {
                reproductionUrge = 0;
                isRunning = false;
                alerted = false;

                // ÖNEMLİ NOT : DİREK HAMİLELİĞE GEÇİLDİĞİNDEN HAMİLELİĞİ BİRAZDA IDLE SÜRESİ EKLENMELİ!
                if (sex == Sex.FEMALE)
                    ai.OnCaseChanged(new CaseChangedEventArgs(new PregnancyCaseData(target.Identity.GeneticCode), Case.PREGNANCY));
                else
                    ai.OnCaseChanged(new CaseChangedEventArgs(new IdleCaseData(10f), Case.IDLE));

                target = null;
                isVFXUsed = false;
                VFXManager.Instance.Push(vfx, VFXType.LOVE);
            }
        }

        if (!alerted && reproductionUrge > reproductionTreshold)
        {
            alerted = true;
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
        }
    }

    private AnimalAI FindPartner()
    {
        Transform partnerTransform = ai.FindClosestThing(transform.position, targetMask, vision);

        if (partnerTransform == null || partnerTransform.gameObject == null)
            return null;

        if (AnimalManager.Instance.animals.ContainsKey(partnerTransform.gameObject.GetInstanceID()))
        {
            AnimalAI partner = AnimalManager.Instance.animals[partnerTransform.gameObject.GetInstanceID()];
            Identity identity = partner.Identity;
            if (identity.Sex != sex && identity.canReproduce && partner.currentState != Case.HUNGER && partner.currentState != Case.THIRST)
            {
                partner.OnCaseChanged(new CaseChangedEventArgs(new ReproductionCaseData(ai), Case.REPRODUCTION));
                return partner;
            }
            else
                return null;
        }

        return null;
    }

    private void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.REPRODUCTION)
        {
            if (e.data != null)
                e.data.SetData(this);

            if (target == null)
                target = FindPartner();

            if (target != null)
            {
                ai.currentState = Case.REPRODUCTION;
                Run();
            }
            else
            {
                ai.HandleSpeed(SpeedPhase.WALK);
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.WANDER));
            }
        }
        else if (e.state == Case.AVAILABLE)
            CaseContainer.Adjust(ai.caseDatas, Case.REPRODUCTION, reproductionUrge);
        else if (e.state == Case.IDENTITY_UPDATE)
            UpdateData();
        else if (e.state == Case.RESET)
        {
            reproductionUrge = 0;
            alerted = false;
            isRunning = false;
            canReproduce = false;
        }
    }

    private void UpdateData()
    {
        Identity identity = ai.Identity;
        sex = identity.Sex;
        vision = identity.Vision;

        if (identity.canReproduce)
            canReproduce = true;
        else
            canReproduce = false;
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

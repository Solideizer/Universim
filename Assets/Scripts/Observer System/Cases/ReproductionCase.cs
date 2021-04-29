using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionCase : MonoBehaviour, ICase
{
    [SerializeField] bool isRunning;
    [SerializeField] LayerMask targetMask;
    [SerializeField, Range(5f, 75f)] float reproductionTreshold = 40f;
    [SerializeField, Range(5f, 20f)] float targetRange = 10f;
    [SerializeField, Range(30f, 60f)] float vision = 40f;
    
    public float reproductionUrge = 0;
    public bool alerted;

    [SerializeField] Transform target;
    AnimalAI ai;
    Sex sex;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;
        ai.caseDatas.Add(new CaseContainer(Case.REPRODUCTION, reproductionUrge, reproductionTreshold));

        isRunning = false;
        alerted = false;
    }

    // Update is called once per frame
    void Update()
    {
        reproductionUrge += Time.deltaTime;
        if(isRunning && target != null)
        {
            ai.Move(target.position);
            if(Vector3.Distance(target.position, transform.position) < targetRange)
            {
                reproductionUrge = 0;
                isRunning = false;
                alerted = false;
                target = null;

                // ÖNEMLİ NOT : DİREK HAMİLELİĞE GEÇİLDİĞİNDEN HAMİLELİĞİ BİRAZDA IDLE SÜRESİ EKLENMELİ!
                if(sex == Sex.FEMALE)
                    ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.PREGNANCY));
                else
                    ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDLE));
            }
        }

        if (!alerted && reproductionUrge > reproductionTreshold)
        {      
            alerted = true;
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
        }
    }

    private Transform FindPartner()
    {
        Transform partnerTransform = ai.FindClosestThing(transform.position, targetMask, vision);
        AnimalAI partner = AnimalManager.Instance.animals[partnerTransform.gameObject.GetInstanceID()]; 
        Identity identity = partner.AnimalIdentity;

        if(partner != null && identity.sex != sex && identity.canReproduce && partner.currentState != Case.HUNGER && partner.currentState != Case.THIRST) 
        {
            partner.OnCaseChanged(new CaseChangedEventArgs(this.transform, Case.REPRODUCTION));
            return partnerTransform;
        }
        else
            return null;
    }

    private void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if(e.state == Case.REPRODUCTION)
        {
            if(e.transform == null)
                target = FindPartner();
            else
                target = e.transform;

            if(target != null)
            {
                ai.currentState = Case.REPRODUCTION;
                Run();
            }
            else
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.WANDER));
        }
        else if(e.state == Case.AVAILABLE)
            CaseContainer.Adjust(ai.caseDatas, Case.REPRODUCTION, reproductionUrge);
        else if(e.state == Case.IDENTITY_UPDATE)
            UpdateData();
    }

    private void UpdateData()
    {
        Identity identity = ai.AnimalIdentity;
        sex = identity.sex;

        if(identity.canReproduce)
            this.enabled = true;
        else
            this.enabled = false;
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstyCase : MonoBehaviour, ICase
{
    [SerializeField] LayerMask targetMask;
    [SerializeField, Range(10f, 30f)] float thirstTreshold = 0f;
    [SerializeField, Range(5f, 20f)] float targetRange = 10f;
    [SerializeField, Range(30f, 60f)] float vision = 40f;
    [SerializeField] bool isRunning;

    public float thirst = 0;
    public bool alerted;

    Transform tForm;
    Transform target;
    AnimalAI ai;
    
    private void Start() 
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;
        ai.caseDatas.Add(new CaseContainer(Case.THIRST, thirst, thirstTreshold));
        tForm = transform;

        isRunning = false;
        alerted = false;
    }

    private void Update()
    {
        thirst += Time.deltaTime;

        if (isRunning)
        {
            if(target != null && Vector3.Distance(target.position, tForm.position) < targetRange)
            {
                thirst = 0;
                isRunning = false;
                alerted = false;
                target = null;
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDLE));
            }
        }

        if(!alerted && thirst > thirstTreshold)
        {
            alerted = true;
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
        }
    }

    private Transform FindWater()
    {
        return ai.FindClosestThing(ai.transform.position, targetMask, vision);
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.THIRST)
        {
            target = FindWater();

            if(target != null)
            {
                ai.currentState = Case.THIRST;
                Run();
                ai.Move(target.position);
            }
            else
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.WANDER));    
        }
        else if(e.state == Case.AVAILABLE)
            CaseContainer.Adjust(ai.caseDatas, Case.THIRST, thirst);
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

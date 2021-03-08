using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCase : MonoBehaviour, ICase
{
    [SerializeField, Range(1f, 3f)] float idleTime = 0f;
    [SerializeField] bool isRunning;

    float idle = 0;
    AnimalAI ai;

    private void Start()
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;

        isRunning = false;
    }

    private void Update() 
    {
        if(isRunning)
        {
            idle += Time.deltaTime;
            if(idle > idleTime)
            {
                idle = 0;
                isRunning = false;
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
            }   
        }    
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if(e.state == Case.IDLE)
        {
            ai.currentState = Case.IDLE;
            ai.Stop();
            Run();
        }
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

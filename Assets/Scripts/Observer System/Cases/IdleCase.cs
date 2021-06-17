using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCase : MonoBehaviour, ICase
{
#pragma warning disable 0649
    [SerializeField, Range(1f, 3f)] float idleTime = 0f;
    [SerializeField] bool isRunning;
    public float idle = 0;
#pragma warning restore 0649

    AnimalAI ai;
    float defaultTime;
    [HideInInspector] public float tempTime;
    AnimationManager _animationManager;

    private void Start()
    {
        ai = GetComponent<AnimalAI>();
        _animationManager = GetComponent<AnimationManager>();
        ai.CaseChanged += OnCaseChanged;

        defaultTime = idleTime;
        isRunning = false;
    }

    private void Update()
    {
        if (isRunning)
        {
            _animationManager.SetState(AnimationType.Idle);
            idle += Time.deltaTime;
            if (idle > tempTime)
            {
                idle = 0;
                isRunning = false;
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
            }
        }
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.IDLE)
        {
            defaultTime = idleTime;

            if (e.data != null)
                e.data.SetData(this);
            else
                tempTime = defaultTime;

            ai.currentState = Case.IDLE;
            ai.Stop();
            Run();
        }
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

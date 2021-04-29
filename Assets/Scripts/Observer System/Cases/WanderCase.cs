using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderCase : MonoBehaviour, ICase
{
    [SerializeField, Range(1f, 3f)] float wanderTime;
    [SerializeField, Range(10f, 25f)] float wanderDistance;
    [SerializeField] bool isRunning;

    [SerializeField] float wander = 0;
    AnimalAI ai;
    Vector3 target;

    private void Start()
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;

        isRunning = false;
    }

    private void Update()
    {
        if (isRunning)
        {
            wander += Time.deltaTime;
            if(wanderTime < wander)
            {
                wander = 0;
                isRunning = false;
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
            }
        }
    }

    private void Wander()
    {
        target = ai.RandomNavSphere(ai.transform.position, wanderDistance);
        ai.Move(target);
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.WANDER)
        {
            ai.currentState = Case.WANDER;
            Run();
            Wander();
        }
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

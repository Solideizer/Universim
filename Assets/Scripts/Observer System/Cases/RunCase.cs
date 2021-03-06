﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCase : MonoBehaviour, ICase
{
#pragma warning disable 0649
    [SerializeField] LayerMask targetMask;
    [SerializeField, Range(20f, 30f)] float vision;
    [SerializeField, Range(0.5f, 3f)] float timeBetweenSearches;
    [SerializeField, Range(5f, 25f)] float enemyDistanceRun;
#pragma warning restore 0649

    public float searchTime = 0;
    public bool flee;

    // Bu kısmın işlenişi diğer caselere benzer.
    // Treshold 0,5 tir. Boolean değer ile tespit yapılır.

    private bool isRunning;
    private AnimalAI ai;
    private Transform target;

    AnimationManager _animationManager;

    private void Start()
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;

        _animationManager = GetComponent<AnimationManager>();

        flee = false;
        ai.caseDatas.Add(new CaseContainer(Case.FLEE, 0, 0.5f, 0.5f, CasePriority.HIGH));

        isRunning = false;
    }

    private void Update()
    {
        if (!flee)
        {
            searchTime += Time.deltaTime;
            if (searchTime >= timeBetweenSearches)
            {
                searchTime = 0;
                target = Search();
                if (target != null)
                {
                    flee = true;
                    ai.currentState = Case.FLEE;
                    ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.FLEE));
                }
            }
        }
        else
        {
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < enemyDistanceRun)
                    RunAway();
                else
                    Stop();
            }
            else
                Stop();
        }

    }

    private void RunAway()
    {
        Vector3 dirToPlayer = transform.position - target.position;
        Vector3 newPos = transform.position + dirToPlayer;
        _animationManager.SetState(AnimationType.Walk);
        ai.Move(newPos);
    }

    private void Stop()
    {
        flee = false;
        target = null;
        ai.Stop();
        ai.HandleSpeed(SpeedPhase.WALK);
        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.WANDER));
    }

    private Transform Search()
    {
        return ai.FindClosestThing(transform.position, targetMask, vision);
    }

    private void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.AVAILABLE)
        {
            CaseContainer.Adjust(ai.caseDatas, Case.FLEE, flee ? 1 : 0);
        }
        else if (e.state == Case.IDENTITY_UPDATE)
            vision = ai.Identity.Vision;
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

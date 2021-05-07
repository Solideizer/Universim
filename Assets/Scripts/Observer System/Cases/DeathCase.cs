using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCase : MonoBehaviour, ICase
{
    [SerializeField] bool isHerbivore;

    AnimalAI ai;
    bool isRunning;

    private void Start() 
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;    
    }

    private void Death()
    {
        if (isHerbivore)
        {
            transform.localScale = AnimalManager.Instance.chickSize;
            AnimalManager.Instance.herbivorePool.Push(ai);
        }
        else
        {
            transform.localScale = AnimalManager.Instance.cubSize;
            AnimalManager.Instance.carnivorePool.Push(ai);
        }
    }

    private void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.DEATH)
        {
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.RESET));
            Death();
        }
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

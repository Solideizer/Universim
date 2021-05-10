using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregnancyCase : MonoBehaviour, ICase
{
    [SerializeField, Range(15f, 35f)] float pregnancyTime = 22f;
    public float pregnancy = 0;
    public bool isRunning;

    bool canPregnant;
    AnimalAI ai;

    private void Start() 
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;
        isRunning = false;

        IdentityUpdate();
    }

    private void Update() 
    {
        if(!canPregnant) return;

        if(isRunning)
        {
            pregnancy += Time.deltaTime;
            if(pregnancy > pregnancyTime)
            {
                pregnancy = 0;
                isRunning = false;
                GiveBirth();
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
            }
        }
    }

    private void GiveBirth()
    {
        AnimalManager.Instance.GetHerbivore(transform.position);
        ai.AnimalIdentity.canReproduce = true;
        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDENTITY_UPDATE));
    }

    private void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if(e.state == Case.PREGNANCY)
        {
            ai.currentState = Case.PREGNANCY;
            ai.Stop();
            ai.AnimalIdentity.canReproduce = false;
            Run();
        }
        else if(e.state == Case.IDENTITY_UPDATE)
            IdentityUpdate();
        else if(e.state == Case.RESET)
        {
            isRunning = false;
            canPregnant = false;
            pregnancy = 0;
        }
    }

    private void IdentityUpdate()
    {
        if (ai.AnimalIdentity.sex == Sex.MALE)
            canPregnant = false;
        else
            canPregnant = true;
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

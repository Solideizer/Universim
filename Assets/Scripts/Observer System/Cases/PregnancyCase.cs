using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregnancyCase : MonoBehaviour, ICase
{
    [SerializeField, Range(1f, 35f)] float pregnancyTime = 22f;
    [SerializeField] bool isRunning;

    public float pregnancy = 0;
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
        // Vector3 position = transform.position;
        // AnimalAI child = Instantiate(childPrefab, position, Quaternion.identity);
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
    }

    private void IdentityUpdate()
    {
        if (ai.AnimalIdentity.sex == Sex.MALE)
            this.enabled = false;
        else
            this.enabled = true;
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}

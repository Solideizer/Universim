using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseContainer
{
    public CaseContainer(Case state, float value, float valueTreshold, CasePriority priority)
    {
        this.state = state;
        this.priority = priority;
        this.value = value;
        this.valueTreshold = valueTreshold;
    }

    public Case state;
    public CasePriority priority;
    public float value;
    public float valueTreshold;

    public static CaseContainer GetCase(List<CaseContainer> cases, Case wantedCase)
    {
        for (var i = 0; i < cases.Count; i++)
        {
            if(cases[i].state == wantedCase)
                return cases[i];
        }

        return null;
    }

    public static void Adjust(List<CaseContainer> cases, Case wantedCase, float updateValue) 
    {
        for (var i = 0; i < cases.Count; i++)
        {
            if(cases[i].state == wantedCase)
            {
                cases[i].value = updateValue;
                return;
            }
        }
    }
}

public enum CasePriority
{
    TOO_LOW,
    LOW,
    MID,
    HIGH
}

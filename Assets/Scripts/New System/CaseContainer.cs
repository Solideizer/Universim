using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseContainer
{
    public CaseContainer(Case state, float value, float valueTreshold)
    {
        this.state = state;
        this.value = value;
        this.valueTreshold = valueTreshold;
    }

    public Case state;
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

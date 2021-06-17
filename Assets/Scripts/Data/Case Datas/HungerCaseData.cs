using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerCaseData : CaseData
{
    private Transform food;

    public HungerCaseData(Transform food)
    {
        this.food = food;
    }

    public override void SetData(ICase _case)
    {
        HungryCase hungryCase = _case as HungryCase;
        hungryCase.reportedTarget = food;
    }
}

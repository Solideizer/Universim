using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstCaseData : CaseData
{
    private Transform water;

    public ThirstCaseData(Transform water)
    {
        this.water = water;
    }

    public override void SetData(ICase _case)
    {
        ThirstyCase thirstyCase = _case as ThirstyCase;
        thirstyCase.reportedTarget = water;
    }
}

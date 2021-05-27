using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionCaseData : CaseData
{
    AnimalAI target;

    public ReproductionCaseData(AnimalAI target)
    {
        this.target = target;
    }

    public override void SetData(ICase _case)
    {
        ReproductionCase reproductionCase = _case as ReproductionCase;
        reproductionCase.target = target;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionCaseData : CaseData
{
    Transform target;

    public ReproductionCaseData(Transform target)
    {
        this.target = target;
    }

    public override void SetData(ICase _case)
    {
        ReproductionCase reproductionCase = _case as ReproductionCase;
        reproductionCase.target = target;
    }
}

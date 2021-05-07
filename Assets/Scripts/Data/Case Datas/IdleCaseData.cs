using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCaseData : CaseData
{
    public float time;

    public IdleCaseData(float time)
    {
        this.time = time;      
    }

    public override void SetData(ICase _case)
    {
        IdleCase idleCase = _case as IdleCase;
        idleCase.tempTime = time;
    }
}

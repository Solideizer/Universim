using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregnancyCaseData : CaseData
{
    Genetic parent;

    public PregnancyCaseData(Genetic parent)
    {
        this.parent = parent;
    }

    public override void SetData(ICase _case)
    {
        PregnancyCase pregnancyCase = _case as PregnancyCase;
        pregnancyCase.PartnerGene = parent;         
    }
}

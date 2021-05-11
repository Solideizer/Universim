using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecisionMaker
{
    AnimalAI ai;
    bool decisionMade;

    public DecisionMaker(AnimalAI ai)
    {
        this.ai = ai;
        ai.CaseChanged += OnCaseChanged;
    }

    // TODO Ekstra bir seviye daha ekle.
    public void Decision()
    {
        if(ai.currentState != Case.AVAILABLE) return;

        List<CaseContainer> cases = ai.caseDatas.OrderByDescending(x => (int) x.priority).ToList();

        for (var i = 0; i < cases.Count; i++)
        {
            if(cases[i].value > cases[i].valueTreshold)
            {
                decisionMade = true;
                ai.OnCaseChanged(new CaseChangedEventArgs(null, cases[i].state));

                return;
            }
        }

        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.WANDER));
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if(e.state == Case.AVAILABLE)
        {
            ai.currentState = Case.AVAILABLE;
            Decision();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DecisionMaker
{
    AnimalAI ai;
    bool decisionMade;

    List<CaseContainer> cases;

    public DecisionMaker(AnimalAI ai)
    {
        this.ai = ai;
        ai.CaseChanged += OnCaseChanged;
    }

    public void Decision()
    {
        if(ai.currentState != Case.AVAILABLE) return;

        cases = ai.caseDatas.OrderByDescending(x => (int) x.priority).ToList();

        for (var i = 0; i < cases.Count; i++)
        {
            if (cases[i].value > cases[i].criticalTreshold)
            {
                ai.HandleSpeed(SpeedPhase.SPRINT);
                MakeDecision(i);
                return;
            }
        }

        for (var i = 0; i < cases.Count; i++)
        {
            if(cases[i].value > cases[i].valueTreshold)
            {
                ai.HandleSpeed(SpeedPhase.RUN);
                MakeDecision(i);
                return;
            }
        }

        cases.Clear();
        ai.HandleSpeed(SpeedPhase.WALK);
        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.WANDER));
    }

    private void MakeDecision(int index)
    {
        decisionMade = true;
        ai.OnCaseChanged(new CaseChangedEventArgs(null, cases[index].state));
        cases.Clear();
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

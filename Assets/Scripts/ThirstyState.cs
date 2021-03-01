using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.AI;

public class ThirstyState : IState
{
    public void Execute(Transform closestWater, CreatureAI creature)
    {
        if(closestWater == null) return;

        creature._agent.transform.LookAt(closestWater);
        creature._agent.SetDestination(closestWater.position);
        var waterDist = Vector3.Distance(closestWater.position, creature._transform.position);
        if (waterDist < 10f)
        {
            //fsm.setBool("isDrinking",true);
            creature._stateManager.thirstAmount = 0f;

        }
    }

}

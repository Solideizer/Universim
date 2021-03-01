using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryState : IState
{
    public void Execute(Transform closestFood, CreatureAI creature)
    {
        if(closestFood == null) return;

        creature._agent.transform.LookAt(closestFood);
        creature._agent.SetDestination(closestFood.position);
        
        var foodDist = Vector3.Distance(closestFood.position, creature._transform.position);
        if (foodDist < 10f)
        {
            //fsm.setBool("isDrinking",true);
            creature._stateManager.hungerAmount = 0f;
        }
    }


}

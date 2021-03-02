using UnityEngine;

namespace AI
{
    public class ThirstyState : IState
    {
        public void Execute (Transform closestWater, CreatureAI creature)
        {
            if (closestWater == null) return;

            creature.agent.transform.LookAt (closestWater);
            creature.agent.SetDestination (closestWater.position);

            var waterDist = Vector3.Distance (closestWater.position, creature.tform.position);
            if (waterDist < 10f)
            {
                creature.stateManager.thirstAmount = 0f;

            }
        }

    }
}
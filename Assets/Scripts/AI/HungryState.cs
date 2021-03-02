using UnityEngine;

namespace AI
{
    public class HungryState : IState
    {
        public void Execute (Transform closestFood, CreatureAI creature)
        {
            if (closestFood == null) return;

            creature.agent.transform.LookAt (closestFood);
            creature.agent.SetDestination (closestFood.position);

            var foodDist = Vector3.Distance (closestFood.position, creature.tform.position);
            if (foodDist < 10f)
            {
                creature.stateManager.hungerAmount = 0f;
            }
        }

    }
}
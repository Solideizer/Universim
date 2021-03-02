using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CarnivoreAI : CreatureAI
    {
        #region Variable Declarations
        private List<Transform> _waterLocations;
        #endregion
        protected override void Awake ()
        {
            base.Awake ();
            _waterLocations = new List<Transform> (entity.MemorySize);
            foodLayerMask = LayerMask.GetMask ("Herbivore");
            waterLayerMask = LayerMask.GetMask ("Water");
        }
        public void FindFood ()
        {
            agent.isStopped = false;

            var closestFood = FindClosestThing (foodLayerMask, visionRadius);
            if (closestFood != null)
                ExecuteState (closestFood, hungryState);
            else
                Wander ();
        }

        public void FindWater ()
        {
            agent.isStopped = false;

            var closestWater = FindClosestThing (waterLayerMask, visionRadius);

            if (closestWater != null)
            {
                _waterLocations.Add (closestWater.transform);
                ExecuteState (closestWater, thirstyState);
            }
            else
            {
                //check if there is a location in memory
                if (_waterLocations.Count != 0)
                {
                    for (int i = 0; i < _waterLocations.Count; i++)
                    {
                        var waterMemoryLocation = _waterLocations[i];
                        ExecuteState (waterMemoryLocation, thirstyState);
                    }
                }
                else
                {
                    Wander ();
                }
            }
        }

        public void Wander ()
        {
            stateManager.fsm.SetBool (IsWandering, true);
            stateManager.fsm.SetBool (IsIdling, false);

            Vector3 newPos = RandomNavSphere (tform.position, visionRadius);
            agent.SetDestination (newPos);

            var dist = Vector3.Distance (newPos, tform.position);
            if (dist < 10f)
            {
                stateManager.fsm.SetBool (IsWandering, false);
                stateManager.fsm.SetBool (IsIdling, true);
            }
        }
        public void Idle ()
        {
            var idleDuration = Random.Range (2.0f, 5.0f);
            StartCoroutine (Idling (idleDuration));
        }

        private IEnumerator Idling (float idleDuration)
        {
            agent.isStopped = true;
            yield return new WaitForSeconds (idleDuration);
            agent.isStopped = false;

            stateManager.fsm.SetBool (IsIdling, false);
            stateManager.fsm.SetBool (IsWandering, false);

        }

    }
}
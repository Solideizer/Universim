using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class HerbivoreAI : CreatureAI
    {
        #region Variable Declarations
        //public float wanderTimer;
        private float _timer;
        private List<Transform> _waterLocations;

        #endregion
        protected override void Awake ()
        {
            base.Awake ();
            _waterLocations = new List<Transform> (entity.MemorySize);
            foodLayerMask = LayerMask.GetMask ("Food");
            waterLayerMask = LayerMask.GetMask ("Water");
        }
        public void FindFood ()
        {
            agent.isStopped = false;

            var closestFood = FindClosestThing (foodLayerMask, visionRadius);
            if (closestFood != null)
            {
                ExecuteState (closestFood, hungryState);
            }
            else
            {
                Wander ();
            }
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
            _timer += Time.deltaTime;
            var wanderTimer = Random.Range (4, 11);

            if (_timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere (tform.position, visionRadius);
                agent.SetDestination (newPos);

                var dist = Vector3.Distance (newPos, tform.position);
                if (dist < 5f)
                {
                    Idle ();
                }
                _timer = 0;
            }

        }
        public void Idle ()
        {
            var idleDuration = Random.Range (2.0f, 5.0f);
            StartCoroutine (Idling (idleDuration));
        }

        private IEnumerator Idling (float idleDuration)
        {
            //starts idling
            agent.isStopped = true;
            yield return new WaitForSeconds (idleDuration);
            agent.isStopped = false;

            wanderDuration = Random.Range (4.0f, 10.0f);
            stateManager.fsm.SetBool (IsIdling, false);

        }

    }
}
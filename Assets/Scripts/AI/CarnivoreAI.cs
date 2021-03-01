using UnityEngine;

namespace AI
{
    public class CarnivoreAI : CreatureAI
    {
        #region Variable Declarations
        public float wanderTimer;
        private float timer;
        #endregion
        protected override void Awake ()
        {
            base.Awake ();
            foodLayerMask = LayerMask.GetMask ("Herbivore");
            waterLayerMask = LayerMask.GetMask ("Water");
        }
        public void FindFood ()
        {
            _stateManager.fsm.SetBool (IsIdling, false);
            _stateManager.fsm.SetBool (IsWandering, false);
            _agent.isStopped = false;

            var closestFood = FindClosestThing(foodLayerMask, visionRadius);
            if (closestFood != null)
                ExecuteState(closestFood, hungryState);
            else
                Wander ();
        }

        public void FindWater ()
        {
            _stateManager.fsm.SetBool (IsIdling, false);
            _stateManager.fsm.SetBool (IsWandering, false);
            _agent.isStopped = false;

            var closestWater = FindClosestThing(waterLayerMask, visionRadius);

            if (closestWater != null)
            {
                ExecuteState(closestWater, thirstyState);
            }
            else
            {
                Wander ();
            }
        }

        public void Wander ()
        {
            //_stateManager.fsm.SetBool (IsWandering, true);
            //_stateManager.fsm.SetBool (IsIdling, false);
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere (_transform.position, visionRadius);
                _agent.SetDestination (newPos);

                var dist = Vector3.Distance (newPos, _transform.position);
                if (dist < 5f)
                {
                    _agent.isStopped = true;
                    _stateManager.fsm.SetBool (IsWandering, false);
                    _stateManager.fsm.SetBool (IsIdling, true);

                }
                timer = 0;
            }
        }

    }
}
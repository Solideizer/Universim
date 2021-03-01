using UnityEngine;

namespace AI
{
    public class HerbivoreAI : CreatureAI
    {
        #region Variable Declarations
        public float wanderTimer;
        private float timer;

        #endregion
        protected override void Awake ()
        {
            base.Awake ();
            foodLayerMask = LayerMask.GetMask ("Food");
            waterLayerMask = LayerMask.GetMask ("Water");
        }
        public void FindFood ()
        {
            _stateManager.fsm.SetBool ("isWandering", false);
            _agent.isStopped = false;

            var closestFood = FindClosestThing(foodLayerMask, visionRadius);
            if (closestFood != null)
                ExecuteState(closestFood, hungryState);
            else
                Wander ();
        }
        public void FindWater ()
        {
            _stateManager.fsm.SetBool ("isWandering", false);
            _agent.isStopped = false;
            
            var closestWater = FindClosestThing(waterLayerMask, visionRadius);
            if (closestWater != null)
                ExecuteState(closestWater, thirstyState);
            else
                Wander ();

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
                if (dist < 10f)
                {
                    _stateManager.fsm.SetBool ("isWandering", false);
                    _stateManager.fsm.SetBool ("isIdling", true);

                }
                timer = 0;
            }

        }
        public Transform FindClosestThing (string tag)
        {
            var objects = GameObject.FindGameObjectsWithTag (tag);
            Transform bestTarget = null;
            var closestDistanceSqr = Mathf.Infinity;

            var currentPosition = _transform.position;
            foreach (GameObject potentialTarget in objects)
            {
                var directionToTarget = potentialTarget.transform.position - currentPosition;
                var dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }

            return bestTarget;
        }

    }
}
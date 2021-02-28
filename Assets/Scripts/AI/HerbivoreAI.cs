using UnityEngine;

namespace AI
{
    public class HerbivoreAI : CreatureAI
    {
        #region Variable Declarations
        public float wanderTimer;
        private float timer;
        private const float visionRadius = 40f;
        private int foodLayerMask;
        private int waterLayerMask;
        private static readonly int IsIdling = Animator.StringToHash ("isIdling");
        private static readonly int IsWandering = Animator.StringToHash ("isWandering");
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

            if (FindClosestThing (foodLayerMask, visionRadius))
            {
                var closestFood = FindClosestThing (foodLayerMask, visionRadius);
                _agent.transform.LookAt (closestFood);
                _agent.SetDestination (closestFood.position);
                //Move (closestFood);

                var foodDist = Vector3.Distance (closestFood.position, _transform.position);
                if (foodDist < 10f)
                {
                    //fsm.setBool("isDrinking",true);
                    _stateManager.hungerAmount = 0f;

                }
            }
            else
            {
                Wander ();
            }
        }
        public void FindWater ()
        {
            _stateManager.fsm.SetBool ("isWandering", false);
            _agent.isStopped = false;

            if (FindClosestThing (waterLayerMask, visionRadius))
            {
                var closestWater = FindClosestThing (waterLayerMask, visionRadius);
                _agent.transform.LookAt (closestWater);
                _agent.SetDestination (closestWater.position);
                //Move (closestWater);

                var waterDist = Vector3.Distance (closestWater.position, _transform.position);
                if (waterDist < 10f)
                {
                    //fsm.setBool("isDrinking",true);
                    _stateManager.thirstAmount = 0f;

                }
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
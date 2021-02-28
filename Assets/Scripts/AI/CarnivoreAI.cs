using UnityEngine;

namespace AI
{
    public class CarnivoreAI : CreatureAI
    {
        #region Variable Declarations
        public float wanderTimer;
        private const float VisionRadius = 40f;
        private float timer;
        private int herbivoreLayerMask;
        private int waterLayerMask;
        private static readonly int IsIdling = Animator.StringToHash ("isIdling");
        private static readonly int IsWandering = Animator.StringToHash ("isWandering");

        #endregion
        protected override void Awake ()
        {
            base.Awake ();
            herbivoreLayerMask = LayerMask.GetMask ("Herbivore");
            waterLayerMask = LayerMask.GetMask ("Water");
        }
        public void FindFood ()
        {
            _stateManager.fsm.SetBool (IsIdling, false);
            _stateManager.fsm.SetBool (IsWandering, false);
            _agent.isStopped = false;

            if (FindClosestThing (herbivoreLayerMask, VisionRadius))
            {
                var closestFood = FindClosestThing (herbivoreLayerMask, VisionRadius);
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
            _stateManager.fsm.SetBool (IsIdling, false);
            _stateManager.fsm.SetBool (IsWandering, false);
            _agent.isStopped = false;

            if (FindClosestThing (waterLayerMask, VisionRadius))
            {
                var closestWater = FindClosestThing (waterLayerMask, VisionRadius);
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
                Vector3 newPos = RandomNavSphere (_transform.position, VisionRadius);
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
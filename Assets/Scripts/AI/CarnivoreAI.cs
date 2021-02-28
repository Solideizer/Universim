using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine;

public class CarnivoreAI : CreatureAI
{
    #region Variable Declarations
    public float wanderTimer;
    private float timer;
    private const float visionRadius = 40f;
    private int herbivoreLayerMask;
    private int waterLayerMask;

    #endregion
    protected override void Awake ()
    {
        base.Awake ();
        herbivoreLayerMask = LayerMask.GetMask ("Herbivore");
        waterLayerMask = LayerMask.GetMask ("Water");
    }
    public void FindFood ()
    {
        _stateManager.fsm.SetBool ("isIdling", false);
        _stateManager.fsm.SetBool ("isWandering", false);
        _agent.isStopped = false;

        //var closestFood = FindClosestThing ("Chicken");
        if (FindClosestThing (herbivoreLayerMask, visionRadius))
        {
            var closestFood = FindClosestThing (herbivoreLayerMask, visionRadius);
            _agent.transform.LookAt (closestFood);
            Move (closestFood);
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
        _stateManager.fsm.SetBool ("isIdling", false);
        _stateManager.fsm.SetBool ("isWandering", false);
        _agent.isStopped = false;

        //var closestWater = FindClosestThing ("Water");

        if (FindClosestThing (waterLayerMask, visionRadius))
        {
            var closestWater = FindClosestThing (waterLayerMask, visionRadius);
            _agent.transform.LookAt (closestWater);
            Move (closestWater);
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
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere (_transform.position, visionRadius);
            _agent.SetDestination (newPos);

            var dist = Vector3.Distance (newPos, _transform.position);
            if (dist < 5f)
            {
                _agent.isStopped = true;
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
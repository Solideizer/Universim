using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine;

public class CarnivoreAI : CreatureAI
{
    #region Variable Declarations
    public float wanderTimer;
    private float timer;
    private const float visionRadius = 40f;

    #endregion

    public void FindFood ()
    {
        _stateManager.fsm.SetBool ("isWandering", false);
        _agent.isStopped = false;

        //var closestFood = FindClosestThing ("Chicken");
        Transform closestFood = FindClosestThing(9, visionRadius);
        
        _agent.transform.LookAt (closestFood);
        Move (closestFood);
        var foodDist = Vector3.Distance (closestFood.position, transform.position);

        if (foodDist < 10f)
        {
            //fsm.setBool("isEating",true);
            _stateManager.hungerAmount = 0f;
            Destroy (closestFood.gameObject);

        }
    }

    public void FindWater ()
    {
        _stateManager.fsm.SetBool ("isWandering", false);
        _agent.isStopped = false;

        //var closestWater = FindClosestThing ("Water");
        Transform closestWater = FindClosestThing(4, visionRadius);

        if (closestWater == null) return;

        _agent.transform.LookAt (closestWater);
        Move (closestWater);
        var waterDist = Vector3.Distance (closestWater.position, transform.position);
        if (waterDist < 10f)
        {
            //fsm.setBool("isDrinking",true);
            _stateManager.thirstAmount = 0f;

        }
    }

    public void Wander ()
    {

        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere (transform.position, visionRadius, 1);
            Move (newPos);

            var dist = Vector3.Distance (newPos, transform.position);
            if (dist < 10f)
            {
                _stateManager.fsm.SetBool ("isWandering", false);

            }
            timer = 0;
        }

    }

    public Transform FindClosestThing (string tag)
    {
        var objects = GameObject.FindGameObjectsWithTag (tag);
        Transform bestTarget = null;
        var closestDistanceSqr = Mathf.Infinity;

        var currentPosition = transform.position;
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
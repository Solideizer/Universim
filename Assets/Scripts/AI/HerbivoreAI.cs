﻿using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class HerbivoreAI : CreatureAI
{
    #region Variable Declarations
    public float wanderTimer;
    private float timer;
    private const float VisionRadius = 40f;

    #endregion

    public void FindFood ()
    {
        _stateManager.fsm.SetBool ("isWandering", false);
        _agent.isStopped = false;

        //var closestFood = FindClosestThing ("Plant");
        var closestFood = FindClosestThing(11, VisionRadius);
        _agent.transform.LookAt (closestFood);
        Move (closestFood);
        var foodDist = Vector3.Distance (closestFood.position, transform.position);

        if (foodDist < 10f)
        {
            //fsm.setBool("isEating",true);
            _stateManager.hungerAmount = 0f;

        }
    }
    public void FindWater ()
    {
        _stateManager.fsm.SetBool ("isWandering", false);
        _agent.isStopped = false;

        //var closestWater = FindClosestThing ("Water");
        var closestWater = FindClosestThing(4, VisionRadius);

        if(closestWater == null) return;

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
            Vector3 newPos = RandomNavSphere (transform.position, VisionRadius, 1);
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
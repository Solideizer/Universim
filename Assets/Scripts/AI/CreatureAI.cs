using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected Transform _transform;
    protected StateManager _stateManager;

    protected virtual void Awake ()
    {
        _agent = GetComponent<NavMeshAgent> ();
        _transform = GetComponent<Transform> ();
        _stateManager = GetComponent<StateManager> ();
    }

    public void Move (Transform destination)
    {
        var direction = destination.position - _transform.position;
        Debug.DrawRay (_transform.position, direction, Color.red);
        _transform.rotation = Quaternion.Slerp (
            _transform.rotation, Quaternion.LookRotation (destination.position), 20 * Time.deltaTime);

        if (direction.magnitude > 2f)
        {
            _agent.SetDestination (destination.position);
            var dist = Vector3.Distance (destination.position, _transform.position);
            //_transform.Translate(direction.normalized * MovementSpeed * Time.deltaTime, Space.World);

            if (dist < 5)
            {
                _agent.isStopped = true;
                _stateManager.fsm.SetBool ("isWandering", false);
                _stateManager.fsm.SetBool ("isIdling", true);
            }
        }
    }

    public void Move (Vector3 destination)
    {
        Debug.Log ("destination " + destination);
        var direction = destination - _transform.position;
        Debug.DrawRay (_transform.position, direction, Color.red);
        _transform.rotation = Quaternion.Lerp (
            _transform.rotation, Quaternion.LookRotation (destination), 20 * Time.deltaTime);

        if (direction.magnitude > 2f)
        {
            _agent.SetDestination (destination);
            var dist = Vector3.Distance (destination, _transform.position);

            if (dist < 5)
            {
                _agent.isStopped = true;
                _stateManager.fsm.SetBool ("isWandering", false);
                _stateManager.fsm.SetBool ("isIdling", true);
            }
        }
    }

    protected Transform FindClosestThing (int layerMask, float radius)
    {
        if (CheckColliders (transform.position, radius, layerMask))
        {
            Collider closestThingCollider = CheckColliders (transform.position, radius, layerMask);
            return closestThingCollider.transform;
        }
        else
        {
            return null;
        }

    }

    #region Utilities

    protected Collider CheckColliders (Vector3 position, float radius, int layerMask)
    {
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc (position, radius, hitColliders, layerMask);

        Collider closestTarget = null;
        var currentPosition = _transform.position;
        var closestDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < numColliders; i++)
        {
            var directionToTarget = hitColliders[i].transform.position - currentPosition;
            var dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTarget = hitColliders[i];
            }
            return closestTarget;
        }
        return null;

        // foreach (Collider potentialTarget in hitColliders)
        // {
        //     var directionToTarget = potentialTarget.transform.position - currentPosition;
        //     var dSqrToTarget = directionToTarget.sqrMagnitude;
        //     if (dSqrToTarget < closestDistanceSqr)
        //     {
        //         closestDistanceSqr = dSqrToTarget;
        //         closestTarget = potentialTarget;
        //     }
        // }

        //Collider[] cols = Physics.OverlapSphere (position, radius, layerMask);

        //return hitColliders;
    }
    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, 40f);
    }

    protected Vector3 RandomNavSphere (Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition (randDirection, out navHit, dist, NavMesh.AllAreas);

        return navHit.position;
    }
    #endregion

}
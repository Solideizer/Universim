using AI;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected Transform _transform;
    protected StateManager _stateManager;
    private static readonly int IsWandering = Animator.StringToHash ("isWandering");
    private static readonly int IsIdling = Animator.StringToHash ("isIdling");

    protected virtual void Awake ()
    {
        _agent = GetComponent<NavMeshAgent> ();
        _transform = GetComponent<Transform> ();
        _stateManager = GetComponent<StateManager> ();
    }

    // public void Move (Transform destination)
    // {
    //     var direction = destination.position - _transform.position;
    //     Debug.DrawRay (_transform.position, direction, Color.red);
    //     _transform.rotation = Quaternion.Slerp (
    //         _transform.rotation, Quaternion.LookRotation (destination.position), 20 * Time.deltaTime);

    //     if (direction.magnitude > 2f)
    //     {
    //         _agent.SetDestination (destination.position);
    //         var dist = Vector3.Distance (destination.position, _transform.position);

    //         if (dist < 5)
    //         {
    //             _agent.isStopped = true;
    //             _stateManager.fsm.SetBool (IsWandering, false);
    //             _stateManager.fsm.SetBool (IsIdling, true);
    //         }
    //     }
    // }
    protected Vector3 RandomNavSphere (Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMesh.SamplePosition (randDirection, out var navHit, dist, NavMesh.AllAreas);

        return navHit.position;
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
    }
    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, 40f);
    }

    #endregion

}
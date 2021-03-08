using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;

    private Entity entity;
    private DecisionMaker decisionMaker;

    public Case currentState = Case.AVAILABLE;
    public event EventHandler<CaseChangedEventArgs> CaseChanged;
    public List<CaseContainer> caseDatas = new List<CaseContainer>();

    private void Start() 
    {
        decisionMaker = new DecisionMaker(this);
        agent = GetComponent<NavMeshAgent>();

        decisionMaker.Decision();
    }

    public void Move(Vector3 target)
    {
        agent.isStopped = false;
        agent.SetDestination(target);
    }

    public void Stop()
    {
        agent.SetDestination(transform.position);
        agent.isStopped = true;
    }

    public virtual void OnCaseChanged(CaseChangedEventArgs e)
    {
        EventHandler<CaseChangedEventArgs> handler = CaseChanged;
        if(handler != null)
            handler(this, e);
    }

    #region Utilities

    public Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMesh.SamplePosition(randDirection, out var navHit, dist, NavMesh.AllAreas);
        return navHit.position;
    }

    public Transform FindClosestThing(Vector3 currentPosition, int layerMask, float radius)
    {
        Collider closestThingCollider = CheckColliders(currentPosition, radius, layerMask);

        if (closestThingCollider != null)
            return closestThingCollider.transform;
        else
            return null;
    }

    private Collider CheckColliders(Vector3 currentPosition, float radius, int layerMask)
    {
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(currentPosition, radius, hitColliders, layerMask);

        Collider closestTarget = null;
        var closestDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < numColliders; i++)
        {
            var directionToTarget = hitColliders[i].transform.position - currentPosition;
            var dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                // TODO En yakındakini bulmayı ekle.

                closestDistanceSqr = dSqrToTarget;
                closestTarget = hitColliders[i];
                return closestTarget;
            }
        }

        return null;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(base.transform.position, 20f);
    }
}

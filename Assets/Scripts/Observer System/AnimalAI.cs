using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;

    public Case currentState = Case.AVAILABLE;

    private DecisionMaker decisionMaker;
    public Memory memory;
    public Genetic genetic;
    public Identity animalIdentity;

    public Identity AnimalIdentity { get => animalIdentity; }
    public DecisionMaker DecisionMaker { get => decisionMaker; }

    public event EventHandler<CaseChangedEventArgs> CaseChanged;
    public List<CaseContainer> caseDatas = new List<CaseContainer>();

    [SerializeField] bool isBaby;

    private void Awake() 
    {
        decisionMaker = new DecisionMaker(this);
        memory = new Memory(2, 2);
        agent = GetComponent<NavMeshAgent>();
        CreateIdentity();
    }

    private void Start() 
    {
        Subscribe();
        decisionMaker.Decision();
    }

    private void OnEnable() 
    {
        CreateIdentity();
        decisionMaker.Decision();
        if(isBaby)
            OnCaseChanged(new CaseChangedEventArgs(null, Case.GROWTH));
    }

    public virtual void OnCaseChanged(CaseChangedEventArgs e)
    {
        EventHandler<CaseChangedEventArgs> handler = CaseChanged;
        if(handler != null)
            handler(this, e);

        if(e.state == Case.RESET)
        {
            isBaby = true;
            animalIdentity.canReproduce = false;
            currentState = Case.AVAILABLE;
        }
    }

    public void CreateIdentity()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        Sex sex = (Sex)rand;
        Identity identity = new Identity(sex, isBaby);
        animalIdentity = identity;

        OnCaseChanged(new CaseChangedEventArgs(null, Case.IDENTITY_UPDATE));
    }

    public void Subscribe()
    {
        AnimalManager.Instance.animals.Add(gameObject.GetInstanceID(), this);
    }

    public void Unsubscribe()
    {
        AnimalManager.Instance.animals.Remove(gameObject.GetInstanceID());
    }

    #region NavMeshAgent

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

    #endregion

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
        Collider[] hitColliders;
        int numColliders = GetColliders(currentPosition, radius, layerMask, out hitColliders);

        Collider closestTarget = null;
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
        }

        return closestTarget;
    }

    public static int GetColliders(Vector3 currentPosition, float radius, int layerMask, out Collider[] hitColliders)
    {
        int maxColliders = 10;
        hitColliders = new Collider[maxColliders];
        return Physics.OverlapSphereNonAlloc(currentPosition, radius, hitColliders, layerMask);
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(base.transform.position, 20f);
    }
}

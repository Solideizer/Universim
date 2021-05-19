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
    private Memory memory;
    public Identity identity;
    private Speed speed;

    public Identity Identity { get => identity; }
    public Memory Memory { get => memory; }

    public event EventHandler<CaseChangedEventArgs> CaseChanged;
    public List<CaseContainer> caseDatas = new List<CaseContainer>();

    [SerializeField] bool isBaby;

    private void Awake() 
    {
        decisionMaker = new DecisionMaker(this);
        memory = new Memory(2, 2);
        agent = GetComponent<NavMeshAgent>();

        // TODO Başlangıçta default başlayan ama rasgele genlere sahip agentlar oluştur.
        //CreateIdentity();
        
    }

    private void Start() 
    {
        AwakeAnimal(Genetic.GetRandomizedGene());
        Subscribe();
    }

    public virtual void OnCaseChanged(CaseChangedEventArgs e)
    {
        EventHandler<CaseChangedEventArgs> handler = CaseChanged;
        if(handler != null)
            handler(this, e);

        if(e.state == Case.IDENTITY_UPDATE)
        {
            memory = identity.Memory;
            speed = identity.Speed;
        }
        else if(e.state == Case.RESET)
        {
            isBaby = true;
            identity.canReproduce = false;
            currentState = Case.AVAILABLE;
        }
    }

    #region IDENTITY METHODS

    public void AwakeAnimal(Genetic genetic)
    {
        CreateIdentity(genetic);
        if (isBaby)
            OnCaseChanged(new CaseChangedEventArgs(null, Case.GROWTH));

        decisionMaker.Decision();
    }

    private void CreateIdentity(Genetic genetic)
    {
        Identity identity = new Identity(isBaby, genetic);
        this.identity = identity;

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
    #endregion

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
            if(hitColliders[i].transform.position == currentPosition) continue;

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
        Gizmos.DrawWireSphere(base.transform.position, identity.Vision);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SpeedPhase { WALK, RUN, SPRINT }

public class AnimalAI : MonoBehaviour
{
    [SerializeField] Identity identity;
    public NavMeshAgent agent;
    [SerializeField] bool starter;

    [Header("Growth Variables")]
    [Space]
    [SerializeField] int phase;
    [SerializeField] float growthTime;
    [SerializeField] float growthMultiplier;

    private float growth;
    private DecisionMaker decisionMaker;
    private Memory memory;
    private Speed speed;
    
    [HideInInspector] public LayerMask ownMask;

    public Case currentState = Case.AVAILABLE;
    public Identity Identity { get => identity; }
    public Memory Memory { get => memory; }
    public event EventHandler<CaseChangedEventArgs> CaseChanged;
    public List<CaseContainer> caseDatas = new List<CaseContainer>();

    private void Awake() 
    {
        decisionMaker = new DecisionMaker(this);
        memory = new Memory(2, 2);
        ownMask = gameObject.layer;

        AwakeAnimal(Genetic.GetRandomizedGene(), false);
        if(starter)
            agent.enabled = true;
    }

    private void Start() 
    {
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
            identity.canReproduce = false;
            currentState = Case.AVAILABLE;
        }
    }

    public void HandleSpeed(SpeedPhase phase)
    {
        switch (phase)
        {
            case SpeedPhase.WALK:
                agent.speed = speed.walking;
                break;
            case SpeedPhase.RUN:
                agent.speed = speed.running;
                break;
            case SpeedPhase.SPRINT:
                agent.speed = speed.sprinting;
                break;
        }

    }

    #region IDENTITY METHODS

    public void AwakeAnimal(Genetic genetic, bool isBaby)
    {
        CreateIdentity(genetic, isBaby);

        if(isBaby)
            StartCoroutine(Growth());

        decisionMaker.Decision();
    }

    private void CreateIdentity(Genetic genetic, bool isBaby)
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

    #region GROWTH METHODS

    private IEnumerator Growth()
    {
        identity.canReproduce = false;
        while (phase > 0)
        {
            growth += Time.deltaTime;
            if (growth > growthTime)
            {
                growth = 0;
                GrowUp();
                phase--;
            }
            yield return new WaitForFixedUpdate();
        }

        Identity.canReproduce = true;
        this.enabled = false;
        OnCaseChanged(new CaseChangedEventArgs(null, Case.IDENTITY_UPDATE));
    }

    private void GrowUp()
    {
        Vector3 scale = transform.localScale;
        // TODO Bu kısmın daha genelleşmesi lazım.
        scale += Vector3.one * growthMultiplier;
        transform.localScale = scale;
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

    public void Warp(Vector3 target)
    {
        agent.enabled = true;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(target, out hit, 15, 0))
            agent.Warp(hit.position);
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

using AI;
using UnityEngine;
using UnityEngine.AI;
namespace AI
{
    public class CreatureAI : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Transform tform;
        [HideInInspector] public StateManager stateManager;
        protected const float visionRadius = 60f;
        protected readonly int IsIdling = Animator.StringToHash ("isIdling");

        protected ThirstyState thirstyState;
        protected HungryState hungryState;
        protected Entity entity;
        protected float wanderDuration;

        protected virtual void Awake ()
        {
            agent = GetComponent<NavMeshAgent> ();
            tform = GetComponent<Transform> ();
            stateManager = GetComponent<StateManager> ();
            entity = GetComponent<Entity> ();

            thirstyState = new ThirstyState ();
            hungryState = new HungryState ();
        }

        protected void ExecuteState (Transform destination, IState state)
        {
            state.Execute (destination, this);
        }

        #region Utilities

        protected Collider CheckColliders (Vector3 position, float radius, int layerMask)
        {
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc (position, radius, hitColliders, layerMask);

            Collider closestTarget = null;
            var currentPosition = tform.position;
            var closestDistanceSqr = Mathf.Infinity;

            for (int i = 0; i < numColliders; i++)
            {
                var directionToTarget = hitColliders[i].transform.position - currentPosition;
                var dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestTarget = hitColliders[i];
                    return closestTarget;
                }
            }

            return null;
        }

        private void OnDrawGizmosSelected ()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere (transform.position, visionRadius);
        }

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

        #endregion

    }
}
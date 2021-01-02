using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class HerbivoreAI : MonoBehaviour
{
    #region Variable Declarations
    public float wanderTimer;
    private Transform _transform;
    private NavMeshAgent _agent;
    private StateManager _stateManager;
    private float timer;
    private const float VisionRadius = 40f;

    #endregion
    private void Awake ()
    {
        _stateManager = GetComponent<StateManager> ();
        _transform = GetComponent<Transform> ();
        _agent = GetComponent<NavMeshAgent> ();

    }

    public void FindFood ()
    {
        _stateManager.fsm.SetBool ("isWandering", false);
        // if (_stateManager.fsm.GetBool("isThirsty"))
        // {
        //     _stateManager.fsm.SetBool("isWandering", false);
        // }
        _agent.isStopped = false;
        var closestFood = FindClosestThing ("Plant");
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

        var closestWater = FindClosestThing ("Water");
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
    #region Utility Functions
    public static Vector3 RandomNavSphere (Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);

        return navHit.position;
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

        var direction = destination - _transform.position;
        Debug.DrawRay (_transform.position, direction, Color.red);
        _transform.rotation = Quaternion.Slerp (
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

    #endregion

}
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class HerbivoreAI : MonoBehaviour
{   
    private Transform _transform;
    private NavMeshAgent _agent;
    private StateManager _stateManager;
    private const float VisionRadius = 20f;
    private void Awake()
    {
        _stateManager = GetComponent<StateManager>();
        _transform = GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
    }

    public void FindFood()
    {
        var closestFood = FindClosestThing("Plant");
        Move(closestFood);
        var foodDist = Vector3.Distance(closestFood.position, transform.position);
            
        if (foodDist < 10f)
        {
            //fsm.setBool("isEating",true);
            _stateManager.hungerAmount = 0f;
            
        }
    }
    public void FindWater()
    {
        var closestWater = FindClosestThing("Water");
        Move(closestWater);
        var waterDist = Vector3.Distance(closestWater.position, transform.position);
        Debug.Log(waterDist);
        if (waterDist < 10f)
        {
            //fsm.setBool("isDrinking",true);
            _stateManager.thirstAmount = 0f;
            
        }
    }

    public void Wander()
    {
        var randomPos = Random.insideUnitSphere * VisionRadius;
        _agent.SetDestination(randomPos);
        
    }
    
    public Transform FindClosestThing(string tag)
    {
        var objects = GameObject.FindGameObjectsWithTag(tag);
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
    
    public void Move(Transform destination)
    {
        
        var direction = destination.position - _transform.position;
        Debug.DrawRay(_transform.position, direction, Color.red);
        _transform.rotation = Quaternion.Slerp(
            _transform.rotation, Quaternion.LookRotation(destination.position), 20 * Time.deltaTime);

        if (direction.magnitude > 2f)
        {
            var dist = Vector3.Distance(destination.position, _transform.position);
            //_transform.Translate(direction.normalized * MovementSpeed * Time.deltaTime, Space.World);
            if (dist > 5f)
            {
                _agent.SetDestination(destination.position);
            }

            if (dist < 5)
            {
                _agent.isStopped = true;
            }
        }
    }
}

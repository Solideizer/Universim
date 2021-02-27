using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CreatureAI : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected Transform _transform;
    protected StateManager _stateManager;

    private void Awake() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();
        _stateManager = GetComponent<StateManager>();
    }

    public void Move(Transform destination)
    {

        var direction = destination.position - _transform.position;
        Debug.DrawRay(_transform.position, direction, Color.red);
        _transform.rotation = Quaternion.Slerp(
            _transform.rotation, Quaternion.LookRotation(destination.position), 20 * Time.deltaTime);

        if (direction.magnitude > 2f)
        {
            _agent.SetDestination(destination.position);
            var dist = Vector3.Distance(destination.position, _transform.position);
            //_transform.Translate(direction.normalized * MovementSpeed * Time.deltaTime, Space.World);

            if (dist < 5)
            {
                _agent.isStopped = true;
                _stateManager.fsm.SetBool("isWandering", false);
                _stateManager.fsm.SetBool("isIdling", true);
            }
        }
    }

    public void Move(Vector3 destination)
    {

        var direction = destination - _transform.position;
        Debug.DrawRay(_transform.position, direction, Color.red);
        _transform.rotation = Quaternion.Lerp(
            _transform.rotation, Quaternion.LookRotation(destination), 20 * Time.deltaTime);

        if (direction.magnitude > 2f)
        {
            _agent.SetDestination(destination);
            var dist = Vector3.Distance(destination, _transform.position);

            if (dist < 5)
            {
                _agent.isStopped = true;
                _stateManager.fsm.SetBool("isWandering", false);
                _stateManager.fsm.SetBool("isIdling", true);
            }
        }
    }

}

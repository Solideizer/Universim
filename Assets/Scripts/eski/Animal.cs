using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public abstract class Animal : MonoBehaviour, IAnimalBehavior
{
	#region Variable Declarations
	public StateMachine StateMachine;
	public FindFood FindFood;
	public FindWater FindWater;
	public Wander Wander;
	
	
	protected float HungerAmount = 0f;
	protected float ThirstAmount = 0f;
	protected readonly float HungerThreshold = 500f;
	protected readonly float ThirstThreshold = 500f;
	protected readonly float EatingDuration = 3f;
	protected readonly float DrinkingDuration = 3f;
	protected readonly float ReproduceDuration = 3f;
	protected float ReproductiveUrge = 0f;
	//protected float pregnancyDuration = 10f;
	protected readonly float MovementSpeed = 5f;
	
	protected float WanderDuration = 5f;
	protected Transform _transform;
	protected Animator Animator;
	protected NavMeshAgent Agent;
	private readonly int _walk = Animator.StringToHash("Walk");
	private readonly int _eat = Animator.StringToHash("Eat");

	#endregion

	#region MonoBehaviour Callbacks

	protected virtual void Awake()
	{
		_transform = GetComponent<Transform>();
		Animator = GetComponent<Animator>();
		Agent = GetComponent<NavMeshAgent>();
	}
	private void Start()
	{
		StateMachine = new StateMachine();

		Wander = new Wander(this, StateMachine);
		FindFood = new FindFood(this,StateMachine);
		FindWater = new FindWater(this,StateMachine);
		

		StateMachine.Initialize(Wander);
	}

	private void Update()
	{
		StateMachine.CurrentState.LogicUpdate();
		StateMachine.CurrentState.PhysicUpdate();
	}

	#endregion

	// public virtual void GetThirsty()
	// {
	// 	ThirstAmount += Time.deltaTime * 0.5f;
	// 	//Debug.Log("thirst amount "+ thirstAmount);
	// 	//Debug.Log("thirst threshold "+ thirstThreshold);
	// 	if (ThirstAmount > ThirstThreshold / 50 && ThirstAmount > HungerAmount)
	// 	{
	// 		FindWater();
	// 	}
	// 	if (ThirstAmount >= ThirstThreshold)
	// 	{
	// 		Die();
	// 	}
	// }

	//
	// public virtual void FindWater()
	// {
	// 	Transform closestWater = FindClosest("Water");
	// 	Move(closestWater);
	// 	float dist = Vector3.Distance(closestWater.position, _transform.position);
	// 	if (dist < 8f)
	// 	{
	// 		StartCoroutine(Drink(closestWater));
	// 	}
	// }
	
	public virtual IEnumerator Eat(Transform food)
	{
		Animator.SetBool(_walk,false);
		Animator.SetBool(_eat,true);
		yield return new WaitForSeconds(EatingDuration);
		Animator.SetBool(_eat,false);
		Animator.SetBool(_walk,true);
		HungerAmount = 0f;
		// TO DO : Make plants get smaller 
		// Destroy (food.gameObject, 3f);
	}

	public virtual IEnumerator Drink(Transform water)
	{
		Animator.SetBool(_walk,false);
		Animator.SetBool(_eat,true);
		yield return new WaitForSeconds(DrinkingDuration);
		Animator.SetBool(_eat,false);
		Animator.SetBool(_walk,true);
		ThirstAmount = 0f;
	}

	public virtual void Move(Transform destination)
	{
		Animator.SetBool(_walk,true);
		Vector3 direction = destination.position - _transform.position;
		Debug.DrawRay(_transform.position, direction, Color.red);
		_transform.rotation = Quaternion.Slerp(
			_transform.rotation, Quaternion.LookRotation(destination.position), 20 * Time.deltaTime);

		if (direction.magnitude > 2f)
		{
			var dist = Vector3.Distance(destination.position, _transform.position);
			//_transform.Translate(direction.normalized * MovementSpeed * Time.deltaTime, Space.World);
			if (dist > 5f)
			{
				Agent.SetDestination(destination.position);
			}

			if (dist < 5)
			{
				Agent.isStopped = true;
			}
		}
	}

	//protected abstract void Reproduce(string tag, GameObject prefab);

	public virtual void Die()
	{
		Destroy(gameObject);
	}


	public IEnumerator StartReproducing()
	{
		yield return new WaitForSeconds(ReproduceDuration);
	}
}
using UnityEngine;
using UnityEngine.AI;

public class Wander : State
{
	protected float HungerAmount = 0f;
	protected readonly float HungerThreshold = 500f;
	protected float ThirstAmount = 0f;
	protected readonly float ThirstThreshold = 500f;
	private readonly NavMeshAgent _navMeshAgent;
	private float _wanderDuration = 5f;
	private const float VisionRadius = 20f;
	private readonly Animator _animator;
	private readonly Transform _transform;

	private readonly int _walk = Animator.StringToHash("Walk");
	private readonly int _eat = Animator.StringToHash("Eat");

	public Wander(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
	{
		_transform = animal.transform;
		_animator = animal.GetComponent<Animator>();
		_navMeshAgent = animal.GetComponent<NavMeshAgent>();
	}

	public override void EnterState()
	{
		_animator.SetBool(_eat, false);
		_animator.SetBool(_walk, true);
		_wanderDuration = 5f;
	}

	public override void PhysicUpdate()
	{
		base.PhysicUpdate();
		if (_wanderDuration > 0)
		{
			var randomPos = Random.insideUnitSphere * VisionRadius;
			_navMeshAgent.SetDestination(randomPos);
			_animator.SetBool(_walk, true);
			_wanderDuration -= Time.deltaTime;
		}
		else
		{
			_wanderDuration = Random.Range(5.0f, 10.0f);
			GetRandomDirection();
		}
	}

	public override void LogicUpdate()
	{
		HungerAmount += Time.deltaTime;
		//Debug.Log("hunger amount " + HungerAmount);
		if (HungerAmount > HungerThreshold / 50 && HungerAmount > ThirstAmount)
		{
			StateMachine.ChangeState(Animal.FindFood);
		}

		ThirstAmount += Time.deltaTime * 0.5f;
		//Debug.Log("thirst amount "+ thirstAmount);
		if (ThirstAmount > ThirstThreshold / 50 && ThirstAmount > HungerAmount)
		{
			StateMachine.ChangeState(Animal.FindWater);
		}

		if (HungerAmount >= HungerThreshold || ThirstAmount >= ThirstThreshold)
		{
			Animal.Die();
		}
	}

	private void GetRandomDirection()
	{
		_transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
	}
}
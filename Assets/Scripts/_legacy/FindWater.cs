using UnityEngine;

public class FindWater : Wander
{
	private readonly Animator _animator;
	private readonly int _walk = Animator.StringToHash("Walk");
	private readonly int _eat = Animator.StringToHash("Eat");

	public FindWater(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
	{
		_animator = animal.GetComponent<Animator>();
	}

	public override void PhysicUpdate()
	{
		Transform closestWater = FindClosestThing("Water");
		Animal.Move(closestWater);
		
		float waterDist = Vector3.Distance(closestWater.position, Animal.transform.position);
		
		if (waterDist < 10f)
		{
			_animator.SetBool(_walk, false);
			_animator.SetBool(_eat, true);
			ThirstAmount = 0f;
			StateMachine.ChangeState(Animal.Wander);
		}
	}

	public Transform FindClosestThing(string tag)
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;

		Vector3 currentPosition = Animal.transform.position;
		foreach (GameObject potentialTarget in objects)
		{
			Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget.transform;
			}
		}

		return bestTarget;
	}
}
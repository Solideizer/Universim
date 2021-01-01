
using UnityEngine;

public class FindFood : Wander
	{
		private readonly Animator _animator;
		private readonly int _walk = Animator.StringToHash("Walk");
		private readonly int _eat = Animator.StringToHash("Eat");
		public FindFood(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
		{
			_animator = animal.GetComponent<Animator>();
		}

		public override void PhysicUpdate()
		{
			base.PhysicUpdate();
			Transform closestFood = FindClosestThing("Plant");
			Animal.Move(closestFood);
			float dist = Vector3.Distance(closestFood.position, Animal.transform.position);
			Debug.Log(dist);
			if (dist < 10f)
			{
				//StartCoroutine(Animal.Eat(closestFood));
				_animator.SetBool(_walk,false);
				_animator.SetBool(_eat,true);
				
				
				HungerAmount = 0f;
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

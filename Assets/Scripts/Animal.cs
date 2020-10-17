using System.Collections;
using UnityEngine;

public abstract class Animal : MonoBehaviour, IAnimalBehavior
{
	#region Variable Declarations

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
	//protected float visionRadius = 20f;
	protected float WanderDuration = 5f;
	protected Transform _transform;
	protected Animator Animator;
	private readonly int _walk = Animator.StringToHash("Walk");
	private readonly int _eat = Animator.StringToHash("Eat");

	#endregion

	#region MonoBehaviour Callbacks

	protected virtual void Awake()
	{
		_transform = GetComponent<Transform>();
		Animator = GetComponent<Animator>();
	}

	protected virtual void Update()
	{
		GetHungry();
		GetThirsty();
	}

	#endregion

	public virtual void Wander()
	{
		if (WanderDuration > 0)
		{
			Animator.SetBool(_walk,true);
			transform.Translate(Vector3.forward * (MovementSpeed * Time.deltaTime));
			WanderDuration -= Time.deltaTime;
		}
		else
		{
			WanderDuration = Random.Range(5.0f, 10.0f);
			GetRandomDirection();
		}
	}

	private void GetRandomDirection()
	{
		//Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
		_transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
		//_transform.rotation = Quaternion.Slerp(
			//_transform.rotation, Quaternion.Euler(randomRotation), 15 * Time.deltaTime);
	}

	public virtual void GetThirsty()
	{
		ThirstAmount += Time.deltaTime * 0.5f;
		//Debug.Log("thirst amount "+ thirstAmount);
		//Debug.Log("thirst threshold "+ thirstThreshold);
		if (ThirstAmount > ThirstThreshold / 50 && ThirstAmount > HungerAmount)
		{
			FindWater();
		}
		if (ThirstAmount >= ThirstThreshold)
		{
			Die();
		}
	}

	public virtual void GetHungry()
	{
		HungerAmount += Time.deltaTime;
		Debug.Log("hunger amount " + HungerAmount);
		if (HungerAmount > HungerThreshold / 50 && HungerAmount > ThirstAmount)
		{
			FindFood();
		}
		else
		{
			Wander();
		}

		if (HungerAmount >= HungerThreshold)
		{
			Die();
		}
	}
	public virtual void FindFood()
	{
		Transform closestFood = FindClosest("Plant");
		Move(closestFood);
		float dist = Vector3.Distance(closestFood.position, _transform.position);
		if (dist < 8f)
		{
			StartCoroutine(Eat(closestFood));
		}
	}

	public virtual void FindWater()
	{
		Transform closestWater = FindClosest("Water");
		Move(closestWater);
		float dist = Vector3.Distance(closestWater.position, _transform.position);
		if (dist < 8f)
		{
			StartCoroutine(Drink(closestWater));
		}
	}
	
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
			_transform.Translate(direction.normalized * MovementSpeed * Time.deltaTime, Space.World);
		}
	}

	protected abstract void Reproduce(string tag, GameObject prefab);

	public virtual void Die()
	{
		Destroy(gameObject);
	}

	protected Transform FindClosest(string tag)
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = _transform.position;

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

	public IEnumerator StartReproducing()
	{
		yield return new WaitForSeconds(ReproduceDuration);
	}
}
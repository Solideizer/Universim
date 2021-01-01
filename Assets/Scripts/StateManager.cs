﻿using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateManager : MonoBehaviour
{
	public float hungerAmount;
	public float thirstAmount;

	[SerializeField] private Animator fsm;
	[SerializeField] private float thirstThreshold = 300f;
	[SerializeField] private float hungerThreshold = 500f;

	private float _wanderDuration = 5f;

	private static readonly int IsWandering = Animator.StringToHash ("isWandering");
	private static readonly int IsThirsty = Animator.StringToHash ("isThirsty");
	private static readonly int IsDead = Animator.StringToHash ("isDead");
	private static readonly int IsHungry = Animator.StringToHash ("isHungry");

	private void Start ()
	{
		_wanderDuration = 5f;

	}

	private void FixedUpdate ()
	{
		GetThirsty ();
		GetHungry ();

		if (_wanderDuration > 0)
		{
			fsm.SetBool (IsWandering, true);
			_wanderDuration -= Time.deltaTime;
		}
		else
		{
			var idleDuration = Random.Range (5.0f, 10.0f);
			StartCoroutine (Idling (idleDuration));

			_wanderDuration = Random.Range (5.0f, 10.0f);
		}
	}

	IEnumerator Idling (float idleDuration)
	{
		//starts idling
		fsm.SetBool (IsWandering, false);
		yield return new WaitForSeconds (idleDuration);

	}
	private void GetThirsty ()
	{
		thirstAmount += Time.deltaTime;
		if (thirstAmount > 50)
		{
			fsm.SetBool (IsThirsty, true);
			fsm.SetBool (IsWandering, false);
		}
		else
		{
			fsm.SetBool (IsWandering, true);
		}
		if (thirstAmount >= thirstThreshold)
		{
			fsm.SetBool (IsDead, true);
		}
		if (thirstAmount < 1)
		{
			fsm.SetBool (IsThirsty, false);
		}
	}
	private void GetHungry ()
	{
		hungerAmount += Time.deltaTime;
		if (hungerAmount > 100)
		{
			fsm.SetBool (IsHungry, true);
			fsm.SetBool (IsWandering, false);
		}
		else
		{
			fsm.SetBool (IsWandering, true);
		}
		if (hungerAmount >= hungerThreshold)
		{
			fsm.SetBool (IsDead, true);
		}

		if (hungerAmount < 1)
		{
			fsm.SetBool (IsHungry, false);
		}
	}
}
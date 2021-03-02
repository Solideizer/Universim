using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
	public class StateManager : MonoBehaviour
	{
		public float hungerAmount;
		public float thirstAmount;
		public Animator fsm;
#pragma warning disable 0649
		[SerializeField] private float thirstThreshold = 300f;
		[SerializeField] private float hungerThreshold = 500f;
#pragma warning restore 0649
		private float _criticalThirst;
		private float _criticalHunger;

		private static readonly int IsWandering = Animator.StringToHash ("isWandering");
		private static readonly int IsThirsty = Animator.StringToHash ("isThirsty");
		private static readonly int IsDead = Animator.StringToHash ("isDead");
		private static readonly int IsHungry = Animator.StringToHash ("isHungry");
		private static readonly int IsIdling = Animator.StringToHash ("isIdling");

		private void FixedUpdate ()
		{
			GetThirsty ();
			GetHungry ();
		}
		private void GetThirsty ()
		{
			_criticalThirst = Random.Range (15, 30);
			thirstAmount += Time.deltaTime;
			if (thirstAmount > _criticalThirst)
			{
				fsm.SetBool (IsWandering, false);
				fsm.SetBool (IsIdling, false);
				fsm.SetBool (IsThirsty, true);
			}
			else
			{
				fsm.SetBool (IsWandering, true);
				fsm.SetBool (IsThirsty, false);
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
			_criticalHunger = Random.Range (15, 30);
			hungerAmount += Time.deltaTime * 0.5f;
			if (hungerAmount > _criticalHunger)
			{
				fsm.SetBool (IsWandering, false);
				fsm.SetBool (IsIdling, false);
				fsm.SetBool (IsHungry, true);
			}
			else
			{
				fsm.SetBool (IsHungry, false);
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
}
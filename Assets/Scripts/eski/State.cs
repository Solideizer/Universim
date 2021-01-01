	using UnityEngine;

	public abstract class State
	{
		protected Animal Animal;
		protected StateMachine StateMachine;

		protected State(Animal animal, StateMachine stateMachine)
		{
			this.Animal = animal;
			this.StateMachine = stateMachine;
		}
		
		public virtual void EnterState() { }
		public virtual void ExitState() { }
		public virtual void LogicUpdate() { }
		public virtual void PhysicUpdate() { }
	}

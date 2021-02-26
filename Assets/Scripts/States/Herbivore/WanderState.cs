using UnityEngine;

public class WanderState : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {

    // }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        HerbivoreAI herbivoreAI = animator.gameObject.GetComponent<HerbivoreAI> ();
        herbivoreAI.Wander ();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

}
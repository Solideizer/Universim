using UnityEngine;

public class FindFoodState : StateMachineBehaviour
{
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HerbivoreAI herbivoreAI = animator.gameObject.GetComponent<HerbivoreAI> ();
        herbivoreAI.FindFood ();
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

}
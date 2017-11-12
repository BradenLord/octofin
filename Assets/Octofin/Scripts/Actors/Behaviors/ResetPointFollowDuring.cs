using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPointFollowDuring : StateMachineBehaviour {

    private ActorController controller;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponentInParent<ActorController>();
        controller.SetPointFollow(false, true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.SetPointFollow(true);
    }
}

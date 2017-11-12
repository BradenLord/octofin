using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPointFollowDuring : StateMachineBehaviour {

    private ActorController controller;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        controller = animator.GetComponentInParent<ActorController>();
        controller.SetPointFollow(false);
    }

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        controller.SetPointFollow(true);
	}
}

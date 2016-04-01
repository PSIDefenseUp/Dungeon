using UnityEngine;
using System.Collections;

public class Fire : StateMachineBehaviour {
  public GameObject skill;

  // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
  //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
  //
  //}

  // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
  //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
  //
  //}

  // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    //Vector3 pos = animator.transform.position;
    //Vector3 dir = animator.transform.forward;

    //spawnSkill(pos, dir.normalized);

  }

  // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
  //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
  //
  //}

  // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
  //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
  //
  //}

}

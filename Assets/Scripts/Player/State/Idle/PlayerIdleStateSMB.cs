using BS.Player;
using BS.State;
using UnityEngine;

public class PlayerIdleStateSMB : StateMachineBehaviour
{
    PlayerStateMachine psm;
    PlayerState ps;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (psm == null) // 초기화되지 않았다면 캐싱
        {
            psm = PlayerStateMachine.Instance; // Singleton 사용
        }
        if (ps == null) // 초기화되지 않았다면 캐싱
        {
            ps = FindFirstObjectByType<PlayerState>();
        }

        //psm.ChangeState(psm.IdleState);
        //ps.isUppercuting = false;
        //ps.isBackHandSwinging = false;
        //ps.isChargingPunching = false;
        //animator.SetBool("IsAttacking", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("IsAttacking", false);
        //ps.isUppercuting = false;
        //ps.isBackHandSwing = false;
        //ps.isChargingPunch = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

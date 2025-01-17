using BS.Player;
using BS.State;
using System.Collections;
using UnityEngine;

public class PlayerAttackSMB : StateMachineBehaviour
{
    PlayerStateMachine psm;
    PlayerState ps;
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (psm == null) // 초기화되지 않았다면 캐싱
        {
            psm = PlayerStateMachine.Instance; // Singleton 사용
        }
        if (ps == null) // 초기화되지 않았다면 캐싱
        {
            ps = PlayerState.Instance; // Singleton 사용
            //ps.isAttack = true;
        }
        //Debug.Log("TEST ENTER");
        // Combo 공격이 4번째 모션인 경우
        if (ps.ComboAttackIndex == 4)
        {
            ps.ComboAttackIndex = 1;
        }
        // Combo 공격이 1,2,3번째 모션인 경우
        else
        {
            ps.ComboAttackIndex++;
        }
    }

     //OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ps != null)
        {
            if (ps.isMoving)
            {
                float moveSpeed = 5f;
                if (Input.GetKey(KeyCode.C))
                {
                    ps.inGameMoveSpeed = moveSpeed * 0.5f;
                    psm.ChangeState(psm.WalkState);
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    ps.inGameMoveSpeed = moveSpeed * 2f;
                    psm.ChangeState(psm.SprintState);
                }
                else
                {
                    ps.inGameMoveSpeed = moveSpeed * 0.5f;
                    psm.ChangeState(psm.RunState);
                }
            }
            else if (ps.isBlocking)
            {
                psm.ChangeState(psm.BlockState);
            }
            else if (ps.isUppercut)
            {
                psm.ChangeState(psm.UppercutState);
            }
            else if (ps.isBackHandSwing)
            {
                psm.ChangeState(psm.BackHandSwingState);
            }
            else if (ps.isChargingPunch)
            {
                psm.ChangeState(psm.ChargingPunchState);
            }
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (psm != null)
        {
            //TODO :: 하드코딩
            animator.SetFloat("StateTime", 0.15f);
        }
        if (ps != null) // 초기화되지 않았다면 캐싱
        {
            animator.SetBool("IsAttacking", false);
            //ps.isAttack = false;
            //Debug.Log("TEST Exit");
        }
    }
}

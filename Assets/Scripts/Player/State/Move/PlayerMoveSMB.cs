using BS.Player;
using BS.State;
using UnityEngine;

public class PlayerMoveSMB : StateMachineBehaviour
{
    PlayerStateMachine psm;
    PlayerState ps;
    float time;
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("MOVE ENTER");
        if (psm == null) // 초기화되지 않았다면 캐싱
        {
            psm = PlayerStateMachine.Instance; // Singleton 사용
        }
        if (ps == null) // 초기화되지 않았다면 캐싱
        {
            ps = PlayerState.Instance; // Singleton 사용
        }
        if (ps != null)
        {
            Debug.Log("MOVE STATE ENTER");
            //ps.isAttack = false;
            animator.SetBool("IsAttacking", false);
            animator.SetFloat("StateTime", 0.15f);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if(time >= 0.4f)
        {
            time = 0;
            animator.SetBool("IsAttacking", false);
        }
        if (ps != null)
        {
            //ps.isAttack = false;
            animator.SetFloat("StateTime", 0.15f);
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
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
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (ps != null)
        {
            Debug.Log("MOVE STATEMACHINE ENTER");
            //ps.isAttack = false;
            animator.SetBool("IsAttacking", false);
            animator.SetFloat("StateTime", 0.15f);
        }
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}

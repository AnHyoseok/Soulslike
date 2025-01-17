using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player의 공격 상태를 정의
    /// </summary>
    public class AttackState : BaseState
    {
        #region Variables

        #endregion
        public AttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            //Debug.Log("ATTACK STATE ENTER");
            ResetAndSetTrigger(IsAttack);
            stateMachine.animator.SetBool("IsAttacking", true);
        }
        public override void Update()
        {
            //TODO :: 하드코딩
            stateMachine.animator.SetFloat("StateTime",
                    Mathf.Repeat(stateMachine.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
        }
    }
}
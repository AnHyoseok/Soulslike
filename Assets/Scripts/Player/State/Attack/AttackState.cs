using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player�� ���� ���¸� ����
    /// </summary>
    public class AttackState : BaseState
    {
        #region Variables

        #endregion
        public AttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            ResetAndSetTrigger(IsAttack);
        }
        public override void Update()
        {
            stateMachine.animator.SetFloat("StateTime",
                    Mathf.Repeat(stateMachine.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
        }
    }
}
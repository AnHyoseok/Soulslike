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
            //stateMachine.animator.SetTrigger("Attack");
        }
    }
}
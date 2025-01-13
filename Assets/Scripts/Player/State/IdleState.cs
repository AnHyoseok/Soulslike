using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player�� Idle ���¸� ����
    /// </summary>
    public class IdleState : MoveState
    {
        #region Variables

        #endregion
        public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            ResetAndSetTrigger(IsIdle);
        }

        public override void Exit()
        {

        }
    }
}
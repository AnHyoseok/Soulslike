using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player의 Sprint 상태를 정의
    /// </summary>
    public class SprintState : MoveState
    {
        #region Variables

        #endregion
        public SprintState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            ResetAndSetTrigger(IsSprint);
        }
    }
}
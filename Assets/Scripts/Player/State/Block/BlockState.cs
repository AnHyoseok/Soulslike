using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player의 Idle 상태를 정의
    /// </summary>
    public class BlockState : BaseState
    {
        #region Variables

        #endregion
        public BlockState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            ResetAndSetTrigger(IsBlock);
        }
        public override void Exit()
        {
            if (stateMachine.GetPrevState() != null)
            {
                stateMachine.RestorePrevState();
            }
        }
    }
}

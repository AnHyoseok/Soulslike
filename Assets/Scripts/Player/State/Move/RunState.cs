using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player�� Run ���¸� ����
    /// </summary>
    public class RunState : MoveState
    {
        #region Variables

        #endregion
        public RunState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            ResetAndSetTrigger(IsRun);
        }
    }
}
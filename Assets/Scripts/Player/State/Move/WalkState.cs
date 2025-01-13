using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player�� Walk ���¸� ����
    /// </summary>
    public class WalkState : MoveState
    {
        #region Variables

        #endregion
        public WalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            ResetAndSetTrigger(IsWalk);
        }
    }
}
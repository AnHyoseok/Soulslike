using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player의 Walk 상태를 정의
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

        public override void Update()
        {
            //if (!Input.GetKey(KeyCode.W))
            //{
            //    stateMachine.ChangeState(stateMachine.IdleState);
            //}
        }
    }
}
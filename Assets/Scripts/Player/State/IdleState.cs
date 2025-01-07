using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player의 Idle 상태를 정의
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

        public override void Update()
        {
            //if (Input.GetKey(KeyCode.W))
            //{
            //    stateMachine.ChangeState(stateMachine.MovingState);
            //}
        }
        public override void Exit()
        {

        }
    }
}
using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player�� Idle ���¸� ����
    /// </summary>
    public class BlockState : MoveState
    {
        #region Variables

        #endregion
        public BlockState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            ResetAndSetTrigger(IsBlock);
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
            if (stateMachine.GetPrevState() != null)
            {
                stateMachine.RestorePrevState();
            }
        }
    }
}

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

        public override void Update()
        {
            //if (!Input.GetKey(KeyCode.W))
            //{
            //    stateMachine.ChangeState(stateMachine.IdleState);
            //}
        }
    }
}
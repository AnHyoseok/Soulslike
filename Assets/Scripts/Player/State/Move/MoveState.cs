using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player의 이동 상태를 정의
    /// </summary>
    public class MoveState : BaseState
    {
        #region Variables

        #endregion
        public MoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    }
}
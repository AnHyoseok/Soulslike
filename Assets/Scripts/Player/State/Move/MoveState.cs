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

        // 상태 변경 시 트리거 초기화 및 설정을 위한 공통 메서드
        protected void ResetAndSetTrigger(string triggerToSet)
        {
            // 공통된 트리거 초기화
            stateMachine.animator.ResetTrigger(IsIdle);
            stateMachine.animator.ResetTrigger(IsWalk);
            stateMachine.animator.ResetTrigger(IsRun);
            stateMachine.animator.ResetTrigger(IsSprint);

            // 현재 상태에 맞는 트리거 설정
            stateMachine.animator.SetTrigger(triggerToSet);
        }
    }
}
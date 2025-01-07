using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player의 상태를 관리
    /// </summary>
    public class PlayerStateMachine : MonoBehaviour
    {
        #region Variables

        private BaseState currentState;

        // Animator 및 상태 참조
        public Animator animator;
        public BaseState IdleState { get; private set; }
        public BaseState RunState { get; private set; }
        public BaseState WalkState { get; private set; }
        public BaseState SprintState { get; private set; }
        public BaseState AttackState { get; private set; }
        #endregion

        private void Awake()
        {
            // 상태 초기화
            IdleState = new IdleState(this);
            RunState = new RunState(this);
            WalkState = new WalkState(this);
            SprintState = new SprintState(this);
            AttackState = new AttackState(this);

            // 기본 상태 설정
            ChangeState(IdleState);
        }

        private void Update()
        {
            currentState?.Update();
        }

        public void ChangeState(BaseState newState)
        {
            // 현재 상태와 새 상태가 다를 때만 상태 변경
            if (currentState != newState)
            {
                currentState?.Exit();  // 기존 상태 종료
                currentState = newState;  // 새 상태 설정
                currentState.Enter();  // 새 상태 시작
            }
        }
        public BaseState GetCurrentState()
        {
            return currentState;
        }

        // 애니메이션 이벤트 처리
        public void OnAnimationEvent(string eventName)
        {
            currentState?.HandleAnimationEvent(eventName);
        }
    }
}
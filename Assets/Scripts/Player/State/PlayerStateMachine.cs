using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player의 상태를 관리
    /// </summary>
    // TODO :: 
    public class PlayerStateMachine : Singleton<PlayerStateMachine>
    {
        #region Variables
        private BaseState currentState;
        public BaseState prevState;
        public string currentSkillName;
        public Transform prevTransform;
        // Animator 및 상태 참조
        public Animator animator;
        public BaseState IdleState { get; private set; }
        public BaseState RunState { get; private set; }
        public BaseState WalkState { get; private set; }
        public BaseState SprintState { get; private set; }
        public BaseState AttackState { get; private set; }
        public BaseState BlockState { get; private set; }
        public BaseState UppercutState { get; private set; }
        public BaseState BackHandSwingState { get; private set; }
        public BaseState ChargingPunchState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            // 상태 초기화
            IdleState = new IdleState(this);
            RunState = new RunState(this);
            WalkState = new WalkState(this);
            SprintState = new SprintState(this);
            AttackState = new AttackState(this);
            BlockState = new BlockState(this);
            UppercutState = new UppercutState(this);
            BackHandSwingState = new BackHandSwingState(this);
            ChargingPunchState = new ChargingPunchState(this);

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
            if (currentState != newState || currentState == AttackState)
            {
                //Debug.Log("PREV = " + prevState);
                //Debug.Log("CURR = " + currentState);
                //Debug.Log("NEWW = " + newState);
                prevState = currentState; // 기존 상태는 이전 상태로 저장
                currentState?.Exit();  // 기존 상태 종료
                currentState = newState;  // 새 상태 설정
                currentState.Enter();  // 새 상태 시작
            }
        }
        // 현재 State 반환
        public BaseState GetCurrentState()
        {
            return currentState;
        }
        // 이전 State 반환
        public BaseState GetPrevState()
        {
            return prevState;
        }
        // 이전 상태로 복원
        public void RestorePrevState()
        {
            if (prevState != null)
            {
                ChangeState(prevState);
            }
        }
    }
}
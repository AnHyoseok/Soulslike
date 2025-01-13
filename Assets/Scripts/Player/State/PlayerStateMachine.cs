using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player�� ���¸� ����
    /// </summary>
    // TODO :: 
    public class PlayerStateMachine : Singleton<PlayerStateMachine>
    {
        #region Variables
        private BaseState currentState;
        public BaseState prevState;

        // Animator �� ���� ����
        public Animator animator;
        public BaseState IdleState { get; private set; }
        public BaseState RunState { get; private set; }
        public BaseState WalkState { get; private set; }
        public BaseState SprintState { get; private set; }
        public BaseState AttackState { get; private set; }
        public BaseState BlockState { get; private set; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            // ���� �ʱ�ȭ
            IdleState = new IdleState(this);
            RunState = new RunState(this);
            WalkState = new WalkState(this);
            SprintState = new SprintState(this);
            AttackState = new AttackState(this);
            BlockState = new BlockState(this);

            // �⺻ ���� ����
            ChangeState(IdleState);
        }

        private void Update()
        {
            currentState?.Update();
        }

        public void ChangeState(BaseState newState)
        {
            Debug.Log("PREV = " + prevState);
            Debug.Log("CURR = " + currentState);
            // ���� ���¿� �� ���°� �ٸ� ���� ���� ����
            if (currentState != newState)
            {
                prevState = currentState; // ���� ���´� ���� ���·� ����
                currentState?.Exit();  // ���� ���� ����
                currentState = newState;  // �� ���� ����
                currentState.Enter();  // �� ���� ����
            }

            if(currentState == AttackState)
            {
                prevState = currentState; // ���� ���´� ���� ���·� ����
                currentState?.Exit();  // ���� ���� ����
                currentState = newState;  // �� ���� ����
                currentState.Enter();  // �� ���� ����
            }
        }
        // ���� State ��ȯ
        public BaseState GetCurrentState()
        {
            return currentState;
        }
        // ���� State ��ȯ
        public BaseState GetPrevState()
        {
            return prevState;
        }
        // ���� ���·� ����
        public void RestorePrevState()
        {
            if (prevState != null)
            {
                ChangeState(prevState);
            }
        }
    }
}
using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player�� ���¸� ����
    /// </summary>
    public class PlayerStateMachine : MonoBehaviour
    {
        #region Variables

        private BaseState currentState;

        // Animator �� ���� ����
        public Animator animator;
        public BaseState IdleState { get; private set; }
        public BaseState RunState { get; private set; }
        public BaseState WalkState { get; private set; }
        public BaseState SprintState { get; private set; }
        public BaseState AttackState { get; private set; }
        #endregion

        private void Awake()
        {
            // ���� �ʱ�ȭ
            IdleState = new IdleState(this);
            RunState = new RunState(this);
            WalkState = new WalkState(this);
            SprintState = new SprintState(this);
            AttackState = new AttackState(this);

            // �⺻ ���� ����
            ChangeState(IdleState);
        }

        private void Update()
        {
            currentState?.Update();
        }

        public void ChangeState(BaseState newState)
        {
            // ���� ���¿� �� ���°� �ٸ� ���� ���� ����
            if (currentState != newState)
            {
                currentState?.Exit();  // ���� ���� ����
                currentState = newState;  // �� ���� ����
                currentState.Enter();  // �� ���� ����
            }
        }
        public BaseState GetCurrentState()
        {
            return currentState;
        }

        // �ִϸ��̼� �̺�Ʈ ó��
        public void OnAnimationEvent(string eventName)
        {
            currentState?.HandleAnimationEvent(eventName);
        }
    }
}
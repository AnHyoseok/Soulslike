using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player�� ������ Base
    /// </summary>
    public abstract class BaseState
    {
        #region Variables

        protected PlayerStateMachine stateMachine;
        protected const string IsIdle = "IsIdle";
        protected const string IsWalk = "IsWalk";
        protected const string IsRun = "IsRun";
        protected const string IsSprint = "IsSprint";
        protected const string IsBlock = "IsBlock";
        protected const string IsAttack = "IsAttack";
        #endregion

        public BaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        // ���� ���� �� Ʈ���� �ʱ�ȭ �� ������ ���� ���� �޼���
        protected virtual void ResetAndSetTrigger(string triggerToSet)
        {
            // ����� Ʈ���� �ʱ�ȭ
            stateMachine.animator.ResetTrigger(IsIdle);
            stateMachine.animator.ResetTrigger(IsWalk);
            stateMachine.animator.ResetTrigger(IsRun);
            stateMachine.animator.ResetTrigger(IsSprint);
            stateMachine.animator.ResetTrigger(IsBlock);
            stateMachine.animator.ResetTrigger(IsAttack);

            // ���� ���¿� �´� Ʈ���� ����
            stateMachine.animator.SetTrigger(triggerToSet);
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}
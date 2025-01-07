using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player�� �̵� ���¸� ����
    /// </summary>
    public class MoveState : BaseState
    {
        #region Variables

        #endregion
        public MoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        // ���� ���� �� Ʈ���� �ʱ�ȭ �� ������ ���� ���� �޼���
        protected void ResetAndSetTrigger(string triggerToSet)
        {
            // ����� Ʈ���� �ʱ�ȭ
            stateMachine.animator.ResetTrigger(IsIdle);
            stateMachine.animator.ResetTrigger(IsWalk);
            stateMachine.animator.ResetTrigger(IsRun);
            stateMachine.animator.ResetTrigger(IsSprint);

            // ���� ���¿� �´� Ʈ���� ����
            stateMachine.animator.SetTrigger(triggerToSet);
        }
    }
}
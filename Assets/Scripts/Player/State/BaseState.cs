using BS.Player;
using UnityEngine;

namespace BS.State
{
    /// <summary>
    /// Player의 상태의 Base
    /// </summary>
    public abstract class BaseState
    {
        #region Variables

        protected PlayerStateMachine stateMachine;
        //protected const string IsIdle = "DoIdle";
        protected const string IsWalk = "DoWalk";
        protected const string IsRun = "DoRun";
        protected const string IsSprint = "DoSprint";
        protected const string IsBlock = "DoBlock";
        protected const string IsAttack = "DoAttack";
        protected const string IsUppercut = "IsUppercut";
        protected const string IsBackHandSwing = "IsBackHandSwing";
        protected const string IsChargingPunch = "IsChargingPunch";
        #endregion

        public BaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        // 상태 변경 시 트리거 초기화 및 설정을 위한 공통 메서드
        protected virtual void ResetAndSetTrigger(string triggerToSet)
        {
            // 공통된 트리거 초기화
            //stateMachine.animator.ResetTrigger(IsIdle);
            stateMachine.animator.ResetTrigger(IsWalk);
            stateMachine.animator.ResetTrigger(IsRun);
            stateMachine.animator.ResetTrigger(IsSprint);
            stateMachine.animator.ResetTrigger(IsBlock);
            stateMachine.animator.ResetTrigger(IsAttack);
            stateMachine.animator.ResetTrigger(IsUppercut);
            stateMachine.animator.ResetTrigger(IsBackHandSwing);
            stateMachine.animator.ResetTrigger(IsChargingPunch);

            // 현재 상태에 맞는 트리거 설정
            stateMachine.animator.SetTrigger(triggerToSet);
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update()
        {

        }
    }
}
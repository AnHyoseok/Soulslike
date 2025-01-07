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
        protected const string IsIdle = "IsIdle";
        protected const string IsWalk = "IsWalk";
        protected const string IsRun = "IsRun";
        protected const string IsSprint = "IsSprint";
        #endregion

        public BaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void HandleAnimationEvent(string eventName) { }
    }
}
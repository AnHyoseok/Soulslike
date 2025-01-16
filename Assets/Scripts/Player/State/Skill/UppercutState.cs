using UnityEngine;

namespace BS.State
{
    public class UppercutState : BaseState
    {
        public UppercutState(PlayerStateMachine stateMachine) : base(stateMachine) { }
        public override void Enter()
        {
            ResetAndSetTrigger(IsUppercut);
        }
    }
}

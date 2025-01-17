using UnityEngine;

namespace BS.State
{

    public class BackHandSwingState : BaseState
    {
        public BackHandSwingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            ResetAndSetTrigger(IsBackHandSwing);
        }
    }
}

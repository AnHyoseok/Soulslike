using UnityEngine;
namespace BS.State
{
    public class ChargingPunchState : BaseState
    {
        public ChargingPunchState(PlayerStateMachine stateMachine) : base(stateMachine) { }
        public override void Enter()
        {
            ResetAndSetTrigger(IsChargingPunch);
        }
    }
}

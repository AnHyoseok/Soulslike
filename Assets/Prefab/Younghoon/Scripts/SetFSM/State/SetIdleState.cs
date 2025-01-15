using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetIdleState : ISetState
    {
        private SetProperty property;
        private float idleStartTime;

        public SetIdleState(SetProperty property)
        {
            this.property = property;
        }

        public void Enter()
        {
            property.Agent.isStopped = true;
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_IDLE, true);
            idleStartTime = Time.time; // Idle 상태 진입 시간 저장
        }

        public void Update()
        {
            //float distance = Vector3.Distance(property.Player.position, property.Controller.transform.position);
            //Debug.Log(distance);
            // Idle 상태에서 attackCooldown만큼 대기 후 Attack 상태로 전환
            if (Time.time >= idleStartTime + property.Controller.attackCooldown)
            {
                property.Controller.SetState(new SetAttackState(property));
                return;
            }
        }

        public void Exit()
        {
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_IDLE, false);
        }
    }
}

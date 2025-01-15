using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetChaseState : ISetState
    {
        private SetProperty property;
        private float chaseStartTime;

        public SetChaseState(SetProperty property)
        {
            this.property = property;
        }

        public void Enter()
        {
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_CHASE, true);
            
            property.Agent.isStopped = false;
            chaseStartTime = Time.time;
        }

        public void Update()
        {
            property.Agent.SetDestination(property.Player.position);

            float distance = Vector3.Distance(property.Player.position, property.Controller.transform.position);
            Debug.Log(distance);
            if (Time.time >= chaseStartTime + property.Controller.attackCooldown)
            {
                property.Controller.SetState(new SetAttackState(property));
                return;
            }
        }

        public void Exit()
        {
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_CHASE, false);
            Debug.Log("Boss: Exiting Chase State");
        }

    }
}
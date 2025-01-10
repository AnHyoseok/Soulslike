using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetIdleState : ISetState
    {
        private SetProperty property;

        public SetIdleState(SetProperty property)
        {
            this.property = property;
        }

        public void Enter()
        {
            property.Agent.isStopped = true;
            property.Animator.SetTrigger("Idle");
            Debug.Log("Boss: Entering Idle State");
        }

        public void Update()
        {
            float distance = Vector3.Distance(property.Player.position, property.Controller.transform.position);
            Debug.Log(distance);
            if (distance < property.Controller.ChaseRange)
            {
                property.Controller.SetState(new SetChaseState(property));
            }
        }

        public void Exit()
        {
            property.Animator.ResetTrigger("Idle");
            Debug.Log("Boss: Exiting Idle State");
        }
    }
}
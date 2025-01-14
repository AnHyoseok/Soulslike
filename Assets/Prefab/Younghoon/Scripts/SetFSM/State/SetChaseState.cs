using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetChaseState : ISetState
    {
        private SetProperty property;

        public SetChaseState(SetProperty property)
        {
            this.property = property;
        }

        public void Enter()
        {
            property.Animator.SetTrigger("Chase");
            
            property.Agent.isStopped = false;
            Debug.Log("Boss: Entering Chase State");
        }

        public void Update()
        {
            property.Agent.SetDestination(property.Player.position);

            float distance = Vector3.Distance(property.Player.position, property.Controller.transform.position);
            //if (distance > property.Controller.ChaseRange)
            //{
            //    property.Controller.SetState(new SetIdleState(property));
            //}
            //if (distance < property.Controller.AttackRange)
            //{
            //    property.Controller.SetState(new SetAttackState(property));
            //}
        }

        public void Exit()
        {
            property.Animator.ResetTrigger("Chase");
            Debug.Log("Boss: Exiting Chase State");
        }

    }
}
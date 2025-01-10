using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetAttackState : ISetState
    {

        private SetProperty property;

        public SetAttackState(SetProperty property)
        {
            this.property = property;
        }

        public void Enter()
        {
            property.Agent.isStopped = true;
            property.Animator.SetTrigger("Attack");
            Debug.Log("Boss: Entering Attack State");
        }

        public void Update()
        {
            // 플레이어 공격 로직
            Debug.Log("Boss: Attacking Player!");

            // 일정 조건에서 다시 추격
            float distance = Vector3.Distance(property.Player.position, property.Controller.transform.position);
            if (distance > property.Controller.AttackRange && property.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 3f)
            {
                property.Controller.SetState(new SetChaseState(property));
            }
        }

        public void Exit()
        {
            property.Animator.ResetTrigger("Attack");
            Debug.Log("Boss: Exiting Attack State");
        }
    }
}
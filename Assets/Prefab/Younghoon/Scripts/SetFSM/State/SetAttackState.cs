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
            // �÷��̾� ���� ����
            Debug.Log("Boss: Attacking Player!");

            // ���� ���ǿ��� �ٽ� �߰�
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
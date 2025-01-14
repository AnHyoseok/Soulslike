using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetIdleState : ISetState
    {
        private SetProperty property;
        private float idleDuration = 5f; // Idle ���¿��� ����� �ð�
        private float idleStartTime;

        public SetIdleState(SetProperty property)
        {
            this.property = property;
        }

        public void Enter()
        {
            property.Agent.isStopped = true;
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_IDLE, true);
            idleStartTime = Time.time; // Idle ���� ���� �ð� ����
            Debug.Log("Boss: Entering Idle State");
        }

        public void Update()
        {
            float distance = Vector3.Distance(property.Player.position, property.Controller.transform.position);
            Debug.Log(distance);
            // Idle ���¿��� 5�� ��� �� Attack ���·� ��ȯ
            if (Time.time >= idleStartTime + idleDuration)
            {
                property.Controller.SetState(new SetAttackState(property));
                return;
            }
        }

        public void Exit()
        {
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_IDLE, false);
            Debug.Log("Boss: Exiting Idle State");
        }
    }
}

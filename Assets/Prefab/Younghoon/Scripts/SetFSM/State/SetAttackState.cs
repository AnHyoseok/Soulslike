using BS.Player;
using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetAttackState : ISetState
    {
        private SetProperty property;
        private bool isAttacking;

        private float damage = 5f;

        public SetAttackState(SetProperty property)
        {
            this.property = property;
        }

        public void Enter()
        {
            property.Agent.isStopped = true;
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_ATTACK, true);
            Debug.Log("Boss: Entering Attack State");
        }

        public void Update()
        {

            // �÷��̾���� �Ÿ� ���
            float distance = Vector3.Distance(property.Player.position, property.Controller.transform.position);

            if (!isAttacking)
            {
                // �Ÿ��� ���� ���� ���� ����
                SelectAndPerformAttack(distance);
            }

            if (AttackStateChecker())
            {
                float animTime = property.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                //Debug.Log($"{animTime} �ִϸ��̼� Ÿ��");
                // ������ ������ Idle ���·� ��ȯ
                if (animTime >= 1)
                {
                    property.Controller.SetState(new SetIdleState(property));
                }
            }
        }

        public void Exit()
        {
            ResetAttackState();
            Debug.Log("Boss: Exiting Attack State");
        }

        /// <summary>
        /// �÷��̾���� �Ÿ��� ���� ���� ���� ���� �� ����
        /// </summary>
        private void SelectAndPerformAttack(float distance)
        {
            isAttacking = true;
            if (distance <= property.CloseRange)
            {
                PerformCloseRangeAttack();
            }
            else if (distance <= property.MidRange)
            {
                PerformMidRangeAttack();
            }
            else if (distance <= property.LongRange)
            {
                PerformLongRangeAttack();
            }
            else
            {
                PerformSpecialAttack();
            }
        }

        private void PerformCloseRangeAttack()
        {
            property.Animator.SetTrigger(SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES);
            Debug.Log("Boss: Performing Close Range Attack!" + $"Trigger name = {SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES}");
        }

        private void PerformMidRangeAttack()
        {
            property.Animator.SetTrigger(SetProperty.SET_ANIM_TRIGGER_PULLATTACK);
            Debug.Log("Boss: Performing Mid Range Attack!");
        }

        private void PerformLongRangeAttack()
        {
            property.Animator.SetTrigger(SetProperty.SET_ANIM_TRIGGER_LIGHTNINGMAGIC);
            Debug.Log("Boss: Performing Long Range Attack!");
        }

        private void PerformSpecialAttack()
        {
            property.Animator.SetTrigger(SetProperty.SET_ANIM_TRIGGER_ROAR);
            Debug.Log("Boss: Performing Special Attack!");
        }

        private bool AttackStateChecker()
        {
            if (property.Animator.GetCurrentAnimatorStateInfo(0).IsName(SetProperty.SET_ANIM_TRIGGER_SLASHATTACK) ||
                property.Animator.GetCurrentAnimatorStateInfo(0).IsName(SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES) ||
                property.Animator.GetCurrentAnimatorStateInfo(0).IsName(SetProperty.SET_ANIM_TRIGGER_PULLATTACK) ||
                property.Animator.GetCurrentAnimatorStateInfo(0).IsName(SetProperty.SET_ANIM_TRIGGER_LIGHTNINGMAGIC) ||
                property.Animator.GetCurrentAnimatorStateInfo(0).IsName(SetProperty.SET_ANIM_TRIGGER_ROAR))
            {
                return true;
            }
            return false;
        }

        private void ResetAttackState()
        {
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_ATTACK, false);
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_SLASHATTACK);
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES);
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_PULLATTACK);
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_LIGHTNINGMAGIC);
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_ROAR);
        }
    }
}

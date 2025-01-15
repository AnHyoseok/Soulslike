using BS.Player;
using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetAttackState : ISetState
    {
        private SetProperty property;
        private bool isAttacking;

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

            // 플레이어와의 거리 계산
            float distance = Vector3.Distance(property.Player.position, property.Controller.transform.position);

            if (!isAttacking)
            {
                // 거리에 따라 공격 패턴 선택
                SelectAndPerformAttack(distance);
            }

            if (AttackStateChecker())
            {
                float animTime = property.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                //Debug.Log($"{animTime} 애니메이션 타임");
                // 공격이 끝나면 Idle 상태로 전환
                if (animTime >= 1)
                {
                    property.Controller.SetState(new SetChaseState(property));
                }
            }
        }

        public void Exit()
        {
            ResetAttackState();
            Debug.Log("Boss: Exiting Attack State");
        }

        /// <summary>
        /// 플레이어와의 거리에 따라 공격 패턴 선택 및 수행
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

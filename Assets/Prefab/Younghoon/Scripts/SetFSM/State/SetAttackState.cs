using BS.Player;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetAttackState : ISetState
    {
        private SetProperty property;
        private bool isAttacking;

        private AnimatorStateInfo currentState;

        private string lastAttack = string.Empty; // 마지막 공격을 저장할 변수

        public SetAttackState(SetProperty property)
        {
            this.property = property;
        }

        public void Enter()
        {
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_ATTACK, true);
            property.Agent.isStopped = true;
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

            // 현재 애니메이션 상태 업데이트
            currentState = property.Animator.GetCurrentAnimatorStateInfo(0);

            if (AttackStateChecker())
            {
                // 애니메이션 타임을 체크하여 공격이 끝났다면 상태 전환
                if (currentState.normalizedTime >= 0.9f)
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
            // 해당 거리 범위에 맞는 공격 범위를 리스트에 추가
            var availableAttacks = new List<string>();

            if (distance <= property.CloseRange && property.LastAttackType != SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES)
            {
                availableAttacks.Add(SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES);
            }
            if (distance <= property.MidRange && property.LastAttackType != SetProperty.SET_ANIM_TRIGGER_PULLATTACK)
            {
                availableAttacks.Add(SetProperty.SET_ANIM_TRIGGER_PULLATTACK);
            }
            if (distance <= property.LongRange && property.LastAttackType != SetProperty.SET_ANIM_TRIGGER_LIGHTNINGMAGIC)
            {
                availableAttacks.Add(SetProperty.SET_ANIM_TRIGGER_LIGHTNINGMAGIC);
            }
            if (property.LastAttackType != SetProperty.SET_ANIM_TRIGGER_ROAR) // 특수 공격은 거리와 상관없음
            {
                availableAttacks.Add(SetProperty.SET_ANIM_TRIGGER_ROAR);
            }

            // 만약 선택할 공격이 있다면 랜덤으로 선택
            if (availableAttacks.Count > 0)
            {
                string attackToPerform = availableAttacks[Random.Range(0, availableAttacks.Count)];

                // 선택된 공격을 실행
                switch (attackToPerform)
                {
                    case SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES:
                        PerformCloseRangeAttack();
                        break;
                    case SetProperty.SET_ANIM_TRIGGER_PULLATTACK:
                        PerformMidRangeAttack();
                        break;
                    case SetProperty.SET_ANIM_TRIGGER_LIGHTNINGMAGIC:
                        PerformLongRangeAttack();
                        break;
                    case SetProperty.SET_ANIM_TRIGGER_ROAR:
                        PerformSpecialAttack();
                        break;
                    default:
                        Debug.LogWarning("No attack selected.");
                        break;
                }
                property.LastAttackType = attackToPerform;
            }
        }

        private void PerformCloseRangeAttack()
        {
            TriggerAttackAnimation(SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES);
            Debug.Log("Boss: Performing Close Range Attack!");
        }

        private void PerformMidRangeAttack()
        {
            TriggerAttackAnimation(SetProperty.SET_ANIM_TRIGGER_PULLATTACK);
            Debug.Log("Boss: Performing Mid Range Attack!");
        }

        private void PerformLongRangeAttack()
        {
            TriggerAttackAnimation(SetProperty.SET_ANIM_TRIGGER_LIGHTNINGMAGIC);
            Debug.Log("Boss: Performing Long Range Attack!");
        }

        private void PerformSpecialAttack()
        {
            TriggerAttackAnimation(SetProperty.SET_ANIM_TRIGGER_ROAR);
            Debug.Log("Boss: Performing Special Attack!");
        }

        private void TriggerAttackAnimation(string triggerName)
        {
            property.Animator.SetTrigger(triggerName);
        }

        private bool AttackStateChecker()
        {
            // 여러 애니메이션 트리거를 체크하여 상태를 확인
            return currentState.IsName(SetProperty.SET_ANIM_TRIGGER_SLASHATTACK) ||
                   currentState.IsName(SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES) ||
                   currentState.IsName(SetProperty.SET_ANIM_TRIGGER_PULLATTACK) ||
                   currentState.IsName(SetProperty.SET_ANIM_TRIGGER_LIGHTNINGMAGIC) ||
                   currentState.IsName(SetProperty.SET_ANIM_TRIGGER_ROAR);
        }

        private void ResetAttackState()
        {
            property.LastAttackTime = Time.time;

            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_ATTACK, false);
            ResetAllAttackTriggers();
        }

        private void ResetAllAttackTriggers()
        {
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_SLASHATTACK);
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_SLASHATTACKTHREETIMES);
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_PULLATTACK);
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_LIGHTNINGMAGIC);
            property.Animator.ResetTrigger(SetProperty.SET_ANIM_TRIGGER_ROAR);
        }
    }
}

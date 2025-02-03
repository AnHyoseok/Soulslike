using BS.State;
using UnityEngine;
using System.Collections.Generic;

namespace BS.Player
{
    public class PlayerHitBox : MonoBehaviour
    {
        [Header("Hitbox Settings")]
        public string enemyLayerName = "Enemy"; // 적의 레이어 이름
        public GameObject controller;
        private PlayerState ps;
        private PlayerStateMachine psm;
        private PlayerSkills psk;

        private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>(); // 이미 대미지를 준 적을 추적

        private void Start()
        {
            ps = controller.transform.GetChild(0).GetComponent<PlayerState>();
            psm = PlayerStateMachine.Instance;
            psk = controller.transform.GetChild(0).GetComponent<PlayerSkills>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 적 레이어인지 확인
            if (collision.gameObject.layer == LayerMask.NameToLayer(enemyLayerName)
                //&& psm.animator.GetBool("IsAttacking")
                && ps.isChargingPunching
                )
            {
                var enemyHealth = collision.gameObject.GetComponent<IDamageable>();
                if (enemyHealth != null)
                {
                    // 이미 대미지를 준 적이라면 중복 대미지 방지
                    if (damagedEnemies.Contains(collision.gameObject))
                        return;

                    // 현재 스킬의 대미지 가져오기
                    float currentSkillDamage = GetCurrentSkillDamage();
                    if (currentSkillDamage > 0)
                    {
                        //Vector3 hitPoint = collision.contacts[0].point;
                        //hitPoint.y = 0;
                        //psk.hitPos = hitPoint;
                        //psk.isHit = true;
                        // 적에게 대미지 입힘
                        enemyHealth.TakeDamage((float)currentSkillDamage);

                        // 대미지를 준 적을 기록
                        damagedEnemies.Add(collision.gameObject);

                        // 쿨다운이 끝나거나 일정 시간이 지난 후 다시 공격 가능하도록 설정
                        Invoke(nameof(ResetDamagedEnemies), 0.5f); // 0.5초 후 초기화
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (psm.animator.GetBool("IsAttacking") == false
                && psm.animator.GetBool("IsBackHandSwing") == false
                && psm.animator.GetBool("IsUppercuting") == false
                && psm.animator.GetBool("IsChargingPunch") == false)
            {
                return;
            }
            // 적 레이어인지 확인
            if (other.gameObject.layer == LayerMask.NameToLayer(enemyLayerName))
            {
                var enemyHealth = other.gameObject.GetComponent<IDamageable>();
                if (enemyHealth != null)
                {
                    // 이미 대미지를 준 적이라면 중복 대미지 방지
                    if (damagedEnemies.Contains(other.gameObject))
                        return;

                    // 현재 스킬의 대미지 가져오기
                    float currentSkillDamage = GetCurrentSkillDamage();
                    if (currentSkillDamage > 0)
                    {
                        if (psm.animator.GetBool("IsChargingPunch") == true)
                        {
                            Vector3 hitPoint = other.ClosestPoint(transform.position);
                            hitPoint.y = 0;
                            psk.hitPos = hitPoint;
                            psk.isHit = true;
                        }
                        // 적에게 대미지 입힘
                        enemyHealth.TakeDamage((int)currentSkillDamage);

                        // 대미지를 준 적을 기록
                        damagedEnemies.Add(other.gameObject);

                        // 쿨다운이 끝나거나 일정 시간이 지난 후 다시 공격 가능하도록 설정
                        Invoke(nameof(ResetDamagedEnemies), 0.5f); // 0.5초 후 초기화
                    }
                }
            }
        }

        // 현재 실행 중인 스킬의 대미지를 반환
        private float GetCurrentSkillDamage()
        {
            // PlayerSkillController의 현재 스킬 확인
            if (PlayerSkillController.skillList.TryGetValue(psm.currentSkillName, out var skill))
            {
                return skill.damage;
            }

            // 현재 스킬이 없다면 기본 대미지 반환
            return 10;
        }

        // 공격 대상을 초기화 (쿨다운 이후 재공격 가능)
        private void ResetDamagedEnemies()
        {
            damagedEnemies.Clear();
        }
    }
}

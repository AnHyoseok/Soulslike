using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BS.Enemy.Set
{
    public class MeeleWeapon : MonoBehaviour
    {
        //무기 공격시 상대에게 데미지 입히는 포인트
        [System.Serializable]
        public class AttackPoint
        {
            public float radius;            // 공격 범위의 반지름
            public Vector3 offset;          // 공격 포인트의 오프셋
            public Transform attackRoot;    // 공격 포인트의 기준 Transform

#if UNITY_EDITOR
            public List<Vector3> previousPositions = new List<Vector3>();
#endif
        }

        #region Variables
        //public int damage = 1;          //hit시 데미지 포인트

        public AttackPoint[] attackPoints = new AttackPoint[0];     //공격 포인트

        public ParticleSystem hitParticlePrefab;                    //공격시 타격 이펙트
        public LayerMask targetLayers;                              // 타겟 레이어 설정 (공격 가능한 오브젝트)

        protected GameObject m_Owner;                               // 무기의 소유자 (공격자)

        protected Vector3[] m_PreviousPos = null;                   // 이전 프레임에서의 공격 포인트 위치
        protected Vector3 m_Direction;                              // 무기 이동 방향

        protected bool m_InAttack = false;                          // 공격 중인지 여부

        // 캐싱 배열 (최대 32개의 히트 결과를 저장)
        protected static RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
        protected static Collider[] s_ColliderCache = new Collider[32];
        #endregion

        public void BeginAttack()
        {
            m_PreviousPos = new Vector3[attackPoints.Length];       // 이전 위치 배열 초기화

            // 각 공격 포인트의 초기 위치 저장
            for (int i = 0; i < attackPoints.Length; i++)
            {
                Vector3 worldPos = attackPoints[i].attackRoot.position +
                    attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
                m_PreviousPos[i] = worldPos;

#if UNITY_EDITOR
                // 디버깅용 경로 초기화
                attackPoints[i].previousPositions.Clear();
                attackPoints[i].previousPositions.Add(m_PreviousPos[i]);
#endif
            }

            m_InAttack = true;                                      // 공격 상태 활성화
        }

        public void EndAttack()
        {
            // 공격 종료
            m_InAttack = false;

#if UNITY_EDITOR
            // 디버깅 경로 초기화
            for (int i = 0; i < attackPoints.Length; i++)
            {
                attackPoints[i].previousPositions.Clear();
            }
#endif
        }

        private void CheckDamage(Collider other, AttackPoint apt)
        {
            // 데미지 가능한 대상인지 확인
            //Damageable d = other.GetComponent<Damageable>();
            //if (d == null)
            //{
            //    return;
            //}

            //// 셀프 데미지 방지
            //if (d.gameObject == m_Owner)
            //{
            //    return;
            //}

            //// 타겟 레이어 체크
            //if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            //{
            //    return;
            //}

            //// 데미지 데이터 생성
            //Damageable.DamageMessage data;
            //data.amount = damage;
            //data.damager = this;
            //data.direction = m_Direction.normalized;
            //data.damgeSource = m_Owner.transform.position;
            //data.throwing = ThrowingHit;
            //data.stopCamera = false;

            //// 데미지 적용
            //d.TakeDamage(data);

            //// 타격 이펙트 재생
            //if (hitParticlePrefab != null)
            //{
            //    m_ParticlesPool[m_CurrentParticle].transform.position = apt.attackRoot.transform.position;
            //    m_ParticlesPool[m_CurrentParticle].time = 0;
            //    m_ParticlesPool[m_CurrentParticle].Play();
            //    m_CurrentParticle = (m_CurrentParticle + 1) % PARTICLE_COUNT;
            //}
        }
    }
}
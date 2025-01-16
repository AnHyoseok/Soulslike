using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Demon
{
    public enum DEMON
    {
        Idle,
        Attack01,
        Attack02,
        Attack03,
        Teleport,
        GetDamaged,
        Die
    }
    public abstract class DemonController : MonoBehaviour,IDamageable
    {
        public abstract void NextPhase();
        #region Variables
        [HideInInspector]public DEMON currentState = DEMON.Idle; // 현재 상태
        [HideInInspector]public Animator animator; // 애니메이터

        public float attackRange = 3f; // 공격 범위
        public float[] attackCooldown = new float[3]; //쿨타임
        [HideInInspector]public float[] lastAttackTime = new float[4];
        private List<DEMON> demons = new List<DEMON>() { DEMON.Attack01, DEMON.Attack02 , DEMON.Teleport };
        protected List<DEMON> pesosDemon = new List<DEMON>() { DEMON.Attack01, DEMON.Attack02, DEMON.Teleport };

        protected int index;  //demons의 랜덤 값

        public float maxHealth = 100f; // 최대 체력
        [SerializeField] private float currentHealth; // 현재 체력
        public bool hasRecovered = false; // 회복 실행 여부 플래그
        [SerializeField]private GameObject angryEffect;

        protected DemonPattern pattern;
        private float timer = 1;
        private bool istimer = false;
        #endregion
        private void Start()
        {
            //참조
            animator = GetComponent<Animator>();
            pattern = GetComponent<DemonPattern>();
            currentHealth = maxHealth; // 초기 체력 설정
        }

        private void Update()
        {
            if (currentHealth <= maxHealth * 0.5f && !hasRecovered)
            {
                RecoverHealth();
                istimer = true;
            }
            if(hasRecovered)
            {
                if(istimer)
                {
                    StartCoroutine(DemonCurrentState(true, 2f));
                    istimer=false;
                }
                else
                {
                    StartCoroutine(DemonCurrentState(false));
                }
            }
            StartCoroutine(DemonCurrentState(false));
        }
        public IEnumerator DemonCurrentState(bool istimeing, float time = 0)
        {
            if (istimeing)
            {
                yield return new WaitForSeconds(time);
                switch (currentState)
                {
                    case DEMON.Idle:
                        HandleIdleState();
                        break;
                    //랜덤 위치에서 볼 생성 후 터진다
                    case DEMON.Attack01:
                        ChangeState(DEMON.Idle);
                        break;
                    // 볼 던진 위치에서 터진다
                    case DEMON.Attack02:
                        ChangeState(DEMON.Idle);
                        break;
                    case DEMON.Attack03:
                        ChangeState(DEMON.Idle);
                        break;
                    case DEMON.Teleport:
                        ChangeState(DEMON.Idle);
                        break;
                    case DEMON.GetDamaged:
                        ChangeState(DEMON.Idle);
                        break;
                    case DEMON.Die:
                        HandleDieState();
                        break;
                }
            }
            if (!istimeing)
            {
                switch (currentState)
                {
                    case DEMON.Idle:
                        HandleIdleState();
                        break;
                    //랜덤 위치에서 볼 생성 후 터진다
                    case DEMON.Attack01:
                        ChangeState(DEMON.Idle);
                        break;
                    // 볼 던진 위치에서 터진다
                    case DEMON.Attack02:
                        ChangeState(DEMON.Idle);
                        break;
                    case DEMON.Attack03:
                        ChangeState(DEMON.Idle);
                        break;
                    case DEMON.Teleport:
                        ChangeState(DEMON.Idle);
                        break;
                    case DEMON.GetDamaged:
                        ChangeState(DEMON.Idle);
                        break;
                    case DEMON.Die:
                        HandleDieState();
                        break;
                }
            }
        }
        // 상태 전환
        #region StateMode
        public void ChangeState(DEMON newState)
        {
            if (currentState == newState) return;

            currentState = newState;

            // 상태에 따른 애니메이션 재생
            animator.SetTrigger(newState.ToString());
        }
        public void ChangeFloatState(DEMON newState, float newfloat)
        {
            if (currentState == newState) return;

            currentState = newState;

            // 상태에 따른 애니메이션 재생
            animator.SetFloat(newState.ToString(), newfloat);
        }
        #endregion
        // 상태 처리
        private void HandleIdleState()
        {
            ResetTriggers();
            if(hasRecovered)
            {
                NextPhase();
                return;
            }
            if(Vector3.Distance(transform.position,pattern.player.position) < attackRange)
            {
                ChangeState(DEMON.Attack03);
                return;
            }
            index = Random.Range(0, demons.Count);
            if (Time.time - lastAttackTime[index] >= attackCooldown[index] &&
                Vector3.Distance(transform.position, pattern.player.position) > attackRange)
            {
                switch (index)
                {
                    case 0:
                        ChangeState(demons[0]);
                        break;
                    case 1:
                        ChangeState(demons[1]);
                        break;
                    case 2:
                        ChangeState(demons[2]);
                        break;
                }
                return;
            }
            else
            {
                ChangeState(DEMON.Idle);
            }
        }
        private void HandleDieState()
        {
            // 사망 처리 (애니메이션 완료 후 파괴)
            Destroy(gameObject, 2f);
        }
        // 애니메이션 초기화
        public void ResetTriggers()
        {
            animator.ResetTrigger("Attack01");
            animator.ResetTrigger("Attack02");
            animator.ResetTrigger("Attack03");
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Teleport");
            animator.SetFloat("GetDamaged", 0);
        }
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력 범위 제한
            Debug.Log($"currentHealth : {currentHealth}");
            ChangeFloatState(DEMON.GetDamaged,damage);
            if (currentHealth <= 0)
            {
                ChangeState(DEMON.Die);
            }
        }
        private void RecoverHealth()
        {
            hasRecovered = true; // 회복 플래그 활성화
            animator.SetBool("IsRecovered", hasRecovered);
            //회복 이펙트
            GameObject heal = Instantiate(pattern.effect[3], transform.position, Quaternion.identity);
            angryEffect.SetActive(true);
            // 감소한 체력의 절반만큼 회복
            float healthToRecover = (maxHealth * 0.5f - currentHealth) * 0.5f;
            currentHealth += healthToRecover;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력 범위 제한
            Destroy(heal, 2f);
        }
    }
}
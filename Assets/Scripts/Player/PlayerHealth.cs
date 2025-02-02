using BS.PlayerInput;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BS.Player
{
    public class PlayerHealth : PlayerController
    {
        #region Variables
        // 애니메이션 파라미터 이름 상수
        private static readonly string IS_BLOCKING = "IsBlocking";
        private static readonly string IS_ATTACKING = "IsAttacking";
        private static readonly string DO_BLOCK = "DoBlock";

        // 블록 관련 변수
        public float blockCoolTime = 3f; // 블록 쿨타임 (기본값: 3초)
        public TextMeshProUGUI blockCoolTimeText; // 블록 쿨타임 텍스트 표시

        // 최대 체력
        [SerializeField] private float maxHealth;
        public float MaxHealth
        {
            get { return maxHealth; }
            private set { maxHealth = value; }
        }

        // 현재 체력
        [SerializeField] private float currentHealth;
        public float CurrentHealth
        {
            get { return currentHealth; }
            private set
            {
                currentHealth = value;

                // 체력이 0 이하가 되면 죽음 처리
                if (currentHealth <= 0)
                {
                    IsDeath = true;
                }
            }
        }

        // 죽음 여부
        private bool isDeath = false;
        public bool IsDeath
        {
            get { return isDeath; }
            private set
            {
                isDeath = value;
                // 애니메이션 설정
                // animator.SetBool(AnimationString.IsDeath, value);
            }
        }

        // 이벤트 액션
        public UnityAction<float> OnDamaged;      // 데미지를 받을 때 호출되는 이벤트
        public UnityAction OnBlocked;            // 블록 성공 시 호출되는 이벤트

        public int potionCount = 3;
        public float healthHealAmount = 1000f;
        #endregion

        protected override void Awake()
        {
            m_Input = transform.parent.GetComponent<PlayerInputActions>();

            // 스킬 목록에 블록 스킬 추가
            if (!PlayerSkillController.skillList.ContainsKey("R"))
            {
                PlayerSkillController.skillList.Add("R", new Skill("Block", blockCoolTime, DoBlock));
            }
        }

        private void OnEnable()
        {
            OnDamaged += CalculateDamage; // 데미지 이벤트 구독
        }

        private void OnDisable()
        {
            OnDamaged -= CalculateDamage; // 데미지 이벤트 구독 해제
        }

        protected override void Start()
        {
            base.Start();
            maxHealth = 1000f; // 초기 최대 체력 설정
            currentHealth = MaxHealth; // 현재 체력을 최대 체력으로 초기화
        }

        // 블록 수행 메서드
        public void DoBlock()
        {
            if (!ps.isBlockable) return;

            if (!animator.GetBool(IS_BLOCKING))
            {
                ps.isDashable = false; // 대시 불가 설정
                RotatePlayer(); // 플레이어 회전
                ps.targetPosition = transform.position; // 블록 중 이동 방지
                animator.SetBool(IS_BLOCKING, true);
                animator.SetTrigger(DO_BLOCK);
            }
        }

        // 블록 시작 처리
        public void OnBlock()
        {
            ps.isBlocking = true;
        }

        // 블록 종료 처리
        public void UnBlock()
        {
            ps.isBlocking = false;
        }

        // 플레이어 회전 처리
        protected override void RotatePlayer()
        {
            if (!ps.isUppercuting && !ps.isBackHandSwinging && !ps.isChargingPunching && !animator.GetBool(IS_ATTACKING))
            {
                transform.parent.DOKill(complete: false); // 트랜스폼 관련 모든 트윈 제거 (완료 콜백 실행 안 함)

                // 목표 회전값 계산
                Vector3 direction = (GetMousePosition() - transform.parent.position).normalized;
                direction = new Vector3(direction.x, 0, direction.z);
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.parent.DORotateQuaternion(targetRotation, rotationDuration).SetLink(gameObject); // 회전 애니메이션 실행
            }
        }

        // 최대 체력 설정
        public void SetMaxHealth(float amount)
        {
            maxHealth = amount;
            CurrentHealth = maxHealth;
        }

        // 데미지 처리 (데미지 값, 블록 가능 여부)
        public bool TakeDamage(float damage, bool isBlockable = true)
        {
            if (isBlockable)
            {
                if (ps.isBlocking) // 블록 성공
                {
                    OnBlocked?.Invoke();
                    return true;
                }
                else // 블록 실패
                {
                    OnDamaged?.Invoke(damage);
                    return false;
                }
            }
            else // 블록 불가 상황
            {
                OnDamaged?.Invoke(damage);
                return false;
            }
        }

        // 데미지 계산 메서드
        public void CalculateDamage(float damage)
        {
            float realDamage = Mathf.Min(CurrentHealth, damage); // 실제 데미지 계산

            CurrentHealth -= realDamage; // 체력 감소

            if (CurrentHealth <= 0f) // 체력이 0 이하일 경우
            {
                CurrentHealth = 0;
                //Die(); // 사망 처리 호출 가능
            }

            Debug.Log($"Player OnDamaged = {damage}");
            Debug.Log($"Player Hp = {CurrentHealth}");
        }

        //포션 사용
        public void UsePotion()
        {
            if (potionCount > 0)
            {
                CurrentHealth += healthHealAmount;
                if (CurrentHealth > maxHealth)
                {
                    CurrentHealth = maxHealth;
                }
                potionCount--;
                Debug.Log("포션사용");
            }
            else
            {
                Debug.Log("포션이 없습니다");
            }
        }
    }
}

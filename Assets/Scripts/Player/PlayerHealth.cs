using BS.State;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BS.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        #region Variables
        // Block
        public float blockCoolTime = 3f;

        public TextMeshProUGUI blockCoolTimeText;

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

                //죽음 처리
                if (currentHealth <= 0)
                {
                    IsDeath = true;
                }
            }
        }
        // 죽음여부
        private bool isDeath = false;
        public bool IsDeath
        {
            get { return isDeath; }
            private set
            {
                isDeath = value;
                //애니메이션
                //animator.SetBool(AnimationString.IsDeath, value);
            }
        }

        // State
        PlayerState ps;
        PlayerStateMachine psm;

        // Action
        public UnityAction OnDamaged;                // 데미지를 받을 때 호출하는 이벤트
        public UnityAction OnBlocked;                // 블록 성공할 때 호출하는 이벤트
        #endregion
        void Start()
        {
            ps = PlayerState.Instance;
            psm = PlayerStateMachine.Instance;
            //playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();
            //playerStateMachine.animator = transform.GetChild(0).GetComponent<Animator>();
            PlayerSkillController.skillList.Add(KeyCode.R, ("Block", blockCoolTime, DoBlock));

            maxHealth = 1000f;
            currentHealth = MaxHealth;
        }

        // Update is called once per frame
        void Update()
        {

        }
        // 블락 쿨타임
        IEnumerator CoBlockCooldown()
        {
            ps.currentBlockCoolTime = blockCoolTime;
            while (ps.currentBlockCoolTime > 0f)
            {
                ps.currentBlockCoolTime -= Time.deltaTime;
                blockCoolTimeText.text = Mathf.Max(0, ps.currentBlockCoolTime).ToString("F1");
                yield return null;
            }
        }
        public void DoBlock()
        {
            ps.isBlockingAnim = true;
            ps.targetPosition = transform.position;
            Invoke(nameof(SetIsBlockingAnim), 1f);
            psm.ChangeState(psm.BlockState);
            StartCoroutine(CoBlockCooldown());
        }
        void SetIsBlockingAnim()
        {
            ps.isBlockingAnim = false;
        }
        public void OnBlock()
        {
            ps.isBlocking = true;
        }
        public void UnBlock()
        {
            ps.isBlocking = false;
        }

        // 최대체력 세팅
        public void SetMaxHealth(float amount)
        {
            maxHealth = amount;
            CurrentHealth = maxHealth;
        }

        // 데미지 받는 함수 (데미지 값, 블락가능여부)
        // 반환 값은 Block 성공 여부
        // TODO :: OnDamaged에 변수 담아서 CalculateDamage 추가하기
        public bool TakeDamage(float damage, bool isBlockable)
        {
            // 블락 가능한 경우
            if (isBlockable)
            {
                // 블락 성공
                if (ps.isBlocking)
                {
                    OnBlocked?.Invoke();
                    return true;
                }
                // 블락 실패
                else
                {
                    CalculateDamage(damage);
                    OnDamaged?.Invoke();
                    return false;
                }
            }
            // 블락 불가능한 경우
            else
            {
                CalculateDamage(damage);
                OnDamaged?.Invoke();
                return false;
            }
        }

        // 데미지 계산
        public void CalculateDamage(float damage)
        {
            // 실질적으로 들어온 데미지 계산 및 유효성 검사
            float realDamage = Mathf.Min(CurrentHealth, damage);

            // 체력 감소
            CurrentHealth -= realDamage;

            // 체력이 0 이하라면 사망 처리
            if (CurrentHealth <= 0f)
            {
                CurrentHealth = 0;
                //Die();
            }
        }
    }
}

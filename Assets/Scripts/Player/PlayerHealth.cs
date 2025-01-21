using BS.State;
using DG.Tweening;
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
        public Camera mainCamera;                           // Camera 변수
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
        public float rotationDuration = 0.1f;               // 회전 지속 시간
        // Action
        public UnityAction<float> OnDamaged;        // 데미지를 받을 때 호출하는 이벤트
        public UnityAction OnBlocked;                // 블록 성공할 때 호출하는 이벤트
        #endregion

        private void Awake()
        {
            PlayerSkillController.skillList.Add("R", new Skill("Block", blockCoolTime, DoBlock));
        }

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            
            psm = PlayerStateMachine.Instance;
            ps = FindFirstObjectByType<PlayerState>();
            //playerStateMachine.animator = transform.GetChild(0).GetComponent<Animator>();
            OnDamaged += CalculateDamage;
            maxHealth = 1000f;
            currentHealth = MaxHealth;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void DoBlock()
        {
            if (!ps.isBlockable) return;

            ps.isBlockingAnim = true;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.CompareTag("Ground"))
                {
                    ps.targetPosition = hit.point;
                    ps.isUppercuting = false;
                    ps.isBackHandSwinging = false;
                    ps.isChargingPunching = false;
                    psm.animator.SetBool("IsBlocking", true);
                }
            }
            RotatePlayer();
            Invoke(nameof(SetIsBlockingAnim), 1f);
            psm.animator.SetTrigger("DoBlock");
            //StartCoroutine(CoBlockCooldown());
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
            psm.animator.SetBool("IsBlocking", false);
        }

        // 최대체력 세팅
        public void SetMaxHealth(float amount)
        {
            maxHealth = amount;
            CurrentHealth = maxHealth;
        }

        // 데미지 받는 함수 (데미지 값, 블락가능여부:기본값은 블락이 가능하도록 설정)
        // 반환 값은 Block 성공 여부
        public bool TakeDamage(float damage, bool isBlockable = true)
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
                    OnDamaged?.Invoke(damage);
                    return false;
                }
            }
            // 블락 불가능한 경우
            else
            {
                OnDamaged?.Invoke(damage);
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
            Debug.Log("Player OnDamaged = " + damage);
            Debug.Log("Player Hp = " + CurrentHealth);
        }
        // DoTween 회전 처리
        void RotatePlayer()
        {
            transform.parent.transform.DOKill(complete: false); // 트랜스폼과 관련된 모든 트윈 제거 (완료 콜백은 실행되지 않음)

            // 목표 회전값 계산
            Vector3 direction = (ps.targetPosition - transform.parent.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.parent.transform.DORotateQuaternion(targetRotation, rotationDuration)
                        .SetAutoKill(true)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {
                            ps.targetPosition = transform.position;
                        });
        }
    }
}

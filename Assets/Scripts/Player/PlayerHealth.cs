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

        // �ִ� ü��
        [SerializeField] private float maxHealth;
        public float MaxHealth
        {
            get { return maxHealth; }
            private set { maxHealth = value; }
        }
        // ���� ü��
        [SerializeField] private float currentHealth;
        public float CurrentHealth
        {
            get { return currentHealth; }
            private set
            {
                currentHealth = value;

                //���� ó��
                if (currentHealth <= 0)
                {
                    IsDeath = true;
                }
            }
        }
        // ��������
        private bool isDeath = false;
        public bool IsDeath
        {
            get { return isDeath; }
            private set
            {
                isDeath = value;
                //�ִϸ��̼�
                //animator.SetBool(AnimationString.IsDeath, value);
            }
        }

        // State
        PlayerState ps;
        PlayerStateMachine psm;

        // Action
        public UnityAction OnDamaged;                // �������� ���� �� ȣ���ϴ� �̺�Ʈ
        public UnityAction OnBlocked;                // ��� ������ �� ȣ���ϴ� �̺�Ʈ
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
        // ��� ��Ÿ��
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

        // �ִ�ü�� ����
        public void SetMaxHealth(float amount)
        {
            maxHealth = amount;
            CurrentHealth = maxHealth;
        }

        // ������ �޴� �Լ� (������ ��, ������ɿ���)
        // ��ȯ ���� Block ���� ����
        // TODO :: OnDamaged�� ���� ��Ƽ� CalculateDamage �߰��ϱ�
        public bool TakeDamage(float damage, bool isBlockable)
        {
            // ��� ������ ���
            if (isBlockable)
            {
                // ��� ����
                if (ps.isBlocking)
                {
                    OnBlocked?.Invoke();
                    return true;
                }
                // ��� ����
                else
                {
                    CalculateDamage(damage);
                    OnDamaged?.Invoke();
                    return false;
                }
            }
            // ��� �Ұ����� ���
            else
            {
                CalculateDamage(damage);
                OnDamaged?.Invoke();
                return false;
            }
        }

        // ������ ���
        public void CalculateDamage(float damage)
        {
            // ���������� ���� ������ ��� �� ��ȿ�� �˻�
            float realDamage = Mathf.Min(CurrentHealth, damage);

            // ü�� ����
            CurrentHealth -= realDamage;

            // ü���� 0 ���϶�� ��� ó��
            if (CurrentHealth <= 0f)
            {
                CurrentHealth = 0;
                //Die();
            }
        }
    }
}

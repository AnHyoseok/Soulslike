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
    public class DemonController : MonoBehaviour
    {
        #region Variables
        public DEMON currentState = DEMON.Idle; // ���� ����
        [HideInInspector]public Animator animator; // �ִϸ�����

        public float attackRange = 3f; // ���� ����
        public float[] attackCooldown = new float[3]; //��Ÿ��
        [HideInInspector]public float[] lastAttackTime = new float[4];
        private List<DEMON> demons = new List<DEMON>() { DEMON.Attack01, DEMON.Attack02 , DEMON.Teleport };

        private int index;  //demons�� ���� ��

        public float maxHealth = 100f; // �ִ� ü��
        [SerializeField] private float currentHealth; // ���� ü��
        private bool hasRecovered = false; // ȸ�� ���� ���� �÷���

        private DemonPattern pattern;
        #endregion
        private void Start()
        {
            //����
            animator = GetComponent<Animator>();
            pattern = GetComponent<DemonPattern>();
            currentHealth = maxHealth; // �ʱ� ü�� ����
        }

        private void Update()
        {
            if (currentHealth <= maxHealth * 0.5f && !hasRecovered)
            {
                RecoverHealth();
            }
            DemonCurrentState();
        }
        public void DemonCurrentState()
        {
            switch (currentState)
            {
                case DEMON.Idle:
                    HandleIdleState();
                    break;
                    //���� ��ġ���� �� ���� �� ������
                case DEMON.Attack01:
                    ChangeState(DEMON.Idle);
                    break;
                    // �� ���� ��ġ���� ������
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
                    HandleGetDamagedState();
                    break;
                case DEMON.Die:
                    HandleDieState();
                    break;
            }
        }
        // ���� ��ȯ �޼���
        public void ChangeState(DEMON newState)
        {
            if (currentState == newState) return;

            currentState = newState;

            // ���¿� ���� �ִϸ��̼� ���
            animator.SetTrigger(newState.ToString());
        }
        public void ChangeFloatState(DEMON newState, float newfloat)
        {
            if (currentState == newState) return;

            currentState = newState;

            // ���¿� ���� �ִϸ��̼� ���
            animator.SetFloat(newState.ToString(), newfloat);
        }
        // ���� ó�� �޼����
        private void HandleIdleState()
        {
            ResetTriggers();
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
        
        private void HandleGetDamagedState()
        {
            // ���� �ð� �� �ٽ� Idle ���·� ��ȯ
            Invoke(nameof(ResetToIdle), 1f);
        }

        private void HandleDieState()
        {
            // ��� ó�� (�ִϸ��̼� �Ϸ� �� �ı�)
            Destroy(gameObject, 2f);
        }

        private void ResetToIdle()
        {
            if (currentState == DEMON.GetDamaged)
            {
                ChangeState(DEMON.Idle);
            }
        }

        // �ִϸ��̼� �ʱ�ȭ
        public void ResetTriggers()
        {
            animator.ResetTrigger("Attack01");
            animator.ResetTrigger("Attack02");
            animator.ResetTrigger("Attack03");
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Teleport");
        }
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ü�� ���� ����
            ChangeFloatState(DEMON.GetDamaged,damage);
            if (currentHealth <= 0)
            {
                ChangeState(DEMON.Die);
            }
        }
        private void RecoverHealth()
        {
            hasRecovered = true; // ȸ�� �÷��� Ȱ��ȭ
            animator.SetBool("IsRecovered", hasRecovered);
            //ȸ�� ����Ʈ
            GameObject heal = Instantiate(pattern.effect[3], transform.position,Quaternion.identity);
            // ������ ü���� ���ݸ�ŭ ȸ��
            float healthToRecover = (maxHealth * 0.5f - currentHealth) * 0.5f;
            currentHealth += healthToRecover;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ü�� ���� ����
            Destroy(heal,2f);
        }
    }
}
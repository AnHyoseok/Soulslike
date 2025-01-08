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
        GetDamaged,
        Die
    }
    public class DemonController : MonoBehaviour
    {
        public DEMON currentState = DEMON.Idle; // ���� ����
        private Animator animator; // �ִϸ�����
        public Transform player; // �÷��̾� ����
        public float attackRange = 3f; // ���� ����
        public float moveSpeed = 2f; // �̵� �ӵ�
        public float[] attackCooldown; //��Ÿ��
        public float teleportCooldown = 3f; // �ڷ���Ʈ ��Ÿ��
        public Transform[] teleportPoints; // �ڷ���Ʈ ������ ������
        public float health = 100f;     //ü��
        public GameObject[] effect;
        private float currentTime;
        private float[] lastAttackTime = new float[3];
        private float lastTeleportTime = -Mathf.Infinity; // ������ �ڷ���Ʈ �ð�
        private List<DEMON> demons = new List<DEMON>() { DEMON.Attack01, DEMON.Attack02, DEMON.Attack03 };

        private int index = 0;
        private void Start()
        {
            //����
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            switch (currentState)
            {
                case DEMON.Idle:
                    HandleIdleState();
                    break;
                case DEMON.Attack01:
                    lastAttackTime[0] = Time.time;
                    HandleAttackStateOne();
                    break;
                case DEMON.Attack02:
                    lastAttackTime[1] = Time.time;
                    HandleAttackStateTwo();
                    break;
                case DEMON.Attack03:
                    lastAttackTime[2] = Time.time;
                    HandleAttackStateThree();
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

        // ���� ó�� �޼����
        private void HandleIdleState()
        {
            if(index == 3)
            {
                index = 0;
            }
            if (Time.time - lastAttackTime[index] >= attackCooldown[index])
            {
                Debug.Log("sss");
                ChangeState(demons[index]); // ���� ���·� ��ȯ
                return;
            }
            else
            {
                ChangeState(DEMON.Idle);
            }
        }

        private void HandleAttackStateOne()
        {
            if (index > 0) return;
            DemonPattern pattern = GetComponent<DemonPattern>();
            pattern.SpawnObjects();
            if(index == 0)
            {
                index++;
                ChangeState(DEMON.Idle);
            }

        }
        private void HandleAttackStateTwo()
        {
            if (index > 1) return;
            // ���� ��Ÿ�� Ȯ��
            PerformAttack("Two");
            if (index == 1)
            {
                index++;
                ChangeState(DEMON.Idle);
            }
        }
        private void HandleAttackStateThree()
        {
            if (index > 2) return;
            // ���� ��Ÿ�� Ȯ��
            PerformAttack("three");
            if (index == 2)
            {
                index++;
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

        // ü�� ����
        public void TakeDamage(int damage)
        {
            if (currentState == DEMON.Die) return; // �̹� ��� ���¸� ����

            health -= damage;
            if (health <= 0)
            {
                ChangeState(DEMON.Die); // ��� ���·� ��ȯ
            }
            else
            {
                ChangeState(DEMON.GetDamaged); // �ǰ� ���·� ��ȯ
            }
        }
        private void PerformAttack(string name)
        {
            // ���� ���� (��: �÷��̾�� ������ ����)
            Debug.Log(name);

            // �ִϸ��̼� ����
            animator.SetTrigger("Attack01");
        }

        private void PerformTeleport()
        {
            // �ڷ���Ʈ ������ ���� �� �ϳ��� �̵�
            if (teleportPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, teleportPoints.Length);
                transform.position = teleportPoints[randomIndex].position;

                /*GameObject effgo = Instantiate(effect[5], transform.position, Quaternion.identity);
                Destroy(effgo );*/
                Debug.Log("Teleported!");
            }

            // �ڷ���Ʈ �ִϸ��̼� ���� (���� ����)
            animator.SetTrigger("Teleport");
        }
    }


}
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
        GetDamaged,
        Die
    }
    public class DemonController : MonoBehaviour
    {
        public DEMON currentState = DEMON.Idle; // ���� ����
        [HideInInspector]public Animator animator; // �ִϸ�����
        public Transform player; // �÷��̾� ����
        public float attackRange = 3f; // ���� ����
        public float moveSpeed = 2f; // �̵� �ӵ�
        public float[] attackCooldown; //��Ÿ��
        public float teleportCooldown; // �ڷ���Ʈ ��Ÿ��
        public Transform[] teleportPoints; // �ڷ���Ʈ ������ ������
        public float health = 100f;     //ü��
        public GameObject[] effect;
        [HideInInspector]public float[] lastAttackTime = new float[3];
        private List<DEMON> demons = new List<DEMON>() { DEMON.Attack01, DEMON.Attack02, DEMON.Attack03 };

        private int index;
        private bool isTel = false;
        private float teleportTimer = 0f;
        private void Start()
        {
            //����
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            teleportTimer += Time.deltaTime;
            switch (currentState)
            {
                case DEMON.Idle:
                    HandleIdleState();
                    break;
                case DEMON.Attack01:
                    HandleAttackState();
                    break;
                case DEMON.Attack02:
                    HandleAttackState();
                    break;
                case DEMON.Attack03:
                    HandleAttackState();
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
            ResetTriggers();
            index = Random.Range(0, demons.Count);
            if (Time.time - lastAttackTime[index] >= attackCooldown[index])
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
            }
            else if(!isTel && teleportTimer >= teleportCooldown)
            {
                PerformTeleport();
                teleportTimer = 0;
                ChangeState(DEMON.Idle);
            }
            else
            {
                ChangeState(DEMON.Idle);
                isTel = false;
            }
        }

        private void HandleAttackState()
        {
            ChangeState(DEMON.Idle);
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
            animator.ResetTrigger("GetDamaged");
        }
        void PerformTeleport()
        {
            isTel = true;
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
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
    public class DemonState : MonoBehaviour
    {
        public DEMON currentState = DEMON.Idle; // ���� ����
        public Animator animator; // �ִϸ�����
        public Transform player; // �÷��̾� ����
        public float attackRange = 3f; // ���� ����
        public float moveSpeed = 2f; // �̵� �ӵ�
        public float attackCooldown = 5f; // ���� ��Ÿ��
        public float teleportCooldown = 3f; // �ڷ���Ʈ ��Ÿ��
        public Transform[] teleportPoints; // �ڷ���Ʈ ������ ������
        public float health = 100f;     //ü��
        public GameObject[] effect;

        private float lastAttackTime = -Mathf.Infinity; // ������ ���� �ð�
        private float lastTeleportTime = -Mathf.Infinity; // ������ �ڷ���Ʈ �ð�

        private void Update()
        {
            switch (currentState)
            {
                case DEMON.Idle:
                    HandleIdleState();
                    break;
                case DEMON.Attack01:
                case DEMON.Attack02:
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
            // �÷��̾���� �Ÿ� üũ
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                ChangeState(DEMON.Attack01); // ���� ���·� ��ȯ
            }
            else
            {
                ChangeState(DEMON.Idle); // �ȱ� ���·� ��ȯ
            }
        }

        private void HandleWalkState()
        {
            // �÷��̾ ���� �̵�
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // ���� ������ �����ϸ� ���� ���·� ��ȯ
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                ChangeState(DEMON.Attack01);
            }
        }

        private void HandleAttackState()
        {
            float currentTime = Time.time;

            // ���� ��Ÿ�� Ȯ��
            if (currentTime >= lastAttackTime + attackCooldown)
            {
                PerformAttack();
                lastAttackTime = currentTime;
            }
            else if (currentTime >= lastTeleportTime + teleportCooldown)
            {
                PerformTeleport();
                lastTeleportTime = currentTime;
            }
            else
            {
                // ��Ÿ�� ���
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
        /*public void TakeDamage(int damage)
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
        }*/
        private void PerformAttack()
        {
            // ���� ���� (��: �÷��̾�� ������ ����)
            Debug.Log("Attack performed!");

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

                GameObject effgo = Instantiate(effect[5], transform.position, Quaternion.identity);
                Debug.Log("Teleported!");
            }

            // �ڷ���Ʈ �ִϸ��̼� ���� (���� ����)
            animator.SetTrigger("Teleport");
        }
    }


}
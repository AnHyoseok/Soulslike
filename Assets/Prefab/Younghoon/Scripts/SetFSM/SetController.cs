using UnityEngine;
using UnityEngine.AI;

namespace BS.Enemy.Set
{
    public class SetController : MonoBehaviour
    {
        private ISetState currentState;
        private SetProperty property;

        [SerializeField] private Transform player;
        [SerializeField] private float rotationSpeed = 2f;
        private Animator animator; // �ִϸ����� �߰�
        private void Start()
        {
            // NavMeshAgent�� Player Transform ����
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            property = new SetProperty(this, animator, agent, player);

            // �ʱ� ���� ����
            SetState(new SetIdleState(property));
        }

        private void Update()
        {
            if (!animator.GetBool(SetProperty.SET_ANIM_BOOL_ATTACK))
            {
                RotateToPlayer();
            }

            currentState?.Update();
        }

        public void SetState(ISetState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        private void RotateToPlayer()
        {
            // ���� ����
            Quaternion currentRotation = property.Controller.transform.rotation;

            // ��ǥ ���� (LookAt ����)
            Vector3 directionToPlayer = (property.Player.position - property.Controller.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // õõ�� ȸ��
            property.Controller.transform.rotation = Quaternion.Slerp(
                currentRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}
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

        public float attackCooldown = 3f;

        private Animator animator; // 애니메이터 추가

        private void Start()
        {
            // NavMeshAgent와 Player Transform 참조
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            property = new SetProperty(this, animator, agent, player);

            // 초기 상태 설정
            SetState(new SetChaseState(property));
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
            // 현재 방향
            Quaternion currentRotation = property.Controller.transform.rotation;

            // 목표 방향 (LookAt 방향)
            Vector3 directionToPlayer = (property.Player.position - property.Controller.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // 천천히 회전
            property.Controller.transform.rotation = Quaternion.Slerp(
                currentRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}
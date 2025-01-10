using UnityEngine;
using UnityEngine.AI;

namespace BS.Enemy.Set
{
    public class SetController : MonoBehaviour
    {
        private ISetState currentState;
        private SetProperty property;

        [SerializeField] private Transform player;
        private Animator animator; // 애니메이터 추가
        public float ChaseRange { get; private set; } = 15f;
        public float AttackRange { get; private set; } = 3f;

        private void Start()
        {
            // NavMeshAgent와 Player Transform 참조
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            property = new SetProperty(this, animator, agent, player);

            // 초기 상태 설정
            SetState(new SetIdleState(property));
        }

        private void Update()
        {
            currentState?.Update();
        }

        public void SetState(ISetState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
using BS.Player;
using UnityEngine;
using UnityEngine.AI;

namespace BS.Enemy.Set
{
    public class SetController : MonoBehaviour
    {
        private ISetState currentState;
        private SetProperty property;
        private Transform player;

        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private float attackCooldown = 3f;

        public float AttackCooldown => attackCooldown; // Read-only Property

        private Animator animator;

        private void Start()
        {
            Initialize();
            SetInitialState();
        }

        private void Update()
        {
            RotateToPlayerIfNotAttacking();
            currentState?.Update();
        }

        private void Initialize()
        {
            // PlayerController를 찾아서 player에 할당
            PlayerController playerController = FindAnyObjectByType<PlayerController>();

            if (playerController != null)
            {
                player = playerController.transform; // PlayerController에서 Transform을 할당
            }
            else
            {
                Debug.LogError("PlayerController를 찾을 수 없습니다.");
            }

            var agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            property = new SetProperty(this, animator, agent, player);
        }

        private void SetInitialState()
        {
            SetState(new SetChaseState(property));
        }

        public void SetState(ISetState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        private void RotateToPlayerIfNotAttacking()
        {
            if (animator.GetBool(SetProperty.SET_ANIM_BOOL_ATTACK))
                return;

            RotateToPlayer();
        }

        private void RotateToPlayer()
        {
            Vector3 directionToPlayer = (property.Player.position - transform.position).normalized;

            if (directionToPlayer == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}

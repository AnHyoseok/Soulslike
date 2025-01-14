using BS.State;
using UnityEngine;

namespace BS.Player
{
    /// <summary>
    /// Player�� ����
    /// </summary>
    public class PlayerState : Singleton<PlayerState>
    {
        #region Variables
        // Idle
        public bool isIdle = false;                         // Idle ����

        // Move
        public Vector3 targetPosition;                      // �̵� ��ǥ ����
        public bool isMovable = true;                       // �̵� ���� ����
        public bool isMoving = false;                       // �̵� ������
        public float inGameMoveSpeed;                       // BD �̵� �ӵ�

        // Sprint
        public bool isSprinting = false;                    // ������Ʈ ������
        // Walk
        public bool isWalking = false;                      // �ȴ� ������

        // Dash
        public bool isDashing = false;                      // �뽬 ������
        public float currentDashCoolTime = 0f;              // BD �뽬 ��Ÿ��
        public bool isInvincible = false;                   // ���� ����

        // Block
        public bool isBlockingAnim = false;                 // ��� �ִϸ��̼� ���࿩��
        public bool isBlocking = false;                     // ��� ������
        public float currentBlockCoolTime = 0f;             // BD ��� ��Ÿ��

        // Attack
        public bool isAttack = false;                       // ���� ������
        public int ComboAttackIndex = 1;                    // ComboAttack �ε��� 
        #endregion
        protected override void Awake()
        {
            base.Awake();
        }
        void Start()
        {

        }

        void Update()
        {
            
        }
    }
}
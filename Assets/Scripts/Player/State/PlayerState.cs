using BS.State;
using UnityEngine;

namespace BS.Player
{
    /// <summary>
    /// Player의 상태
    /// </summary>
    public class PlayerState : Singleton<PlayerState>
    {
        #region Variables
        // Move
        public Vector3 targetPosition;                      // 이동 목표 지점
        public bool isMoving = false;                       // 이동 중인지
        public float inGameMoveSpeed;                       // BD 이동 속도

        // Sprint
        public bool isSprinting = false;                    // 스프린트 중인지
        // Walk
        public bool isWalking = false;                      // 걷는 중인지

        // Dash
        public bool isDashing = false;                      // 대쉬 중인지
        public float currentDashCoolTime = 0f;              // BD 대쉬 쿨타임
        public bool isInvincible = false;                   // 무적 인지

        // Block
        public bool isBlockingAnim = false;                 // 블락 애니메이션 진행여부
        public bool isBlocking = false;                     // 블락 중인지
        public float currentBlockCoolTime = 0f;             // BD 블락 쿨타임
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
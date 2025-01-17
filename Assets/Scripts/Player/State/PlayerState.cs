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
        // Idle
        public bool isIdle = false;                         // Idle 인지

        // Move
        public Vector3 targetPosition;                      // 이동 목표 지점
        public bool isMovable = true;                       // 이동 가능 여부
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
        public bool isDashable = false;                     // 대쉬가 가능한지

        // Block
        public bool isBlockingAnim = false;                 // 블락 애니메이션 진행여부
        public bool isBlocking = false;                     // 블락 중인지
        public float currentBlockCoolTime = 0f;             // BD 블락 쿨타임
        public bool isBlockable = false;                    // 블락 가능한지

        // Attack
        //public bool isAttack = false;                       // 공격 중인지
        public int ComboAttackIndex = 1;                    // ComboAttack 인덱스 

        // 어퍼컷
        public bool isUppercut = false;
        public float currentUppercutCoolTime = 0f;              // BD 어퍼컷 쿨타임
        // 차징펀치
        public bool isChargingPunch = false;
        public float currentChargingPunchCoolTime = 0f;              // BD 차징펀치 쿨타임
        // 백스윙
        public bool isBackHandSwing = false;
        public float currentBackHandSwingCoolTime = 0f;              // BD 백스윙 쿨타임

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
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BS.PlayerInput
{

    public class PlayerInputActions : MonoBehaviour
    {
        #region Variables
        [SerializeField] private InputActionAsset inputActionAsset;     // InputActionAsset 참조
        [NonSerialized] public InputActionMap playerActionMap;          // Player Action Map 참조

        #region PlayerInputActions Hash
        private string hash_ActionMap_Player = "Player";

        private string hash_LeftClick = "LeftClick";
        private string hash_RightClick = "RightClick";
        //private string hash_MousePosition = "MousePosition";
        //private string hash_Q = "Q";
        //private string hash_W = "W";
        //private string hash_E = "E";
        //private string hash_R = "R";
        //private string hash_A = "A";
        //private string hash_S = "S";
        //private string hash_D = "D";
        //private string hash_F = "F";
        //private string hash_C = "C";
        //private string hash_Shift = "Shift";
        //private string hash_Space = "Space";
        #endregion

        public Vector2 MousePosition { get; private set; }              // Mouse Position
        public bool RightClick { get; private set; }                    // 우클릭 여부
        public bool LeftClick { get; private set; }                     // 좌클릭 여부
        public double PrevLeftClickTime { get; private set; }           // 이전 좌클릭 시간
        public double CurrLeftClickTime { get; private set; }           // 현재 좌클릭 시간
        #endregion

        private void Awake()
        {
            // InputActionAsset을 통해 Player Action Map을 가져오기
            if (inputActionAsset != null)
            {
                playerActionMap = inputActionAsset.FindActionMap(hash_ActionMap_Player);
            }
        }

        private void Start()
        {
            PrevLeftClickTime = 0;
            CurrLeftClickTime = 0;
        }

        private void OnEnable()
        {
            // "RightClick" 액션에 대한 performed 및 canceled 이벤트 핸들러 설정
            playerActionMap.FindAction(hash_RightClick).performed += OnRightClickEvent;
            playerActionMap.FindAction(hash_RightClick).canceled += OnRightClickEvent;

            // "LeftClick" 액션에 대한 performed 및 canceled 이벤트 핸들러 설정
            playerActionMap.FindAction(hash_LeftClick).performed += OnLeftClickEvent;
            playerActionMap.FindAction(hash_LeftClick).canceled += OnLeftClickEvent;

            // Action Map 활성화
            playerActionMap?.Enable();
        }

        private void OnDisable()
        {
            // "RightClick" 액션에 대한 이벤트 핸들러 제거
            playerActionMap.FindAction(hash_RightClick).performed -= OnRightClickEvent;
            playerActionMap.FindAction(hash_RightClick).canceled -= OnRightClickEvent;

            // "LeftClick" 액션에 대한 이벤트 핸들러 제거
            playerActionMap.FindAction(hash_LeftClick).performed -= OnLeftClickEvent;
            playerActionMap.FindAction(hash_LeftClick).canceled -= OnLeftClickEvent;

            // Action Map 비활성화
            playerActionMap?.Disable();
        }


        #region NewInput SendMessage
        // RightClick 액션 처리 (performed와 canceled를 동일한 메서드에서 처리)
        private void OnRightClickEvent(InputAction.CallbackContext context)
        {
            RightClick = context.performed;  // performed가 true이면 클릭, canceled면 false
        }

        // LeftClick 액션 처리 (performed와 canceled를 동일한 메서드에서 처리)
        private void OnLeftClickEvent(InputAction.CallbackContext context)
        {
            LeftClick = context.performed;  // performed가 true이면 클릭, canceled면 false
            PrevLeftClickTime = CurrLeftClickTime;
            CurrLeftClickTime = context.time;
        }

        // MousePosition SendMessage
        public void OnMousePosition(InputValue value)
        {
            MousePositionInput(value.Get<Vector2>());
        }
        #endregion

        // Set MousePosition
        public void MousePositionInput(Vector2 newMousePosition)
        {
            MousePosition = newMousePosition;
        }

        // PlayerActionMap 활성화
        public void OnInputActions()
        {
            playerActionMap?.Enable();
        }
        // PlayerActionMap 비활성화
        public void UnInputActions()
        {
            playerActionMap?.Disable();
        }
    }
}
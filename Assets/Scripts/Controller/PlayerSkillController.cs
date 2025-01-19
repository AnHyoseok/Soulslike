//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//
//namespace BS.Player
//{
//    public class PlayerSkillController : MonoBehaviour
//    {
//        #region Variables
//        // Skill
//        // TODO :: 스킬리스트 만들고 Dictionary 관리하자 키코드 : 이름, 쿨타임, 기능함수
//        // 해당 스킬리스트를 시작할때 가져오고 코루틴으로 쿨타임관리
//        // 기능함수 - 끝나고 호출하는 함수 - 쿨타임 관리 함수 형태로 구현
//        // TODO :: CS 파일을 반영가능할까 ?
//        public static Dictionary<KeyCode, (string, float, UnityAction)> skillList = new Dictionary<KeyCode, (string, float, UnityAction)>();
//
//        //state
//        PlayerState ps;
//        #endregion
//        void Start()
//        {
//            ps = FindFirstObjectByType<PlayerState>();
//        }
//
//        // Update is called once per frame
//        void Update()
//        {
//            foreach (var skill in skillList)
//            {
//                if (Input.GetKeyDown(skill.Key))
//                {
//                    ExecuteSkill(skill.Key);
//                }
//            }
//        }
//
//        // 스킬 시전
//        void ExecuteSkill(KeyCode key)
//        {
//            if (skillList.TryGetValue(key, out var skill))
//            {
//                // 현재 스킬에 맞는 쿨타임 변수 선택
//                float currentCoolTime = 0f;
//
//                // 쿨타임을 스킬 이름에 따라 설정
//                switch (skill.Item1)
//                {
//                    case "Dash":
//                        currentCoolTime = ps.currentDashCoolTime;
//                        break;
//
//                    case "Block":
//                        currentCoolTime = ps.currentBlockCoolTime;
//                        break;
//                    case "Uppercut":
//                        currentCoolTime = ps.currentUppercutCoolTime;
//                        break;
//                    case "BackHandSwing":
//                        currentCoolTime = ps.currentBackHandSwingCoolTime;
//                        break;
//                    case "ChargingPunch":
//                        currentCoolTime = ps.currentChargingPunchCoolTime;
//                        break;
//
//                    default:
//                        return; // 처리할 수 없는 스킬일 경우
//                }
//
//                // 쿨타임이 끝났다면 스킬 실행
//                if (currentCoolTime <= 0f)
//                {
//                    skill.Item3?.Invoke(); // 스킬 실행
//                }
//            }
//        }
//    }
//}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;

namespace BS.Player
{
    public struct Skill
    {
        public string Name;          // 스킬 이름
        public float CooldownTime;   // 쿨타임
        public UnityAction Action;   // 스킬 실행 시 호출할 액션

        // Skill 생성자
        public Skill(string name, float cooldownTime, UnityAction action)
        {
            Name = name;
            CooldownTime = cooldownTime;
            Action = action;
        }
    }
    public class PlayerSkillController : PlayerController
    {
        #region Variables
        public static Dictionary<string, Skill> skillList = new Dictionary<string, Skill>();

        // 스킬 쿨타임 UI 요소를 관리할 Dictionary (각 스킬 이름 -> UI 요소)
        public static Dictionary<string, Image> skillCooldownUI = new Dictionary<string, Image>(); // fillAmount를 조정할 UI Image
        public static Dictionary<string, TextMeshProUGUI> skillCooldownTextUI = new Dictionary<string, TextMeshProUGUI>(); // 쿨타임 숫자를 표시할 UI Text

        // 쿨타임을 관리할 딕셔너리 (각 스킬에 대해 코루틴을 관리)
        private Dictionary<string, Coroutine> skillCooldownCoroutines = new Dictionary<string, Coroutine>();

        public Image blockCooldownImage;
        public TextMeshProUGUI blockCooldownText;
        public Image dashCooldownImage;
        public TextMeshProUGUI dashCooldownText;
        #endregion

        protected override void Start()
        {
            base.Start();

            // 스킬에 대한 UI 요소를 초기화 (예시: UI 오브젝트 연결)
            // 대쉬 스킬의 쿨타임 UI 연결
            if(dashCooldownImage != null)
                skillCooldownUI.Add("Dash", dashCooldownImage); // dashCooldownImage는 UI Image 컴포넌트
            if (dashCooldownText != null)
                skillCooldownTextUI.Add("Dash", dashCooldownText); // dashCooldownText는 UI Text 컴포넌트

            // 블록 스킬의 쿨타임 UI 연결
            if (blockCooldownImage != null)
                skillCooldownUI.Add("Block", blockCooldownImage); // blockCooldownImage는 UI Image 컴포넌트
            if (blockCooldownText != null)
                skillCooldownTextUI.Add("Block", blockCooldownText); // blockCooldownText는 UI Text 컴포넌트

            // 각 스킬에 대해 초기 쿨타임 설정
            foreach (var skill in skillList)
            {
                skillCooldownCoroutines[skill.Value.Name] = null; // 초기화
            }
        }

        protected void OnEnable()
        {
            foreach (var skill in skillList.Keys)
            {
                var action = m_Input.playerActionMap.FindAction(skill);
                if (action != null)
                {
                    action.performed += OnSkillInput;
                }
            }
        }

        protected void OnDisable()
        {
            foreach (var skill in skillList.Keys)
            {
                var action = m_Input.playerActionMap.FindAction(skill);
                if (action != null)
                {
                    action.performed -= OnSkillInput;
                }
            }
        }

        // 스킬 입력 처리
        private void OnSkillInput(InputAction.CallbackContext context)
        {
            string key = context.action.name;
            if (skillList.ContainsKey(key))
            {
                ExecuteSkill(key);
            }
        }

        private void ExecuteSkill(string key)
        {
            if (skillList.TryGetValue(key, out var skill))
            {
                // 쿨타임이 끝났는지 확인
                if (skillCooldownCoroutines[skill.Name] == null) // 아직 쿨타임이 돌고 있지 않으면
                {
                    skill.Action?.Invoke(); // 스킬 실행
                    skillCooldownCoroutines[key] = StartCoroutine(CooldownCoroutine(skill.Name, skill.CooldownTime)); // 쿨타임 코루틴 시작
                }
                else
                {
                    //Debug.Log($"{skill.Name} is on cooldown.");
                }
            }
        }

        // 쿨타임을 처리하는 코루틴
        private IEnumerator CooldownCoroutine(string skillName, float cooldownDuration)
        {
            float cooldownTimeLeft = cooldownDuration;

            // 쿨타임 동안 UI 갱신
            while (cooldownTimeLeft > 0f)
            {
                cooldownTimeLeft -= Time.deltaTime;

                // 쿨타임 UI 업데이트 (Image UI를 사용해 네모난 배경에 채워지기)
                if (skillCooldownUI.ContainsKey(skillName))
                {
                    float fillAmount = cooldownTimeLeft / cooldownDuration;
                    skillCooldownUI[skillName].fillAmount = fillAmount; // 네모난 이미지의 진행 상태 표시
                }

                // 쿨타임 숫자 업데이트 (Text UI에 소수점 한자리까지 남은 시간 표시)
                if (skillCooldownTextUI.ContainsKey(skillName))
                {
                    skillCooldownTextUI[skillName].text = $"{Mathf.Max(0, cooldownTimeLeft):F1}"; // 소수점 1자리까지 표시
                }

                yield return null; // 한 프레임 대기
            }

            skillCooldownCoroutines[skillName] = null; // 쿨타임이 끝났으면 코루틴을 null로 설정하여 다시 사용할 수 있도록

            // 쿨타임 끝나면 UI 초기화
            if (skillCooldownUI.ContainsKey(skillName))
            {
                skillCooldownUI[skillName].fillAmount = 1f; // UI의 쿨타임이 끝났으므로 채움
            }

            if (skillCooldownTextUI.ContainsKey(skillName))
            {
                skillCooldownTextUI[skillName].text = "0"; // 쿨타임 끝나면 "Ready"로 표시
            }

            //Debug.Log($"{skillName} is ready to use.");
        }
    }
}

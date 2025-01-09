using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BS.Player
{
    public class PlayerSkillController : MonoBehaviour
    {
        #region Variables
        // Skill
        // TODO :: 스킬리스트 만들고 Dictionary 관리하자 키코드 : 이름, 쿨타임, 기능함수
        // 해당 스킬리스트를 시작할때 가져오고 코루틴으로 쿨타임관리
        // 기능함수 - 끝나고 호출하는 함수 - 쿨타임 관리 함수 형태로 구현
        // TODO :: CS 파일을 반영가능할까 ?
        public static Dictionary<KeyCode, (string, float, UnityAction)> skillList = new Dictionary<KeyCode, (string, float, UnityAction)>();

        //state
        PlayerState ps;
        #endregion
        void Start()
        {
            ps = PlayerState.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var skill in skillList)
            {
                if (Input.GetKeyDown(skill.Key))
                {
                    ExecuteSkill(skill.Key);
                }
            }
        }

        // 스킬 시전
        void ExecuteSkill(KeyCode key)
        {
            if (skillList.TryGetValue(key, out var skill))
            {
                // 현재 스킬에 맞는 쿨타임 변수 선택
                float currentCoolTime = 0f;

                // 쿨타임을 스킬 이름에 따라 설정
                switch (skill.Item1)
                {
                    case "Dash":
                        currentCoolTime = ps.currentDashCoolTime;
                        break;

                    case "Block":
                        currentCoolTime = ps.currentBlockCoolTime;
                        break;

                    default:
                        return; // 처리할 수 없는 스킬일 경우
                }

                // 쿨타임이 끝났다면 스킬 실행
                if (currentCoolTime <= 0f)
                {
                    skill.Item3?.Invoke(); // 스킬 실행
                }
            }
        }
    }
}

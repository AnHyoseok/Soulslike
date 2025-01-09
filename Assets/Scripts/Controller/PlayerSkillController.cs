using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BS.Player
{
    public class PlayerSkillController : MonoBehaviour
    {
        #region Variables
        // Skill
        // TODO :: ��ų����Ʈ ����� Dictionary �������� Ű�ڵ� : �̸�, ��Ÿ��, ����Լ�
        // �ش� ��ų����Ʈ�� �����Ҷ� �������� �ڷ�ƾ���� ��Ÿ�Ӱ���
        // ����Լ� - ������ ȣ���ϴ� �Լ� - ��Ÿ�� ���� �Լ� ���·� ����
        // TODO :: CS ������ �ݿ������ұ� ?
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

        // ��ų ����
        void ExecuteSkill(KeyCode key)
        {
            if (skillList.TryGetValue(key, out var skill))
            {
                // ���� ��ų�� �´� ��Ÿ�� ���� ����
                float currentCoolTime = 0f;

                // ��Ÿ���� ��ų �̸��� ���� ����
                switch (skill.Item1)
                {
                    case "Dash":
                        currentCoolTime = ps.currentDashCoolTime;
                        break;

                    case "Block":
                        currentCoolTime = ps.currentBlockCoolTime;
                        break;

                    default:
                        return; // ó���� �� ���� ��ų�� ���
                }

                // ��Ÿ���� �����ٸ� ��ų ����
                if (currentCoolTime <= 0f)
                {
                    skill.Item3?.Invoke(); // ��ų ����
                }
            }
        }
    }
}

using BS.Player;
using BS.State;
using System.Collections;
using TMPro;
using UnityEngine;

namespace BS.PlayerHealth
{
    public class PlayerHealth : MonoBehaviour
    {
        #region Variables
        // Block
        public float blockCoolTime = 3f;
        
        public TextMeshProUGUI blockCoolTimeText;

        // State
        PlayerState ps;
        PlayerStateMachine playerStateMachine;
        #endregion
        void Start()
        {
            ps = PlayerState.Instance;
            playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();
            //playerStateMachine.animator = transform.GetChild(0).GetComponent<Animator>();
            PlayerSkillController.skillList.Add(KeyCode.R, ("Block", blockCoolTime, DoBlock));
        }

        // Update is called once per frame
        void Update()
        {

        }
        // ºí¶ô ÄðÅ¸ÀÓ
        IEnumerator CoBlockCooldown()
        {
            ps.currentBlockCoolTime = blockCoolTime;
            while (ps.currentBlockCoolTime > 0f)
            {
                ps.currentBlockCoolTime -= Time.deltaTime;
                blockCoolTimeText.text = Mathf.Max(0, ps.currentBlockCoolTime).ToString("F1");
                yield return null;
            }
        }
        public void DoBlock()
        {
            ps.isBlockingAnim = true;
            ps.targetPosition = transform.position;
            Invoke(nameof(SetIsBlockingAnim), 1f);
            playerStateMachine.ChangeState(playerStateMachine.BlockState);
            StartCoroutine(CoBlockCooldown());
        }
        void SetIsBlockingAnim()
        {
            ps.isBlockingAnim = false;
        }
        public void OnBlock()
        {
            ps.isBlocking = true;
        }
        public void UnBlock()
        {
            ps.isBlocking = false;
            playerStateMachine.ChangeState(playerStateMachine.IdleState);
        }
    }
}

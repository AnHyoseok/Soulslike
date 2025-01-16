using BS.State;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace BS.Player
{
    public class PlayerSkills : MonoBehaviour
    {
        // State
        PlayerState ps;
        PlayerStateMachine psm;
        // 어퍼컷
        public float uppercutDuration = 0.1f;                        // 어퍼컷 시간
        public float uppercutCoolTime = 3f;                          // SD 어퍼컷 쿨타임

        // 차징펀치
        public float chargingPunchDuration = 1f;                   // 차징펀치 시간
        public float chargingPunchCoolTime = 3f;                     // SD 차징펀치 쿨타임
        public float chargingPunchDistance = 5f;                     // 차징펀치 거리

        // 백스윙
        public float backHandSwingDuration = 1.5f;                   // 백스윙 시간
        public float backHandSwingCoolTime = 1f;                     // SD 백스윙 쿨타임
        //public float backHandSwingDistance = 5f;                   // 백스윙 거리
        
        public Camera mainCamera;                           // Camera 변수
        public float rotationDuration = 0.1f;               // 회전 지속 시간
        Animator animator;
        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            ps = PlayerState.Instance;
            psm = PlayerStateMachine.Instance;
            animator = GetComponent<Animator>();
            PlayerSkillController.skillList.Add(KeyCode.E, ("Uppercut", uppercutCoolTime, DoUppercut));
            PlayerSkillController.skillList.Add(KeyCode.W, ("ChargingPunch", chargingPunchCoolTime, DoChargingPunch));
            PlayerSkillController.skillList.Add(KeyCode.Q, ("BackHandSwing", backHandSwingCoolTime, DobackHandSwing));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DoUppercut()
        {
            //TODO :: 다른애니메이션이랑 연계되는데 필요한 분기처리
            
            //if (!ps.isDashing && ps.currentDashCoolTime <= 0f && !ps.isBlockingAnim)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        //TODO :: 땅으로 꺼지는현상 
                        //TODO :: 이동중 사용할시 멈췄다가 시전하도록

                        psm.ChangeState(psm.UppercutState);
                        ps.targetPosition = hit.point;
                        RotatePlayer();
                        StartCoroutine(CoUppercutCooldown());
                    }
                }
            }
        }

        public void DoChargingPunch()
        {

        }

        public void DobackHandSwing()
        {

        }
        // 대쉬 쿨타임
        IEnumerator CoUppercutCooldown()
        {
            ps.currentUppercutCoolTime = uppercutCoolTime;
            while (ps.currentUppercutCoolTime > 0f)
            {
                ps.currentUppercutCoolTime -= Time.deltaTime;
                //uppercutCoolTimeText.text = Mathf.Max(0, ps.currentuppercutCoolTime).ToString("F1");
                yield return null;
            }
        }
        // 대쉬 쿨타임
        IEnumerator CoChargingPunchCooldown()
        {
            ps.currentChargingPunchCoolTime = chargingPunchCoolTime;
            while (ps.currentChargingPunchCoolTime > 0f)
            {
                ps.currentChargingPunchCoolTime -= Time.deltaTime;
                //dashCoolTimeText.text = Mathf.Max(0, ps.currentchargingPunchCoolTime).ToString("F1");
                yield return null;
            }
        }
        // 대쉬 쿨타임
        IEnumerator CobackHandSwingCooldown()
        {
            ps.currentBackHandSwingCoolTime = backHandSwingCoolTime;
            while (ps.currentBackHandSwingCoolTime > 0f)
            {
                ps.currentBackHandSwingCoolTime -= Time.deltaTime;
                //dashCoolTimeText.text = Mathf.Max(0, ps.currentbackHandSwingCoolTime).ToString("F1");
                yield return null;
            }
        }

        // DoTween 회전 처리
        void RotatePlayer()
        {
            transform.parent.transform.DOKill(complete: false); // 트랜스폼과 관련된 모든 트윈 제거 (완료 콜백은 실행되지 않음)

            // 목표 회전값 계산
            Vector3 direction = (ps.targetPosition - transform.parent.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.parent.transform.DORotateQuaternion(targetRotation, rotationDuration)
                        .SetAutoKill(true)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {

                        });
        }
        void OnAnimatorMove()
        {
            // Root Motion 데이터를 PlayerController에 반영
            if (animator)
            //&& characterTransform)
            {
                transform.parent.transform.position = animator.rootPosition; // 캐릭터의 Root Motion 위치
            }
        }
    }
}

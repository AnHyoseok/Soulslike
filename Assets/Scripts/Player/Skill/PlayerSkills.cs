using BS.State;
using DG.Tweening;
using System.Collections;
using TMPro;
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

        public TextMeshProUGUI uppercutCoolTimeText;
        public TextMeshProUGUI backHandSwingCoolTimeText;
        public TextMeshProUGUI chargingPunchCoolTimeText;
        Animator animator;
        void Awake()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            ps = FindFirstObjectByType<PlayerState>();
            psm = PlayerStateMachine.Instance;
            animator = GetComponent<Animator>();
            // 스킬 구조체에 맞게 스킬을 추가
            PlayerSkillController.skillList.Add("Q", new Skill("Uppercut", uppercutCoolTime, DoUppercut));
            PlayerSkillController.skillList.Add("W", new Skill("BackHandSwing", backHandSwingCoolTime, DoBackHandSwing));
            PlayerSkillController.skillList.Add("E", new Skill("ChargingPunch", chargingPunchCoolTime, DoChargingPunch));
        }

        // Update is called once per frame
        void Update()
        {

        }

        // 애니메이션 실행중 호출 Update
        void OnAnimatorMove()
        {
            // Root Motion 데이터를 PlayerController에 반영
            if (animator)
            //&& characterTransform)
            {
                transform.parent.transform.position = animator.rootPosition; // 캐릭터의 Root Motion 위치
            }
        }

        public void DoUppercut()
        {
            if (psm.animator.GetBool("IsAttacking")) return;
            if (!ps.isDashing && !ps.isBlockingAnim && !ps.isBackHandSwinging && !ps.isChargingPunching)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        ps.targetPosition = hit.point;
                        RotatePlayer();

                        psm.ChangeState(psm.UppercutState);
                        //StartCoroutine(CoUppercutCooldown());
                    }
                }
                ps.isUppercuting = true;
            }
        }

        public void DoChargingPunch()
        {
            if (psm.animator.GetBool("IsAttacking")) return;
            if (!ps.isDashing && !ps.isBlockingAnim && !ps.isBackHandSwinging && !ps.isUppercuting)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        ps.targetPosition = hit.point;
                        RotatePlayer();

                        psm.ChangeState(psm.ChargingPunchState);
                        //StartCoroutine(CoChargingPunchCooldown());
                    }
                }
                ps.isChargingPunching = true;
            }
        }

        public void DoBackHandSwing()
        {
            if (psm.animator.GetBool("IsAttacking")) return;
            if (!ps.isDashing && !ps.isBlockingAnim && !ps.isUppercuting && !ps.isChargingPunching)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.CompareTag("Ground"))
                    {
                        ps.targetPosition = hit.point;
                        RotatePlayer();

                        psm.ChangeState(psm.BackHandSwingState);
                        //StartCoroutine(CobackHandSwingCooldown());
                    }
                }
                ps.isBackHandSwinging = true;
                Debug.Log("IS TEST1 = " + ps.isBackHandSwinging);
            }
        }

        // DoTween 회전 처리
        void RotatePlayer()
        {
            transform.parent.transform.DOKill(complete: false); // 트랜스폼과 관련된 모든 트윈 제거 (완료 콜백은 실행되지 않음)

            if (ps.isUppercuting || ps.isBackHandSwinging || ps.isChargingPunching) return;

            // 목표 회전값 계산
            Vector3 direction = (ps.targetPosition - transform.parent.transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.parent.transform.DORotateQuaternion(targetRotation, rotationDuration)
                        .SetAutoKill(true)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {

                        });
        }
        // 대쉬 쿨타임
        IEnumerator CoUppercutCooldown()
        {
            ps.currentUppercutCoolTime = uppercutCoolTime;
            while (ps.currentUppercutCoolTime > 0f)
            {
                ps.currentUppercutCoolTime -= Time.deltaTime;
                if (uppercutCoolTimeText != null)
                {
                    uppercutCoolTimeText.text = Mathf.Max(0, ps.currentUppercutCoolTime).ToString("F1");
                }
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
                if (chargingPunchCoolTimeText != null)
                {
                    chargingPunchCoolTimeText.text = Mathf.Max(0, ps.currentChargingPunchCoolTime).ToString("F1");
                }
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
                if (backHandSwingCoolTimeText != null)
                {
                    backHandSwingCoolTimeText.text = Mathf.Max(0, ps.currentBackHandSwingCoolTime).ToString("F1");
                }
                yield return null;
            }
        }




        // 어퍼컷이 끝날때 호출
        public void EndUppercut()
        {
            ps.isUppercuting = false;
            ps.targetPosition = this.transform.position;
        }
        // 백핸드스윙이 끝날때 호출
        public void EndBackHandSwing()
        {
            ps.isBackHandSwinging = false;
            ps.targetPosition = this.transform.position;
        }
        // 차징펀지이 끝날때 호출
        public void EndChargingPunch()
        {
            ps.isChargingPunching = false;
            ps.targetPosition = this.transform.position;
        }

        // 다른행동이 불가능 하도록 설정
        public void CannotOtherAct()
        {
            ps.isDashable = false;
            ps.isBlockable = false;
        }
        // 다른행동이 가능 하도록 설정
        public void CanOtherAct()
        {
            ps.isDashable = true;
            ps.isBlockable = true;
        }
    }
}

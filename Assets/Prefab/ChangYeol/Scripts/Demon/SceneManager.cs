using BS.Player;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BS.Demon
{
    public class SceneManager : MonoBehaviour
    {
        #region Variables
        public DemonPattern pattern;
        public GameObject WarningCanvas;
        public Camera main;
        public GameObject drectingCamera;
        public GameObject bossCanvas;
        public GameObject presentEffect;
        public PlayerController player;

        private bool isPhase = false;
        ToggleRendererFeature toggleRenderer;
        #endregion

        private void Start()
        {
            toggleRenderer = GetComponent<ToggleRendererFeature>();
            toggleRenderer.enabled = false;
            StartCoroutine(OpeningDemon());
        }
        void Update()
        {
            WarningCanvas.transform.LookAt(WarningCanvas.transform.position + main.transform.rotation * Vector3.forward, main.transform.rotation * Vector3.up);
            if (Input.GetKeyDown(KeyCode.V))
            {
                pattern.demon.TakeDamage(5);
            }
            if (pattern.demon.hasRecovered && isPhase == false)
            {
                
                StartCoroutine (PhaseDemon());
                isPhase = true;
            }
        }
        IEnumerator OpeningDemon()
        {
            pattern.demon.enabled = false;
            player.enabled = false;
            pattern.demon.gameObject.SetActive(false);
            GameObject eff = Instantiate(presentEffect, pattern.gameObject.transform.position, presentEffect.transform.rotation);
            yield return new WaitForSeconds(0.2f);
            pattern.demon.gameObject.SetActive(true);
            float descentDuration = 4f; //작아지는 시간
            float elapsedTime = 0f;
            
            yield return new WaitForSeconds(0.5f);
            ToggleRendererFeature toggleRenderer = GetComponent<ToggleRendererFeature>();
            toggleRenderer.enabled = true;
            while (elapsedTime < descentDuration)
            {
                eff.transform.localScale = Vector3.Lerp(eff.transform.localScale, new Vector3(0.1f,0.1f,0.1f)*-0f, elapsedTime / descentDuration);
                elapsedTime += Time.deltaTime;
                // 목표 크기에 도달했는지 확인
                if (Vector3.Distance(transform.localScale, new Vector3(0.1f, 0.1f, 0.1f) * -0f) < 0.01f)
                {
                    Destroy(eff,0.5f);
                }
                yield return null;
            }
            //TODO : 카메라 흔들면서 연출 효과 나오고 이름 나오고 시작

            yield return new WaitForSeconds(0.5f);
            toggleRenderer.enabled = false;
            drectingCamera.SetActive(false);
            pattern.demon.enabled = true;
            pattern.demon.ChangeState(DEMON.Idle);
            player.enabled = true;
            yield return null;
        }
        IEnumerator PhaseDemon()
        {
            drectingCamera.SetActive(!isPhase);
            Debug.Log("dd");
            yield return new WaitForSeconds(0.5f);
            pattern.demon.enabled = false;
            player.enabled = false;

            yield return new WaitForSeconds(10f);
            //TODO : 카메라 흔들리면서 시작

            drectingCamera.SetActive(false);
            pattern.demon.enabled = true;
            pattern.demon.ChangeState(DEMON.Idle);
            player.enabled = true;
            yield return null;
        }
    }
}
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using BS.PlayerInput;

public class AlienController : MonoBehaviour
{
    #region Variables
    public GameObject boss;
    public GameObject player;
    public Camera mainCamera;
    public GameObject sequencerCamera;
    
    #endregion

    private void Start()
    {
        StartCoroutine(ScriptsEnabledControll());
    }

    IEnumerator ScriptsEnabledControll()
    {
        AlienBossPattern alienBossPattern = boss.GetComponent<AlienBossPattern>();
        PlayerInputActions playerInputActions = player.GetComponent<PlayerInputActions>();
        //CinemachineSequencerCamera cinemachineCamera = sequencerCamera.GetComponent<CinemachineSequencerCamera>();

        alienBossPattern.enabled = false;       // 보스 패턴 끄기
        playerInputActions.UnInputActions();    // 플레이어 입력 끄기
        //cinemachineCamera.enabled = true;

        yield return new WaitForSeconds(8.0f);

        alienBossPattern.enabled = true;        // 보스 패턴 켜기
        playerInputActions.OnInputActions();    // 플레이어 입력 켜기
        sequencerCamera.SetActive(false);       // 시네머신 끄기
        mainCamera.fieldOfView = 60f;           // 메인 카메라 FOV값 60으로 설정



        //playerController.enabled = true;
    }
}


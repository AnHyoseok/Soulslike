using BS.Player;
using BS.State;
using BS.Title;
using UnityEngine;

namespace BS.Managers
{
    public class SceneManager : MonoBehaviour
    {
        #region Variables
        public Camera maincamera;
        public Camera m_camera;
        public GameObject canvas;
        public GameObject Player;
        public GameObject falsePlayer;
        public GameObject[] particle;
        public GameObject keyCanvas;
        public Stage[] stage =new Stage[3];
        public Outline[] outline = new Outline[3];
        //시작시 카메라 이동
        TitleCamera title;
        #endregion
        private void Start()
        {
            title = maincamera.GetComponent<TitleCamera>();
            maincamera.fieldOfView = 90;
            StartCoroutine(title.ZoomIn());
            falsePlayer.SetActive(true);
            maincamera.gameObject.SetActive(true);
            m_camera.gameObject.SetActive(false);
            canvas.SetActive(true);
            Player.GetComponent<PlayerController>().enabled = false;
            foreach (Outline outlines in outline)
            {
                outlines.enabled = false;
            }
            particle[0].SetActive(true);
            particle[0].transform.position = new Vector3(-5, 1.3f, -6.3f);
            particle[1].SetActive(false);
        }
        private void Update()
        {
            //player 위에 있는 캔버스 보이는 방향
            keyCanvas.transform.LookAt(keyCanvas.transform.position + m_camera.transform.rotation * Vector3.forward,m_camera.transform.rotation * Vector3.up);
            if(Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Escape))
            {
                ResetPlayer();
            }
        }
        //Start버튼
        public void StartScene()
        {
            maincamera.gameObject.SetActive(false);
            m_camera.gameObject.SetActive(true);
            canvas.SetActive(false);
            Player.SetActive(true);
            Player.transform.localScale = Vector3.one;
            Player.GetComponent<PlayerController>().enabled = true;
            falsePlayer.SetActive(false);
            particle[0].transform.position = new Vector3(-10.4f, 1.3f, -14.4f);
            particle[1].SetActive(true);
            foreach (Outline outlines in outline)
            {
                outlines.enabled = true;
            }
        }
        //Quit버튼
        public void Quit()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
        //초기 화면으로 돌아가고 싶을 때 Esc키를 누르면 실행되는 함수
        void ResetPlayer()
        {
            Player.GetComponent<PlayerController>().enabled = false;
            PlayerState state = Player.GetComponent<PlayerState>();
            state.targetPosition = falsePlayer.transform.position;
            Player.transform.localPosition = new Vector3(0.5f, 0, -5f);
            Player.transform.LookAt(this.transform.position);
            maincamera.gameObject.SetActive(true);
            m_camera.gameObject.SetActive(false);
            canvas.SetActive(true);
            PlayerStateMachine playerState = Player.GetComponent<PlayerStateMachine>();
            playerState.ChangeState(playerState.IdleState);
            falsePlayer.SetActive(true);
            Player.SetActive(false);
            foreach(Stage stages in stage)
            {
                stages.keyText.text = "";
                stages.stageText.text = "";
                if(stages.canvas)
                {
                    stages.canvas.SetActive(false);
                }
            }
            foreach (Outline outlines in outline)
            {
                outlines.enabled = false;
            }
            Player.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            particle[0].transform.position = new Vector3(-5, 1.3f, -6.3f);
            particle[1].SetActive(false);
            maincamera.fieldOfView = 90;
            StartCoroutine(title.ZoomIn());
        }
    }
}
using BS.Player;
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
        #endregion
        private void Start()
        {
            maincamera.gameObject.SetActive(true);
            m_camera.gameObject.SetActive(false);
            canvas.SetActive(true);
            Player.GetComponent<PlayerController>().enabled = false;
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                maincamera.gameObject.SetActive(true);
                m_camera.gameObject.SetActive(false);
                canvas.SetActive(true);
                Player.GetComponent<PlayerController>().enabled = false;
            }
        }
        public void StartScene()
        {
            maincamera.gameObject.SetActive(false);
            m_camera.gameObject.SetActive(true);
            canvas.SetActive(false);
            Player.transform.localScale = Vector3.one;
            Player.GetComponent<PlayerController>().enabled = true;
        }
        public void Quit()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
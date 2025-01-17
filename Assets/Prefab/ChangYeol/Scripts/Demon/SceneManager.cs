using DG.Tweening;
using UnityEngine;

namespace BS.Demon
{
    public class SceneManager : MonoBehaviour
    {
        public DemonController controller;
        public GameObject WarningCanvas;
        public Camera main;
        void Update()
        {
            WarningCanvas.transform.LookAt(WarningCanvas.transform.position + main.transform.rotation * Vector3.forward, main.transform.rotation * Vector3.up);
            if (Input.GetKeyDown(KeyCode.V))
            {
                controller.TakeDamage(5);
            }
        }
    }
}
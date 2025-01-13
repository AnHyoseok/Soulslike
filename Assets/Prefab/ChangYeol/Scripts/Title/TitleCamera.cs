using UnityEngine;
using System.Collections;

namespace BS.Title
{
    public class CameraZoomIn : MonoBehaviour
    {
        public Camera mainCamera;
        public float startFOV = 60f;
        public float endFOV = 20f;
        public float zoomDuration = 2f;

        void Start()
        {
            StartCoroutine(ZoomIn());
        }

        IEnumerator ZoomIn()
        {
            float elapsedTime = 0f;
            float startFOV = mainCamera.fieldOfView;

            while (elapsedTime < zoomDuration)
            {
                float t = elapsedTime / zoomDuration;
                mainCamera.fieldOfView = Mathf.Lerp(startFOV, endFOV, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            mainCamera.fieldOfView = endFOV;
        }
    }
}
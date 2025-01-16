using UnityEngine;

namespace BS.Demon
{
    public class SceneManager : MonoBehaviour
    {
        public DemonController controller;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.V))
            {
                controller.TakeDamage(10);
            }
        }
    }
}
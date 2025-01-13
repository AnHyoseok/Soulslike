using BS.Player;
using UnityEngine;

namespace BS.vampire
{
    public class Attack3Bat : MonoBehaviour
    {
        #region Variables

        public GameObject effectPrefab;
        private Rigidbody rb;
        private Vector3 velocity;
        #endregion

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Initialize(Vector3 direction, float speed)
        {
            velocity = direction * speed;
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = velocity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == " Player")
            {
                //������ �Դ� �ҽ�
                GameObject effectGo = Instantiate(effectPrefab,transform.position,Quaternion.identity);
                //�΋H��������Ʈ
            }


          
        }
    }
}

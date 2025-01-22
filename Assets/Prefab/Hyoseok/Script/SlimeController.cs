using BS.Player;
using System.Collections;
using UnityEngine;

namespace BS.Title
{
    public class SlimeController : MonoBehaviour
    {
        #region Variables
        public GameObject player;
        public GameObject attackPaticle;
        public int damageAmount = 1;

        //private Animator animator;
        #endregion

        private void Start()
        {
            //if (animator == null)
            //{
            //    animator = GetComponent<Animator>();
            //}
            //StartCoroutine(Attak());
        }
        private void Update()
        {
            transform.LookAt(player.transform);
           
        }

        void OnTriggerEnter(Collider other)
        {

            // 자식 객체에서 PlayerHealth 컴포넌트를 찾음
            PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log($"{damageAmount}만큼 데미지 입음");
                playerHealth.TakeDamage(damageAmount, false);
                Debug.Log($"hp={playerHealth.CurrentHealth}");

            }

        }
        //IEnumerator Attak()
        //{
        //    animator.SetTrigger("attack");
        //    attackPaticle.SetActive(true);
        //    yield return new WaitForSeconds(1f);

        //    attackPaticle.SetActive(false);
        //}
 
    }
}
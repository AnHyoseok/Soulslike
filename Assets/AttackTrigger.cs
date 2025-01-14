using BS.Player;
using UnityEngine;

namespace BS.Enemy.Set
{
    public class AttackTrigger : MonoBehaviour
    {
        private bool hitCheck;
        [SerializeField] float damage = 10;

        private void OnEnable()
        {
            hitCheck = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (hitCheck) return;

            // �浹�� ������Ʈ�� ���̾ "Player"���� Ȯ��
            if (other.gameObject.layer == LayerMask.NameToLayer(SetProperty.PLAYER_LAYER))
            {
                // �÷��̾�� �������� �ִ� ���� ó��
                Debug.Log("�÷��̾�� ������!");
                hitCheck = true;

                //TODO : �������� �ְ� �ʿ�� �˹�, ���� ����
                //PlayerStats playerStats = other.GetComponent<PlayerStats>();
                //if (playerStats != null)
                //{
                //    playerStats.TakeDamage(damage); // ���� ������ ��
                //}


            }
        }
    }
}
using UnityEngine;
using UnityEngine.Events;

namespace BS.vampire
{
    public class SummonHealth : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float maxHealth = 100f;    //�ִ� Hp
        public float CurrentHealth { get; private set; }    //���� Hp
        private bool isDeath = false;                       //���� üũ

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction OnDie;
      

        //ü�� ���� �����
        [SerializeField] private float criticalHealRatio = 0.3f;

      
        #endregion

      
        //UI HP ������ ��
        public float GetRatio() => CurrentHealth / maxHealth;
        //���� üũ
        public bool IsCritical() => GetRatio() <= criticalHealRatio;


        private void Start()
        {
            //�ʱ�ȭ
            CurrentHealth = maxHealth;
      
        }

      

        //damageSource: �������� �ִ� ��ü
        public void TakeDamage(float damage, GameObject damageSource)
        {
           
            float beforeHealth = CurrentHealth;
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);
            Debug.Log($"{gameObject.name} CurrentHealth: {CurrentHealth}");

            //real Damage ���ϱ�
            float realDamage = beforeHealth - CurrentHealth;
            if (realDamage > 0f)
            {
                //������ ����                
                OnDamaged?.Invoke(realDamage, damageSource);
            }

            //���� ó��
            HandleDeath();
        }

        //���� ó�� ����
        void HandleDeath()
        {
            //���� üũ
            if (isDeath)
                return;

            if (CurrentHealth <= 0f)
            {
                isDeath = true;

                //���� ����
                OnDie?.Invoke();
            }
        }
    }
}
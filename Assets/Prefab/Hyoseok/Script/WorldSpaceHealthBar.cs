using UnityEngine;
using UnityEngine.UI;
namespace BS.vampire
{
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        #region Variables
        public SummonHealth health;
        public Image healthBarImage;

        public Transform healthBarPivot;

        //hp�� Ǯ�̸� healthBar�� �����
        [SerializeField] private bool hideFullHealthBar = true;
        #endregion

        private void Update()
        {
            healthBarImage.fillAmount = health.GetRatio();

            //UI�� �÷��̾ �ٶ󺸵��� �Ѵ�
            healthBarPivot.LookAt(Camera.main.transform.position);

            //hp�� Ǯ�̸� healthBar�� �����
            if (hideFullHealthBar)
            {
                healthBarPivot.gameObject.SetActive(healthBarImage.fillAmount != 1f);
            }
        }
    }
}
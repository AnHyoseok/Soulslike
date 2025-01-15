using UnityEngine;

namespace BS.Demon
{
    public class BallRise : DemonBall
    {
        #region Variables
        [SerializeField]private GameObject effgo;
        #endregion
        public override void TargetRise()
        {
            if(effgo == null)
            {

                return;
            }
            else if (effgo)
            {
                GameObject effcetgo = Instantiate(effgo, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                Destroy(effgo, 1.5f);
            }
        }
    }
}
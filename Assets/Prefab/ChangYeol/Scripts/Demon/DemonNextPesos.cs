using UnityEngine;

namespace BS.Demon
{
    public class DemonNextPesos : DemonController
    {
        #region Variables
        public float[] lastPesosTime = new float[3];
        [SerializeField]private float[] pesosAttackCool = new float[3] {6, 4, 3 };
        #endregion
        public override void NextPesos()
        {
            index = Random.Range(0, pesosDemon.Count);
            if (Time.time - lastPesosTime[index] >= attackCooldown[index] &&
                Vector3.Distance(transform.position, pattern.player.position) > attackRange)
            {
                switch (index)
                {
                    case 0:
                        ChangeState(pesosDemon[0]);
                        break;
                    case 1:
                        ChangeState(pesosDemon[1]);
                        break;
                    case 2:
                        ChangeState(pesosDemon[2]);
                        break;
                }
                return;
            }
            else
            {
                ChangeState(DEMON.Idle);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class BoosTransUpdate : MonoBehaviour
{
    #region Variables
    public Transform boss;
    public Transform Player;
    private float moveSpeed = 100f;
    public Image directionimage;
    public Sprite directionSprite;
    public Sprite ChangeSprite;
    [SerializeField]private float bossRange = 4f;
    #endregion
    void Update()
    {
        BossPos();
        transform.LookAt(boss);
        if(Vector3.Distance(Player.position, boss.position) <= bossRange)
        {
            directionimage.sprite = ChangeSprite;
        }
        else if (Vector3.Distance(Player.position, boss.position) > bossRange)
        {
            directionimage.sprite = directionSprite;
        }
    }
    void BossPos()
    {
        Vector3 pos = Player.position + new Vector3(0,0.01f,0);
        transform.position = Vector3.Lerp(transform.position, pos, moveSpeed* Time.deltaTime);
    }
}
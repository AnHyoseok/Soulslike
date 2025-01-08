using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //TODO : 데미지를 주고 필요시 넉백 구현
        Debug.Log("데미지!");
    }
}

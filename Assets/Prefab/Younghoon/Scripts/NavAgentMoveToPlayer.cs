using BS.Player;
using UnityEngine;
using UnityEngine.AI;

namespace BS.Enemy.Set
{
    public class NavAgentMoveToPlayer : MonoBehaviour
    {
        #region Variables
        GameObject target;
        NavMeshAgent agent;
        #endregion
        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            target = FindAnyObjectByType<PlayerController>().gameObject;
        }
        void Update()
        {
            //agent.SetDestination(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            agent.transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        }
    }
}
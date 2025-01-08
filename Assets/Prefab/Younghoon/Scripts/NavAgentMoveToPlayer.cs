using BS.Player;
using UnityEngine;
using UnityEngine.AI;

namespace BS.NavAgent
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
            //target = FindAnyObjectByType<PlayerMovement>().gameObject;
        }
        void Update()
        {
            //agent.SetDestination(target.transform.position);

        }

    }
}
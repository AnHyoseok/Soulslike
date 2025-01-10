using UnityEngine;
using UnityEngine.AI;

namespace BS.Enemy.Set
{
    /// <summary>
    /// 
    /// </summary>
    public class SetProperty
    {
        public SetController Controller { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Transform Player { get; private set; }
        public Animator Animator { get; private set; }

        public SetProperty(SetController controller, Animator animator, NavMeshAgent agent, Transform player)
        {
            Controller = controller;
            Agent = agent;
            Player = player;
            Animator = animator;
        }
    }
}
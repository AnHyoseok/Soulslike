using UnityEngine;

namespace BS.Enemy.Set
{
    public class SetDeathState : ISetState
    {
        private SetProperty property;

        public SetDeathState(SetProperty property)
        {
            this.property = property;
        }

        public void Enter()
        {
            Debug.Log("보스가 사망했습니다.");
            property.Animator.SetBool(SetProperty.SET_ANIM_BOOL_DEAD, true);

            // NavMeshAgent 멈추기
            //property.Agent.isStopped = true;
        }

        public void Update()
        {
            //Empty
        }

        public void Exit()
        {
            //Empty
        }


    }
}
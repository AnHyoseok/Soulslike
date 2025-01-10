using UnityEngine;

namespace BS.Particle
{
    public class ParticleRange : MonoBehaviour
    {
        private new ParticleSystem particleSystem;
        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
        private void OnParticleCollision(GameObject other)
        {
            Debug.Log($"other ={other}");
        }
    }
}
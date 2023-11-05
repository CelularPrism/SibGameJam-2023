using UnityEngine;

namespace Assets.Scripts.Tears
{
    public class TearsSource : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _leftEyeParticleSystem, _rightEyeParticleSystem;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                StartTearing();

            if (Input.GetMouseButtonUp(0))
                StopTearing();
        }

        private void StartTearing()
        {
            _leftEyeParticleSystem.Play();
            _rightEyeParticleSystem.Play();
        }

        private void StopTearing()
        {
            _leftEyeParticleSystem.Stop();
            _rightEyeParticleSystem.Stop();
        }
    }
}
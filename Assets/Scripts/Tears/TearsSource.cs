using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Tears
{
    public class TearsSource : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _leftEyeParticleSystem, _rightEyeParticleSystem;
        [SerializeField] private EventReference _event;
        [SerializeField] private Transform _camera;

        private EventInstance instance;

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                StartTearing();

            if (Input.GetMouseButtonUp(0))
                StopTearing();
        }

        private void StartTearing()
        {
            Debug.Log("start shoot");
            var muted = false;
            RuntimeManager.GetBus("bus:/SFX").getMute(out muted);
            if (muted)
                RuntimeManager.GetBus("bus:/SFX").setMute(false);

            instance = RuntimeManager.CreateInstance(_event);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(_camera.position));
            instance.start();
            instance.release();

            _leftEyeParticleSystem.Play();
            _rightEyeParticleSystem.Play();
        }

        private void StopTearing()
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            _leftEyeParticleSystem.Stop();
            _rightEyeParticleSystem.Stop();
        }
    }
}
﻿using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Assets.Scripts.Tears
{
    public class TearsSource : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1)] private float _speedModifire = 0.25f;
        [SerializeField] private float _reactiveForce;
        [SerializeField] private ParticleSystem _leftEyeParticleSystem, _rightEyeParticleSystem;
        [SerializeField] private Transform _look;
        [SerializeField] private EventReference _event;
        [SerializeField] private Transform _camera;
        private bool _isTearing;
        private EventInstance instance;
        private RBMotionController _motionController;

        private void Awake()
        {
            _motionController = GetComponent<RBMotionController>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                StartTearing();

            if (Input.GetMouseButtonUp(0))
                StopTearing();
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0))
                _motionController.Rigidbody.AddForce(-_reactiveForce * Time.fixedDeltaTime * _look.forward, ForceMode.Force);
        }

        private void StartTearing()
        {
            if (_isTearing)
                return;

            _isTearing = true;
            instance = RuntimeManager.CreateInstance(_event);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(_camera.position));
            instance.start();
            instance.release();

            //_motionController.SpeedFactor *= _speedModifire;
            _leftEyeParticleSystem.Play();
            _rightEyeParticleSystem.Play();
        }

        private void StopTearing()
        {
            if (_isTearing == false)
                return;
            
            _isTearing = false;
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //_motionController.SpeedFactor /= _speedModifire;
            _leftEyeParticleSystem.Stop();
            _rightEyeParticleSystem.Stop();
        }
    }
}
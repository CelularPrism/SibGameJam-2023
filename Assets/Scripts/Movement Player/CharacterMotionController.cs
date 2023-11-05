using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMotionController : MonoBehaviour
{
    [SerializeField] private InputAction _moveInputAction;
    [SerializeField, Range(0, 100)] float _moveSpeed;
    Rigidbody _rigidbody;
    private bool _stopped = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _moveInputAction.Enable();
    }

    private void FixedUpdate()
    {
        if (!_stopped)
        {
            Vector2 input = _moveInputAction.ReadValue<Vector2>();
            _rigidbody.velocity = new Vector3(input.x, 0, input.y) * _moveSpeed;
        }
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
        _moveInputAction.Disable();
    }

    public void Stop()
    {
        _stopped = true;
    }
}

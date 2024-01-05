using Assets.Scripts.Movement_Player;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorWide : MonoBehaviour
{
    [SerializeField] private float _force, _charcterControllersForce;
    //[SerializeField] private float _forceInetarval;
    //private float _intervalTime;
    private Rigidbody _rigidbody;
    private MeshRenderer _renderer;
    private float _offset;
    private BoxCollider _collider;
    private Vector3 _characterControllerDirection;
    private readonly List<Rigidbody> _rigidbodies = new();
    private readonly List<CharacterController> _characterControllers = new();
    private readonly List<CharacterController> _passedCharacterControllers = new();

    public void Push(float speed)
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();

        if (_collider == null)
            _collider = GetComponent<BoxCollider>();

        _characterControllerDirection = speed * _charcterControllersForce * Time.deltaTime * transform.forward;

        foreach (CharacterController characterController in _characterControllers)
            characterController.Move(-_characterControllerDirection);
    }

    public void FixedPush(float speed)
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();

        if (_collider == null)
            _collider = GetComponent<BoxCollider>();

        //_collider.material.dynamicFriction = speed * 10;
        Vector3 direction = speed * _force * Time.fixedDeltaTime * transform.forward;
        //Vector3 position = _rigidbody.position;
        //_rigidbody.position += direction;
        //_rigidbody.MovePosition(position);

        //if (_intervalTime >= _forceInetarval)
        //    foreach (Rigidbody rigidbody in _rigidbodies)
        //    {
        //        _intervalTime = 0;
        //        //float velocityMagnitude = Mathf.Abs(rigidbody.velocity.magnitude);
        //        rigidbody.AddForce(-direction/* * rigidbody.mass / Mathf.Clamp(velocityMagnitude, 1, velocityMagnitude)*/, ForceMode.VelocityChange);
        //    }

        //_intervalTime += Time.fixedDeltaTime;

        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.velocity = -direction;
    }

    public void LatePush(float speed)
    {
        if (_renderer == null)
            _renderer = GetComponent<MeshRenderer>();

        _offset += -speed * Time.deltaTime;
        _renderer.material.mainTextureOffset = new(0, _offset);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody rigidbody))
        {
            if (_rigidbodies.Contains(rigidbody) == false)
                _rigidbodies.Add(rigidbody);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterController characterController))
        {
            if (_characterControllers.Contains(characterController) == false)
            {
                characterController.GetComponent<ICharacterMotionController>().RemoveForce();
                _characterControllers.Add(characterController);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody rigidbody))
        {
            _rigidbodies.Remove(rigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterController characterController))
        {
            if (_characterControllers.Contains(characterController))
            {
                characterController.GetComponent<ICharacterMotionController>().AddForce(-_characterControllerDirection);
                _characterControllers.Remove(characterController);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ConveyorWide : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _forceInetarval;
    private float _intervalTime;
    private Rigidbody _rigidbody;
    private MeshRenderer _renderer;
    private float _offset;
    private BoxCollider _collider;
    private readonly List<Rigidbody> _rigidbodies = new();

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

        if (_intervalTime >= _forceInetarval)
            foreach (Rigidbody rigidbody in _rigidbodies)
            {
                _intervalTime = 0;
                //float velocityMagnitude = Mathf.Abs(rigidbody.velocity.magnitude);
                rigidbody.AddForce(-direction/* * rigidbody.mass / Mathf.Clamp(velocityMagnitude, 1, velocityMagnitude)*/, ForceMode.VelocityChange);
            }

        _intervalTime += Time.fixedDeltaTime;
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
            _rigidbodies.Add(rigidbody);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody rigidbody))
        {
            _rigidbodies.Remove(rigidbody);
        }
    }
}
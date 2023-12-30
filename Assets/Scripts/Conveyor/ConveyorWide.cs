using UnityEngine;

public class ConveyorWide : MonoBehaviour
{
    [SerializeField] private float _rigidbodiesSpeedModifire;
    private Rigidbody _rigidbody;
    private MeshRenderer _renderer;
    private float _offset;
    private BoxCollider _collider;

    public void FixedPush(float speed)
    {
        if (_renderer == null)
            _renderer = GetComponent<MeshRenderer>();

        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();

        if (_collider == null)
            _collider = GetComponent<BoxCollider>();

        _collider.material.dynamicFriction = speed * 10;
        _offset += -speed * Time.fixedDeltaTime;
        _renderer.material.mainTextureOffset = new(0, _offset);
        Vector3 direction = speed * _rigidbodiesSpeedModifire * Time.fixedDeltaTime * transform.forward;
        Vector3 position = _rigidbody.position;
        _rigidbody.position += direction;
        _rigidbody.MovePosition(position);
    }
}
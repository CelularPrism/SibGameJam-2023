using Assets.Scripts.Movement_Player;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorWide : MonoBehaviour
{
    [SerializeField] private float _movablesSpeedModifire, _rigidbodiesSpeedModifire;
    private MeshRenderer _renderer;
    private readonly List<IMovable> _movables = new();
    private readonly List<Rigidbody> _rigidbodies = new();
    private float _offset;

    public void Push(float speed)
    {
        if (_renderer == null)
            _renderer = GetComponent<MeshRenderer>();

        _offset += -speed * Time.deltaTime;
        _renderer.material.mainTextureOffset = new(0, _offset);

        for (int i = 0; i < _movables.Count; i++)
        {
            _movables[i].Move(-speed * _movablesSpeedModifire * Time.deltaTime * transform.forward);
        }

        for (int i = 0; i < _rigidbodies.Count; i++)
        {
            Vector3 direction = -speed * _rigidbodiesSpeedModifire * Time.deltaTime * transform.forward;
            _rigidbodies[i].velocity = new(direction.x, _rigidbodies[i].velocity.y, direction.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMovable movable))
        {
            _movables.Add(movable);
            return;
        }

        if (other.TryGetComponent(out Rigidbody rigidbody))
        {
            _rigidbodies.Add(rigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IMovable movable))
        {
            _movables.Remove(movable);
            return;
        }

        if (other.TryGetComponent(out Rigidbody rigidbody))
        {
            _rigidbodies.Remove(rigidbody);
        }
    }
}
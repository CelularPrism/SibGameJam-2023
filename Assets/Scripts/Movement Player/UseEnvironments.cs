using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(SphereCollider))]
public class UseEnvironments : MonoBehaviour
{
    [SerializeField] private MovementInput input;
    [SerializeField] private LayerMask layerMask;

    private SphereCollider _sphere;
    private Collider[] _colliders;
    private Collider _collider;
    private bool _canUse = false;
    private float _raduis;

    public bool CanUse() => _canUse;

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    void Awake()
    {
        input = new MovementInput();
        input.Player.Use.performed += Use;
        _sphere = GetComponent<SphereCollider>();
        _raduis = _sphere.radius;
    }

    private void FixedUpdate()
    {
        _colliders = Physics.OverlapSphere(transform.position, _raduis, layerMask);
        if (_colliders.Length > 0)
        {
            _canUse = true;
            _collider = _colliders[0];
        } else
        {
            _canUse = false;
            _collider = null;
        }
    }

    private void Use(CallbackContext calbackContext)
    {
        if (_canUse)
        {
            var item = _collider.GetComponent<IItem>();
            if (item != null)
                item.Use();
        }
    }
}

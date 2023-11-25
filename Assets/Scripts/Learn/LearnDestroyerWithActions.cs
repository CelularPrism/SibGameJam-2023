using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LearnDestroyer))]
public class LearnDestroyerWithActions : MonoBehaviour
{
    [SerializeField] private InputAction _inputAction;
    private LearnDestroyer _destroyer;

    void OnEnable()
    {
        if (_inputAction != null)
            _inputAction.Enable();

        _destroyer = GetComponent<LearnDestroyer>();
    }

    private void OnDisable()
    {
        if (_inputAction != null)
            _inputAction.Disable();
    }

    void Update()
    {
        if (_inputAction != null)
        {
            var destroy = false;
            try
            {
                if (_inputAction.ReadValue<Vector2>() != Vector2.zero)
                    destroy = true;
            } catch
            {
                destroy = false;
            }

            try
            {
                if (_inputAction.IsPressed())
                    destroy = true;
            } catch
            {
                destroy = false;
            }

            if (destroy)
                _destroyer.Destroy();
        }
    }
}

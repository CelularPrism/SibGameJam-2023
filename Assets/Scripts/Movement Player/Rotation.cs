using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private RotationAction _inputActions;

    private void Start()
    {
        _inputActions = new RotationAction();
        _inputActions.Rotation.Enable();
    }

    private void FixedUpdate()
    {
        Vector3 mousePos = _inputActions.Rotation.MousePosition.ReadValue<Vector2>();
        Vector3 pos = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 dir = mousePos - pos;
        if (Mathf.Abs(dir.x) > 10f && Mathf.Abs(dir.y) > 10f)
        {
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.up);
        }
    }
}

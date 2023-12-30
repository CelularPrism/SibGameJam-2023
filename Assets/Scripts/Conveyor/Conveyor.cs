using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private ConveyorWide _wide;

    private void FixedUpdate()
    {
        if (_wide != null)
            _wide.FixedPush(_speed);
    }

    private void LateUpdate()
    {
        if (_wide != null)
            _wide.LatePush(_speed);
    }
}

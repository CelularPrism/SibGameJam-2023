using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private ConveyorWide _wide;

    private void Update()
    {
        if (_wide != null)
            _wide.Push(_speed);
    }
}

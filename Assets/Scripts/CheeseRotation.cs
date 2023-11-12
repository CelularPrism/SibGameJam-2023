using UnityEngine;

public class CheeseRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private float initialHight = 1f;
    [SerializeField] private float swingSpeed = 0.5f;
    [SerializeField] private float swingAmplitude = 1f;
    private Vector3 pos;

    private void Start()
    {
        pos = transform.localPosition;
        pos.y += initialHight;
    }

    void Update()
    {
        transform.localPosition = new Vector3(pos.x, pos.y + swingAmplitude * Mathf.Sin(Time.time * swingSpeed), pos.z);
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
    }
}

using UnityEngine;

public class WallDestroyer : MonoBehaviour
{

    [SerializeField] private float _fadingSpeed = 0.1f;
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        float value = Mathf.MoveTowards(_material.GetFloat("_Fade"), 1, _fadingSpeed * Time.deltaTime);
        _material.SetFloat("_Fade", value);

        if (value <= 0)
            gameObject.SetActive(false);
    }
}

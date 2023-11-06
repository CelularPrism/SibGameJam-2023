using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroyer : MonoBehaviour
{

    [SerializeField] private float speedSmooth = 1.0f;

    private bool _destroy = false;
    private Color _alphaColor;

    private void Start()
    {
        _alphaColor = transform.GetComponent<MeshRenderer>().materials[0].color;
    }

    private void FixedUpdate()
    {
        if (_destroy)
        {
            _alphaColor = transform.GetComponent<MeshRenderer>().material.color;
            transform.GetComponent<MeshRenderer>().material.color = new Color(_alphaColor.r, _alphaColor.g, _alphaColor.b, _alphaColor.a - speedSmooth / 255);
            //Debug.Log(_alphaColor);
            if (_alphaColor.a < 0.1)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    public void Destroy()
    {
        _destroy = true;
    }
}

using Assets.Scripts.Fire;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LearnDestroyer : MonoBehaviour
{
    [SerializeField] private float rangeSeconds;
    [SerializeField] private float speedSmooth;
    private Image _image;
    private DateTime _now;

    void Start()
    {
        _image = transform.GetComponent<Image>();
        _now = DateTime.Now;
    }

    private void OnEnable()
    {
        var learns = FindObjectsOfType<LearnDestroyer>();
        foreach (var learn in learns)
        {
            Debug.Log(learn);
            if (learn.gameObject.activeInHierarchy && learn.gameObject != transform.gameObject) 
                Destroy(learn.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (_now.AddSeconds(rangeSeconds) < DateTime.Now)
        {
            var red = _image.color.r;
            var green = _image.color.g;
            var blue = _image.color.b;
            var alpha = _image.color.a;
            _image.color = new Color(red, green, blue, alpha - speedSmooth / 255);
            if (_image.color.a < 0.1)
                Destroy(_image.gameObject);
        }
    }
}

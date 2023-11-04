using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DragButton : MonoBehaviour
{
    [SerializeField] private EventReference _dragEvent;
    [SerializeField] private Camera _camera;

    public void Drag() => RuntimeManager.PlayOneShot(_dragEvent, _camera.GetComponent<Transform>().position);

    void OnMouseEnter() =>
        Debug.Log("Enter");
}

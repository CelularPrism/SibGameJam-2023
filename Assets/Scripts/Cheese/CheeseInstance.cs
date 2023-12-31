﻿using Assets.Scripts.Movement_Player;
using System;
using UnityEngine;

public class CheeseInstance : MonoBehaviour, IMovable
{
    [field: SerializeField, Range(0, 1)] public float Size { get; private set; }
    [field: SerializeField, Range(0, 1)] public float Weight { get; private set; }
    [field: SerializeField] public Mesh Mesh { get; private set; }

    public void Construct(Mesh mesh, float size, float weight)
    {
        Size = size;
        Weight = weight;
        Mesh = mesh;
    }

    public void Move(Vector3 direction) => transform.position += direction;
}
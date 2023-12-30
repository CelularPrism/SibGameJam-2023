using System;
using UnityEngine;

namespace Assets._Project.Gameplay.Ground_Check
{
    public interface IGroundChecker
    {
        event Action OnGrounded;
        bool IsGrounded { get; }
        Vector3 Normal { get; }
        void Check();
        void OnDrawGizmos();
    }
}

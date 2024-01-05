using UnityEngine;

namespace Assets.Scripts.Movement_Player
{
    internal interface ICharacterMotionController
    {
        Rigidbody Rigidbody { get; }
        float DefaultMass { get; }

        void AddForce(Vector3 direction);
        void RemoveForce();
    }
}

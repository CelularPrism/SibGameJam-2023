using UnityEngine;

namespace Assets.Scripts.Movement_Player
{
    internal interface ICharacterMotionController
    {
        Rigidbody Rigidbody { get; }
        float DefaultMass { get; }
        float SpeedFactor { get; set; }
        float Inertia { get; set; }

        void AddForce(Vector3 direction);
        void RemoveForce();
    }
}

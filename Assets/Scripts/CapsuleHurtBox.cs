using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class CapsuleHurtBox : HurtBox<CapsuleCollider>
    {
    }
}

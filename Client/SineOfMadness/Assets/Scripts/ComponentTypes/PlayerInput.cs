using System;
using Unity.Entities;
using Unity.Mathematics;

namespace SineOfMadness
{
    [Serializable]
    public struct PlayerInput : IComponentData {
        public float2 Move;
        public float2 Shoot;
        public float FireCooldown;

        public bool Fire => FireCooldown <= 0.0 && math.length(Shoot) > 0.5f;
    }
}
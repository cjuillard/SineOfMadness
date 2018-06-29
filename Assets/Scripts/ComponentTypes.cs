using Unity.Entities;
using Unity.Mathematics;

namespace SineOfMadness {

    public struct PlayerInput : IComponentData {
        public float2 Move;
        public float2 Shoot;
        public float FireCooldown;

        public bool Fire => FireCooldown <= 0.0 && math.length(Shoot) > 0.5f;
    }

    // Purely tag types
    public struct Enemy : IComponentData { }

    public struct Health : IComponentData {
        public float Value;
    }
}

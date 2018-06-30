using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SineOfMadness {

    public struct PlayerInput : IComponentData {
        public float2 Move;
        public float2 Shoot;
        public float FireCooldown;

        public bool Fire => FireCooldown <= 0.0 && math.length(Shoot) > 0.5f;
    }

    // Purely tag types
    public struct Enemy : IComponentData { }
    public struct Player : IComponentData { }
    public struct FollowPlayer : IComponentData { } // this tag will constantly turn the entity's heading the players direction

    public struct Health : IComponentData {
        public float Value;
    }

    public struct EnemySpawnCooldown : IComponentData {
        public float Value;
    }

    public struct EnemySpawnSystemState : IComponentData {
        public int SpawnedEnemyCount;
        public Random.State RandomState;
    }
}

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;
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
    public struct PlayerShot : IComponentData { }

    public struct Health : IComponentData {
        public float Value;
    }

    public struct EnemySpawnCooldown : IComponentData {
        public float Value;
    }

    public struct Factions {
        public const int kPlayer = 0;
        public const int kEnemy = 1;
    }

    public struct SpawnableTags : IComponentData {
        public const int FRIENDLY = 1;
        public const int ENEMY = 1 << 1;
        public const int SHOT = 1 << 2;
        public const int ENEMY_SHOT = SHOT | ENEMY;
        public const int FRIENDLY_SHOT = SHOT | FRIENDLY;

        public int Value;
    }

    public struct Shot : IComponentData {
        public float Energy;
    }

    public struct ShotSpawnData : IComponentData {
        public Position2D Position;
        public Heading2D Heading;
    }

    public struct EnemySpawnSystemState : IComponentData {
        public int SpawnedEnemyCount;
        public Random.State RandomState;
    }
}

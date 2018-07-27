using UnityEngine;
using System.Collections;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms2D;

namespace SineOfMadness
{

    class EnemyCollisionSystem : JobComponentSystem
    {
        struct Players
        {
            public int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<PlayerInput> PlayerMarker;
        }

        [Inject] Players m_Players;

        struct Enemies
        {
            public int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<Enemy> EnemyMarker;
        }

        [Inject] Enemies m_Enemies;

        [Inject] ComponentDataFromEntity<Health> m_AllHealth;

        [BurstCompile]
        struct PlayerCollisionJob : IJobParallelFor
        {
            public float CollisionRadiusSquared;

            // Player data
            [ReadOnly] public EntityArray PlayerEntities;
            [ReadOnly] public ComponentDataArray<Position2D> PlayerPositions;
            [NativeDisableParallelForRestriction]
            public ComponentDataFromEntity<Health> AllHealth;

            // Enemies data
            [ReadOnly] public EntityArray EnemyEntities;
            [ReadOnly] public ComponentDataArray<Position2D> EnemyPositions;

            public void Execute(int index)
            {
                float2 enemyPos = EnemyPositions[index].Value;
                for (int pi = 0; pi < PlayerEntities.Length; ++pi)
                {
                    float2 playerPos = PlayerPositions[pi].Value;
                    if (math.lengthSquared(playerPos - enemyPos) <= CollisionRadiusSquared)
                    {
                        // Kill player
                        Health hp = AllHealth[PlayerEntities[pi]];
                        hp.Value--;
                        AllHealth[PlayerEntities[pi]] = hp;

                        // Kill the enemy
                        Health enemyHp = AllHealth[EnemyEntities[index]];
                        enemyHp.Value--;
                        AllHealth[EnemyEntities[index]] = enemyHp;

                        break;
                    }
                }

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var settings = Boot.Settings;

            if (settings == null)
                return inputDeps;
            
            var enemiesVsPlayers = new PlayerCollisionJob
            {
                CollisionRadiusSquared = settings.enemyCollisionRadius * settings.enemyCollisionRadius,
                PlayerEntities = m_Players.Entities,
                AllHealth = m_AllHealth,
                PlayerPositions = m_Players.Position,
                EnemyEntities = m_Enemies.Entities,
                EnemyPositions = m_Enemies.Position
            }.Schedule(m_Enemies.Length, 1, inputDeps);

            return enemiesVsPlayers;
        }
    }
}


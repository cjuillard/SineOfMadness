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

namespace SineOfMadness {

    /// <summary>
    /// Assigns out damage from shots colliding with entities of other factions.
    /// </summary>
    class ShotDamageSystem : JobComponentSystem {

        struct Enemies {
            public int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<Enemy> EnemyMarker;
        }

        [Inject] Enemies m_Enemies;

        [Inject] ComponentDataFromEntity<Health> m_AllHealth;

        /// <summary>
        /// All player shots.
        /// </summary>
        struct PlayerShotData {
            public int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Shot> Shot;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<PlayerShot> PlayerShotMarker;
        }
        [Inject] PlayerShotData m_PlayerShots;

        [BurstCompile]
        struct EnemyCollisionJob : IJobParallelFor {
            public float CollisionRadiusSquared;
            
            [ReadOnly] public EntityArray PlayerShotEntities;
            [ReadOnly] public EntityArray EnemyEntities;

            [NativeDisableParallelForRestriction]
            public ComponentDataFromEntity<Health> Health;

            [ReadOnly] public ComponentDataArray<Position2D> Positions;

            [NativeDisableParallelForRestriction]
            [ReadOnly] public ComponentDataArray<Shot> Shots;

            [NativeDisableParallelForRestriction]
            [ReadOnly] public ComponentDataArray<Position2D> ShotPositions;

            public void Execute(int index) {
                float damage = 0.0f;

                float2 receiverPos = Positions[index].Value;

                for (int si = 0; si < ShotPositions.Length; ++si) {
                    float2 shotPos = ShotPositions[si].Value;
                    float2 delta = shotPos - receiverPos;
                    float distSquared = math.dot(delta, delta);
                    if (distSquared <= CollisionRadiusSquared) {
                        Shot shot = Shots[si];

                        damage += shot.Energy;

                        Health[PlayerShotEntities[si]] = new Health { Value = 0 };
                        //ShotHealth[si] = new Health { Value = 0 };
                    }
                }

                Entity enemyEntity = EnemyEntities[index];
                var h = Health[enemyEntity];
                h.Value = math.max(h.Value - damage, 0.0f);
                Health[enemyEntity] = h;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps) {
            var settings = Boot.Settings;

            if (settings == null)
                return inputDeps;

            var playersVsEnemies = new EnemyCollisionJob {
                PlayerShotEntities = m_PlayerShots.Entities,
                EnemyEntities = m_Enemies.Entities,
                ShotPositions = m_PlayerShots.Position,
                Shots = m_PlayerShots.Shot,
                Health = m_AllHealth,
                CollisionRadiusSquared = settings.enemyCollisionRadius * settings.enemyCollisionRadius,
                Positions = m_Enemies.Position,
            }.Schedule(m_Enemies.Length, 1, inputDeps);

            return playersVsEnemies;
        }
    }
}


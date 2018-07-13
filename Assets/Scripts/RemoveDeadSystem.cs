using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;

namespace SineOfMadness {
    public class RemoveDeadBarrier : BarrierSystem {
    }

    /// <summary>
    /// This system deletes entities that have a Health component with a value less than or equal to zero.
    /// </summary>
    public class RemoveDeadSystem : JobComponentSystem {
        struct Data {
            [ReadOnly] public EntityArray Entity;
            [ReadOnly] public ComponentDataArray<Health> Health;
            [ReadOnly] public ComponentDataArray<SpawnableTags> SpawnableTags;
        }

        struct PlayerCheck {
            [ReadOnly] public ComponentDataArray<PlayerInput> PlayerInput;
        }

        public struct RoundStatsData {
            public int Length;
            public ComponentDataArray<KillStats> State;
        }
        [Inject] RoundStatsData m_RoundStats;

        [Inject] private Data m_Data;
        [Inject] private PlayerCheck m_PlayerCheck;
        [Inject] private RemoveDeadBarrier m_RemoveDeadBarrier;

        [BurstCompile]
        struct RemoveDeadJob : IJob {
            public bool playerDead;
            [ReadOnly] public EntityArray Entity;
            [ReadOnly] public ComponentDataArray<Health> Health;
            [ReadOnly] public ComponentDataArray<SpawnableTags> SpawnableTag;
            public EntityCommandBuffer Commands;

            public ComponentDataArray<KillStats> KillStats;

            public void Execute() {
                int numKilled = 0;
                for (int i = 0; i < Entity.Length; ++i) {
                    if (Health[i].Value <= 0.0f || playerDead) {
                        if((SpawnableTag[i].Value & SpawnableTags.FRIENDLY_SHOT) == 0)
                            numKilled++;

                        Commands.DestroyEntity(Entity[i]);
                    }
                }

                KillStats stats = KillStats[0];
                stats.numberOfKills += numKilled;
                KillStats[0] = stats;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps) {
            return new RemoveDeadJob {
                playerDead = m_PlayerCheck.PlayerInput.Length == 0,
                Entity = m_Data.Entity,
                Health = m_Data.Health,
                SpawnableTag = m_Data.SpawnableTags,
                KillStats = m_RoundStats.State,
                Commands = m_RemoveDeadBarrier.CreateCommandBuffer(),
            }.Schedule(inputDeps);
        }
    }

}

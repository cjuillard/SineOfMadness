using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms2D;

namespace SineOfMadness {
    public class FollowPlayerSystem : JobComponentSystem {
        struct Enemies {
            public int Length;
            public ComponentDataArray<Heading2D> Headings;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<Enemy> EnemyMarker;
        }

        public struct PlayerData {
            public int Length;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<Player> Player;
        }

        [Inject] Enemies m_Enemies;
        [Inject] PlayerData m_Players;

        [BurstCompile]
        struct FollowJob : IJobParallelFor {

            public float2 targetPos;

            [ReadOnly] public ComponentDataArray<Position2D> Position;

            public ComponentDataArray<Heading2D> Headings;

            public void Execute(int index) {
                Position2D pos = Position[index];
                float2 delta = targetPos - pos.Value;
                if (math.lengthSquared(delta) == 0) {
                    return;
                }
                float2 dir = math.normalize(targetPos - pos.Value);
                Headings[index] = new Heading2D() { Value = dir };
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps) {

            if (m_Players.Length == 0)
                return inputDeps;

            var followJob = new FollowJob() {
                targetPos = m_Players.Position[0].Value,
                Position = m_Enemies.Position,
                Headings = m_Enemies.Headings
            }.Schedule(m_Enemies.Length, 1, inputDeps);


            return followJob;
        }
    }
}
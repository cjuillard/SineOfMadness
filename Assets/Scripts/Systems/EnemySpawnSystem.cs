using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;

namespace SineOfMadness {
    public class EnemySpawnSystem : ComponentSystem {

        struct State {
            public int Length;
            public ComponentDataArray<EnemySpawnCooldown> Cooldown;
            public ComponentDataArray<EnemySpawnSystemState> S;
        }

        struct Players {
            public int Length;
            public ComponentDataArray<Position2D> Position;
            public ComponentDataArray<Player> PlayerTag;
        }

        [Inject] State m_State;
        [Inject] Players players;

        public static void SetupComponentData(EntityManager entityManager) {
            var arch = entityManager.CreateArchetype(typeof(EnemySpawnCooldown), typeof(EnemySpawnSystemState));
            var stateEntity = entityManager.CreateEntity(arch);
            var oldState = Random.state;
            Random.InitState(0xaf77);
            entityManager.SetComponentData(stateEntity, new EnemySpawnCooldown { Value = 0.0f });
            entityManager.SetComponentData(stateEntity, new EnemySpawnSystemState
            {
                SpawnedEnemyCount = 0,
                RandomState = Random.state
            });
            Random.state = oldState;
        }


        protected override void OnUpdate() {
            float cooldown = m_State.Cooldown[0].Value;

            cooldown = Mathf.Max(0.0f, m_State.Cooldown[0].Value - Time.deltaTime);
            bool spawn = cooldown <= 0.0f;

            if (spawn) {
                cooldown = ComputeCooldown();
            }

            m_State.Cooldown[0] = new EnemySpawnCooldown { Value = cooldown };

            if (spawn) {
                SpawnEnemy();
            }
        }

        void SpawnEnemy() {
            var state = m_State.S[0];
            var oldState = Random.state;
            Random.state = state.RandomState;

            float2 spawnPosition = ComputeSpawnLocation();
            state.SpawnedEnemyCount++;

            PostUpdateCommands.CreateEntity(Boot.BasicEnemyArchetype);
            PostUpdateCommands.SetComponent(new Position2D { Value = spawnPosition });
            PostUpdateCommands.SetComponent(new Heading2D { Value = new float2(0.0f, -1.0f) });
            PostUpdateCommands.SetComponent(default(Enemy));
            PostUpdateCommands.SetComponent(new Health { Value = Boot.Settings.enemyInitialHealth });
            PostUpdateCommands.SetComponent(new MoveSpeed { speed = Boot.Settings.enemySpeed });
            PostUpdateCommands.SetComponent(new SpawnableTags { Value = SpawnableTags.ENEMY });

            PostUpdateCommands.AddSharedComponent(Boot.BasicEnemyLook);
            
            state.RandomState = Random.state;

            m_State.S[0] = state;
            Random.state = oldState;
        }

        float ComputeCooldown() {
            return Boot.Settings.spawnDelay;
        }

        float2 ComputeSpawnLocation() {
            var bounds = Boot.Settings.playfield;
            float minSpawnDist = Boot.Settings.minSpawnDist;

            float x = bounds.xMin + (bounds.xMax - bounds.xMin) * Random.value;
            float y = bounds.yMin + (bounds.yMax - bounds.yMin) * Random.value;

            float2 newPos = new float2(x, y);
            if (players.Length > 0) { 
                float len2 = math.lengthSquared(players.Position[0].Value - newPos);
                if(len2 < minSpawnDist * minSpawnDist) {
                    float2 dir = math.normalize(newPos - players.Position[0].Value);
                    return players.Position[0].Value + dir * minSpawnDist;
                }
            }

            return newPos;
        }
        
    }
}
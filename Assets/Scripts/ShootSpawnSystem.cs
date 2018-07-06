using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace SineOfMadness {
    public class ShootSpawnSystem : ComponentSystem {

        public struct Data {
            public int Length;
            public EntityArray SpawnedEntities;
            [ReadOnly] public ComponentDataArray<ShotSpawnData> SpawnData;
        }

        [Inject] private Data m_Data;

        protected override void OnUpdate() {
            var em = PostUpdateCommands;

            for (int i = 0; i < m_Data.Length; ++i) {
                var sd = m_Data.SpawnData[i];
                var shotEntity = m_Data.SpawnedEntities[i];

                em.RemoveComponent<ShotSpawnData>(shotEntity);
                em.AddSharedComponent(shotEntity, default(MoveForward));
                em.AddComponent(shotEntity, sd.Position);
                em.AddComponent(shotEntity, sd.Heading);
                em.AddComponent(shotEntity, default(TransformMatrix));
                em.AddComponent(shotEntity, new Shot { Energy = Boot.Settings.bulletEnergy });
                em.AddComponent(shotEntity, new PlayerShot());
                em.AddComponent(shotEntity, new MoveSpeed { speed = Boot.Settings.bulletMoveSpeed });
                em.AddComponent(shotEntity, new Health { Value = 1 });
                em.AddComponent(shotEntity, new SpawnableTags { Value = SpawnableTags.FRIENDLY_SHOT });

                em.AddSharedComponent(shotEntity, Boot.PlayerShotLook);
            }
        }
    }
}
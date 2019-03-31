using SineOfMadness;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerEnemyCollisionSystem : JobComponentSystem
    {
        public const float CollisionDistSqrd = 1;
        private ComponentGroup playerGroup;
        private ComponentGroup enemyGroup;
        
        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            playerGroup = GetComponentGroup(ComponentType.ReadOnly<PlayerComponent>(), 
                ComponentType.ReadOnly<Translation>(), 
                typeof(Health));

            enemyGroup = GetComponentGroup(ComponentType.ReadOnly<Enemy>(),
                ComponentType.ReadOnly<Translation>());
        }

//        struct PlayerEnemyCollisionJob : IJobParallelFor
//        {
//            [DeallocateOnJobCompletion] public NativeArray<ArchetypeChunk> EnemyChunks;
//            public ArchetypeChunkComponentType<Translation> TranslationType;
//            [DeallocateOnJobCompletion] public NativeArray<ArchetypeChunk> PlayerChunks;
//            
//            public void Execute(int enemyChunkIndex)
//            {
//                var enemyChunk = EnemyChunks[enemyChunkIndex];
//                var enemyTranslations = enemyChunk.GetNativeArray(TranslationType);
//                var instanceCount = enemyChunk.Count;
//                
//                for (int i = 0; i < instanceCount; i++)
//                {
//                    var enemyTranslation = enemyTranslations[i];
//                    
////                    for(int j = 0; j < )
//                }
//            }
//        }
        
        [BurstCompile]
        struct PlayerEnemyCollisionJob : IJobProcessComponentData<Enemy, Translation, Health>
        {
//            public ArchetypeChunkComponentType<Translation> TranslationType;
//            [DeallocateOnJobCompletion] public NativeArray<ArchetypeChunk> PlayerChunks;
            
            public void Execute([ReadOnly] ref Enemy enemyTag, [ReadOnly] ref Translation position, ref Health health)
            {
//                for (int chunk = 0; chunk < PlayerChunks.Length; chunk++)
//                {
//                    var playerChunk = PlayerChunks[chunk];
//                    var playerTranslations = playerChunk.GetNativeArray(TranslationType);
//                    for (int i = 0, n = playerChunk.Count; i < n; i++)
//                    {
//                        var playerTranslation = playerTranslations[i];
//
//                        if (math.distancesq(playerTranslation.Value, position.Value) <= CollisionDistSqrd)
//                        {
//                            health.Value--;
//                            
//                            
//                        }
//                    }
//                }
            }
        }
    
        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
//            var playerChunks = playerGroup.CreateArchetypeChunkArray(Allocator.TempJob);

            var job = new PlayerEnemyCollisionJob
            {
//                  TranslationType =  GetArchetypeChunkComponentType<Translation>(false),
//                  PlayerChunks = playerChunks
            };
            
            return job.Schedule(this, inputDependencies);
        }
    }
}
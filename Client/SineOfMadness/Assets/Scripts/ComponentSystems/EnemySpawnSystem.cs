using System;
using SineOfMadness;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace DefaultNamespace
{
    public class EnemySpawnSystem : ComponentSystem
    {

        private Random rand = new Random((uint)DateTime.Now.Millisecond);
        protected override void OnUpdate()
        {
            float elapsed = Time.deltaTime;
            Entities.ForEach( (ref SpawnSource spawnSource) =>
            {
                Rect spawnArea = spawnSource.spawnArea;
                spawnSource.currDelay -= elapsed;
                while (spawnSource.currDelay < 0)
                {
                    Entity spawnedEntity = PostUpdateCommands.Instantiate(spawnSource.spawnType);
                    PostUpdateCommands.SetComponent(spawnedEntity, new Translation
                    {
                        Value = new float3(spawnArea.xMin + rand.NextFloat() * spawnArea.xMax - spawnArea.xMin,
                            spawnArea.yMin + rand.NextFloat() * spawnArea.yMax - spawnArea.yMin,
                            0)
                    });

                    spawnSource.currDelay += spawnSource.delayPerSpawn;
                }
            });
        }
    }
}
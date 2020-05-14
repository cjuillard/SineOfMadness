using SineOfMadness;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyFollowSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            float dt = Time.deltaTime;

            if (Boot.Instance.PlayerEntity == null)
                return;
            
            var playerPos = EntityManager.GetComponentData<Translation>(Boot.Instance.PlayerEntity.Value).Value.xy;
            Entities.ForEach((Speed speed, ref Translation translation, ref Enemy enemy) =>
            {
                float2 offset = playerPos - translation.Value.xy;
                translation.Value.xy += math.normalize(offset) * speed.Value * dt;
            });
        }
    }
}
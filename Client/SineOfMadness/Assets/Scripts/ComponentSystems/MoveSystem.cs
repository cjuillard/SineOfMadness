using SineOfMadness;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            float dt = Time.deltaTime;
            Entities.ForEach((ref Translation translation, ref Velocity vel) =>
            {
                float2 newVal = translation.Value.xy + vel.Value * dt;
                translation.Value = new float3(newVal.x, newVal.y, translation.Value.z);
            });
        }
    }
}
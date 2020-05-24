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

            float localFlockmateRadius = Boot.Settings.FlockmateRadius;
            float localFlockmateRadius2 = localFlockmateRadius * localFlockmateRadius;
            float alignmentAcc = Boot.Settings.BoidAlignmentAcceleration * dt;
            float cohesionAcc = Boot.Settings.BoidCohesionAcceleration * dt;
            float goalAcc = Boot.Settings.BoidGoalAcceleration * dt;
            
            var playerPos = EntityManager.GetComponentData<Translation>(Boot.Instance.PlayerEntity.Value).Value.xy;
            Entities.ForEach((Speed speed, ref Velocity vel, ref Translation pos1, ref Enemy enemy) =>
            {
                var pos1Copy = pos1;
                float3 avgPos = pos1.Value;
                float2 avgHeading = vel.Value;
                if (avgHeading.x == 0 && avgHeading.y == 0)
                    avgHeading.x = .00001f;    // just make this non-zero so we don't get NaN's
                
                int localFlockCount = 1;
                Entities.ForEach((ref Velocity vel2, ref Translation pos2, ref Enemy enemy2) =>
                {
                    float3 delta = pos2.Value - pos1Copy.Value;
                    float dist2 = math.lengthsq(delta);
                    if (dist2 <= localFlockmateRadius2)
                    {
                        localFlockCount++;
                        float a = 1f / localFlockCount;
                        avgPos = math.lerp(pos2.Value, avgPos, a);
                        if (vel2.Value.x != 0 || vel2.Value.y != 0)
                        {
                            avgHeading = math.lerp(vel2.Value, avgHeading, a);   
                        }
                    }
                });

                if (avgHeading.x == 0 && avgHeading.y == 0)
                    avgHeading.x = .000001f;
                
                avgHeading = math.normalize(avgHeading);
                
                // TODO Separation - steer to avoid crowding flockmates

                // Alignment - steer towards average heading of local flockmates
                vel.Value += alignmentAcc * avgHeading;
                    
                // Cohesion - steer towards the center position of flockmates
                float2 cohesionOffset = avgPos.xy - pos1.Value.xy;
                if(cohesionOffset.x != 0 || cohesionOffset.y != 0)
                    vel.Value += (cohesionAcc * math.normalize(cohesionOffset)).xy;

                // Goal acceleration
                float2 offset = playerPos - pos1.Value.xy;
                if(offset.x != 0 || offset.y != 0)
                    vel.Value += math.normalize(offset) * goalAcc;
                
                float2 velUncapped = vel.Value;
                float uncappedSpeed2 = math.lengthsq(velUncapped);
                if (uncappedSpeed2 > speed.Value * speed.Value)
                {
                    // Cap at max speed
                    vel.Value = (velUncapped / math.sqrt(uncappedSpeed2)) * speed.Value;
                }
                // float2 offset = playerPos - pos1.Value.xy;
                // pos1.Value.xy += math.normalize(offset) * speed.Value * dt;
            });
        }
    }
}
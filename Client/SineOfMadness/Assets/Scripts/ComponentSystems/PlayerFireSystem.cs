using System;
using SineOfMadness;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerFireSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entity bulletPrototype = Boot.Instance.PlayerBulletEntity;
            float dt = Time.deltaTime;
            Entities.ForEach( (ref Player player, ref PlayerInput playerInput, ref Translation pos, ref Rotation rotation) =>
            {
                player.CurrFireCooldown -= dt;
                if (player.CurrFireCooldown <= 0 && math.lengthsq(playerInput.Shoot) > .5f * .5f)
                {
//                    var newPos = pos + (rotation.Value * new float3(1, 0, 0));
                    var newBullet = PostUpdateCommands.Instantiate(bulletPrototype);
                    float3 vel = math.mul(rotation.Value, new float3(0,0, 5));
                    PostUpdateCommands.AddComponent(newBullet, new Velocity
                    {
                        Value = vel.xy
                    });
                    PostUpdateCommands.SetComponent(newBullet, new Translation
                    {
                        Value = pos.Value
                    });

                    player.CurrFireCooldown += player.FireCooldown;
                }
            });
        }
    }
}
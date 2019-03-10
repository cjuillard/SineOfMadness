using SineOfMadness;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DefaultNamespace
{
    public class PlayerMoveSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            ForEach( (ref Boot.Player player, ref PlayerInput playerInput, ref Translation pos, ref Rotation rotation) =>
            {
                pos.Value.xy += playerInput.Move.xy * player.MaxSpeed;
                if (math.lengthsq(playerInput.Shoot) != 0)
                {
                    rotation.Value = quaternion.LookRotationSafe(new float3(playerInput.Shoot.x, playerInput.Shoot.y, 0),
                        new float3(0,0,1));
                }
            });
        }
    }
}
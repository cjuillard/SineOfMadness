using SineOfMadness;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class RemoveOutOfBoundsSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Rect playArea = Boot.Settings.PlayArea;
        Entities.ForEach((Entity entity, ref BulletTag bullet, ref Translation translation, ref Velocity vel) =>
        {
            if(!playArea.Contains(translation.Value.xy))
                PostUpdateCommands.DestroyEntity(entity);
        });
    }
}
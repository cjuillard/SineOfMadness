using SineOfMadness;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public class RemoveDeadSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref Health health, ref Translation pos) =>
        {
            if (health.Value <= 0)
            {
                PostUpdateCommands.DestroyEntity(entity);

                if (EntityManager.HasComponent<Player>(entity))
                {
                    Boot.Instance.OnPlayerDeath();
                }
//                if (EntityManager.HasComponent(entity, typeof(Player)))
//                {
////                    Settings.PlayerDied();
//                }
//
//                else if (EntityManager.HasComponent(entity, typeof(Enemy)))
//                {
//                    PostUpdateCommands.DestroyEntity(entity);
////                    BulletImpactPool.PlayBulletImpact(pos.Value);
//                }
            }
        });
    }
}
using UnityEngine;
using System.Collections;
using Unity.Entities;
using SineOfMadness;
using Unity.Collections;

[UpdateAfter(typeof(RemoveDeadSystem))]
public class GameOverSystem : ComponentSystem
{
    public struct Players
    {
        public int Length;
        [ReadOnly] public ComponentDataArray<Player> players;
    }

    public struct Round
    {
        public int Length;
        [ReadOnly] public ComponentDataArray<RoundState> state;
    }

    [Inject] Players players;
    [Inject] Round round;

    protected override void OnUpdate()
    {
        if(players.Length == 0 && round.Length > 0)
        {
            // End round
            // TODO re-show button
            var entityManager = World.Active.GetExistingManager<EntityManager>();
            var entities = entityManager.GetAllEntities();
            entityManager.DestroyEntity(entities);
            entities.Dispose();

        }
    }
}

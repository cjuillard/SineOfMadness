using System;
using Unity.Entities;

namespace SineOfMadness
{
    [Serializable]
    public struct EnemyFollowTag : IComponentData
    {
    }

    public class EnemyFollowTagComponent : ComponentDataProxy<EnemyFollowTag>
    {
        
    }
}
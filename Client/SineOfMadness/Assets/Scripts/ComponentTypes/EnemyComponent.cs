using System;
using Unity.Entities;

namespace SineOfMadness
{
    [Serializable]
    public struct Enemy : IComponentData
    {
    }
    
    public class EnemyComponent : ComponentDataProxy<Enemy>
    {
        
    }
}
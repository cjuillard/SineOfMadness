using Unity.Entities;

namespace SineOfMadness
{
    public struct Enemy : IComponentData
    {
    }
    
    public class EnemyComponent : ComponentDataProxy<Enemy>
    {
        
    }
}
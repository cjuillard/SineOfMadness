using System;
using Unity.Entities;

namespace SineOfMadness
{
    [Serializable]
    public struct Health : IComponentData {
        public float Value;
    }
    
    public class HealthComponent : ComponentDataProxy<Health>
    {
        
    }
}
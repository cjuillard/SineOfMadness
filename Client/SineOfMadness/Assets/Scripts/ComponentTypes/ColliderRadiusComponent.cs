using System;
using Unity.Entities;

namespace SineOfMadness
{
    [Serializable]
    public struct ColliderRadius : IComponentData
    {
        public float Radius;
    }
    
    public class ColliderRadiusComponent : ComponentDataProxy<ColliderRadius>
    {
        
    }
}
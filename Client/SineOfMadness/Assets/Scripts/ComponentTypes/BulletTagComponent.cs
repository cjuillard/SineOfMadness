using System;
using Unity.Entities;

namespace SineOfMadness
{
    [Serializable]
    public struct BulletTag : IComponentData
    {
    }
    
    public class BulletTagComponent : ComponentDataProxy<BulletTag>
    {
        
    }
}
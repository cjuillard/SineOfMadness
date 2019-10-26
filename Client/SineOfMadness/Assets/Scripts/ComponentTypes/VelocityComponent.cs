using System;
using Unity.Entities;
using Unity.Mathematics;

namespace SineOfMadness
{
    [Serializable]
    public struct Velocity : IComponentData
    {
        public float2 Value;
    }

    public class VelocityComponent : ComponentDataProxy<Velocity>
    {
        
    }
}
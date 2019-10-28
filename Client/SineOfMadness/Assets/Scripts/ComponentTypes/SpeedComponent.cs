using System;
using Unity.Entities;

namespace SineOfMadness
{
    [Serializable]
    public struct Speed : ISharedComponentData
    {
        public float Value;
    }

    public class SpeedComponent : SharedComponentDataProxy<Speed>
    {
        
    }
}
using System;
using Unity.Entities;

namespace SineOfMadness
{
    [Serializable]
    public struct Player : IComponentData
    {
        public float MaxSpeed;
    }

    public class PlayerComponent : ComponentDataProxy<Player>
    {
        
    }
}
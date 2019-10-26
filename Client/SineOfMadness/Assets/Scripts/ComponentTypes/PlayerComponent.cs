using System;
using Unity.Entities;

namespace SineOfMadness
{
    [Serializable]
    public struct Player : IComponentData
    {
        public float MaxSpeed;
        public float FireCooldown;    // Time between fires
        public float CurrFireCooldown;    // Current time between firing
    }

    public class PlayerComponent : ComponentDataProxy<Player>
    {
        
    }
}
using System;
using Unity.Entities;
using Unity.Mathematics;

namespace SineOfMadness
{
    [Serializable]
    public struct PlayerInput : IComponentData {
        public float2 Move;
        public float2 Shoot;
    }
}
using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace SineOfMadness
{
    [Serializable]
    public struct SpawnSource : IComponentData
    {
        public Entity spawnType;
        public Rect spawnArea;
        public float delayPerSpawn;
        public float currDelay;
    }
}
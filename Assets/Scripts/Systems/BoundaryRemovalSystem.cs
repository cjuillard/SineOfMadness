using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Transforms2D;
using SineOfMadness;
using Unity.Collections;
using Unity.Jobs;

class BoundaryRemovalSystem : JobComponentSystem {
    public struct BoundaryKillJob : IJobProcessComponentData<Health, Position2D> {
        public float MinY, MaxY;
        public float MinX, MaxX;

        public void Execute(ref Health health, [ReadOnly] ref Position2D pos) {
            if (pos.Value.x > MaxX || pos.Value.x < MinX ||
                pos.Value.y > MaxY || pos.Value.y < MinY) {
                health.Value = -1.0f;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        //if (TwoStickBootstrap.Settings == null)
        //    return inputDeps;

        var boundaryKillJob = new BoundaryKillJob {
            MinX = Boot.Settings.playfield.xMin,
            MaxX = Boot.Settings.playfield.xMax,
            MinY = Boot.Settings.playfield.yMin,
            MaxY = Boot.Settings.playfield.yMax,
        };

        return boundaryKillJob.Schedule(this, 64, inputDeps);
    }
}

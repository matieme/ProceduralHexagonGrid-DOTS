using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class GridCreatorSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithBurst(synchronousCompilation: true)
            .ForEach((ref Translation position, ref GridDataComponent gridDataComponent) =>
            {
                for (int z = -gridDataComponent.worldHalfSize; z <= gridDataComponent.worldHalfSize; z++)
                {
                    for (int x = -gridDataComponent.worldHalfSize; x <= gridDataComponent.worldHalfSize; x++)
                    {
                        //Instantiate grid
                    }
                }
            }).ScheduleParallel();
    }
}

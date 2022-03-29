using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class GridCreatorSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithBurst(synchronousCompilation: true)            
            .ForEach((ref Translation position, ref HexDataComponent blockData) =>
            {
                float3 vertex = position.Value;
                float perlin = Mathf.PerlinNoise(vertex.x * 3f, vertex.z * 4f);
                
                position.Value = new float3(vertex.x, perlin, vertex.z);

            }).ScheduleParallel();
    }
}

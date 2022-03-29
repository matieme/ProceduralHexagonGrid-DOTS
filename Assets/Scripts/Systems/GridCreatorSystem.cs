using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public partial class GridCreatorSystem : SystemBase
{
    private EntityQuery blockQuery;

    protected override void OnCreate()
    {
        blockQuery = GetEntityQuery(typeof(HexDataComponent));
    }
    
    protected override void OnUpdate()
    {
        float strength = GameDataManager.strength;
        float scale = GameDataManager.scale;
        float perlinScaleMultiplierOne = GameDataManager.perlinScaleMultiplierOne;
        float perlinScaleMultiplierTwo = GameDataManager.perlinScaleMultiplierTwo;
        float perlinScaleMultiplierThree = GameDataManager.perlinScaleMultiplierThree;
        float perlinStrengthMultiplierOne = GameDataManager.perlinStrengthMultiplierOne;
        float perlinStrengthMultiplierTwo = GameDataManager.perlinStrengthMultiplierTwo;
        float perlinStrengthMultiplierThree = GameDataManager.perlinStrengthMultiplierThree;

        Entities
            .WithBurst(synchronousCompilation: true)            
            .ForEach((ref Translation position, ref HexDataComponent blockData) =>
            {
                float3 vertex = position.Value;
                float perlin1 = Mathf.PerlinNoise(vertex.x * scale * perlinScaleMultiplierOne, vertex.z * scale * perlinScaleMultiplierOne) * strength * perlinStrengthMultiplierOne;
                float perlin2 = Mathf.PerlinNoise(vertex.x * scale * perlinScaleMultiplierTwo, vertex.z * scale * perlinScaleMultiplierTwo) * strength * perlinStrengthMultiplierTwo;
                float perlin3 = Mathf.PerlinNoise(vertex.x * scale * perlinScaleMultiplierThree, vertex.z * scale * perlinScaleMultiplierThree) * strength * perlinStrengthMultiplierThree;
                var height = perlin1 + perlin2 + perlin3;

                position.Value = new float3(vertex.x, height, vertex.z);
            }).ScheduleParallel();

        Dependency.Complete();
            
        if (GameDataManager.changedData)
        {
            using (NativeArray<Entity> blockEntities = blockQuery.ToEntityArray(Allocator.TempJob))
            {
                foreach (var entity in blockEntities)
                {
                    float height = EntityManager.GetComponentData<Translation>(entity).Value.y;

                    Entity hex;

                    if (height <= GameDataManager.waterHeightLevel)
                    {
                        hex = GameDataManager.water;
                    }
                    else if (height <= GameDataManager.sandHeightLevel)
                    {
                        hex = GameDataManager.sand;
                    }
                    else if(height <= GameDataManager.dirtHeightLevel)
                    {
                        hex = GameDataManager.dirt;
                    }
                    else if (height <= GameDataManager.forestHeightLevel)
                    {
                        hex = GameDataManager.forest;
                    }
                    else if (height <= GameDataManager.rockHeightLevel)
                    {
                        hex = GameDataManager.rock;
                    }
                    else
                    {
                        hex = GameDataManager.snow;
                    }

                    RenderMesh colourRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(hex);
                    var entityRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(entity);
                    entityRenderMesh.material = colourRenderMesh.material;
                    EntityManager.SetSharedComponentData(entity, entityRenderMesh);
                }
            }

            GameDataManager.changedData = false;
        }
    }
}

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("World Data")]
    [SerializeField] private WorldBaseData worldData;
    
    [Header("General Perlin Values")]
    [Range(0.1f, 10f)] public float strength = 1f;
    [Range(0.01f, 1f)] public float scale = 0.1f;
    
    [Header("Perlin Noise Params #1")]
    [Range(0.1f, 10f)] public float perlinStrengthMultiplierOne = 1f;
    [Range(0.01f, 1f)] public float perlinScaleMultiplierOne = 0.1f;

    [Header("Perlin Noise Params #2")]
    [Range(0.1f, 10f)] public float perlinStrengthMultiplierTwo = 1f;
    [Range(0.01f, 1f)] public float perlinScaleMultiplierTwo = 0.1f;
    
    [Header("Perlin Noise Params #3")]
    [Range(0.1f, 10f)] public float perlinStrengthMultiplierThree = 1f;
    [Range(0.1f, 1f)] public float perlinScaleMultiplierThree = 0.1f;
    
    [Header("Height Values")]
    [Range(0f, 100f)] public float waterHeightLevel = 2f;
    [Range(0f, 100f)] public float sandHeightLevel = 4f;
    [Range(0f, 100f)] public float dirtHeightLevel = 6f;
    [Range(0f, 100f)] public float forestHeightLevel = 8f;
    [Range(0f, 100f)] public float rockHeightLevel = 10f;
    [Range(0f, 100f)] public float snowHeightLevel = 12f;

    
    private Entity prefab;
    private EntityManager manager;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        ConvertPrefabToEntity();
        
        for (int z = -worldData.worldHalfSize; z <= worldData.worldHalfSize; z++)
        {
            for (int x = -worldData.worldHalfSize; x <= worldData.worldHalfSize; x++)
            {
                float height = Mathf.PerlinNoise(x * scale * perlinScaleMultiplierOne, z * scale * perlinScaleMultiplierOne) * strength * perlinStrengthMultiplierOne;
                var position = new float3(x, 0, z)
                {
                    x = (x + z * 0.5f - z / 2) * (HexMetrics.apothem * 2f),
                    z = z * HexMetrics.area
                };
                Entity instance = CreateEntityByHeight(height);

                manager.SetComponentData(instance, new Translation{Value = position});
            }
        }
    }
    
    private void OnValidate()
    {
        GameDataManager.changedData = true;
    }

    private void ConvertPrefabToEntity()
    {
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        GameDataManager.water = GameObjectConversionUtility.ConvertGameObjectHierarchy(worldData.waterPrefab, settings);
        GameDataManager.sand = GameObjectConversionUtility.ConvertGameObjectHierarchy(worldData.sandPrefab, settings);
        GameDataManager.dirt = GameObjectConversionUtility.ConvertGameObjectHierarchy(worldData.dirtPrefab, settings);
        GameDataManager.forest = GameObjectConversionUtility.ConvertGameObjectHierarchy(worldData.forestPrefab, settings);
        GameDataManager.rock = GameObjectConversionUtility.ConvertGameObjectHierarchy(worldData.rockPrefab, settings);
        GameDataManager.snow = GameObjectConversionUtility.ConvertGameObjectHierarchy(worldData.snowPrefab, settings);
    }

    private Entity CreateEntityByHeight(float height)
    {
        if (height <= GameDataManager.waterHeightLevel)
        {
            return manager.Instantiate(GameDataManager.water);
        }
        else if (height <= GameDataManager.sandHeightLevel)
        {
            return manager.Instantiate(GameDataManager.sand);
        }
        else if(height <= GameDataManager.dirtHeightLevel)
        {
            return manager.Instantiate(GameDataManager.dirt);
        }
        else if (height <= GameDataManager.forestHeightLevel)
        {
            return manager.Instantiate(GameDataManager.forest);
        }
        else if (height <= GameDataManager.rockHeightLevel)
        {
            return manager.Instantiate(GameDataManager.rock);
        }
        else
        {
            return manager.Instantiate(GameDataManager.snow);
        }
    }
    
    private void Update()
    {
        GameDataManager.strength = strength;
        GameDataManager.scale = scale;
        GameDataManager.perlinStrengthMultiplierOne = perlinStrengthMultiplierOne;
        GameDataManager.perlinScaleMultiplierOne = perlinScaleMultiplierOne;
        GameDataManager.perlinStrengthMultiplierTwo = perlinStrengthMultiplierTwo;
        GameDataManager.perlinScaleMultiplierTwo = perlinScaleMultiplierTwo;
        GameDataManager.perlinStrengthMultiplierThree = perlinStrengthMultiplierThree;
        GameDataManager.perlinScaleMultiplierThree = perlinScaleMultiplierThree;

        GameDataManager.waterHeightLevel = waterHeightLevel;
        GameDataManager.sandHeightLevel = sandHeightLevel;
        GameDataManager.dirtHeightLevel = dirtHeightLevel;
        GameDataManager.forestHeightLevel = forestHeightLevel;
        GameDataManager.rockHeightLevel = rockHeightLevel;
        GameDataManager.snowHeightLevel = snowHeightLevel;
    }
}

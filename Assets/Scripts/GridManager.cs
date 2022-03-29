using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private EntityManager manager;
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private int worldHalfSize = 75;

    private Entity prefab;

    private void Start()
    {
        
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(grassPrefab, settings);

        for (int z = -worldHalfSize; z <= worldHalfSize; z++)
        {
            for (int x = -worldHalfSize; x <= worldHalfSize; x++)
            {
                var position = new float3(x, 0, z);
                position.x = (x + z * 0.5f - z / 2) * (HexMetrics.apothem * 2f);
                position.z = z * HexMetrics.area;
                Entity instance = manager.Instantiate(prefab);;
                
                manager.SetComponentData(instance, new Translation{Value = position});
            }
        }
        
    }
}

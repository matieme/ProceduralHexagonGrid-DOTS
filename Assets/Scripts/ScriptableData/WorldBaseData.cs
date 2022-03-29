using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "ScriptableObjects/WorldBaseData", order = 1)]
public class WorldBaseData : ScriptableObject
{
    [Header("Hexagon prefabs")]
    public GameObject waterPrefab;
    public GameObject sandPrefab;
    public GameObject dirtPrefab;
    public GameObject forestPrefab;
    public GameObject rockPrefab;
    public GameObject snowPrefab;
    
    public int worldHalfSize = 75;
}

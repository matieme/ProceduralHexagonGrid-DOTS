using Unity.Entities;

public static class GameDataManager
{
    public static float scale;
    public static float strength;
    
    public static float perlinStrengthMultiplierOne;
    public static float perlinScaleMultiplierOne;

    public static float perlinStrengthMultiplierTwo;
    public static float perlinScaleMultiplierTwo;
    
    public static float perlinStrengthMultiplierThree;
    public static float perlinScaleMultiplierThree;

    public static Entity water;
    public static Entity sand;
    public static Entity dirt;
    public static Entity forest;
    public static Entity rock;
    public static Entity snow;
    
    public static float waterHeightLevel = 2f;
    public static float sandHeightLevel = 4f;
    public static float dirtHeightLevel = 6f;
    public static float forestHeightLevel = 10f;
    public static float rockHeightLevel = 12f;
    public static float snowHeightLevel = 14f;

    public static bool changedData = false;
}

using Unity.Entities;

[GenerateAuthoringComponent]
public struct GridDataComponent : IComponentData
{
    public int worldHalfSize;
}

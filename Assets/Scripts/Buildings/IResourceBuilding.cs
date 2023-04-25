namespace Assets.Scripts.Buildings
{
    public interface IResourceBuilding
    {
        int InnerStorage { get; set; }
        float TimeLeft { get; set; }
        string ResourceType { get; }

        void CollectStorage();
        int GetInstanceID();
    }
}
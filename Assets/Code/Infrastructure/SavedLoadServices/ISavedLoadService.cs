namespace Code.Infrastructure.SavedLoadServices
{
    public interface ISavedLoadService
    {
        bool HasSavedData { get; }
        SavedData SavedData { get; }
        void Save(SavedData savedData);
    }
}
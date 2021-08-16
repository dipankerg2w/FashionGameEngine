namespace EditorTools
{
    public interface ITools
    {
        string GetName { get; }
        void DoUpdate();
    }
}
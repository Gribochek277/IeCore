namespace Irrational.Utils.Interfaces
{
    public interface IModelLoader
    {
        Mesh LoadFromFile(string path);
        Mesh LoadFromString(string objModel);
    }
}

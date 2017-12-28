namespace Irrational.Utils.Interfaces
{
    interface IModelLoader
    {
        Mesh LoadFromFile(string path);
        Mesh LoadFromString(string objModel);
    }
}

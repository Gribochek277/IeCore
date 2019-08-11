using IrrationalEngineCore.Core.Entities;

namespace IrrationalEngineCore.Loaders.Interfaces
{
    public interface IModelLoader
    {
        Mesh LoadFromFile(string path);
    }
}

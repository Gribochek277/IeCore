using Irrational.Core.Entities;

namespace Irrational.Loaders.Interfaces
{
    public interface IModelLoader
    {
        Mesh LoadFromFile(string path);
    }
}

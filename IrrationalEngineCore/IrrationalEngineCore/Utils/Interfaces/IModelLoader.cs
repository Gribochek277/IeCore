using Irrational.Core.Entities;

namespace Irrational.Utils.Interfaces
{
    public interface IModelLoader
    {
        Mesh LoadFromFile(string path);
    }
}

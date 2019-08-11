using IrrationalEngineCore.Core.Entities;
using System.Collections.Generic;

namespace IrrationalEngineCore.Loaders.Interfaces
{
    public interface IMaterialLoader
    {
        Dictionary<string, Material> LoadFromFile(string path);
    }
}

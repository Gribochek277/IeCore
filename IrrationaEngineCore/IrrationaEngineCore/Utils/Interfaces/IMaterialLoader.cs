using Irrational.Core.Entities;
using System.Collections.Generic;

namespace Irrational.Utils.Interfaces
{
    public interface IMaterialLoader
    {
        Dictionary<string, Material> LoadFromFile(string path);
    }
}

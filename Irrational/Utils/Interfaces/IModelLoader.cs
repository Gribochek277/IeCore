using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrrationalSpace.Utils.Interfaces
{
    interface IModelLoader
    {
        Mesh LoadFromFile(string path);
        Mesh LoadFromString(string objModel);
    }
}

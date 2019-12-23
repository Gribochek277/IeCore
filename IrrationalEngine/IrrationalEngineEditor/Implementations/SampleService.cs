using IrrationalEngineEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IrrationalEngineEditor.Implementations
{
    public class SampleService : ISampleService
    {
        public string GetCurrentDate() => DateTime.Now.ToLongDateString();
    }
}

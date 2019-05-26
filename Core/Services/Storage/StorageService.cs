using System;
using System.Collections.Generic;
using System.Text;
using SharedKernel.Engines;

namespace Core.Services.Storage
{
    public class StorageService
    {
        private readonly IEngine _engine;

        public  StorageService(IEngine engine)
        {
            _engine = engine;
        }


    }
}

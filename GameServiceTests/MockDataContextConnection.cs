using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository;

namespace GameServiceTests
{
    class MockDataContextConnection : IDataContextConnection
    {
        public UnitOfWork Database
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

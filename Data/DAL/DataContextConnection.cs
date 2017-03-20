using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository;

namespace Data.DAL
{
    public class DataContextConnection : IDataContextConnection
    {
        public UnitOfWork Database
        {
            get
            {
                return new UnitOfWork();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FootballDataApiV2.Interfaces
{
    public interface IApi<T> where T: class
    {
        IEnumerable<T> Get();
    }
}

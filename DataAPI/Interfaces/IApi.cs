using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Interfaces
{
    public interface IApi<T> where T: IModel
    {
        List<T> Get();
        List<T> Get(int id);
        List<T> Get(string name);
    }
}

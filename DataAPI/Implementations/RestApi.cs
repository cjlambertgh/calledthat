using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Implementations
{
    public class RestApi
    {
        private readonly string _uri;

        public RestApi(string uri)
        {
            _uri = uri;
        }

        public string Get()
        {
            using (var wc = new HttpClient())
            {
                var response = wc.GetAsync(_uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    return data;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}

using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballDataApiV2.Implementations
{
    public class BaseApi
    {
        public RestClient FootballDataApiClient => new RestClient("http://api.football-data.org");
    }
}

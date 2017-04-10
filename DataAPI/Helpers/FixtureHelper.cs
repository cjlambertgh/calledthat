using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Helpers
{
    public static class FixtureHelper
    {
        public static bool IsFixtureInPlay(Fixture fixture)
        {
            return (fixture.FixtureStatus == FixtureStatus.InPlay);
        }

        public static bool IsFixtureInFinished(Fixture fixture)
        {
            return (fixture.FixtureStatus == FixtureStatus.Finished);
        }
    }
}

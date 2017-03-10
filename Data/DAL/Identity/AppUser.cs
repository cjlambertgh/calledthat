using Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.DAL.Identity
{
    public class AppUser : IdentityUser
    {
        public virtual ICollection<Player> Players { get; set; }
    }
}
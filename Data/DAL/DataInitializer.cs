using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAL
{
    sealed class DataInitializer : System.Data.Entity.MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>
    {
        //protected override void Seed(DataContext context)
        //{
            //if (context.Competitions.Any()) return;

            //var sqlAssembly = Assembly.Load(new AssemblyName("Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
            //foreach (var resourcename in sqlAssembly.GetManifestResourceNames())
            //{
            //    if (!resourcename.Contains(".sql")) continue;
            //    using (var stream = sqlAssembly.GetManifestResourceStream(resourcename))
            //    {
            //        using (var reader = new StreamReader(stream))
            //        {
            //            var resource = reader.ReadToEnd();

            //            try
            //            {
            //                string cmdtext = null;
            //                if (resource.ToUpper().IndexOf("CREATE PROCEDURE") >= 0)
            //                {
            //                    cmdtext = resource.Substring(resource.ToUpper().IndexOf("CREATE PROCEDURE"));
            //                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
            //                        cmdtext);
            //                }
            //                else if (resource.IndexOf("CREATE FUNCTION") >= 0)
            //                {
            //                    cmdtext = resource.Substring(resource.IndexOf("CREATE FUNCTION"));
            //                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmdtext);
            //                }
            //            }
            //            catch (Exception exc)
            //            {
            //                //errors.Add($"Fatal error in SSRS.SQL.dll resource:{resourcename} Error was:{exc.Message}");
            //            }
            //        }
            //    }
            //}
            //base.Seed(context);
        //}
    }
}

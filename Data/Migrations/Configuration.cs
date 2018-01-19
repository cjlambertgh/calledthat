namespace Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.DataContext context)
        {

            if (context.Competitions.Any()) return;

            var sqlAssembly = Assembly.Load(new AssemblyName("Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
            foreach (var resourcename in sqlAssembly.GetManifestResourceNames())
            {
                if (!resourcename.Contains(".sql")) continue;
                using (var stream = sqlAssembly.GetManifestResourceStream(resourcename))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var resource = reader.ReadToEnd();

                        try
                        {
                            string cmdtext = null;
                            if (resource.ToUpper().IndexOf("CREATE PROCEDURE") >= 0)
                            {
                                cmdtext = resource.Substring(resource.ToUpper().IndexOf("CREATE PROCEDURE"));
                                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                                    cmdtext);
                            }
                            else if (resource.IndexOf("CREATE FUNCTION") >= 0)
                            {
                                cmdtext = resource.Substring(resource.IndexOf("CREATE FUNCTION"));
                                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmdtext);
                            }
                        }
                        catch (Exception)
                        {
                            //errors.Add($"Fatal error in SSRS.SQL.dll resource:{resourcename} Error was:{exc.Message}");
                        }
                    }
                }
            }
            base.Seed(context);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

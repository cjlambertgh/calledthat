using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Data.Repository;
using Data.Interfaces;
using Data.DAL;
using GameServices;
using Microsoft.AspNet.Identity;
using Data.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CalledThat.Controllers;
using System.Data.Entity;
using Data;
using AppServices;
using System.Web.Mvc;
using Microsoft.Practices.Unity.Mvc;
using EmailService;

namespace CalledThat.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<IDataContextConnection, DataContextConnection>();
            container.RegisterType<IGameService, GameServices.GameService>();
            container.RegisterType<IGameEmailService, GameEmailService>();
            container.RegisterType<ILeagueService, LeagueService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IPlayerService, PlayerService>();
            container.RegisterType<IMailService, SmtpMailService>();
            container.RegisterType<IUserStore<AppUser>, UserStore<AppUser>>();
            container.RegisterType<IReminderService, ReminderService>();

            container.RegisterType<DbContext, DataContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<AppUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<AppUser>, UserStore<AppUser>>(new HierarchicalLifetimeManager());
            //container.RegisterType<AccountController>(new InjectionConstructor());

            container.RegisterType<FootballDataApiV2.Interfaces.ICompetitionApi, FootballDataApiV2.Implementations.CompetitionApi>();
            container.RegisterType<FootballDataApiV2.Interfaces.ITeamApi, FootballDataApiV2.Implementations.TeamApi>();
            container.RegisterType<FootballDataApiV2.Interfaces.IMatchApi, FootballDataApiV2.Implementations.MatchApi>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}

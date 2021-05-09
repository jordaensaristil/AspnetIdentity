using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ProjetoAspNetIdentity.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

[assembly: OwinStartup(typeof(ProjetoAspNetIdentity.Startup))]


namespace ProjetoAspNetIdentity
{
    public class Startup
    {
        public void Configuration(IAppBuilder Builder)
        {

            SqlConnection conn = new SqlConnection(@"Server=DESKTOP-NV0Q7I6\SQLEXPRESS01;Database=MinhaAgenda;trusted_connection=true");
            conn.Open();

            Builder.CreatePerOwinContext<DbContext>(() =>
            new IdentityDbContext<UsuarioAplicacao>("DefaultConnection"));

          //  conn.Close();

            Builder.CreatePerOwinContext<IUserStore<UsuarioAplicacao>>(
            (opcoes, contextoOwin) =>
            {
                var dbContext = contextoOwin.Get<DbContext>();
                return new UserStore<UsuarioAplicacao>(dbContext);
            });

          
           Builder.CreatePerOwinContext<UserManager<UsuarioAplicacao>>(
           (opcoes, contextoOwin) =>
           {
               var userStore = contextoOwin.Get<IUserStore<UsuarioAplicacao>>();

               var usermanager = new UserManager<UsuarioAplicacao>(userStore);
               var userValidator = new UserValidator<UsuarioAplicacao>(usermanager);
               userValidator.RequireUniqueEmail = true;
               userValidator.AllowOnlyAlphanumericUserNames = true;

               usermanager.UserValidator = userValidator;

               return usermanager;
           });

        }
    }
}
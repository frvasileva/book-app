using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Data;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API {
  public class Startup {
    public Startup (IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices (IServiceCollection services) {
      services.AddDbContext<DataContext> (x => x.UseSqlServer (Configuration.GetConnectionString ("DefaultConnection")));

      IdentityBuilder builder = services.AddIdentityCore<User> (opt => {
        opt.Password.RequireDigit = false;
        opt.Password.RequiredLength = 6;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = true;
        opt.User.RequireUniqueEmail = true;
      });

      builder = new IdentityBuilder (builder.UserType, typeof (Role), builder.Services);
      builder.AddEntityFrameworkStores<DataContext> ();
      builder.AddRoleValidator<RoleValidator<Role>> ();
      builder.AddRoleManager<RoleManager<Role>> ();
      builder.AddSignInManager<SignInManager<User>> ();

      services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer (Options => {
          Options.TokenValidationParameters = new TokenValidationParameters {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes (Configuration.GetSection ("AppSettings:Token").Value)),
          ValidateAudience = false
          };
        });

      // Every user have to authorized by default. Use [AllowAnonymous] attribute 
      services.AddMvc (options => {
          var policy = new AuthorizationPolicyBuilder ()
            .RequireAuthenticatedUser ()
            .Build ();

          options.Filters.Add (new AuthorizeFilter (policy));
        })

        .SetCompatibilityVersion (CompatibilityVersion.Version_2_2)
        .AddJsonOptions (opt => {
          opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
      services.AddCors ();
      services.Configure<CloudinarySettings> (Configuration.GetSection ("CloudinarySettings"));

      services.AddAutoMapper ();
      services.AddScoped<IUserRepository, UserRepository> ();
      services.AddScoped<IProfileRepository, ProfileRepository> ();
      services.AddScoped<IBookRepository, BookRepository> ();
      services.AddScoped<IAuthorRepository, AuthorRepository> ();
      services.AddScoped<ICatalogRepository, CatalogRepository> ();
      services.AddTransient<DbContext> ();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
      } else {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts ();
      }

      app.UseHttpsRedirection ();
      app.UseCors (x => x.AllowAnyOrigin ().AllowAnyHeader ().AllowAnyMethod ());
      app.UseAuthentication ();
      app.UseMvc ();
    }
  }
}
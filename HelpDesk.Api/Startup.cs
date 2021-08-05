using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Api.Model.Context;
using HelpDesk.Api.Model.Interfaces;
using HelpDesk.Api.Model.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using HelpDesk.Api.Entities.Services;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace HelpDesk.Api
{
    public class Startup
    {

        readonly string MyPoliciy = "_MyPoliciy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<HelpDeskContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            services.AddScoped<IAltaTicket, AltaTicketRepository>();
            services.AddCors(opts =>
            {
                opts.AddPolicy(name: MyPoliciy, 
                    builder => 
                    {
                        builder.WithOrigins("https://telintecapp.com", "https//telintecapp.com", "http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                    });
            });
   
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<EmailSenderOptions>(Configuration.GetSection("EmailSenderOptions"));
       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors();

            app.UseCors(this.MyPoliciy);
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            //    RequestPath = new PathString("/Resources")
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                     .RequireCors(MyPoliciy);
            });
        }
    }
}

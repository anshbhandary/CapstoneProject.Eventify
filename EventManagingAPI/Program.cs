
using AutoMapper;
using EventManagingAPI.Data;
using EventManagingAPI.Repository;
using EventManagingAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<EventManagingContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("csManagingDb"));
            });

            builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
            builder.Services.AddScoped<IItemRequiredRepository, ItemRequiredRepository>();
            builder.Services.AddScoped<IManagedEventRepository, ManagedEventRepository>();

            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

namespace GlobalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ Add HttpClient support
            builder.Services.AddHttpClient();

            // ✅ Add controllers
            builder.Services.AddControllers();

            // ✅ Add Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ✅ Access configuration (if needed in services later)
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddCors(options =>

            {

                options.AddPolicy("AllowAngularApp", policy =>

                {

                    policy.WithOrigins("http://localhost:4200")

                       .AllowAnyHeader()

                       .AllowAnyMethod();

                });

            });

            var app = builder.Build();

            // ✅ Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("AllowAngularApp");
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

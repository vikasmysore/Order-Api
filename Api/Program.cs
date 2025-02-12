using Api.Mappers;
using Api.Validations;
using Application.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Persistence.Extensions;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();

            builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<OrderApiMappingProfile>(); });
            builder.Services.RegisterApplicationServices(builder.Configuration);
            builder.Services.RegisterInMemoryAdapterExtensions();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

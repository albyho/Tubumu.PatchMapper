using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Tubumu.PatchMapper.Test;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddMvcCore(options =>
        {
            //options.ModelBinderProviders.Insert(0, new KeysModelBinderProvider());
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        });
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddPatchMapper();

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}


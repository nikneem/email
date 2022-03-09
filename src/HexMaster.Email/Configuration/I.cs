using HexMaster.Email.Abstractions.Services;
using HexMaster.Email.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HexMaster.Email.Configuration
{

    public static class ServiceCollectionExtensions
    {

        public static void ConfigureEmailService(this IServiceCollection serviceCollection)
        {

            var configuration = serviceCollection
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            serviceCollection.Configure<EmailOptions>(configuration);

            serviceCollection.AddScoped<IMailService, MailService>();

        }
    }
}
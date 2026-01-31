using FinanControl.App.Converters;
using FinanControl.App.Services;
using FinanControl.App.ViewModels;
using FinanControl.App.Views;
using CommunityToolkit.Maui;
using FinanControl.Infra.Data;
using FinanControl.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanControl.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configurar banco de dados
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "financontrol.db");
            options.UseSqlite($"Filename={databasePath}");
        });

        // Registrar repositórios
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Registrar serviços
        builder.Services.AddSingleton<AuthService>();

        // Registrar ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegistroViewModel>();
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddSingleton<ContasViewModel>();
        builder.Services.AddSingleton<CategoriasViewModel>();
        builder.Services.AddSingleton<FontesRendaViewModel>();
        builder.Services.AddSingleton<TransacoesViewModel>();

        // Registrar Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegistroPage>();
        builder.Services.AddSingleton<DashboardPage>();
        builder.Services.AddSingleton<ContasPage>();
        builder.Services.AddSingleton<CategoriasPage>();
        builder.Services.AddSingleton<FontesRendaPage>();
        builder.Services.AddSingleton<TransacoesPage>();

        // Registrar conversores
        builder.Services.AddSingleton<NotNullToBoolConverter>();
        builder.Services.AddSingleton<TipoTransacaoToColorConverter>();
        builder.Services.AddSingleton<SaldoToColorConverter>();
        builder.Services.AddSingleton<SaldoToStatusConverter>();
        builder.Services.AddSingleton<BoolToColorConverter>();
        builder.Services.AddSingleton<BoolToStatusConverter>();
        builder.Services.AddSingleton<CategoriaToIdConverter>();
        builder.Services.AddSingleton<ContaToIdConverter>();

        var mauiApp = builder.Build();

        ServiceLocator.ServiceProvider = mauiApp.Services;



        return mauiApp;
    }
}
using FinanControl.App.Converters;
using FinanControl.App.ViewModels;
using FinanControl.App.Views;
using Microsoft.Extensions.Logging;

namespace FinanControl.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<ContasViewModel>();
            builder.Services.AddSingleton<ContasPage>();

            builder.Services.AddSingleton<NotNullToBoolConverter>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

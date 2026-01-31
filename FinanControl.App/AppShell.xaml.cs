using FinanControl.App.Views;

namespace FinanControl.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Registrar rotas para navegação
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegistroPage), typeof(RegistroPage));
    }
}
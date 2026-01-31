using FinanControl.App.Services;
using FinanControl.App.ViewModels;
using FinanControl.App.Views;
using Microsoft.Extensions.DependencyInjection;

namespace FinanControl.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Obter serviços do contêiner de DI
        var authService = ServiceLocator.GetService<AuthService>();

        // Criar ViewModel e Page
        var loginViewModel = new LoginViewModel(authService);
        var loginPage = new LoginPage(loginViewModel);

        MainPage = new NavigationPage(loginPage);
    }

    // Método para navegar para o AppShell após login
    public void NavigateToMainApp()
    {
        MainPage = new AppShell();
    }

    // Método para voltar para a tela de Login (logout)
    public void NavigateToLogin()
    {
        var authService = ServiceLocator.GetService<AuthService>();
        var loginViewModel = new LoginViewModel(authService);
        var loginPage = new LoginPage(loginViewModel);
        MainPage = new NavigationPage(loginPage);
    }
}
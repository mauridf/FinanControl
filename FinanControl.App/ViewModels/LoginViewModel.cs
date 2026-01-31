using FinanControl.App.Services;
using System.Windows.Input;

namespace FinanControl.App.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _email = string.Empty;
    private string _senha = string.Empty;
    private readonly AuthService _authService;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Senha
    {
        get => _senha;
        set => SetProperty(ref _senha, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand RegistrarCommand { get; }

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;

        LoginCommand = new Command(async () => await Login());
        RegistrarCommand = new Command(async () => await Registrar());
    }

    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
        {
            await Shell.Current.DisplayAlert("Erro", "Preencha todos os campos", "OK");
            return;
        }

        IsBusy = true;

        try
        {
            var sucesso = await _authService.LoginAsync(Email, Senha);
            if (sucesso)
            {
                // Navegar para Dashboard
                await Shell.Current.GoToAsync("//DashboardPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Erro", "Email ou senha inválidos", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro ao fazer login: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task Registrar()
    {
        await Shell.Current.GoToAsync("//RegistroPage");
    }
}
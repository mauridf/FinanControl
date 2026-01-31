using System.Windows.Input;
using FinanControl.App.Services;
using FinanControl.App.Views;

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
            await Application.Current.MainPage.DisplayAlert("Erro", "Preencha todos os campos", "OK");
            return;
        }

        IsBusy = true;

        try
        {
            var sucesso = await _authService.LoginAsync(Email, Senha);
            if (sucesso)
            {
                // Navegar para o AppShell
                if (Application.Current is App app)
                {
                    app.NavigateToMainApp();
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Email ou senha inválidos", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao fazer login: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task Registrar()
    {
        // Navegar para a página de registro
        await Application.Current.MainPage.Navigation.PushAsync(CreateRegistroPage());
    }

    private RegistroPage CreateRegistroPage()
    {
        // Usar o AuthService já injetado no view model em vez de acessar um campo privado inexistente em App
        var viewModel = new RegistroViewModel(_authService);
        return new RegistroPage(viewModel);
    }
}
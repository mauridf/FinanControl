using FinanControl.App.Services;
using FinanControl.Core.Entities;
using System.Windows.Input;

namespace FinanControl.App.ViewModels;

public class RegistroViewModel : BaseViewModel
{
    private string _nome = string.Empty;
    private string _email = string.Empty;
    private string _telefone = string.Empty;
    private DateTime _dataNascimento = DateTime.Now.AddYears(-18);
    private string _endereco = string.Empty;
    private string _senha = string.Empty;
    private string _confirmarSenha = string.Empty;
    private readonly AuthService _authService;

    public string Nome
    {
        get => _nome;
        set => SetProperty(ref _nome, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Telefone
    {
        get => _telefone;
        set => SetProperty(ref _telefone, value);
    }

    public DateTime DataNascimento
    {
        get => _dataNascimento;
        set => SetProperty(ref _dataNascimento, value);
    }

    public string Endereco
    {
        get => _endereco;
        set => SetProperty(ref _endereco, value);
    }

    public string Senha
    {
        get => _senha;
        set => SetProperty(ref _senha, value);
    }

    public string ConfirmarSenha
    {
        get => _confirmarSenha;
        set => SetProperty(ref _confirmarSenha, value);
    }

    public ICommand RegistrarCommand { get; }
    public ICommand VoltarCommand { get; }

    public RegistroViewModel(AuthService authService)
    {
        _authService = authService;

        RegistrarCommand = new Command(async () => await Registrar());
        VoltarCommand = new Command(async () => await Voltar());
    }

    private async Task Registrar()
    {
        if (string.IsNullOrWhiteSpace(Nome) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Senha) ||
            string.IsNullOrWhiteSpace(ConfirmarSenha))
        {
            await Shell.Current.DisplayAlert("Erro", "Preencha todos os campos obrigatórios", "OK");
            return;
        }

        if (Senha != ConfirmarSenha)
        {
            await Shell.Current.DisplayAlert("Erro", "As senhas não coincidem", "OK");
            return;
        }

        if (Senha.Length < 6)
        {
            await Shell.Current.DisplayAlert("Erro", "A senha deve ter pelo menos 6 caracteres", "OK");
            return;
        }

        IsBusy = true;

        try
        {
            var usuario = new Usuario
            {
                Nome = Nome,
                Email = Email,
                Telefone = Telefone,
                DataNascimento = DataNascimento,
                Endereco = Endereco,
                DataCadastro = DateTime.UtcNow
            };

            var sucesso = await _authService.RegistrarAsync(usuario, Senha);
            if (sucesso)
            {
                await Shell.Current.DisplayAlert("Sucesso", "Cadastro realizado com sucesso!", "OK");
                await Shell.Current.GoToAsync("//DashboardPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Erro", "Este email já está em uso", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro ao registrar: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task Voltar()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
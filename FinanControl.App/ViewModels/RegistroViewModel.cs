using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FinanControl.App.Services;
using FinanControl.Core.Entities;

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

    // Propriedades públicas que expõem os campos privados e notificam mudança
    public string Nome
    {
        get => _nome;
        set
        {
            if (_nome == value) return;
            _nome = value;
            OnPropertyChanged(nameof(Nome));
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (_email == value) return;
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    public string Telefone
    {
        get => _telefone;
        set
        {
            if (_telefone == value) return;
            _telefone = value;
            OnPropertyChanged(nameof(Telefone));
        }
    }

    public DateTime DataNascimento
    {
        get => _dataNascimento;
        set
        {
            if (_dataNascimento == value) return;
            _dataNascimento = value;
            OnPropertyChanged(nameof(DataNascimento));
        }
    }

    public string Endereco
    {
        get => _endereco;
        set
        {
            if (_endereco == value) return;
            _endereco = value;
            OnPropertyChanged(nameof(Endereco));
        }
    }

    public string Senha
    {
        get => _senha;
        set
        {
            if (_senha == value) return;
            _senha = value;
            OnPropertyChanged(nameof(Senha));
        }
    }

    public string ConfirmarSenha
    {
        get => _confirmarSenha;
        set
        {
            if (_confirmarSenha == value) return;
            _confirmarSenha = value;
            OnPropertyChanged(nameof(ConfirmarSenha));
        }
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
            await Application.Current.MainPage.DisplayAlert("Erro", "Preencha todos os campos obrigatórios", "OK");
            return;
        }

        if (Senha != ConfirmarSenha)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", "As senhas não coincidem", "OK");
            return;
        }

        if (Senha.Length < 6)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", "A senha deve ter pelo menos 6 caracteres", "OK");
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
                await Application.Current.MainPage.DisplayAlert("Sucesso", "Cadastro realizado com sucesso!", "OK");
                // Navegar para o AppShell
                if (Application.Current is App app)
                {
                    app.NavigateToMainApp();
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Este email já está em uso", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao registrar: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task Voltar()
    {
        // Voltar para a página de login
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}
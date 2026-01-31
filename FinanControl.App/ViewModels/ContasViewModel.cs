using System.Collections.ObjectModel;
using System.Windows.Input;
using FinanControl.App.Services;
using FinanControl.Core.Entities;
using FinanControl.Core.Enums;
using FinanControl.Infra.Data.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace FinanControl.App.ViewModels;

public class ContasViewModel : BaseViewModel
{
    private readonly IRepository<Conta> _contaRepository;
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly AuthService _authService;

    private ObservableCollection<Conta> _contas;
    private Conta _contaSelecionada;
    private string _nome;
    private string _instituicao;
    private TipoConta _tipo;
    private decimal _saldoInicial;
    private bool _isEditing;

    public ObservableCollection<Conta> Contas
    {
        get => _contas;
        set => SetProperty(ref _contas, value);
    }

    public Conta ContaSelecionada
    {
        get => _contaSelecionada;
        set
        {
            SetProperty(ref _contaSelecionada, value);
            if (value != null)
                PreencherCampos(value);
        }
    }

    public string Nome
    {
        get => _nome;
        set => SetProperty(ref _nome, value);
    }

    public string Instituicao
    {
        get => _instituicao;
        set => SetProperty(ref _instituicao, value);
    }

    public TipoConta Tipo
    {
        get => _tipo;
        set => SetProperty(ref _tipo, value);
    }

    public decimal SaldoInicial
    {
        get => _saldoInicial;
        set => SetProperty(ref _saldoInicial, value);
    }

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public ICommand SalvarCommand { get; }
    public ICommand EditarCommand { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand NovoCommand { get; }
    public ICommand LimparCommand { get; }
    public ICommand CarregarContasCommand { get; }

    public IEnumerable<TipoConta> TiposConta => Enum.GetValues<TipoConta>();

    public ContasViewModel(
        IRepository<Conta> contaRepository,
        IRepository<Usuario> usuarioRepository,
        AuthService authService)
    {
        _contaRepository = contaRepository;
        _usuarioRepository = usuarioRepository;
        _authService = authService;

        Contas = new ObservableCollection<Conta>();

        CarregarContasCommand = new Command(async () => await CarregarContas());
        SalvarCommand = new Command(async () => await SalvarConta());
        EditarCommand = new Command(() => IsEditing = true);
        ExcluirCommand = new Command(async () => await ExcluirConta());
        NovoCommand = new Command(NovaConta);
        LimparCommand = new Command(LimparCampos);

        CarregarContas();
    }

    public async Task CarregarContas()
    {
        try
        {
            IsBusy = true;
            var usuario = await _authService.GetUsuarioLogadoAsync();

            if (usuario == null) return;

            var contas = await _contaRepository.GetAllAsync();
            Contas = new ObservableCollection<Conta>(
                contas.Where(c => c.UsuarioId == usuario.Id)
                      .OrderBy(c => c.Nome));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao carregar contas: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SalvarConta()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                await Shell.Current.DisplayAlert("Atenção",
                    "Informe o nome da conta", "OK");
                return;
            }

            var usuario = await _authService.GetUsuarioLogadoAsync();
            if (usuario == null) return;

            Conta conta;

            if (IsEditing && ContaSelecionada != null)
            {
                conta = ContaSelecionada;
            }
            else
            {
                conta = new Conta
                {
                    UsuarioId = usuario.Id,
                    DataCriacao = DateTime.UtcNow
                };
            }

            conta.Nome = Nome;
            conta.Instituicao = Instituicao;
            conta.Tipo = Tipo;
            conta.SaldoAtual = SaldoInicial;
            conta.Ativo = true;

            if (IsEditing)
            {
                await _contaRepository.UpdateAsync(conta);
                await Shell.Current.DisplayAlert("Sucesso",
                    "Conta atualizada com sucesso!", "OK");
            }
            else
            {
                await _contaRepository.AddAsync(conta);
                await Shell.Current.DisplayAlert("Sucesso",
                    "Conta cadastrada com sucesso!", "OK");
            }

            await CarregarContas();
            LimparCampos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao salvar conta: {ex.Message}", "OK");
        }
    }

    private async Task ExcluirConta()
    {
        if (ContaSelecionada == null)
        {
            await Shell.Current.DisplayAlert("Atenção",
                "Selecione uma conta para excluir", "OK");
            return;
        }

        var confirm = await Shell.Current.DisplayAlert("Confirmar",
            $"Excluir a conta '{ContaSelecionada.Nome}'?",
            "Sim", "Não");

        if (!confirm) return;

        try
        {
            // Em produção, verificar se há transações vinculadas
            await _contaRepository.DeleteAsync(ContaSelecionada.Id);
            await Shell.Current.DisplayAlert("Sucesso",
                "Conta excluída com sucesso!", "OK");

            await CarregarContas();
            LimparCampos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao excluir conta: {ex.Message}", "OK");
        }
    }

    private void PreencherCampos(Conta conta)
    {
        Nome = conta.Nome;
        Instituicao = conta.Instituicao;
        Tipo = conta.Tipo;
        SaldoInicial = conta.SaldoAtual;
        IsEditing = true;
    }

    private void NovaConta()
    {
        LimparCampos();
        IsEditing = false;
        ContaSelecionada = null;
    }

    private void LimparCampos()
    {
        Nome = string.Empty;
        Instituicao = string.Empty;
        Tipo = TipoConta.ContaCorrente;
        SaldoInicial = 0;
        ContaSelecionada = null;
        IsEditing = false;
    }

    public bool IsNotBusy => !IsBusy;
}
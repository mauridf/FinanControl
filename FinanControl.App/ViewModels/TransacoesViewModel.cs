using System.Collections.ObjectModel;
using System.Windows.Input;
using FinanControl.App.Services;
using FinanControl.Core.Entities;
using FinanControl.Core.Enums;
using FinanControl.Infra.Data.Interfaces;

namespace FinanControl.App.ViewModels;

public class TransacoesViewModel : BaseViewModel
{
    private readonly IRepository<Transacao> _transacaoRepository;
    private readonly IRepository<Conta> _contaRepository;
    private readonly IRepository<Categoria> _categoriaRepository;
    private readonly AuthService _authService;

    private ObservableCollection<Transacao> _transacoes;
    private ObservableCollection<Conta> _contas;
    private ObservableCollection<Categoria> _categorias;
    private Transacao _transacaoSelecionada;
    private string _descricao;
    private string _observacao;
    private decimal _valor;
    private TipoTransacao _tipo;
    private DateTime _data = DateTime.Today;
    private bool _efetivada = true;
    private bool _recorrente;
    private int _contaId;
    private int _categoriaId;
    private bool _isEditing;

    public ObservableCollection<Transacao> Transacoes
    {
        get => _transacoes;
        set => SetProperty(ref _transacoes, value);
    }

    public ObservableCollection<Conta> Contas
    {
        get => _contas;
        set => SetProperty(ref _contas, value);
    }

    public ObservableCollection<Categoria> Categorias
    {
        get => _categorias;
        set => SetProperty(ref _categorias, value);
    }

    public Transacao TransacaoSelecionada
    {
        get => _transacaoSelecionada;
        set
        {
            SetProperty(ref _transacaoSelecionada, value);
            if (value != null)
                PreencherCampos(value);
        }
    }

    public string Descricao
    {
        get => _descricao;
        set => SetProperty(ref _descricao, value);
    }

    public string Observacao
    {
        get => _observacao;
        set => SetProperty(ref _observacao, value);
    }

    public decimal Valor
    {
        get => _valor;
        set => SetProperty(ref _valor, value);
    }

    public TipoTransacao Tipo
    {
        get => _tipo;
        set
        {
            SetProperty(ref _tipo, value);
            // Filtrar categorias pelo tipo selecionado
            _ = FiltrarCategoriasPorTipo();
        }
    }

    public DateTime Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }

    public bool Efetivada
    {
        get => _efetivada;
        set => SetProperty(ref _efetivada, value);
    }

    public bool Recorrente
    {
        get => _recorrente;
        set => SetProperty(ref _recorrente, value);
    }

    public int ContaId
    {
        get => _contaId;
        set => SetProperty(ref _contaId, value);
    }

    public int CategoriaId
    {
        get => _categoriaId;
        set => SetProperty(ref _categoriaId, value);
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
    public ICommand CarregarDadosCommand { get; }

    public IEnumerable<TipoTransacao> TiposTransacao => Enum.GetValues<TipoTransacao>();

    public TransacoesViewModel(
        IRepository<Transacao> transacaoRepository,
        IRepository<Conta> contaRepository,
        IRepository<Categoria> categoriaRepository,
        AuthService authService)
    {
        _transacaoRepository = transacaoRepository;
        _contaRepository = contaRepository;
        _categoriaRepository = categoriaRepository;
        _authService = authService;

        Transacoes = new ObservableCollection<Transacao>();
        Contas = new ObservableCollection<Conta>();
        Categorias = new ObservableCollection<Categoria>();

        SalvarCommand = new Command(async () => await SalvarTransacao());
        EditarCommand = new Command(() => IsEditing = true);
        ExcluirCommand = new Command(async () => await ExcluirTransacao());
        NovoCommand = new Command(NovaTransacao);
        LimparCommand = new Command(LimparCampos);
        CarregarDadosCommand = new Command(async () => await CarregarDados());

        // Carregar dados inicialmente
        Task.Run(async () => await CarregarDados());
    }

    private async Task CarregarDados()
    {
        try
        {
            IsBusy = true;
            var usuario = await _authService.GetUsuarioLogadoAsync();

            if (usuario == null) return;

            // Carregar contas
            var contas = await _contaRepository.GetAllAsync();
            var contasUsuario = contas
                .Where(c => c.UsuarioId == usuario.Id && c.Ativo)
                .OrderBy(c => c.Nome)
                .ToList();

            Contas = new ObservableCollection<Conta>(contasUsuario);

            // Carregar categorias (inicialmente sem filtro)
            var categorias = await _categoriaRepository.GetAllAsync();
            var categoriasUsuario = categorias
                .Where(c => c.UsuarioId == usuario.Id && c.Ativo && c.Tipo == Tipo)
                .OrderBy(c => c.Nome)
                .ToList();

            Categorias = new ObservableCollection<Categoria>(categoriasUsuario);

            // Carregar transações do mês atual
            var primeiroDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var ultimoDiaMes = primeiroDiaMes.AddMonths(1).AddDays(-1);

            var transacoes = await _transacaoRepository.GetAllAsync();
            var transacoesUsuario = transacoes
                .Where(t => t.UsuarioId == usuario.Id &&
                           t.Data >= primeiroDiaMes &&
                           t.Data <= ultimoDiaMes)
                .OrderByDescending(t => t.Data)
                .ThenByDescending(t => t.Id)
                .ToList();

            Transacoes = new ObservableCollection<Transacao>(transacoesUsuario);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao carregar dados: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task FiltrarCategoriasPorTipo()
    {
        try
        {
            var usuario = await _authService.GetUsuarioLogadoAsync();
            if (usuario == null) return;

            var categorias = await _categoriaRepository.GetAllAsync();
            var categoriasFiltradas = categorias
                .Where(c => c.UsuarioId == usuario.Id && c.Ativo && c.Tipo == Tipo)
                .OrderBy(c => c.Nome)
                .ToList();

            Categorias = new ObservableCollection<Categoria>(categoriasFiltradas);

            // Se houver categorias, selecionar a primeira
            if (categoriasFiltradas.Any() && CategoriaId == 0)
            {
                CategoriaId = categoriasFiltradas.First().Id;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao filtrar categorias: {ex.Message}");
        }
    }

    private async Task SalvarTransacao()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Descricao))
            {
                await Shell.Current.DisplayAlert("Atenção",
                    "Informe a descrição da transação", "OK");
                return;
            }

            if (Valor <= 0)
            {
                await Shell.Current.DisplayAlert("Atenção",
                    "O valor deve ser maior que zero", "OK");
                return;
            }

            if (ContaId <= 0)
            {
                await Shell.Current.DisplayAlert("Atenção",
                    "Selecione uma conta", "OK");
                return;
            }

            if (CategoriaId <= 0)
            {
                await Shell.Current.DisplayAlert("Atenção",
                    "Selecione uma categoria", "OK");
                return;
            }

            var usuario = await _authService.GetUsuarioLogadoAsync();
            if (usuario == null) return;

            Transacao transacao;

            if (IsEditing && TransacaoSelecionada != null)
            {
                transacao = TransacaoSelecionada;
            }
            else
            {
                transacao = new Transacao
                {
                    UsuarioId = usuario.Id,
                    DataCriacao = DateTime.UtcNow
                };
            }

            transacao.Descricao = Descricao;
            transacao.Observacao = Observacao;
            transacao.Valor = Valor;
            transacao.Tipo = Tipo;
            transacao.Data = Data;
            transacao.Efetivada = Efetivada;
            transacao.Recorrente = Recorrente;
            transacao.ContaId = ContaId;
            transacao.CategoriaId = CategoriaId;

            if (IsEditing)
            {
                await _transacaoRepository.UpdateAsync(transacao);
                await Shell.Current.DisplayAlert("Sucesso",
                    "Transação atualizada com sucesso!", "OK");
            }
            else
            {
                await _transacaoRepository.AddAsync(transacao);
                await Shell.Current.DisplayAlert("Sucesso",
                    "Transação cadastrada com sucesso!", "OK");
            }

            // Atualizar saldo da conta se a transação estiver efetivada
            if (Efetivada)
            {
                await AtualizarSaldoConta(ContaId, Valor, Tipo);
            }

            await CarregarDados();
            LimparCampos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao salvar transação: {ex.Message}", "OK");
        }
    }

    private async Task AtualizarSaldoConta(int contaId, decimal valor, TipoTransacao tipo)
    {
        try
        {
            var conta = await _contaRepository.GetByIdAsync(contaId);
            if (conta == null) return;

            if (tipo == TipoTransacao.Receita)
            {
                conta.SaldoAtual += valor;
            }
            else if (tipo == TipoTransacao.Despesa)
            {
                conta.SaldoAtual -= valor;
            }

            await _contaRepository.UpdateAsync(conta);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar saldo da conta: {ex.Message}");
        }
    }

    private async Task ExcluirTransacao()
    {
        if (TransacaoSelecionada == null)
        {
            await Shell.Current.DisplayAlert("Atenção",
                "Selecione uma transação para excluir", "OK");
            return;
        }

        var confirm = await Shell.Current.DisplayAlert("Confirmar",
            $"Excluir a transação '{TransacaoSelecionada.Descricao}'?",
            "Sim", "Não");

        if (!confirm) return;

        try
        {
            // Reverter saldo da conta se a transação estiver efetivada
            if (TransacaoSelecionada.Efetivada)
            {
                await ReverterSaldoConta(TransacaoSelecionada);
            }

            await _transacaoRepository.DeleteAsync(TransacaoSelecionada.Id);
            await Shell.Current.DisplayAlert("Sucesso",
                "Transação excluída com sucesso!", "OK");

            await CarregarDados();
            LimparCampos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao excluir transação: {ex.Message}", "OK");
        }
    }

    private async Task ReverterSaldoConta(Transacao transacao)
    {
        try
        {
            var conta = await _contaRepository.GetByIdAsync(transacao.ContaId);
            if (conta == null) return;

            if (transacao.Tipo == TipoTransacao.Receita)
            {
                conta.SaldoAtual -= transacao.Valor;
            }
            else if (transacao.Tipo == TipoTransacao.Despesa)
            {
                conta.SaldoAtual += transacao.Valor;
            }

            await _contaRepository.UpdateAsync(conta);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao reverter saldo da conta: {ex.Message}");
        }
    }

    private void PreencherCampos(Transacao transacao)
    {
        Descricao = transacao.Descricao;
        Observacao = transacao.Observacao;
        Valor = transacao.Valor;
        Tipo = transacao.Tipo;
        Data = transacao.Data;
        Efetivada = transacao.Efetivada;
        Recorrente = transacao.Recorrente;
        ContaId = transacao.ContaId;
        CategoriaId = transacao.CategoriaId;
        IsEditing = true;
    }

    private void NovaTransacao()
    {
        LimparCampos();
        IsEditing = false;
        TransacaoSelecionada = null;
    }

    private void LimparCampos()
    {
        Descricao = string.Empty;
        Observacao = string.Empty;
        Valor = 0;
        Tipo = TipoTransacao.Despesa;
        Data = DateTime.Today;
        Efetivada = true;
        Recorrente = false;
        ContaId = 0;
        CategoriaId = 0;
        TransacaoSelecionada = null;
        IsEditing = false;
    }
}
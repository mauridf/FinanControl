using System.Collections.ObjectModel;
using System.Windows.Input;
using FinanControl.App.Services;
using FinanControl.Core.Entities;
using FinanControl.Core.Enums;
using FinanControl.Infra.Data.Interfaces;

namespace FinanControl.App.ViewModels;

public class CategoriasViewModel : BaseViewModel
{
    private readonly IRepository<Categoria> _categoriaRepository;
    private readonly AuthService _authService;

    private ObservableCollection<Categoria> _categorias;
    private Categoria _categoriaSelecionada;
    private string _nome;
    private string _descricao;
    private TipoTransacao _tipo;
    private string _cor = "#6200EE";
    private string _icone = "category";
    private bool _isEditing;

    public ObservableCollection<Categoria> Categorias
    {
        get => _categorias;
        set => SetProperty(ref _categorias, value);
    }

    public Categoria CategoriaSelecionada
    {
        get => _categoriaSelecionada;
        set
        {
            SetProperty(ref _categoriaSelecionada, value);
            if (value != null)
                PreencherCampos(value);
        }
    }

    public string Nome
    {
        get => _nome;
        set => SetProperty(ref _nome, value);
    }

    public string Descricao
    {
        get => _descricao;
        set => SetProperty(ref _descricao, value);
    }

    public TipoTransacao Tipo
    {
        get => _tipo;
        set => SetProperty(ref _tipo, value);
    }

    public string Cor
    {
        get => _cor;
        set => SetProperty(ref _cor, value);
    }

    public string Icone
    {
        get => _icone;
        set => SetProperty(ref _icone, value);
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
    public ICommand CarregarCategoriasCommand { get; }

    public IEnumerable<TipoTransacao> TiposTransacao => Enum.GetValues<TipoTransacao>();

    // Cores pré-definidas para categorias
    public List<string> CoresDisponiveis => new()
    {
        "#6200EE", "#03DAC6", "#018786", "#FF0000", "#FF9800", "#4CAF50",
        "#2196F3", "#9C27B0", "#FF5722", "#795548", "#607D8B"
    };

    // Ícones disponíveis
    public List<string> IconesDisponiveis => new()
    {
        "category", "restaurant", "home", "directions_car", "local_grocery_store",
        "local_hospital", "school", "flight", "shopping_cart", "savings",
        "payments", "account_balance", "trending_up", "trending_down"
    };

    public CategoriasViewModel(
        IRepository<Categoria> categoriaRepository,
        AuthService authService)
    {
        _categoriaRepository = categoriaRepository;
        _authService = authService;

        Categorias = new ObservableCollection<Categoria>();

        SalvarCommand = new Command(async () => await SalvarCategoria());
        EditarCommand = new Command(() => IsEditing = true);
        ExcluirCommand = new Command(async () => await ExcluirCategoria());
        NovoCommand = new Command(NovaCategoria);
        LimparCommand = new Command(LimparCampos);
        CarregarCategoriasCommand = new Command(async () => await CarregarCategorias());

        CarregarCategorias();
    }

    public async Task CarregarCategorias()
    {
        try
        {
            IsBusy = true;
            var usuario = await _authService.GetUsuarioLogadoAsync();

            if (usuario == null) return;

            var categorias = await _categoriaRepository.GetAllAsync();
            var categoriasUsuario = categorias
                .Where(c => c.UsuarioId == usuario.Id && c.Ativo)
                .OrderBy(c => c.Tipo)
                .ThenBy(c => c.Nome)
                .ToList();

            Categorias = new ObservableCollection<Categoria>(categoriasUsuario);

            // Se não houver categorias, criar algumas padrão
            if (!categoriasUsuario.Any())
            {
                await CriarCategoriasPadrao(usuario.Id);
                await CarregarCategorias();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao carregar categorias: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CriarCategoriasPadrao(int usuarioId)
    {
        var categoriasPadrao = new List<Categoria>
        {
            new() { Nome = "Salário", Tipo = TipoTransacao.Receita, Cor = "#4CAF50", Icone = "account_balance", UsuarioId = usuarioId },
            new() { Nome = "Aluguel", Tipo = TipoTransacao.Despesa, Cor = "#F44336", Icone = "home", UsuarioId = usuarioId },
            new() { Nome = "Alimentação", Tipo = TipoTransacao.Despesa, Cor = "#FF9800", Icone = "restaurant", UsuarioId = usuarioId },
            new() { Nome = "Transporte", Tipo = TipoTransacao.Despesa, Cor = "#2196F3", Icone = "directions_car", UsuarioId = usuarioId },
            new() { Nome = "Saúde", Tipo = TipoTransacao.Despesa, Cor = "#9C27B0", Icone = "local_hospital", UsuarioId = usuarioId },
            new() { Nome = "Educação", Tipo = TipoTransacao.Despesa, Cor = "#795548", Icone = "school", UsuarioId = usuarioId },
            new() { Nome = "Lazer", Tipo = TipoTransacao.Despesa, Cor = "#03DAC6", Icone = "flight", UsuarioId = usuarioId },
            new() { Nome = "Investimentos", Tipo = TipoTransacao.Receita, Cor = "#6200EE", Icone = "trending_up", UsuarioId = usuarioId }
        };

        foreach (var categoria in categoriasPadrao)
        {
            categoria.DataCriacao = DateTime.UtcNow;
            categoria.Ativo = true;
            await _categoriaRepository.AddAsync(categoria);
        }
    }

    private async Task SalvarCategoria()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                await Shell.Current.DisplayAlert("Atenção",
                    "Informe o nome da categoria", "OK");
                return;
            }

            var usuario = await _authService.GetUsuarioLogadoAsync();
            if (usuario == null) return;

            Categoria categoria;

            if (IsEditing && CategoriaSelecionada != null)
            {
                categoria = CategoriaSelecionada;
            }
            else
            {
                categoria = new Categoria
                {
                    UsuarioId = usuario.Id,
                    DataCriacao = DateTime.UtcNow
                };
            }

            categoria.Nome = Nome;
            categoria.Descricao = Descricao;
            categoria.Tipo = Tipo;
            categoria.Cor = Cor;
            categoria.Icone = Icone;
            categoria.Ativo = true;

            if (IsEditing)
            {
                await _categoriaRepository.UpdateAsync(categoria);
                await Shell.Current.DisplayAlert("Sucesso",
                    "Categoria atualizada com sucesso!", "OK");
            }
            else
            {
                await _categoriaRepository.AddAsync(categoria);
                await Shell.Current.DisplayAlert("Sucesso",
                    "Categoria cadastrada com sucesso!", "OK");
            }

            await CarregarCategorias();
            LimparCampos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao salvar categoria: {ex.Message}", "OK");
        }
    }

    private async Task ExcluirCategoria()
    {
        if (CategoriaSelecionada == null)
        {
            await Shell.Current.DisplayAlert("Atenção",
                "Selecione uma categoria para excluir", "OK");
            return;
        }

        var confirm = await Shell.Current.DisplayAlert("Confirmar",
            $"Excluir a categoria '{CategoriaSelecionada.Nome}'?",
            "Sim", "Não");

        if (!confirm) return;

        try
        {
            // Verificar se há transações vinculadas
            await _categoriaRepository.DeleteAsync(CategoriaSelecionada.Id);
            await Shell.Current.DisplayAlert("Sucesso",
                "Categoria excluída com sucesso!", "OK");

            await CarregarCategorias();
            LimparCampos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao excluir categoria: {ex.Message}", "OK");
        }
    }

    private void PreencherCampos(Categoria categoria)
    {
        Nome = categoria.Nome;
        Descricao = categoria.Descricao;
        Tipo = categoria.Tipo;
        Cor = categoria.Cor ?? "#6200EE";
        Icone = categoria.Icone ?? "category";
        IsEditing = true;
    }

    private void NovaCategoria()
    {
        LimparCampos();
        IsEditing = false;
        CategoriaSelecionada = null;
    }

    private void LimparCampos()
    {
        Nome = string.Empty;
        Descricao = string.Empty;
        Tipo = TipoTransacao.Despesa;
        Cor = "#6200EE";
        Icone = "category";
        CategoriaSelecionada = null;
        IsEditing = false;
    }

    public bool IsNotBusy => !IsBusy;
}
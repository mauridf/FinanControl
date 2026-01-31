using System.Collections.ObjectModel;
using System.Windows.Input;
using FinanControl.App.Services;
using FinanControl.Core.Entities;
using FinanControl.Core.Enums;
using FinanControl.Infra.Data.Interfaces;

namespace FinanControl.App.ViewModels;

public class FontesRendaViewModel : BaseViewModel
{
    private readonly IRepository<FonteRenda> _fonteRendaRepository;
    private readonly AuthService _authService;

    private ObservableCollection<FonteRenda> _fontesRenda;
    private FonteRenda _fonteRendaSelecionada;
    private string _nome;
    private string _descricao;
    private TipoRenda _tipo;
    private decimal _valorEstimado;
    private FrequenciaRenda _frequencia;
    private DateTime? _dataInicio = DateTime.Today;
    private DateTime? _dataFim;
    private bool _isEditing;

    public ObservableCollection<FonteRenda> FontesRenda
    {
        get => _fontesRenda;
        set => SetProperty(ref _fontesRenda, value);
    }

    public FonteRenda FonteRendaSelecionada
    {
        get => _fonteRendaSelecionada;
        set
        {
            SetProperty(ref _fonteRendaSelecionada, value);
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

    public TipoRenda Tipo
    {
        get => _tipo;
        set => SetProperty(ref _tipo, value);
    }

    public decimal ValorEstimado
    {
        get => _valorEstimado;
        set => SetProperty(ref _valorEstimado, value);
    }

    public FrequenciaRenda Frequencia
    {
        get => _frequencia;
        set => SetProperty(ref _frequencia, value);
    }

    public DateTime? DataInicio
    {
        get => _dataInicio;
        set => SetProperty(ref _dataInicio, value);
    }

    public DateTime? DataFim
    {
        get => _dataFim;
        set => SetProperty(ref _dataFim, value);
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
    public ICommand CarregarFontesRendaCommand { get; }

    public IEnumerable<TipoRenda> TiposRenda => Enum.GetValues<TipoRenda>();
    public IEnumerable<FrequenciaRenda> FrequenciasRenda => Enum.GetValues<FrequenciaRenda>();

    public FontesRendaViewModel(
        IRepository<FonteRenda> fonteRendaRepository,
        AuthService authService)
    {
        _fonteRendaRepository = fonteRendaRepository;
        _authService = authService;

        FontesRenda = new ObservableCollection<FonteRenda>();

        SalvarCommand = new Command(async () => await SalvarFonteRenda());
        EditarCommand = new Command(() => IsEditing = true);
        ExcluirCommand = new Command(async () => await ExcluirFonteRenda());
        NovoCommand = new Command(NovaFonteRenda);
        LimparCommand = new Command(LimparCampos);
        CarregarFontesRendaCommand = new Command(async () => await CarregarFontesRenda());

        CarregarFontesRenda();
    }

    public async Task CarregarFontesRenda()
    {
        try
        {
            IsBusy = true;
            var usuario = await _authService.GetUsuarioLogadoAsync();

            if (usuario == null) return;

            var fontes = await _fonteRendaRepository.GetAllAsync();
            var fontesUsuario = fontes
                .Where(f => f.UsuarioId == usuario.Id && f.Ativo)
                .OrderByDescending(f => f.DataCriacao)
                .ToList();

            FontesRenda = new ObservableCollection<FonteRenda>(fontesUsuario);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao carregar fontes de renda: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SalvarFonteRenda()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                await Shell.Current.DisplayAlert("Atenção",
                    "Informe o nome da fonte de renda", "OK");
                return;
            }

            if (ValorEstimado <= 0)
            {
                await Shell.Current.DisplayAlert("Atenção",
                    "O valor estimado deve ser maior que zero", "OK");
                return;
            }

            var usuario = await _authService.GetUsuarioLogadoAsync();
            if (usuario == null) return;

            FonteRenda fonteRenda;

            if (IsEditing && FonteRendaSelecionada != null)
            {
                fonteRenda = FonteRendaSelecionada;
            }
            else
            {
                fonteRenda = new FonteRenda
                {
                    UsuarioId = usuario.Id,
                    DataCriacao = DateTime.UtcNow
                };
            }

            fonteRenda.Nome = Nome;
            fonteRenda.Descricao = Descricao;
            fonteRenda.Tipo = Tipo;
            fonteRenda.ValorEstimado = ValorEstimado;
            fonteRenda.Frequencia = Frequencia;
            fonteRenda.DataInicio = DataInicio;
            fonteRenda.DataFim = DataFim;
            fonteRenda.Ativo = true;

            if (IsEditing)
            {
                await _fonteRendaRepository.UpdateAsync(fonteRenda);
                await Shell.Current.DisplayAlert("Sucesso",
                    "Fonte de renda atualizada com sucesso!", "OK");
            }
            else
            {
                await _fonteRendaRepository.AddAsync(fonteRenda);
                await Shell.Current.DisplayAlert("Sucesso",
                    "Fonte de renda cadastrada com sucesso!", "OK");
            }

            await CarregarFontesRenda();
            LimparCampos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao salvar fonte de renda: {ex.Message}", "OK");
        }
    }

    private async Task ExcluirFonteRenda()
    {
        if (FonteRendaSelecionada == null)
        {
            await Shell.Current.DisplayAlert("Atenção",
                "Selecione uma fonte de renda para excluir", "OK");
            return;
        }

        var confirm = await Shell.Current.DisplayAlert("Confirmar",
            $"Excluir a fonte de renda '{FonteRendaSelecionada.Nome}'?",
            "Sim", "Não");

        if (!confirm) return;

        try
        {
            await _fonteRendaRepository.DeleteAsync(FonteRendaSelecionada.Id);
            await Shell.Current.DisplayAlert("Sucesso",
                "Fonte de renda excluída com sucesso!", "OK");

            await CarregarFontesRenda();
            LimparCampos();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Erro ao excluir fonte de renda: {ex.Message}", "OK");
        }
    }

    private void PreencherCampos(FonteRenda fonteRenda)
    {
        Nome = fonteRenda.Nome;
        Descricao = fonteRenda.Descricao;
        Tipo = fonteRenda.Tipo;
        ValorEstimado = fonteRenda.ValorEstimado;
        Frequencia = fonteRenda.Frequencia;
        DataInicio = fonteRenda.DataInicio;
        DataFim = fonteRenda.DataFim;
        IsEditing = true;
    }

    private void NovaFonteRenda()
    {
        LimparCampos();
        IsEditing = false;
        FonteRendaSelecionada = null;
    }

    private void LimparCampos()
    {
        Nome = string.Empty;
        Descricao = string.Empty;
        Tipo = TipoRenda.Salario;
        ValorEstimado = 0;
        Frequencia = FrequenciaRenda.Mensal;
        DataInicio = DateTime.Today;
        DataFim = null;
        FonteRendaSelecionada = null;
        IsEditing = false;
    }

    public bool IsNotBusy => !IsBusy;
}
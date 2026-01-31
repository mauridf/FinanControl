using System.Collections.ObjectModel;
using System.Windows.Input;
using FinanControl.App.Services;
using FinanControl.Core.Entities;
using FinanControl.Core.Enums;
using FinanControl.Infra.Data.Interfaces;

namespace FinanControl.App.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IRepository<Conta> _contaRepository;
        private readonly IRepository<Transacao> _transacaoRepository;
        private readonly AuthService _authService;

        private decimal _saldoTotal;
        private decimal _totalReceitasMes;
        private decimal _totalDespesasMes;
        private decimal _saldoMes;
        private ObservableCollection<Conta> _contas;
        private ObservableCollection<Transacao> _ultimasTransacoes;

        public decimal SaldoTotal
        {
            get => _saldoTotal;
            set => SetProperty(ref _saldoTotal, value);
        }

        public decimal TotalReceitasMes
        {
            get => _totalReceitasMes;
            set => SetProperty(ref _totalReceitasMes, value);
        }

        public decimal TotalDespesasMes
        {
            get => _totalDespesasMes;
            set => SetProperty(ref _totalDespesasMes, value);
        }

        public decimal SaldoMes
        {
            get => _saldoMes;
            set => SetProperty(ref _saldoMes, value);
        }

        public ObservableCollection<Conta> Contas
        {
            get => _contas;
            set => SetProperty(ref _contas, value);
        }

        public ObservableCollection<Transacao> UltimasTransacoes
        {
            get => _ultimasTransacoes;
            set => SetProperty(ref _ultimasTransacoes, value);
        }

        public ICommand CarregarDadosCommand { get; }
        public ICommand VerTransacoesCommand { get; }
        public ICommand NovaReceitaCommand { get; }
        public ICommand NovaDespesaCommand { get; }

        public DashboardViewModel(
            IRepository<Conta> contaRepository,
            IRepository<Transacao> transacaoRepository,
            AuthService authService)
        {
            _contaRepository = contaRepository;
            _transacaoRepository = transacaoRepository;
            _authService = authService;

            Contas = new ObservableCollection<Conta>();
            UltimasTransacoes = new ObservableCollection<Transacao>();

            // Inicializar comandos no construtor
            CarregarDadosCommand = new Command(async () => await CarregarDados());
            VerTransacoesCommand = new Command(async () => await VerTransacoes());
            NovaReceitaCommand = new Command(async () => await NovaTransacao(TipoTransacao.Receita));
            NovaDespesaCommand = new Command(async () => await NovaTransacao(TipoTransacao.Despesa));

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
                    .ToList();

                Contas = new ObservableCollection<Conta>(contasUsuario);
                SaldoTotal = contasUsuario.Sum(c => c.SaldoAtual);

                // Carregar transações do mês atual
                var hoje = DateTime.Now;
                var primeiroDiaMes = new DateTime(hoje.Year, hoje.Month, 1);
                var ultimoDiaMes = primeiroDiaMes.AddMonths(1).AddDays(-1);

                var transacoes = await _transacaoRepository.GetAllAsync();
                var transacoesMes = transacoes
                    .Where(t => t.UsuarioId == usuario.Id &&
                               t.Data >= primeiroDiaMes &&
                               t.Data <= ultimoDiaMes &&
                               t.Efetivada)
                    .ToList();

                TotalReceitasMes = transacoesMes
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor);

                TotalDespesasMes = transacoesMes
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor);

                SaldoMes = TotalReceitasMes - TotalDespesasMes;

                // Carregar últimas transações
                var ultimas = transacoes
                    .Where(t => t.UsuarioId == usuario.Id)
                    .OrderByDescending(t => t.Data)
                    .ThenByDescending(t => t.Id)
                    .Take(10)
                    .ToList();

                UltimasTransacoes = new ObservableCollection<Transacao>(ultimas);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro",
                    $"Erro ao carregar dashboard: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task VerTransacoes()
        {
            await Shell.Current.GoToAsync("//TransacoesPage");
        }

        private async Task NovaTransacao(TipoTransacao tipo)
        {
            await Shell.Current.GoToAsync("//TransacoesPage");
            // Aqui poderia passar parâmetros para pré-selecionar o tipo
        }
    }
}
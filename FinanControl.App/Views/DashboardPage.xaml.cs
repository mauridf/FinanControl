using FinanControl.App.ViewModels;

namespace FinanControl.App.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DashboardViewModel viewModel)
        {
            // Verificar autenticação antes de carregar dados
            if (await viewModel.VerificarAutenticacao())
            {
                _ = viewModel.CarregarDados();
            }
        }
    }
}
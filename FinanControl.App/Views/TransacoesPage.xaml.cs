using FinanControl.App.ViewModels;

namespace FinanControl.App.Views;

public partial class TransacoesPage : ContentPage
{
    public TransacoesPage(TransacoesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is TransacoesViewModel viewModel)
        {
            _ = viewModel.CarregarDados();
        }
    }
}
using FinanControl.App.ViewModels;

namespace FinanControl.App.Views;

public partial class ContasPage : ContentPage
{
    public ContasPage(ContasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Recarregar contas quando a página aparecer
        if (BindingContext is ContasViewModel viewModel)
        {
            _ = viewModel.CarregarContas();
        }
    }
}
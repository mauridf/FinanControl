using FinanControl.App.ViewModels;

namespace FinanControl.App.Views;

public partial class CategoriasPage : ContentPage
{
    public CategoriasPage(CategoriasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CategoriasViewModel viewModel)
        {
            _ = viewModel.CarregarCategorias();
        }
    }
}
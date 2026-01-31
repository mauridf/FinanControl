using FinanControl.App.ViewModels;

namespace FinanControl.App.Views;

public partial class FontesRendaPage : ContentPage
{
    public FontesRendaPage(FontesRendaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is FontesRendaViewModel viewModel)
        {
            _ = viewModel.CarregarFontesRenda();
        }
    }
}
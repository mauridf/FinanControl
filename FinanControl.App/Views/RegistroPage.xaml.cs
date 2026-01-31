using FinanControl.App.ViewModels;

namespace FinanControl.App.Views;

public partial class RegistroPage : ContentPage
{
    public RegistroPage(RegistroViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
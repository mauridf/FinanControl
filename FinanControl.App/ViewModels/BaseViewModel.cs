using System.ComponentModel;
using System.Runtime.CompilerServices;
using FinanControl.App.Services;

namespace FinanControl.App.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    private bool _isBusy;
    private string _title = string.Empty;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string propertyName = "",
        Action onChanged = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        var changed = PropertyChanged;
        if (changed == null)
            return;

        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected async Task<bool> VerificarAutenticacao()
    {
        var authService = App.Current.Handler.MauiContext.Services.GetService<AuthService>();
        var usuario = await authService.GetUsuarioLogadoAsync();

        if (usuario == null)
        {
            if (App.Current is App currentApp)
            {
                currentApp.NavigateToLogin();
            }

            return false;
        }

        return true;
    }
}
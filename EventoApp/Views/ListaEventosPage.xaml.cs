using EventoApp.Models;
using EventoApp.Services;

namespace EventoApp.Views;

public partial class ListaEventosPage : ContentPage
{
    public ListaEventosPage()
    {
        InitializeComponent();
    }

    // Recarrega a lista do banco sempre que a página volta a aparecer
    // (após cadastrar, editar ou excluir).
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            EventosView.ItemsSource = await EventoDatabase.Instancia.ListarAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar os eventos: {ex.Message}", "OK");
        }
    }

    private async void OnEventoSelecionado(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Evento evento)
            return;

        EventosView.SelectedItem = null;
        await Navigation.PushAsync(new ResumoEventoPage(evento));
    }

    private async void OnNovoEventoClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastroEventoPage());
    }
}

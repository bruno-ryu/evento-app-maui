using EventoApp.Models;
using EventoApp.Services;

namespace EventoApp.Views;

public partial class ResumoEventoPage : ContentPage
{
    private readonly Evento _evento;

    public ResumoEventoPage(Evento evento)
    {
        InitializeComponent();
        _evento = evento;
        BindingContext = evento;
    }

    private async void OnEditarClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastroEventoPage(_evento));
    }

    private async void OnExcluirClicked(object? sender, EventArgs e)
    {
        bool confirmou = await DisplayAlert(
            "Excluir evento",
            $"Deseja excluir \"{_evento.Nome}\"? Esta ação não pode ser desfeita.",
            "Excluir",
            "Cancelar");

        if (!confirmou)
            return;

        try
        {
            await EventoDatabase.Instancia.ExcluirAsync(_evento);
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível excluir: {ex.Message}", "OK");
        }
    }

    private async void OnVoltarClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

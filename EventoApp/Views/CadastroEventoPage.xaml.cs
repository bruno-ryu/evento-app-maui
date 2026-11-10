using EventoApp.Models;
using EventoApp.Services;

namespace EventoApp.Views;

public partial class CadastroEventoPage : ContentPage
{
    private readonly Evento? _original;   // null = novo cadastro; preenchido = edição
    private readonly Evento _evento;      // objeto ligado ao BindingContext

    public CadastroEventoPage(Evento? existente = null)
    {
        InitializeComponent();

        _original = existente;

        // Na edição trabalhamos numa cópia: se o usuário cancelar, nada muda.
        _evento = existente?.Clonar() ?? new Evento();

        Title = existente is null ? "Novo evento" : "Editar evento";
        BindingContext = _evento;
    }

    private async void OnSalvarClicked(object? sender, EventArgs e)
    {
        var erro = _evento.Validar();

        if (erro is not null)
        {
            MensagemErroLabel.Text = erro;
            MensagemErroLabel.IsVisible = true;
            return;
        }

        MensagemErroLabel.IsVisible = false;

        try
        {
            await EventoDatabase.Instancia.SalvarAsync(_evento);
        }
        catch (Exception ex)
        {
            MensagemErroLabel.Text = $"Erro ao salvar: {ex.Message}";
            MensagemErroLabel.IsVisible = true;
            return;
        }

        if (_original is null)
        {
            // Novo cadastro: mostra o resumo do evento (e tira o formulário da pilha,
            // para o botão Voltar do resumo levar à lista).
            await Navigation.PushAsync(new ResumoEventoPage(_evento));
            Navigation.RemovePage(this);
        }
        else
        {
            // Edição: atualiza o objeto exibido no resumo e volta para ele.
            _original.CopiarDe(_evento);
            await Navigation.PopAsync();
        }
    }

    private async void OnCancelarClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

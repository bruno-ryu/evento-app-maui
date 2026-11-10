namespace EventoApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var navegacao = new NavigationPage(new Views.ListaEventosPage())
        {
            BarBackgroundColor = (Color)Resources["Primaria"],
            BarTextColor = Colors.White
        };

        return new Window(navegacao) { Title = "Cadastro de Eventos" };
    }
}

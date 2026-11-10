using System.Globalization;

namespace EventoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Datas e moeda formatadas no padrão brasileiro (dd/MM/yyyy e R$ 1.234,56)
        var cultura = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = cultura;
        CultureInfo.DefaultThreadCurrentUICulture = cultura;
        CultureInfo.CurrentCulture = cultura;
        CultureInfo.CurrentUICulture = cultura;

        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        return builder.Build();
    }
}

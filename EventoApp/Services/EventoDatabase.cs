using EventoApp.Models;
using SQLite;

namespace EventoApp.Services;

public class EventoDatabase
{
    private static readonly Lazy<EventoDatabase> _instancia = new(() => new EventoDatabase());
    public static EventoDatabase Instancia => _instancia.Value;

    private readonly SemaphoreSlim _trava = new(1, 1);
    private SQLiteAsyncConnection? _conexao;

    private EventoDatabase() { }

    private async Task<SQLiteAsyncConnection> ObterConexaoAsync()
    {
        if (_conexao is not null)
            return _conexao;

        await _trava.WaitAsync();
        try
        {
            if (_conexao is null)
            {
                var caminho = Path.Combine(FileSystem.AppDataDirectory, "eventos.db3");
                var conexao = new SQLiteAsyncConnection(
                    caminho,
                    SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

                await conexao.CreateTableAsync<Evento>();
                _conexao = conexao;
            }
        }
        finally
        {
            _trava.Release();
        }

        return _conexao;
    }

    public async Task<List<Evento>> ListarAsync()
    {
        var db = await ObterConexaoAsync();
        return await db.Table<Evento>().OrderBy(e => e.DataInicio).ToListAsync();
    }

    public async Task SalvarAsync(Evento evento)
    {
        var db = await ObterConexaoAsync();

        if (evento.Id == 0)
            await db.InsertAsync(evento);   
        else
            await db.UpdateAsync(evento);
    }

    public async Task ExcluirAsync(Evento evento)
    {
        var db = await ObterConexaoAsync();
        await db.DeleteAsync(evento);
    }
}

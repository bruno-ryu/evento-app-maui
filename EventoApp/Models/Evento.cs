using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace EventoApp.Models;


public class Evento : INotifyPropertyChanged
{
    private string _nome = string.Empty;
    private DateTime _dataInicio = DateTime.Today;
    private DateTime _dataTermino = DateTime.Today;
    private int _numeroParticipantes;
    private string _local = string.Empty;
    private decimal _custoPorParticipante;

    public event PropertyChangedEventHandler? PropertyChanged;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Nome
    {
        get => _nome;
        set => SetProperty(ref _nome, value);
    }

    public DateTime DataInicio
    {
        get => _dataInicio;
        set
        {
            if (SetProperty(ref _dataInicio, value.Date))
            {
                if (_dataTermino < _dataInicio)
                    DataTermino = _dataInicio;

                NotificarCalculos();
            }
        }
    }

    public DateTime DataTermino
    {
        get => _dataTermino;
        set
        {
            if (SetProperty(ref _dataTermino, value.Date))
                NotificarCalculos();
        }
    }

    public int NumeroParticipantes
    {
        get => _numeroParticipantes;
        set
        {
            if (SetProperty(ref _numeroParticipantes, value))
                NotificarCalculos();
        }
    }

    public string Local
    {
        get => _local;
        set => SetProperty(ref _local, value);
    }

    public decimal CustoPorParticipante
    {
        get => _custoPorParticipante;
        set
        {
            if (SetProperty(ref _custoPorParticipante, value))
                NotificarCalculos();
        }
    }

    
    public TimeSpan Duracao => DataTermino.Date - DataInicio.Date;

    
    public int DuracaoEmDias => Duracao.Days + 1;

    public decimal CustoTotal => NumeroParticipantes * CustoPorParticipante;


    public string DuracaoTexto => DuracaoEmDias == 1 ? "1 dia" : $"{DuracaoEmDias} dias";

    public string DiferencaTimeSpanTexto => Duracao.ToString(@"d\.hh\:mm\:ss");

    public string PeriodoTexto => $"{DataInicio:dd/MM/yyyy} a {DataTermino:dd/MM/yyyy}";

 
    public Evento Clonar()
    {
        var copia = new Evento { Id = Id };
        copia.CopiarDe(this);
        return copia;
    }

    public void CopiarDe(Evento outro)
    {
        Nome = outro.Nome;
        DataInicio = outro.DataInicio;
        DataTermino = outro.DataTermino;
        NumeroParticipantes = outro.NumeroParticipantes;
        Local = outro.Local;
        CustoPorParticipante = outro.CustoPorParticipante;
    }


    public string? Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            return "Informe o nome do evento.";

        if (DataTermino < DataInicio)
            return "A data de término não pode ser anterior à data de início.";

        if (NumeroParticipantes <= 0)
            return "O número de participantes deve ser maior que zero.";

        if (string.IsNullOrWhiteSpace(Local))
            return "Informe o local do evento.";

        if (CustoPorParticipante < 0)
            return "O custo por participante não pode ser negativo.";

        return null;
    }


    private void NotificarCalculos()
    {
        OnPropertyChanged(nameof(Duracao));
        OnPropertyChanged(nameof(DuracaoEmDias));
        OnPropertyChanged(nameof(DuracaoTexto));
        OnPropertyChanged(nameof(DiferencaTimeSpanTexto));
        OnPropertyChanged(nameof(PeriodoTexto));
        OnPropertyChanged(nameof(CustoTotal));
    }

    private bool SetProperty<T>(ref T campo, T valor, [CallerMemberName] string? nome = null)
    {
        if (EqualityComparer<T>.Default.Equals(campo, valor))
            return false;

        campo = valor;
        OnPropertyChanged(nome);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? nome = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
}

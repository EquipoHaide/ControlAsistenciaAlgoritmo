using System;
namespace ControlAccesoES
{
    public class ModeloHorario
    {
        public DateTime LunesEntrada { get; set; }
        public DateTime LunesSalida{ get; set; }

        public DateTime MartesEntrada { get; set; }
        public DateTime MartesSalida { get; set; }

        public DateTime MiercolesEntrada { get; set; }
        public DateTime MiercolesSalida { get; set; }

        public DateTime JuevesEntrada { get; set; }
        public DateTime JuevesSalida { get; set; }

        public DateTime ViernesEntrada { get; set; }
        public DateTime ViernesSalida { get; set; }

        public DateTime SabadoEntrada { get; set; }
        public DateTime SabadoSalida { get; set; }

        public DateTime DomingoEntrada { get; set; }
        public DateTime DomingoSalida { get; set; }


        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoTurno { get; set; }
    }



}

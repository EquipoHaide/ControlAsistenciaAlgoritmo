using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace ControlAccesoES
{
    class MainClass
    {
        
        public static void Main(string[] args)
        {
            ControlAsistenciaLocal.Service1Client servicio = new ControlAsistenciaLocal.Service1Client();

            var check = true;
            while (check)
            {
                Console.WriteLine("Introduzca una hora: ");
                String texto;
                texto = Console.ReadLine();
                //DateTime d = DateTime.ParseExact(texto, "H:m:s", null);
                //DateTime registroNuevo = DateTime.ParseExact(texto, "d/M/yyyy H:m:s", null);

                string timeString = texto;//"26/12/2022 20:05:02";
                IFormatProvider culture = new CultureInfo("en-US", true);
                DateTime fecha = DateTime.ParseExact(timeString, "dd/MM/yyyy HH:mm:ss", culture);

                DateTime diaNulo = new DateTime(1900, 1, 1);

                List<ControlAsistenciaLocal.ObjHorario> horarios = CargarHoras();

                var horariosVigentes = SeleccionarHorarioCorrespondiente(fecha, horarios);


           
                var fechaI = new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0);
                var fechaF = new DateTime(fecha.Year, fecha.Month, fecha.Day, 23, 59, 59);

                List<ControlAsistenciaLocal.ObjControlAcceso> listaChecadas = servicio.ControlAccesosSeleccionarXEmp(21325, fechaI, fechaF, true).ToList();

                if (horariosVigentes.Count() == 1) {

                    var registroEntrada = listaChecadas.Where(x => x.ES == "E" && x.turno == 1).Select(y => y.tiempo).FirstOrDefault();
                       
                    var registroSalida = listaChecadas.Where(x => x.ES == "S" && x.turno == 1).Select(y => y.tiempo).FirstOrDefault();
                
                    switch (fecha.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            ValidaES(fecha, horariosVigentes[0].lunesEntrada, horariosVigentes[0].lunesSalida, registroEntrada, registroSalida);
                            break;
                        case DayOfWeek.Tuesday:
                            //ValidaES(fecha, horario.MartesEntrada, horario.MartesSalida, registroEntrada, registroSalida);
                            break;
                        case DayOfWeek.Wednesday:
                            //ValidaES(fecha, horario.MiercolesEntrada, horario.MiercolesSalida, registroEntrada, registroSalida);
                            break;
                        case DayOfWeek.Thursday:

                            break;
                        case DayOfWeek.Friday:

                            break;
                        case DayOfWeek.Saturday:

                            break;
                        case DayOfWeek.Sunday:

                            break;
                    }
                }
                else
                {
                    var registroEntradaUno = listaChecadas.Where(x => x.ES == "E" && x.turno == 1).Select(y => y.tiempo).FirstOrDefault();

                    var registroSalidaUno = listaChecadas.Where(x => x.ES == "S" && x.turno == 1).Select(y => y.tiempo).FirstOrDefault();

                    var registroEntradaDos = listaChecadas.Where(x => x.ES == "E" && x.turno == 2).Select(y => y.tiempo).FirstOrDefault();

                    var registroSalidaDos = listaChecadas.Where(x => x.ES == "S" && x.turno == 2).Select(y => y.tiempo).FirstOrDefault();



                    switch (fecha.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            //ValidaES(fecha, horariosVigentes[0].lunesEntrada, horariosVigentes[0].lunesSalida, registroEntrada, registroSalida);
                      
                            break;
                        case DayOfWeek.Tuesday:
                            //ValidaES(fecha, horario.MartesEntrada, horario.MartesSalida, registroEntrada, registroSalida);
                            break;
                        case DayOfWeek.Wednesday:
                            //ValidaES(fecha, horario.MiercolesEntrada, horario.MiercolesSalida, registroEntrada, registroSalida);
                            break;
                        case DayOfWeek.Thursday:

                            break;
                        case DayOfWeek.Friday:

                            break;
                        case DayOfWeek.Saturday:

                            break;
                        case DayOfWeek.Sunday:

                            break;
                    }
                }




                Console.WriteLine("Continuar Registros: ");
                check = Convert.ToBoolean(Console.ReadLine());
            }







        }
       

        private static List<ControlAsistenciaLocal.ObjHorario> SeleccionarHorarioCorrespondiente ( DateTime time, List<ControlAsistenciaLocal.ObjHorario> listaHorarios) {

            var horariosXFecha = listaHorarios.Where(x => x.tipoTurno == "N" && x.periodoInicial <= time && x.periodoFinal >= time).ToList();
            var intervaloConfianza = 70; // 1 hora 10 min 
          
            var listaHorario = new List<ControlAsistenciaLocal.ObjHorario>();
            foreach (var horario in horariosXFecha)
            {
                DateTime diaNulo = new DateTime(time.Year, time.Month, time.Day);

                switch (time.DayOfWeek)
                {  
                    case DayOfWeek.Monday:
                        DateTime horaLunesEntrada = new DateTime(time.Year, time.Month, time.Day, horario.lunesEntrada.Hour, horario.lunesEntrada.Minute, horario.lunesEntrada.Second);
                        DateTime horaLunesSalida = new DateTime(time.Year, time.Month, time.Day, horario.lunesSalida.Hour, horario.lunesSalida.Minute, horario.lunesSalida.Second);

                        if (horaLunesEntrada == diaNulo)
                        {
                            if (time >= horaLunesSalida.AddMinutes(intervaloConfianza))
                            {
                                listaHorario.Add(horario);
                            }

                        }else if(horaLunesSalida == diaNulo)
                        {
                            if (time >= horaLunesEntrada.AddMinutes(-intervaloConfianza) && time <= horaLunesEntrada.AddMinutes(intervaloConfianza))
                            {
                                listaHorario.Add(horario);
                            }
                        }
                        else
                        {
                            if((time > horaLunesEntrada.AddMinutes(-intervaloConfianza) || time <= horaLunesSalida.AddMinutes(intervaloConfianza)) 
                                && time <= horaLunesSalida.AddMinutes(intervaloConfianza))
                            {
                                listaHorario.Add(horario);
                            }
                        }
                     
                        break;
                    case DayOfWeek.Tuesday:
                        DateTime horaMartesEntrada = new DateTime(time.Year, time.Month, time.Day, horario.martesEntrada.Hour, horario.martesEntrada.Minute, horario.martesEntrada.Second);
                        DateTime horaMartesSalida = new DateTime(time.Year, time.Month, time.Day, horario.martesSalida.Hour, horario.martesSalida.Minute, horario.martesSalida.Second);
                        if (time <= horaMartesEntrada.AddMinutes(-intervaloConfianza) || time >= horaMartesSalida.AddMinutes(intervaloConfianza))
                        {
                            listaHorario.Add(horario);
                        }
                        break;
                    case DayOfWeek.Wednesday:
                        DateTime horaMiercolesEntrada = new DateTime(time.Year, time.Month, time.Day, horario.miercolesEntrada.Hour, horario.miercolesEntrada.Minute, horario.miercolesEntrada.Second);
                        DateTime horaMiercolesSalida = new DateTime(time.Year, time.Month, time.Day, horario.miercolesSalida.Hour, horario.miercolesSalida.Minute, horario.miercolesSalida.Second);
                        if (time <= horaMiercolesEntrada.AddMinutes(-intervaloConfianza) || time >= horaMiercolesSalida.AddMinutes(intervaloConfianza))
                        {
                            listaHorario.Add(horario);
                        }
                        break;
                    case DayOfWeek.Thursday:
                        DateTime horaJuevesEntrada = new DateTime(time.Year, time.Month, time.Day, horario.juevesEntrada.Hour, horario.juevesEntrada.Minute, horario.juevesEntrada.Second);
                        DateTime horaJuevesSalida = new DateTime(time.Year, time.Month, time.Day, horario.juevesSalida.Hour, horario.juevesSalida.Minute, horario.juevesSalida.Second);
                        if (time <= horaJuevesEntrada.AddMinutes(-intervaloConfianza) || time >= horaJuevesSalida.AddMinutes(intervaloConfianza))
                        {
                            listaHorario.Add(horario);
                        }
                        break;
                    case DayOfWeek.Friday:
                        DateTime horaViernesEntrada = new DateTime(time.Year, time.Month, time.Day, horario.viernesEntrada.Hour, horario.viernesEntrada.Minute, horario.viernesEntrada.Second);
                        DateTime horaViernesSalida = new DateTime(time.Year, time.Month, time.Day, horario.viernesSalida.Hour, horario.viernesSalida.Minute, horario.viernesSalida.Second);
                        if (time <= horaViernesEntrada.AddMinutes(-intervaloConfianza) || time >= horaViernesSalida.AddMinutes(intervaloConfianza))
                        {
                            listaHorario.Add(horario);
                        }
                        break;
                    case DayOfWeek.Saturday:
                        DateTime horaSabadoEntrada = new DateTime(time.Year, time.Month, time.Day, horario.sabadoEntrada.Hour, horario.sabadoEntrada.Minute, horario.sabadoEntrada.Second);
                        DateTime horaSabadoSalida = new DateTime(time.Year, time.Month, time.Day, horario.sabadoSalida.Hour, horario.sabadoSalida.Minute, horario.sabadoSalida.Second);
                        if (time <= horaSabadoEntrada.AddMinutes(-intervaloConfianza) || time >= horaSabadoSalida.AddMinutes(intervaloConfianza))
                        {
                            listaHorario.Add(horario);
                        }
                        break;
                    case DayOfWeek.Sunday:
                        DateTime horaDomingoEntrada = new DateTime(time.Year, time.Month, time.Day, horario.domingoEntrada.Hour, horario.domingoEntrada.Minute, horario.domingoEntrada.Second);
                        DateTime horaDomingoSalida = new DateTime(time.Year, time.Month, time.Day, horario.domingoSalida.Hour, horario.domingoSalida.Minute, horario.domingoSalida.Second);
                        if (time <= horaDomingoEntrada.AddMinutes(-intervaloConfianza) || time >= horaDomingoSalida.AddMinutes(intervaloConfianza))
                        {
                            listaHorario.Add(horario);
                        }
                        break;
                }
            }

            return listaHorario;
        }


        private static void ValidaES(DateTime horaCheck , DateTime horaEntrada, DateTime horaSalida, DateTime registroEntrada, DateTime registroSalida)
        {

            //ControlAsistenciaLocal.Service1Client servicio = new ControlAsistenciaLocal.Service1Client();
            DateTime diaNulo = new DateTime(horaCheck.Year,horaCheck.Month, horaCheck.Day);

             if ((horaCheck.Hour <= horaEntrada.Hour || horaCheck.Hour > horaEntrada.Hour)
               // && horaEntrada.Hour != diaNulo.Hour
                && registroEntrada == diaNulo)
            {
                if (horaEntrada != diaNulo)
                {
                    if (horaCheck.Hour < horaSalida.Hour && horaEntrada == diaNulo)
                    {
                        Console.WriteLine("Entrada 1 ");
                        //servicio.ControlAccesoInsertar(21325, horaCheck, "E", 1, "N");
                    }
                    else
                    {
                        if (horaSalida.Hour != diaNulo.Hour)
                        {
                            if (horaCheck.Hour > horaSalida.Hour)
                            {
                                Console.WriteLine("Salida 1");
                            }
                            else if (horaCheck.Hour <= horaEntrada.Hour || horaCheck.Hour > horaEntrada.Hour && horaCheck.Hour < horaEntrada.AddHours(3).Hour)
                            {
                                Console.WriteLine("Entrada 2");
                            }
                            else
                            {
                                Console.WriteLine("Salida Anticipada o Salida Normal Porque aun no se manda la entrada");
                            }
                        }
                        else
                        {
                            if (registroEntrada == diaNulo)
                            {
                                Console.WriteLine("Entrada 3");
                            }
                            else
                            {
                                if (registroEntrada.Hour < horaCheck.Hour)
                                {
                                    Console.WriteLine("Actualizo la hora de entrada");
                                }

                                Console.WriteLine("Sigue siendo entrada pero no se guarda ");
                            }
                        }
                    }
                }
                else
                {
                    if(horaSalida.Hour != diaNulo.Hour)
                    {
                        if(registroSalida.Hour == diaNulo.Hour)
                        {
                            Console.WriteLine("Salida 2");
                        }
                    }
                }
            }
            else
            {
                //if (registroSalida == null)
                //{
                if (registroSalida.Hour == diaNulo.Hour)
                {
                    Console.WriteLine("Salida 2 (PUEDE SER UNA SALIDA CORRECTA CONFIRME A SU HORA O UNA SALIDA ANTICIPADA)");
                }


                //}

            }
        }

        private static List<ControlAsistenciaLocal.ObjHorario> CargarHoras()
        {
            ControlAsistenciaLocal.Service1Client servicio = new ControlAsistenciaLocal.Service1Client();

            //ID MIO = 21325
            List<ControlAsistenciaLocal.ObjHorario> listaHorarios = servicio.HorariosSeleccionarXIdEmpleado(21325).ToList();

            
            return new List<ControlAsistenciaLocal.ObjHorario> ( listaHorarios );
        }
    }

}

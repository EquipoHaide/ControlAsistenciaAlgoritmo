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

                    var registroEntrada = listaChecadas.Where(x => x.ES == "E" && x.turno == horariosVigentes[0].TipoTurno).Select(y => y.tiempo).FirstOrDefault();
                       
                    var registroSalida = listaChecadas.Where(x => x.ES == "S" && x.turno == horariosVigentes[0].TipoTurno).Select(y => y.tiempo).FirstOrDefault();
                
                    switch (fecha.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            //DateTime s = new DateTime();
                            //TimeSpan ts = new TimeSpan(10, 30, 0);
                            //s = s.Date + ts;
                        
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.lunesEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.lunesSalida.TimeOfDay,
                                fecha.Date + registroEntrada.TimeOfDay, fecha.Date + registroSalida.TimeOfDay, horariosVigentes[0].TipoTurno);
                            break;
                        case DayOfWeek.Tuesday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.martesEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.martesSalida.TimeOfDay,
                                  fecha.Date + registroEntrada.TimeOfDay, fecha.Date + registroSalida.TimeOfDay, horariosVigentes[0].TipoTurno);
                            break;
                        case DayOfWeek.Wednesday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.miercolesEntrada.TimeOfDay,fecha.Date + horariosVigentes[0].Horario.miercolesSalida.TimeOfDay,
                                fecha.Date + registroEntrada.TimeOfDay, fecha.Date + registroSalida.TimeOfDay, horariosVigentes[0].TipoTurno);
                            break;
                        case DayOfWeek.Thursday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.juevesEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.juevesSalida.TimeOfDay,
                                 fecha.Date + registroEntrada.TimeOfDay, fecha.Date + registroSalida.TimeOfDay, horariosVigentes[0].TipoTurno);
                            break;
                        case DayOfWeek.Friday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.viernesEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.viernesSalida.TimeOfDay,
                              fecha.Date + registroEntrada.TimeOfDay, fecha.Date + registroSalida.TimeOfDay, horariosVigentes[0].TipoTurno);
                            break;
                        case DayOfWeek.Saturday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.sabadoEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.sabadoSalida.TimeOfDay,
                                 fecha.Date + registroEntrada.TimeOfDay, fecha.Date + registroSalida.TimeOfDay, horariosVigentes[0].TipoTurno);
                            break;
                        case DayOfWeek.Sunday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.domingoEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.domingoSalida.TimeOfDay,
                                fecha.Date + registroEntrada.TimeOfDay, fecha.Date + registroSalida.TimeOfDay, horariosVigentes[0].TipoTurno);
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
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.lunesEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.lunesSalida.TimeOfDay,
                                fecha.Date + registroEntradaUno.TimeOfDay, fecha.Date + registroSalidaUno.TimeOfDay, horariosVigentes[0].TipoTurno);

                            ValidaES(fecha, fecha.Date + horariosVigentes[1].Horario.lunesEntrada.TimeOfDay, fecha.Date + horariosVigentes[1].Horario.lunesSalida.TimeOfDay,
                                 fecha.Date + registroEntradaDos.TimeOfDay, fecha.Date + registroSalidaDos.TimeOfDay, horariosVigentes[1].TipoTurno);

                            break;
                        case DayOfWeek.Tuesday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.martesEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.martesSalida.TimeOfDay,
                              fecha.Date + registroEntradaUno.TimeOfDay, fecha.Date + registroSalidaUno.TimeOfDay, horariosVigentes[0].TipoTurno);

                            ValidaES(fecha, fecha.Date + horariosVigentes[1].Horario.martesEntrada.TimeOfDay, fecha.Date + horariosVigentes[1].Horario.martesSalida.TimeOfDay,
                              fecha.Date + registroEntradaDos.TimeOfDay, fecha.Date + registroSalidaDos.TimeOfDay, horariosVigentes[1].TipoTurno);

                            break;
                        case DayOfWeek.Wednesday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.miercolesEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.miercolesSalida.TimeOfDay,
                                fecha.Date + registroEntradaUno.TimeOfDay, fecha.Date + registroSalidaUno.TimeOfDay, horariosVigentes[0].TipoTurno);

                            ValidaES(fecha, fecha.Date + horariosVigentes[1].Horario.miercolesEntrada.TimeOfDay, fecha.Date + horariosVigentes[1].Horario.miercolesSalida.TimeOfDay,
                                 fecha.Date + registroEntradaDos.TimeOfDay, fecha.Date + registroSalidaDos.TimeOfDay, horariosVigentes[1].TipoTurno);

                            break;
                        case DayOfWeek.Thursday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.juevesEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.juevesSalida.TimeOfDay,
                                 fecha.Date + registroEntradaUno.TimeOfDay, fecha.Date + registroSalidaUno.TimeOfDay, horariosVigentes[0].TipoTurno);

                            ValidaES(fecha, fecha.Date + horariosVigentes[1].Horario.juevesEntrada.TimeOfDay, fecha.Date + horariosVigentes[1].Horario.juevesSalida.TimeOfDay,
                                fecha.Date + registroEntradaDos.TimeOfDay, fecha.Date + registroSalidaDos.TimeOfDay, horariosVigentes[1].TipoTurno);

                            break;
                        case DayOfWeek.Friday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.viernesEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.viernesSalida.TimeOfDay,
                                fecha.Date + registroEntradaUno.TimeOfDay, fecha.Date + registroSalidaUno.TimeOfDay, horariosVigentes[0].TipoTurno);

                            ValidaES(fecha, fecha.Date + horariosVigentes[1].Horario.viernesEntrada.TimeOfDay, fecha.Date + horariosVigentes[1].Horario.viernesSalida.TimeOfDay,
                                fecha.Date + registroEntradaDos.TimeOfDay, fecha.Date + registroSalidaDos.TimeOfDay, horariosVigentes[1].TipoTurno);

                            break;
                        case DayOfWeek.Saturday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.sabadoEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.sabadoSalida.TimeOfDay,
                               fecha.Date + registroEntradaUno.TimeOfDay, fecha.Date + registroSalidaUno.TimeOfDay, horariosVigentes[0].TipoTurno);

                            ValidaES(fecha, fecha.Date + horariosVigentes[1].Horario.sabadoEntrada.TimeOfDay, fecha.Date + horariosVigentes[1].Horario.sabadoSalida.TimeOfDay,
                                fecha.Date + registroEntradaDos.TimeOfDay, fecha.Date + registroSalidaDos.TimeOfDay, horariosVigentes[1].TipoTurno);

                            break;
                        case DayOfWeek.Sunday:
                            ValidaES(fecha, fecha.Date + horariosVigentes[0].Horario.domingoEntrada.TimeOfDay, fecha.Date + horariosVigentes[0].Horario.domingoSalida.TimeOfDay,
                                fecha.Date + registroEntradaUno.TimeOfDay, fecha.Date + registroSalidaUno.TimeOfDay, horariosVigentes[0].TipoTurno);

                            ValidaES(fecha, fecha.Date + horariosVigentes[1].Horario.domingoEntrada.TimeOfDay, fecha.Date + horariosVigentes[1].Horario.domingoSalida.TimeOfDay,
                                fecha.Date + registroEntradaDos.TimeOfDay, fecha.Date + registroSalidaDos.TimeOfDay, horariosVigentes[1].TipoTurno);

                            break;
                    }
                }

                Console.WriteLine("Continuar Registros: ");
                check = Convert.ToBoolean(Console.ReadLine());
            }

        }
       
        private static List<ModeloHorario> SeleccionarHorarioCorrespondiente (DateTime time, List<ControlAsistenciaLocal.ObjHorario> listaHorarios) {

            var horariosXFecha = listaHorarios.Where(x => x.tipoTurno == "N" && x.periodoInicial <= time && x.periodoFinal >= time).ToList();
            var intervaloConfianza = 70; // 1 hora 10 min 
          
            var listaHorario = new List<ModeloHorario>();
            var turno = 1;
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
                            TimeSpan inicioDia = new TimeSpan(00, 00, 01);
                            //EVALUAR SI SE ENCUENTRADA ENTRE 00:00:01 Y LA HORA DE SALIDA 
                            if (time.TimeOfDay >= inicioDia && horaLunesSalida.AddMinutes(intervaloConfianza) >= time)
                            {
                                listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                            }

                        }
                        else if (horaLunesSalida == diaNulo)
                        {
                            if (time >= horaLunesEntrada.AddMinutes(-intervaloConfianza) && time <= horaLunesEntrada.AddMinutes(intervaloConfianza))
                            {
                                //listaHorario.Add(horario);
                                listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                            }
                        }
                        else
                        {
                            if (horaLunesEntrada.TimeOfDay > horaLunesSalida.TimeOfDay)
                            {
                                if (time >= horaLunesEntrada.AddMinutes(-intervaloConfianza) || horaLunesSalida.AddMinutes(intervaloConfianza) >= time)
                                {
                                    listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                                }
                            }
                            else
                            {
                                if (time >= horaLunesEntrada.AddMinutes(-intervaloConfianza) && horaLunesSalida.AddMinutes(intervaloConfianza) >= time)
                                //&& time <= horaMartesSalida.AddMinutes(intervaloConfianza))
                                {
                                    //listaHorario.Add(horario);
                                    listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                                }
                            }
                        }

                        break;
                    case DayOfWeek.Tuesday:
                        DateTime horaMartesEntrada = new DateTime(time.Year, time.Month, time.Day, horario.martesEntrada.Hour, horario.martesEntrada.Minute, horario.martesEntrada.Second);
                        DateTime horaMartesSalida = new DateTime(time.Year, time.Month, time.Day, horario.martesSalida.Hour, horario.martesSalida.Minute, horario.martesSalida.Second);
                       

                        if (horaMartesEntrada == diaNulo)
                        {
                            TimeSpan inicioDia = new TimeSpan(00, 00, 01);
                            //EVALUAR SI SE ENCUENTRADA ENTRE 00:00:01 Y LA HORA DE SALIDA 
                            if (time.TimeOfDay >= inicioDia && horaMartesSalida.AddMinutes(intervaloConfianza) >= time)
                            {
                                listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                            }

                        }
                        else if (horaMartesSalida == diaNulo)
                        {
                            if (time >= horaMartesEntrada.AddMinutes(-intervaloConfianza) && time <= horaMartesEntrada.AddMinutes(intervaloConfianza))
                            {
                                //listaHorario.Add(horario);
                                listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                            }
                        }
                        else
                        {
                            if(horaMartesEntrada.TimeOfDay > horaMartesSalida.TimeOfDay)
                            {
                                if(time >= horaMartesEntrada.AddMinutes(-intervaloConfianza) || horaMartesSalida.AddMinutes(intervaloConfianza) >= time)
                                {
                                    listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                                }
                            }
                            else
                            {
                                if (time >= horaMartesEntrada.AddMinutes(-intervaloConfianza) && horaMartesSalida.AddMinutes(intervaloConfianza) >= time)
                                //&& time <= horaMartesSalida.AddMinutes(intervaloConfianza))
                                {
                                    //listaHorario.Add(horario);
                                    listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                                }
                            }  
                        }

                        break;
                    case DayOfWeek.Wednesday:
                        DateTime horaMiercolesEntrada = new DateTime(time.Year, time.Month, time.Day, horario.miercolesEntrada.Hour, horario.miercolesEntrada.Minute, horario.miercolesEntrada.Second);
                        DateTime horaMiercolesSalida = new DateTime(time.Year, time.Month, time.Day, horario.miercolesSalida.Hour, horario.miercolesSalida.Minute, horario.miercolesSalida.Second);
                        if (time <= horaMiercolesEntrada.AddMinutes(-intervaloConfianza) || time >= horaMiercolesSalida.AddMinutes(intervaloConfianza))
                        {
                            //listaHorario.Add(horario);
                            listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                        }
                        break;
                    case DayOfWeek.Thursday:
                        DateTime horaJuevesEntrada = new DateTime(time.Year, time.Month, time.Day, horario.juevesEntrada.Hour, horario.juevesEntrada.Minute, horario.juevesEntrada.Second);
                        DateTime horaJuevesSalida = new DateTime(time.Year, time.Month, time.Day, horario.juevesSalida.Hour, horario.juevesSalida.Minute, horario.juevesSalida.Second);
                        if (time <= horaJuevesEntrada.AddMinutes(-intervaloConfianza) || time >= horaJuevesSalida.AddMinutes(intervaloConfianza))
                        {
                            //listaHorario.Add(horario);
                            listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                        }
                        break;
                    case DayOfWeek.Friday:
                        DateTime horaViernesEntrada = new DateTime(time.Year, time.Month, time.Day, horario.viernesEntrada.Hour, horario.viernesEntrada.Minute, horario.viernesEntrada.Second);
                        DateTime horaViernesSalida = new DateTime(time.Year, time.Month, time.Day, horario.viernesSalida.Hour, horario.viernesSalida.Minute, horario.viernesSalida.Second);
                      
                        if (horaViernesEntrada == diaNulo)
                        {
                            TimeSpan inicioDia = new TimeSpan(00, 00, 01);
                            //EVALUAR SI SE ENCUENTRADA ENTRE 00:00:01 Y LA HORA DE SALIDA 
                            if (time.TimeOfDay >= inicioDia && horaViernesSalida.AddMinutes(intervaloConfianza) >= time)
                            {
                                listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                            }

                        }
                        else if (horaViernesSalida == diaNulo)
                        {
                            if (time >= horaViernesEntrada.AddMinutes(-intervaloConfianza) && time <= horaViernesEntrada.AddMinutes(intervaloConfianza))
                            {
                                //listaHorario.Add(horario);
                                listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                            }
                        }
                        else
                        {
                            if (horaViernesEntrada.TimeOfDay > horaViernesSalida.TimeOfDay)
                            {
                                if (time >= horaViernesEntrada.AddMinutes(-intervaloConfianza) || horaViernesSalida.AddMinutes(intervaloConfianza) >= time)
                                {
                                    listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                                }
                            }
                            else
                            {
                                if (time >= horaViernesEntrada.AddMinutes(-intervaloConfianza) && horaViernesSalida.AddMinutes(intervaloConfianza) >= time)
                                //&& time <= horaMartesSalida.AddMinutes(intervaloConfianza))
                                {
                                    //listaHorario.Add(horario);
                                    listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                                }
                            }
                        }
                        break;
                    case DayOfWeek.Saturday:
                        DateTime horaSabadoEntrada = new DateTime(time.Year, time.Month, time.Day, horario.sabadoEntrada.Hour, horario.sabadoEntrada.Minute, horario.sabadoEntrada.Second);
                        DateTime horaSabadoSalida = new DateTime(time.Year, time.Month, time.Day, horario.sabadoSalida.Hour, horario.sabadoSalida.Minute, horario.sabadoSalida.Second);
                        if (time <= horaSabadoEntrada.AddMinutes(-intervaloConfianza) || time >= horaSabadoSalida.AddMinutes(intervaloConfianza))
                        {
                            //listaHorario.Add(horario);
                            listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                        }
                        break;
                    case DayOfWeek.Sunday:
                        DateTime horaDomingoEntrada = new DateTime(time.Year, time.Month, time.Day, horario.domingoEntrada.Hour, horario.domingoEntrada.Minute, horario.domingoEntrada.Second);
                        DateTime horaDomingoSalida = new DateTime(time.Year, time.Month, time.Day, horario.domingoSalida.Hour, horario.domingoSalida.Minute, horario.domingoSalida.Second);
                        if (time <= horaDomingoEntrada.AddMinutes(-intervaloConfianza) || time >= horaDomingoSalida.AddMinutes(intervaloConfianza))
                        {
                            //listaHorario.Add(horario);
                            listaHorario.Add(new ModeloHorario() { Horario = horario, TipoTurno = turno });
                        }
                        break;
                }
                turno++;
            }

            return listaHorario;
        }

        private static void ValidaES(DateTime horaCheck , DateTime horaEntrada, DateTime horaSalida, DateTime registroEntrada, DateTime registroSalida,int turno)
        {
            ControlAsistenciaLocal.ObjControlAcceso asiatencia = new ControlAsistenciaLocal.ObjControlAcceso();

            ControlAsistenciaLocal.Service1Client servicio = new ControlAsistenciaLocal.Service1Client();
            DateTime diaNulo = new DateTime(horaCheck.Year,horaCheck.Month, horaCheck.Day);

            //Evalua si el horario involucra otro dia 
            if(horaEntrada > horaSalida)
            {
                if ((horaCheck <= horaSalida || horaCheck > horaSalida && horaSalida.AddMinutes(70) >= horaCheck)
                 && registroSalida == diaNulo)
                { 
                    if(horaSalida != diaNulo)
                    {

                        Console.WriteLine("Salida");
                        servicio.ControlAccesoInsertar(21325, horaCheck, "S", turno, "N");
                    }

                }
                else
                {
                    if (registroEntrada == diaNulo)
                    {
                        Console.WriteLine("Entrada");
                      
                        servicio.ControlAccesoInsertar(21325, horaCheck, "E", turno, "N");
                    }
                }
            }
            else
            {
                if ((horaCheck <= horaEntrada || horaCheck > horaEntrada && horaEntrada.AddMinutes(70) >= horaCheck)
               // && horaEntrada.Hour != diaNulo.Hour
               && registroEntrada == diaNulo)
                    {

                        //Evalua cuando en el registro esta en  00:00:00 
                        if (horaEntrada != diaNulo)
                        {
                            Console.WriteLine("Entrada");
                            servicio.ControlAccesoInsertar(21325, horaCheck, "E", turno, "N");
                        }
                        //else
                        //{
                        //    if (horaSalida.Hour != diaNulo.Hour)
                        //    {
                        //        if (registroSalida.Hour == diaNulo.Hour)
                        //        {
                        //            Console.WriteLine("Salida 2");
                        //            servicio.ControlAccesoInsertar(21325, horaCheck, "S", turno, "N");
                        //        }
                        //    }

                        //}
                    }
                    else
                    {
                        //if (registroSalida == null)
                        //{
                        if (registroSalida == diaNulo)
                        {
                            Console.WriteLine("Salida 2 (PUEDE SER UNA SALIDA CORRECTA CONFIRME A SU HORA O UNA SALIDA ANTICIPADA)");
                            //servicio.ControlAccesoInsertar(21325, horaCheck, "S", turno, "N");
                            servicio.ControlAccesoInsertar(21325, horaCheck, "S", turno, "N");
                        }

                        //}

                    }
            }
         
        }

        private static List<ControlAsistenciaLocal.ObjHorario> CargarHoras()
        {
            ControlAsistenciaLocal.Service1Client servicio = new ControlAsistenciaLocal.Service1Client();

            //ID MIO = 21325
            List<ControlAsistenciaLocal.ObjHorario> listaHorarios = servicio.HorariosSeleccionarXIdEmpleado(21325).ToList();

            
            return new List<ControlAsistenciaLocal.ObjHorario> ( listaHorarios );
        }



        //if (horaCheck < horaSalida  /* && horaEntrada == diaNulo*/)
        //{
        //    Console.WriteLine("Entrada 1 ");
        //    servicio.ControlAccesoInsertar(21325, horaCheck, "E", turno, "N");
        //}
        //else
        //{
        //    if (horaSalida.Hour != diaNulo.Hour)
        //    {
        //        if (horaCheck >= horaSalida && registroSalida == diaNulo)
        //        {
        //            Console.WriteLine("Salida 1");
        //            servicio.ControlAccesoInsertar(21325, horaCheck, "S", turno, "N");
        //        }
        //        else if (horaCheck <= horaEntrada || horaCheck > horaEntrada && horaCheck < horaEntrada.AddHours(3))
        //        {
        //            Console.WriteLine("Entrada 2");
        //            servicio.ControlAccesoInsertar(21325, horaCheck, "E", turno, "N");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Salida Anticipada o Salida Normal Porque aun no se manda la entrada");
        //            servicio.ControlAccesoInsertar(21325, horaCheck, "S", turno, "N");
        //        }
        //    }
        //    else
        //    {
        //        if (registroEntrada.Hour == diaNulo.Hour)
        //        {
        //            Console.WriteLine("Entrada 3");
        //            servicio.ControlAccesoInsertar(21325, horaCheck, "E", turno, "N");
        //        }
        //        else
        //        {
        //            if (registroEntrada.Hour < horaCheck.Hour)
        //            {
        //                Console.WriteLine("Actualizo la hora de entrada");

        //            }

        //            Console.WriteLine("Sigue siendo entrada pero no se guarda ");

        //        }
        //    }
        //}
    }

}

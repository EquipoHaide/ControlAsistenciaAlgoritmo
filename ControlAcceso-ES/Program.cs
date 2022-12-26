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
            //Console.WriteLine("Hello World!");


            //var t1 = "22:00:00 PM";
            ////var t2 = "00:00:00 AM";
            //DateTime.ParseExact(t1, "M/d/yyyy hh:mm", CultureInfo.InvariantCulture);
            //var time1 = new  DateTime.ParseExact(t1, "hh:mm:ss tt");



            //d.AddDays(DateTime.Now.Date);


            //var time1 = new DateTime(2022, 12, 05, 08, 05, 00);

            // ENTRADA
            //var time2 = new DateTime(2022,12,09,06,04,45); // SALIDA


            //registroAcceso.Add(new DateTime(2022, 12, 05, 23, 58, 00)); // ENTRADA  --- 11:58:00 pm 
            //registroAcceso.Add(new DateTime(2022, 12, 06, 05, 30, 00)); //SALIDA ANTICIPADA --- 05:30:00 am
            //registroAcceso.Add(new DateTime(2022, 12, 06, 23, 05, 00)); // ENTRADA  --- 11:05:00 pm 
            //registroAcceso.Add(new DateTime(2022, 12, 07, 06, 01, 00)); //SALIDA NORMAL --- 06:01:00 am

            var check = true;
            while (check)
            {
                Console.WriteLine("Introduzca una hora: ");
                String texto;
                texto = Console.ReadLine();
                //DateTime d = DateTime.ParseExact(texto, "H:m:s", null);
                //DateTime registroNuevo = DateTime.ParseExact(texto, "d/M/yyyy H:m:s", null);

                string timeString = texto;//"05/12/2022 13:30:00";
                IFormatProvider culture = new CultureInfo("en-US", true);
                DateTime fecha = DateTime.ParseExact(timeString, "dd/MM/yyyy HH:mm:ss", culture);

                DateTime diaNulo = new DateTime(1900, 1, 1);

                List<ModeloHorario> horarios = CargarHoras();
                //List<DateTime> registroAcceso = new List<DateTime>();
                //Validar SI EL EMPLEADO TIENE HORARIO NOCTURNO
                //SI NO TIENE TOMAR LOS HORARIOS NORMALES
                var horariosNOC = horarios.Where(x => x.TipoTurno == "Noc" && x.FechaInicio.Date <= fecha.Date && x.FechaFin.Date >= fecha.Date).ToList();
                if(horariosNOC.Count() > 0) {


                    var horario = SeleccionarHorarioCorrespondiente(fecha,horariosNOC);


                    List<ModeloRegistro> registroBD = new List<ModeloRegistro>();

                    var registroEntrada = registroBD.Where(x => x.horaAcceso == fecha && x.tipo == "E" && x.turno == horario.TipoTurno).Select(y => y.horaAcceso).FirstOrDefault();
                       

                    var registroSalida = registroBD.Where(x => x.horaAcceso == fecha && x.tipo == "S" && x.turno == horario.TipoTurno).Select(y => y.horaAcceso).FirstOrDefault();
                
                    //foreach (var hora in registroAcceso)
                    //{

                    switch (fecha.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            ValidaES(fecha, horario.LunesEntrada, horario.LunesSalida, registroEntrada, registroSalida);
                            break;
                        case DayOfWeek.Tuesday:
                            ValidaES(fecha, horario.MartesEntrada, horario.MartesSalida, registroEntrada, registroSalida);
                            break;
                        case DayOfWeek.Wednesday:
                            ValidaES(fecha, horario.MiercolesEntrada, horario.MiercolesSalida, registroEntrada, registroSalida);
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


                    //}


                }
                else
                {
                   
                }




                Console.WriteLine("Continuar Registros: ");
                check = Convert.ToBoolean(Console.ReadLine());
            }


         




        }


        private static ModeloHorario SeleccionarHorarioCorrespondiente(DateTime time,List<ModeloHorario> listaHorarios) {




            return new ModeloHorario();
        }


        private static void ValidaES(DateTime horaCheck , DateTime horaEntrada, DateTime horaSalida, DateTime registroEntrada, DateTime registroSalida)
        {
            DateTime diaNulo = new DateTime(1900, 1, 1);

             if ((horaCheck.Hour <= horaEntrada.Hour || horaCheck.Hour > horaEntrada.Hour)
               // && horaEntrada.Hour != diaNulo.Hour
                && registroEntrada == diaNulo)
            {
                if (horaEntrada != diaNulo)
                {
                    if (horaCheck.Hour < horaSalida.Hour && horaEntrada == diaNulo)
                    {
                        Console.WriteLine("Entrada 1 ");
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

        private static List<ModeloHorario> CargarHoras()
        {
            var horaEntradaLunes = new DateTime(1900, 01, 01, 22, 00, 00);
            var horaSalidaLunes = new DateTime(1900, 01, 01, 00, 00, 00);

            var horaEntradaMartes = new DateTime(1900, 01, 01, 22, 00, 00);
            var horaSalidaMartes = new DateTime(1900, 01, 01, 06, 00, 00);

            var horaEntradaMiercoles = new DateTime(1900, 01, 01, 22, 00, 00);
            var horaSalidaMiercoles = new DateTime(1900, 01, 01, 06, 00, 00);

            var fechaInicio = new DateTime(2022, 11, 15, 00, 00, 00);
            var fechaFin = new DateTime(3000, 01, 01, 00, 00, 00);


            var horaEntradaLunes2 = new DateTime(1900, 01, 01, 22, 00, 00);
            var horaSalidaLunes2 = new DateTime(1900, 01, 01, 00, 00, 00);

            var horaEntradaMartes2 = new DateTime(1900, 01, 01, 22, 00, 00);
            var horaSalidaMartes2 = new DateTime(1900, 01, 01, 06, 00, 00);

            var horaEntradaMiercoles2 = new DateTime(1900, 01, 01, 22, 00, 00);
            var horaSalidaMiercoles2 = new DateTime(1900, 01, 01, 06, 00, 00);

            var fechaInicio2 = new DateTime(2022, 12, 01, 00, 00, 00);
            var fechaFin2 = new DateTime(3000, 01, 01, 00, 00, 00);

            ModeloHorario horarioTurnoUNO = new ModeloHorario();
            horarioTurnoUNO.LunesEntrada = horaEntradaLunes;
            horarioTurnoUNO.LunesSalida = horaSalidaLunes;
            horarioTurnoUNO.MartesEntrada = horaEntradaMartes;
            horarioTurnoUNO.MartesSalida = horaSalidaMartes;
            horarioTurnoUNO.MiercolesEntrada = horaEntradaMiercoles;
            horarioTurnoUNO.MiercolesSalida = horaSalidaMiercoles;
            horarioTurnoUNO.FechaInicio = fechaInicio;
            horarioTurnoUNO.FechaFin = fechaFin;
            horarioTurnoUNO.TipoTurno = "Noc";


            //ModeloHorario horarioTurnoDOS = new ModeloHorario();
            //horarioTurnoDOS.LunesEntrada = horaEntradaLunes2;
            //horarioTurnoDOS.LunesSalida = horaSalidaLunes2;
            //horarioTurnoDOS.MartesEntrada = horaEntradaMartes2;
            //horarioTurnoDOS.MartesSalida = horaSalidaMartes2;
            //horarioTurnoDOS.MiercolesEntrada = horaEntradaMiercoles2;
            //horarioTurnoDOS.MiercolesSalida = horaSalidaMiercoles2;
            //horarioTurnoDOS.FechaInicio = fechaInicio2;
            //horarioTurnoDOS.FechaFin = fechaFin2;

            return new List<ModeloHorario> { horarioTurnoUNO /*,horarioTurnoDOS*/ };
        }
    }

}

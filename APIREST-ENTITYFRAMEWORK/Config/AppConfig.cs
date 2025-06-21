using Microsoft.EntityFrameworkCore;
using ApiRest_EntityFramework.Models;
using ApiRest_EntityFramework.Tools;
using System.Text;

namespace ApiRest_EntityFramework.Config
{
    public static class AppConfig
    {

        // ATRIBUTOS

        private static string rutaDatosCsv = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "futbolistas19.csv"); // RUTA AL FICHERO

        private static List<Equipo> equipos = new List<Equipo>(); // EQUIPOS DEL CSV

        private static List<Jugador> jugadores = new List<Jugador>(); // JUGADORES DEL CSV


        // METODOS 
        public static async Task<bool> cargarConfiguracion(AplicationDbContext context) // METODO PARA AGREGAR LOS DATOS DEL CSV EN LA BD
        {
            try
            {
                if (context != null)
                {
                    if (!await context.equipos.AnyAsync() && !await context.jugadores.AnyAsync())
                    {
                        FileInfo file = new FileInfo(rutaDatosCsv);

                        if (file.Exists && file.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            
                            using (FileStream fs = new FileStream(rutaDatosCsv, FileMode.Open, FileAccess.Read, FileShare.None, 4096))
                            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                            {
                                
                                await sr.ReadLineAsync();

                                while (!sr.EndOfStream)
                                {
                                    var frase = await sr.ReadLineAsync();
                                    var objetos = Utils.obtenerDatosFrase(frase);

                                    if (objetos.Count == 2)
                                    {
                                        agregarDatos(objetos);
                                    }
                                }

                                
                                await context.equipos.AddRangeAsync(equipos);
                                await context.jugadores.AddRangeAsync(jugadores);
                                await context.SaveChangesAsync();

                                Console.WriteLine("DATOS CARGADOS DEL CSV CORRECTAMENTE");
                                return true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("FICHERO ERRONEO O NO EXISTENTE");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("DATOS YA EXISTENTES EN LA BASE DE DATOS");
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: DBCONTEXT NULO");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR EN LA CONFIGURACION: {ex.Message}");
                return false;
            }
        }
            


        public static void agregarDatos(List<Object> lista) // METODO PARA AGREGAR LOS DATOS A LA LISTA
        {

            try
            {

                Equipo auxEquipo = (Equipo)lista[0];
                Jugador auxJugador = (Jugador)lista[1];

                if (!equipos.Any(e => e.nombre.Equals(auxEquipo.nombre)))
                {
                    equipos.Add(auxEquipo);
                }

                var equipoIndice = equipos.FirstOrDefault(e => e.nombre.Equals(auxEquipo.nombre));

                auxJugador.idEquipo = equipos.IndexOf(equipoIndice) + 1;

                if (!jugadores.Any(j => j.nombre.Equals(auxJugador.nombre)))
                {
                    jugadores.Add(auxJugador);
                }

            }
            catch (Exception)
            {
                Console.WriteLine("ERROR AL AGREGAR LOS DATOS A LA LISTA");
            }

        }

    }
}

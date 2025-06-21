using ApiRest_EntityFramework.Models;


namespace ApiRest_EntityFramework.Tools
{
    public static class Utils
    {

        public static List<Object> obtenerDatosFrase(string frase) // METODO QUE OBTIENE LOS DATOS DE LA FRASE DEL CSV
        {

            try
            {

                List<string> datos = new List<string>(frase.Split(","));

                Equipo equipo = new Equipo(); // 9 // 1 , 2
                equipo.nombre = datos[1];
                equipo.liga = datos[2];
                equipo.jugadores = new List<Jugador>();

                Jugador jugador = new Jugador(); // 3 , 4 , 6 , 7 , 8 // 0 , 4 , 5 , 6 ,  3
                jugador.nombre = datos[0];
                jugador.edad = int.Parse(datos[4]);
                jugador.altura = int.Parse(datos[5]);
                jugador.peso = int.Parse(datos[6]);
                jugador.nacionalidad = datos[3];

                return new List<Object>() { equipo, jugador };

            }
            catch (Exception)
            {
                Console.WriteLine("ERROR AL PARSEAR LA FRASE DEL CSV");
                return new List<Object> { };
            }

        }

    }
}

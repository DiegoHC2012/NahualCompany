using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient; //Instalar paquete nuget

namespace GymGTPSoftware
{
    public class Conexion
    {
        private MySqlConnection conexion;
        private string server = "localhost";
        private string database = "SoftwareGPT_NewDB"; 
        private string user = "root";
        private string password = "";
        private string cadenaConexion;

        public Conexion() {
            cadenaConexion = $"Database={database}; DataSource={server}" +
                  $"; User Id={user}; Password={password};";
        }

        public MySqlConnection getConexion()
        {
            if (conexion == null)
            {
                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
            }

            return conexion;
        }

        //Obtener datos de una tabla

        public DataTrabajador ObtenerDatosBD(string nombreUsuario, MySqlConnection conexion) //Para comprobar contrasena
        {
            /*
             Obtener los datos a partir de un nombre de usuario.
            Primero obtenemos el id que queremos conocer
             */
            DataTrabajador trabajador;
            string query;
            int id = 1;
            // Utilizar una declaración parametrizada para evitar inyección SQL

            query = "SELECT id_Trabajador FROM Usuario_Trabajador WHERE usuario = @NombreUsuario";


            using (MySqlCommand cmd = new MySqlCommand(query, conexion))
            {
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = (int)reader["id_trabajador"];
                    }
                }
            }

            query = $"SELECT * FROM Trabajador WHERE id_trabajador = {id}";

            //Obtener datos generales nombre, telefono, etc

            using (MySqlCommand cmd = new MySqlCommand(query, conexion))
            {
                //cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string nombre = reader["nombre"].ToString();
                        string apellidoP = reader["apellido_p"].ToString();
                        string apellidoM = reader["apellido_m"].ToString();
                        string Ine = reader["ine"].ToString();
                        string telefono = reader["telefono"].ToString();
                        string correo = reader["correo_e"].ToString();
                        string matricula = reader["matricula"].ToString();
                        return new DataTrabajador(nombre, apellidoP, apellidoM, Ine, telefono, correo, matricula);
                    }
                }
            }
            return null;
        }
    }
}

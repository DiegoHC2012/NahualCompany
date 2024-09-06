using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GymGTPSoftware
{
    public partial class EntrenadorHorarios : Form
    {
        private Conexion conexion;
        private Conexion mconexion;
        private Conexion mnconexion;
        private DataTable dtHorarios;
        private Diseño diseño;

        public EntrenadorHorarios()
        {
            InitializeComponent();
            conexion = new Conexion();
            mconexion = new Conexion();
            mnconexion = new Conexion();
            diseño = new Diseño();
            CargarHorarios();
            CargarEntrenadores();
            CargarTiposEntreno();
            CargarDiasSemana();
        }

        private void CargarHorarios()
        {
            string query = "SELECT id_horario, id_entrenador, id_tipo_entreno, dia, hora_inicio, duracion,ADDTIME(hora_inicio, duracion) as hora_fin FROM Horarios_Clase";
            MySqlCommand cmd = new MySqlCommand(query, conexion.getConexion());
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            dtHorarios = new DataTable();
            adapter.Fill(dtHorarios);
            dataGridViewHorarios.DataSource = dtHorarios;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            
        }

        private bool VerificarChoqueHorario(int idEntrenador, string dia, TimeSpan horaInicio, TimeSpan duracion)
        {
            foreach (DataRow row in dtHorarios.Rows)
            {
                if ((int)row["id_entrenador"] == idEntrenador && (string)row["dia"] == dia)
                {
                    TimeSpan existenteInicio = (TimeSpan)row["hora_inicio"];
                    TimeSpan existenteFin = existenteInicio + (TimeSpan)row["duracion"];
                    TimeSpan nuevoFin = horaInicio + duracion;

                    if (horaInicio < existenteFin && nuevoFin > existenteInicio)
                    {
                        return true; // Hay un choque
                    }
                }
            }
            return false;
        }
        private void GuardarHorario(int idEntrenador, int idTipoEntreno, string dia, TimeSpan horaInicio, TimeSpan duracion)
        {
            string query = "INSERT INTO Horarios_Clase (id_entrenador, id_tipo_entreno, dia, hora_inicio, duracion) VALUES (@idEntrenador, @idTipoEntreno, @dia, @horaInicio, @duracion)";
            MySqlCommand cmd = new MySqlCommand(query, conexion.getConexion());
            cmd.Parameters.AddWithValue("@idEntrenador", idEntrenador);
            cmd.Parameters.AddWithValue("@idTipoEntreno", idTipoEntreno);
            cmd.Parameters.AddWithValue("@dia", dia);
            cmd.Parameters.AddWithValue("@horaInicio", horaInicio.ToString());
            cmd.Parameters.AddWithValue("@duracion", duracion.ToString());
            cmd.ExecuteNonQuery();
            MessageBox.Show("Horario guardado correctamente.");
            CargarHorarios();
        }

        private void CargarEntrenadores()
        {

            try
            {
                //conexion.getConexion().Open();
                //string query = "SELECT id_trabajador, CONCAT(nombre, ' ', apellido_p, ' ', apellido_m) AS NombreCompleto FROM Trabajador";
                string query = @"
            SELECT t.id_trabajador, CONCAT(t.nombre, ' ', t.apellido_p, ' ', t.apellido_m) AS NombreCompleto 
            FROM Trabajador t
            JOIN Usuario_Trabajador ut ON t.id_trabajador = ut.id_trabajador
            WHERE ut.isadmin = 0";
                MySqlCommand cmd = new MySqlCommand(query, mconexion.getConexion());
                MySqlDataReader reader;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ComboboxItem item = new ComboboxItem
                    {
                        Text = reader["NombreCompleto"].ToString(),
                        Value = reader["id_trabajador"]
                    };
                    comboBoxEntrenadores.Items.Add(item);
                }

                comboBoxEntrenadores.DisplayMember = "Text";
                comboBoxEntrenadores.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los entrenadores: " + ex.Message);
            }
            //finally
            //{
            //    conexion.getConexion().Close();
            //}
        }

        private void CargarTiposEntreno()
        {
            

            try
            {
                //conexion.getConexion().Open();
                string query = "SELECT id_tipo_entreno, nombre FROM Tipo_Entreno";
                MySqlCommand cmd = new MySqlCommand(query, mnconexion.getConexion());
                MySqlDataReader reader;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ComboboxItem item = new ComboboxItem
                    {
                        Text = reader["nombre"].ToString(),
                        Value = reader["id_tipo_entreno"]
                    };
                    comboBoxTipoEntreno.Items.Add(item);
                }

                comboBoxTipoEntreno.DisplayMember = "Text";
                comboBoxTipoEntreno.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los tipos de entrenamiento: " + ex.Message);
            }
            //finally
            //{
            //    conexion.getConexion().Close();
            //}
        }

        private void CargarDiasSemana()
        {
            string[] dias = new string[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

            foreach (string dia in dias)
            {
                comboBoxDias.Items.Add(dia);
            }
        }

        //private void dataGridViewHorarios_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (dataGridViewHorarios.SelectedRows.Count > 0)
        //    {
        //        DataGridViewRow selectedRow = dataGridViewHorarios.SelectedRows[0];
        //        if (selectedRow != null)
        //        {
        //            // Suponiendo que las columnas se llamen 'id_horario', 'hora_inicio', 'duracion', y 'dia'
        //            textBoxHoraInicio.Text = selectedRow.Cells["hora_inicio"].Value.ToString();
        //            textBoxDuracion.Text = selectedRow.Cells["hora_fin"].Value.ToString();
        //            comboBoxDias.SelectedItem = selectedRow.Cells["dia"].Value.ToString();
        //            comboBoxEntrenadores.SelectedItem = selectedRow.Cells["id_entrenador"].Value;

        //            // Habilitar el botón de editar
        //            btnEditar.Enabled = true;

        //            // Guardar el ID del horario en una etiqueta o variable si es necesario
        //            btnEditar.Tag = selectedRow.Cells["id_horario"].Value.ToString();
        //        }
        //    }
        //}

        private void dataGridViewHorarios_SelectionChanged(object sender, EventArgs e)
        {
            
            if (dataGridViewHorarios.SelectedRows.Count > 0)
            {
                
                DataGridViewRow selectedRow = dataGridViewHorarios.SelectedRows[0];
                if (selectedRow != null)
                {
                    string strHoraInicio = "";
                    // Asegurarse de que la fila tiene datos válidos para hora_inicio y hora_fin
                    if (selectedRow.Cells["hora_inicio"].Value != null && selectedRow.Cells["hora_fin"].Value != null)
                    {
                        strHoraInicio = selectedRow.Cells["hora_inicio"].Value.ToString();
                        string strHoraFin = selectedRow.Cells["hora_fin"].Value.ToString();

                        // Parsear las horas a TimeSpan
                        TimeSpan horaInicio, horaFin;
                        if (TimeSpan.TryParse(strHoraInicio, out horaInicio) && TimeSpan.TryParse(strHoraFin, out horaFin))
                        {
                            // Calcular la duración
                            TimeSpan duracion = horaFin - horaInicio;

                            // Mostrar la duración en el TextBox
                            textBoxDuracion.Text = duracion.ToString(@"hh\:mm");
                        }
                        else
                        {
                            textBoxDuracion.Text = "Formato de hora inválido";
                        }
                    }

                    // Continuar con otros controles como se hace normalmente
                    textBoxHoraInicio.Text = strHoraInicio;
                    comboBoxDias.SelectedItem = selectedRow.Cells["dia"].Value.ToString();

                    // Habilitar el botón de editar
                    btnEditar.Enabled = true;
                    btnEditar.Tag = selectedRow.Cells["id_horario"].Value.ToString();
                }
            }
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
           
        }



        private void EntrenadorHorarios_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e) //Boton de Guardar
        {
            diseño.CambioColor(pictureBox2, "EMHorarioGuardar");

            string dia = comboBoxDias.SelectedItem.ToString();

            TimeSpan horaInicio = TimeSpan.Parse(textBoxHoraInicio.Text);
            TimeSpan duracion = TimeSpan.Parse(textBoxDuracion.Text);
            int idEntrenador = (int)(comboBoxEntrenadores.SelectedItem as ComboboxItem).Value;
            int idTipoEntreno = (int)(comboBoxTipoEntreno.SelectedItem as ComboboxItem).Value;

            if (!VerificarChoqueHorario(idEntrenador, dia, horaInicio, duracion))
            {
                GuardarHorario(idEntrenador, idTipoEntreno, dia, horaInicio, duracion);
            }
            else
            {
                MessageBox.Show("El horario para este entrenador entra en conflicto con otro ya existente.");
            }
        }

        private void picActualizar_Click(object sender, EventArgs e)
        {
            diseño.CambioColor(picActualizar, "EMHorarioActualizar");

            // Asumiendo que el ID está almacenado en el Tag del botón
            if (btnEditar.Tag == null)
            {
                MessageBox.Show("Seleccione un horario para editar primero.");
                return;
            }

            int idHorario = int.Parse(btnEditar.Tag.ToString());
            string horaInicio = textBoxHoraInicio.Text;
            string duracion = textBoxDuracion.Text;
            string dia = comboBoxDias.SelectedItem.ToString();

            // Comando SQL para actualizar el registro
            string query = @"
        UPDATE Horarios_Clase 
        SET 
            hora_inicio = @horaInicio,
            duracion = @duracion,
            dia = @dia
        WHERE 
            id_horario = @idHorario";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conexion.getConexion()))
                {
                    // Abre la conexión si está cerrada
                    if (conexion.getConexion().State == ConnectionState.Closed)
                        conexion.getConexion().Open();

                    // Añadir parámetros al comando
                    cmd.Parameters.AddWithValue("@horaInicio", horaInicio);
                    cmd.Parameters.AddWithValue("@duracion", duracion);
                    cmd.Parameters.AddWithValue("@dia", dia);
                    cmd.Parameters.AddWithValue("@idHorario", idHorario);

                    // Ejecutar el comando
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Horario actualizado correctamente.");
                        CargarHorarios();
                    }
                    else
                    {
                        MessageBox.Show("No se realizó ninguna actualización.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el horario: " + ex.Message);
            }
            finally
            {
                if (conexion.getConexion().State == ConnectionState.Open)
                    conexion.getConexion().Close();
            }
        }

        private void EntrenadorHorarios_FormClosing(object sender, FormClosingEventArgs e)
        {
            Variables.CerrarFormManual(this);
            EntrenadorMenu.HorariosAbierto = false;
        }
    }
}

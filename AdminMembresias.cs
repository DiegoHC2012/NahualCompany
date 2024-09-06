using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymGTPSoftware
{

    public partial class AdminMembresias : Form
    {
        //private Conexion mConexion;
        private mConexion mConexion;
        private Diseño diseño;
        private string IDMiembro;
        private int pagina = 0;
        public AdminMembresias()
        {
            InitializeComponent();
            InitializeComponents();
            mConexion = new mConexion();
            ApagarTextbox();
            // Cargar datos en DataGridView
            LoadData();
            diseño = new Diseño();
            
        }
        private void InitializeComponents()
        {
            // Configuración de DataGridView
            dataGridView1.Location = new System.Drawing.Point(120, 140);
            dataGridView1.Size = new System.Drawing.Size(555, 168);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionChanged += new EventHandler(DataGridView1_SelectionChanged);

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            this.Controls.Add(dataGridView1);
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0 && dataGridView1 != null)
                {
                    var memberId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    IDMiembro = memberId;
                    if (memberId != null && memberId != "")
                    {
                        GetMemberDetails(memberId);
                    }

                }
            }
            catch (Exception exce)
            {
                MessageBox.Show(exce.Message);
            }
        }
        private void GetMemberDetails(string memberId)
        {
            string apellidoP;
            string apellidoM;
            string adeudo = "";
            string currentMembershipName = "";
            int currentMembershipId = -1;
            try
            {
                string query = $"SELECT * FROM Miembro WHERE id_miembro = {memberId}"; 
                string query2 = $@"
                        SELECT Miembro.nombre, Membresia.nombre AS nombre_membresia
                        FROM Miembro
                        JOIN Membresia ON Miembro.id_membresia = Membresia.id_membresia
                        WHERE Miembro.id_miembro = {memberId};
                    ";
                string query3 = $@"
                        SELECT id_miembro, cantidad, fecha_corte, fecha_limite, adeudo
                        FROM Pago
                        WHERE id_miembro = {memberId};
                    ";
                string query4 = "SELECT id_membresia, nombre, costo FROM Membresia";
                string query5 = $@"
                SELECT Miembro.nombre, Membresia.nombre AS nombre_membresia, Miembro.id_membresia
                FROM Miembro
                JOIN Membresia ON Miembro.id_membresia = Membresia.id_membresia
                WHERE Miembro.id_miembro = {memberId};
                ";
                MySqlCommand cmd = new MySqlCommand(query, mConexion.getConexion());
                MySqlCommand cmd2 = new MySqlCommand(query2, mConexion.getConexion());
                MySqlCommand cmd3 = new MySqlCommand(query3, mConexion.getConexion());
                MySqlCommand cmd4 = new MySqlCommand(query4, mConexion.getConexion());
                MySqlCommand cmd5 = new MySqlCommand(query5, mConexion.getConexion());
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textBoxNombre.Text = reader["nombre"].ToString();
                        apellidoP = reader["apellido_p"].ToString();
                        apellidoM = reader["apellido_m"].ToString();
                        textBoxApellido.Text = $"{apellidoP}  {apellidoM}";
                        textBoxMatricula.Text = reader["matricula"].ToString();
                    }
                }
                using (MySqlDataReader reader = cmd2.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Asumiendo que tienes una columna 'nombre' en tu tabla Miembro
                        textBoxMembresia.Text = reader["nombre_membresia"].ToString();
                    }
                }
                using (MySqlDataReader reader = cmd3.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textBoxFechaL.Text = reader["fecha_corte"].ToString();
                        textBoxFechaLimite.Text = reader["fecha_limite"].ToString();
                        adeudo = reader["adeudo"].ToString();
                        textBoxTotalPagar.Text = reader["cantidad"].ToString();
                        if (adeudo == "1" || adeudo == "True")
                        {
                            textBoxAdeudo.Text = "Pago pendiente";
                        }
                        if (adeudo == "0" || adeudo == "False")
                        {
                            textBoxAdeudo.Text = "Pago realizado";
                        }
                    }
                }
                using (MySqlDataReader reader = cmd5.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentMembershipName = reader["nombre_membresia"].ToString();
                        currentMembershipId = Convert.ToInt32(reader["id_membresia"]); // Asegúrate de que esta columna exista y sea del tipo correcto
                    }
                }
                // Llena el ComboBox con todas las membresías disponibles
                comboBoxMembresia.Items.Clear();
                using (MySqlDataReader reader = cmd4.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ComboboxItem item = new ComboboxItem
                        {
                            Text = reader["nombre"].ToString(),
                            Value = reader["id_membresia"],
                            Costo = Convert.ToDecimal(reader["costo"])
                        };
                        comboBoxMembresia.Items.Add(item);

                        // Si el ID de este item coincide con el ID de la membresía actual, seleccionarlo
                        if (currentMembershipId != -1 && Convert.ToInt32(item.Value) == currentMembershipId)
                        {
                            comboBoxMembresia.SelectedItem = item;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchForMembresia(string matricula)
        {
            string apellidoP;
            string apellidoM;
            string adeudo;
            int memberId = -1;
            int currentMembershipId = -1;
            try
            {
                string query = $"SELECT * FROM Miembro WHERE matricula = {matricula}";
                string queryID = "SELECT Miembro.id_miembro FROM Miembro WHERE matricula = @matricula";
                string query2 = $@"
                SELECT Miembro.nombre, Membresia.nombre AS nombre_membresia, Miembro.id_membresia
                FROM Miembro
                JOIN Membresia ON Miembro.id_membresia = Membresia.id_membresia
                WHERE Miembro.matricula = {matricula};
                ";


                MySqlCommand cmd = new MySqlCommand(query, mConexion.getConexion());
                MySqlCommand cmdID = new MySqlCommand(queryID, mConexion.getConexion());
                cmdID.Parameters.AddWithValue("@matricula", matricula);
                MySqlCommand cmd2 = new MySqlCommand(query2, mConexion.getConexion());
                
                using (MySqlDataReader readerID = cmdID.ExecuteReader())
                {
                    if (readerID.Read())
                    {
                        memberId = (int)readerID["id_miembro"];
                        IDMiembro = memberId.ToString();
                        // Continúa con el procesamiento de memberId
                    }
                }


                string query3 = $@"
                        SELECT id_miembro, cantidad, fecha_corte, fecha_limite, adeudo
                        FROM Pago
                        WHERE id_miembro = {memberId};
                    ";
                MySqlCommand cmd3 = new MySqlCommand(query3, mConexion.getConexion());


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textBoxNombre.Text = reader["nombre"].ToString();
                        apellidoP = reader["apellido_p"].ToString();
                        apellidoM = reader["apellido_m"].ToString();
                        textBoxApellido.Text = $"{apellidoP}  {apellidoM}";
                        textBoxMatricula.Text = reader["matricula"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("El usuario no existe");
                    }
                }
                using (MySqlDataReader reader = cmd2.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Asumiendo que tienes una columna 'nombre' en tu tabla Miembro
                        textBoxMembresia.Text = reader["nombre_membresia"].ToString();
                        currentMembershipId = Convert.ToInt32(reader["id_membresia"]);
                        // Asigna otros valores a TextBoxes o Labels según necesites
                    }
                }

                using (MySqlDataReader reader = cmd3.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Asumiendo que tienes una columna 'nombre' en tu tabla Miembro
                        textBoxFechaL.Text = reader["fecha_corte"].ToString();
                        textBoxFechaLimite.Text = reader["fecha_limite"].ToString();
                        //textBoxAdeudo.Text = reader["adeudo"].ToString();
                        textBoxTotalPagar.Text = reader["cantidad"].ToString();
                        adeudo = reader["adeudo"].ToString();
                        //textBoxAdeudo.Text = reader["adeudo"].ToString();
                        textBoxTotalPagar.Text = reader["cantidad"].ToString();
                        if (adeudo == "1" || adeudo == "True")
                        {
                            //textBoxAdeudo.Text = reader["fecha_limite"].ToString();
                            textBoxAdeudo.Text = "Pago pendiente";
                        }
                        if (adeudo == "0" || adeudo == "False")
                        {
                            textBoxAdeudo.Text = "Pago realizado";
                        }
                    }
                }
                string queryMembresias = "SELECT id_membresia, nombre, costo FROM Membresia";
                MySqlCommand cmdMembresias = new MySqlCommand(queryMembresias, mConexion.getConexion());
                comboBoxMembresia.Items.Clear();
                using (MySqlDataReader reader = cmdMembresias.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ComboboxItem item = new ComboboxItem
                        {
                            Text = reader["nombre"].ToString(),
                            Value = reader["id_membresia"],
                            Costo = Convert.ToDecimal(reader["costo"])
                        };
                        comboBoxMembresia.Items.Add(item);

                        // Si el ID de este item coincide con el ID de la membresía actual, seleccionarlo
                        if (currentMembershipId != -1 && Convert.ToInt32(item.Value) == currentMembershipId)
                        {
                            comboBoxMembresia.SelectedItem = item;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadData()
        {
            string query;
            try
            {
                if (pagina == 0)
                {
                    query = "SELECT id_miembro,nombre, matricula FROM Miembro where activo = true";
                }
                else
                {
                    query = "SELECT id_miembro,nombre, matricula FROM Miembro where activo = false";
                }

                MySqlCommand cmd = new MySqlCommand(query, mConexion.getConexion());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.AutoGenerateColumns = true; 

                dataGridView1.Columns["nombre"].HeaderText = "Nombre";
                dataGridView1.Columns["matricula"].HeaderText = "Matrícula";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ActualizarDatos(string matricula)
        {
            try
            {
                string apellidoP = "";
                string apellidoM = "";
                string Apellidos = textBoxApellido.Text;
                int indiceEspacio = Apellidos.IndexOf(' ');
                if (indiceEspacio != -1)
                {
                    apellidoP = Apellidos.Substring(0, indiceEspacio);
                    apellidoM = Apellidos.Substring(indiceEspacio + 1);
                }
                string updateMiembro = @"
                UPDATE Miembro
                SET nombre = @Nombre,
                apellido_p = @ApellidoP,
                apellido_m = @ApellidoM,
                id_membresia = @IdMembresia
                WHERE matricula = @Matricula;";
                string updatePago = @"
                UPDATE Pago
                SET cantidad = @Cantidad
                WHERE id_miembro = (SELECT id_miembro FROM Miembro WHERE matricula = @Matricula);";
                MySqlCommand cmdUpdateMiembro = new MySqlCommand(updateMiembro, mConexion.getConexion());
                cmdUpdateMiembro.Parameters.AddWithValue("@Nombre", textBoxNombre.Text);
                cmdUpdateMiembro.Parameters.AddWithValue("@ApellidoP", apellidoP);
                cmdUpdateMiembro.Parameters.AddWithValue("@ApellidoM", apellidoM);
                cmdUpdateMiembro.Parameters.AddWithValue("@Matricula", matricula);
                cmdUpdateMiembro.Parameters.AddWithValue("@IdMembresia", (comboBoxMembresia.SelectedItem as ComboboxItem).Value);
                cmdUpdateMiembro.ExecuteNonQuery();
                MySqlCommand cmdUpdatePago = new MySqlCommand(updatePago, mConexion.getConexion());
                cmdUpdatePago.Parameters.AddWithValue("@Cantidad", Convert.ToDecimal(textBoxTotalPagar.Text));
                cmdUpdatePago.Parameters.AddWithValue("@Matricula", matricula);
                cmdUpdatePago.ExecuteNonQuery();
                MessageBox.Show("Datos actualizados correctamente");
            }
            catch (Exception Excep)
            {
                MessageBox.Show(Excep.Message);
            }
            finally
            {
                LoadData(); 
            }
        }

        public void ActualizarPago(string idMiembro)
        {
            try
            {
                using (MySqlConnection mySqlConnection = mConexion.getConexion())
                {
                    // Actualizar el estado del adeudo actual
                    string queryActualizarAdeudo = "UPDATE Pago SET adeudo = false WHERE id_miembro = @idMiembro AND adeudo = true";
                    using (MySqlCommand cmdActualizar = new MySqlCommand(queryActualizarAdeudo, mySqlConnection))
                    {
                        cmdActualizar.Parameters.AddWithValue("@idMiembro", idMiembro);
                        cmdActualizar.ExecuteNonQuery();
                    }
                    DateTime ultimaFechaCorte;
                    string queryFechaCorte = "SELECT MAX(fecha_corte) FROM Pago WHERE id_miembro = @idMiembro";
                    using (MySqlCommand cmdFechaCorte = new MySqlCommand(queryFechaCorte, mySqlConnection))
                    {
                        cmdFechaCorte.Parameters.AddWithValue("@idMiembro", idMiembro);
                        var resultado = cmdFechaCorte.ExecuteScalar();
                        if (resultado != DBNull.Value)
                        {
                            ultimaFechaCorte = Convert.ToDateTime(resultado);
                        }
                        else
                        {
                            ultimaFechaCorte = DateTime.Today;
                        }
                    }
                    DateTime nuevaFechaCorte = ultimaFechaCorte.AddDays(27);
                    DateTime nuevaFechaLimite = nuevaFechaCorte.AddDays(7);
                    // Insertar el nuevo registro de pago
                    string queryNuevoPago = "INSERT INTO Pago (id_miembro, cantidad, fecha_corte, fecha_limite, adeudo) VALUES (@idMiembro, @cantidad, @fechaCorte, " +
                        "@fechaLimite, true)";
                    using (MySqlCommand cmdNuevoPago = new MySqlCommand(queryNuevoPago, mySqlConnection))
                    {
                        decimal cantidad = ObtenerCantidadPorMembresia(idMiembro);

                        cmdNuevoPago.Parameters.AddWithValue("@idMiembro", idMiembro);
                        cmdNuevoPago.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdNuevoPago.Parameters.AddWithValue("@fechaCorte", nuevaFechaCorte);
                        cmdNuevoPago.Parameters.AddWithValue("@fechaLimite", nuevaFechaLimite);
                        cmdNuevoPago.ExecuteNonQuery();
                        mySqlConnection.Close(); // Cerrar conexión
                    }

                    MessageBox.Show("Pago actualizado y próximo pago programado con éxito.");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al actualizar el pago: " + ex.Message);
            }
        }



        private decimal ObtenerCantidadPorMembresia(string idMiembro)
        {
            decimal cantidad = 0m;
            try
            {
                using (MySqlConnection mySqlConnection = mConexion.getConexion())
                {
                    // Consultar el tipo de membresía del miembro
                    string queryTipoMembresia = "SELECT id_membresia FROM Miembro WHERE id_miembro = @idMiembro";
                    MySqlCommand cmdTipoMembresia = new MySqlCommand(queryTipoMembresia, mySqlConnection);
                    cmdTipoMembresia.Parameters.AddWithValue("@idMiembro", idMiembro);

                    //mySqlConnection.Open();
                    int idMembresia = Convert.ToInt32(cmdTipoMembresia.ExecuteScalar());

                    // Consultar el costo de la membresía
                    string queryCostoMembresia = "SELECT costo FROM Membresia WHERE id_membresia = @idMembresia";
                    MySqlCommand cmdCostoMembresia = new MySqlCommand(queryCostoMembresia, mySqlConnection);
                    cmdCostoMembresia.Parameters.AddWithValue("@idMembresia", idMembresia);

                    cantidad = Convert.ToDecimal(cmdCostoMembresia.ExecuteScalar());
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al obtener el costo de la membresía: " + ex.Message);
            }
            return cantidad;
        }

        private void comboBoxMembresia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMembresia.SelectedItem is ComboboxItem selectedItem)
            {
                textBoxTotalPagar.Text = selectedItem.Costo.ToString("F2");
            }
        }
        private void ApagarTextbox()
        {
            textBoxNombre.ReadOnly = true;
            textBoxApellido.ReadOnly = true;
            textBoxMembresia.ReadOnly = true;
            textBoxFechaL.ReadOnly = true;
        }
        private void EncenderTextbox()
        {
            textBoxNombre.ReadOnly = false;
            textBoxApellido.ReadOnly = false;
            textBoxFechaL.ReadOnly = false;

            textBoxMembresia.Visible = false;
            comboBoxMembresia.Visible = true;
        }

        private void btnBuscarMiembro_Click(object sender, EventArgs e)
        {
            SearchForMembresia(textBoxBuscarMatricula.Text);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string matricula = textBoxMatricula.Text;
            ActualizarDatos(matricula);
        }
        private async void picBuscarMiembro_Click(object sender, EventArgs e)
        {
            diseño.CambioColor(picBuscarMiembro, "Admin_Buscar_Miembro");
            await Task.Delay(120);

            SearchForMembresia(textBoxBuscarMatricula.Text);
        }
        private async void picEditarMiembro_Click(object sender, EventArgs e)
        {
            EncenderTextbox();
            diseño.CambioColor(picEditarMiembro, "Admin_Editar_Miembro");
            await Task.Delay(120);

            picEditarMiembro.Visible = false;
            picGuardarMiembro.Visible = true;
        }
        private async void picGuardarMiembro_Click(object sender, EventArgs e)
        {
            diseño.CambioColor(picGuardarMiembro, "Admin_Guardar_Miembro");
            await Task.Delay(120);

            string matricula = textBoxMatricula.Text;
            ActualizarDatos(matricula);
            picGuardarMiembro.Visible = false;
            picEditarMiembro.Visible = true;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void AdminMembresias_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Esta parte sirve para eliminar el form de la lista de forms
            Variables.CerrarFormManual(this);
            AdminMenu.AdministrarMembresiasAbierto = false;
        }

        private void btnConfirmarPago_Click(object sender, EventArgs e)
        {
            ActualizarPago(IDMiembro);
        }

        private async void picHabilitar_Click(object sender, EventArgs e)
        {
            diseño.CambioColor(picHabilitar, "AMHabilitar_miembro");
            await Task.Delay(120);

            if (IDMiembro != "")
            {
                try
                {
                    using (MySqlConnection mySqlConnection = mConexion.getConexion())
                    {
                        // Consultar el tipo de membresía del miembro
                        string query = $"UPDATE Miembro SET activo = true WHERE id_miembro = {IDMiembro}";
                        MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Miembro habilitado con exito!");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al habilitar miembro: " + ex.Message);
                }
                finally
                {
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un miembro!");
            }

        }

        private async void picVerActivos_Click(object sender, EventArgs e)
        {
            diseño.CambioColor(picVerActivos, "AMVer_Activos");
            await Task.Delay(120);
            pagina = 0;

            picDeshabilitar.Visible = true;
            picVerInactivos.Visible = true;
            picHabilitar.Visible = false;
            picVerActivos.Visible = false;
            LoadData();
        }

        private async void picDeshabilitar_Click(object sender, EventArgs e)
        {
            diseño.CambioColor(picDeshabilitar, "AMDeshabilitar_miembro");
            await Task.Delay(120);

            if (IDMiembro != "")
            {
                try
                {
                    using (MySqlConnection mySqlConnection = mConexion.getConexion())
                    {
                        // Consultar el tipo de membresía del miembro
                        string query = $"UPDATE Miembro SET activo = false WHERE id_miembro = {IDMiembro}";
                        MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Miembro deshabilitado con exito!");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al deshabilitar miembro: " + ex.Message);
                }
                finally
                {
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un miembro!");
            }
        }

        private async void picVerInactivos_Click(object sender, EventArgs e)
        {
            diseño.CambioColor(picVerInactivos, "AMVer_Inactivos");
            await Task.Delay(120);

            pagina = 1;

            picDeshabilitar.Visible = false;
            picVerInactivos.Visible = false;
            picHabilitar.Visible = true;
            picVerActivos.Visible = true;
            LoadData();

        }
        private void textBoxFechaL_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            EncenderTextbox();
        }

        private void AdminMembresias_Load(object sender, EventArgs e)
        {

        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public decimal Costo { get; set; }

        // Sobreescribe el método ToString para asegurar que el texto se muestre en el ComboBox
        public override string ToString()
        {
            return Text;
        }
    }

    public class mConexion
    {
        private string server = "localhost";
        private string database = "SoftwareGPT_NewDB";
        private string user = "root";
        private string password = "";
        private string cadenaConexion;

        public mConexion()
        {
            cadenaConexion = $"Database={database}; DataSource={server}; User Id={user}; Password={password};";
        }

        public MySqlConnection getConexion()
        {
            MySqlConnection nuevaConexion = new MySqlConnection(cadenaConexion);
            nuevaConexion.Open();
            return nuevaConexion;
        }
    }

}

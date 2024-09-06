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

namespace GymGTPSoftware
{
    public partial class AMRegistrarPagos : Form
    {
        nConexion mConexion;
        Diseño diseño;
        public AMRegistrarPagos()
        {
            InitializeComponent();
            InitializeComponents();
            mConexion = new nConexion();
            diseño = new Diseño();
            LoadDataMiembros();
        }
        private void LoadDataMiembros()
        {
            try
            {
                string query = "SELECT id_miembro,nombre, matricula FROM Miembro where activo = true";
                MySqlCommand cmd = new MySqlCommand(query, mConexion.getConexion());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                datagridListaMiembros.DataSource = dt;
                datagridListaMiembros.AutoGenerateColumns = true;
                datagridListaMiembros.Columns["nombre"].HeaderText = "Nombre";
                datagridListaMiembros.Columns["matricula"].HeaderText = "Matrícula";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadDataPagos(string id)
        {
            try
            { 
                string query = "SELECT Miembro.nombre, Pago.cantidad, " +
                    "Pago.fecha_corte, Pago.fecha_limite, Pago.adeudo " +
                    "FROM Pago INNER JOIN Miembro ON Pago.id_miembro = Miembro.id_miembro " +
                    $"WHERE Pago.id_miembro = {id};";
                MySqlCommand cmd = new MySqlCommand(query, mConexion.getConexion());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                datagridListaPagos.DataSource = dt;
                datagridListaPagos.AutoGenerateColumns = true;
                datagridListaPagos.Columns["nombre"].HeaderText = "Nombre";
                datagridListaPagos.Columns["cantidad"].HeaderText = "Cantidad";
                datagridListaPagos.Columns["fecha_corte"].HeaderText = "Fecha Corte";
                datagridListaPagos.Columns["fecha_limite"].HeaderText = "Fecha Limite";
                datagridListaPagos.Columns["adeudo"].HeaderText = "Adeudo";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private string IDMiembro = "";
        private void datagridListaMiembros_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (datagridListaMiembros.SelectedRows.Count > 0 && datagridListaMiembros != null)
                {
                    var memberId = datagridListaMiembros.SelectedRows[0].Cells[0].Value.ToString();
                    IDMiembro = memberId;
                    if (memberId != null && memberId != "")
                    {
                        GetMemberDetails(memberId);
                        LoadDataPagos(memberId);
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
            try
            {
                string query = $"SELECT nombre,apellido_p,apellido_m FROM Miembro WHERE id_miembro = {memberId}";
                string query2 = $"Select Max(fecha_corte), Max(fecha_limite) from Pago where id_miembro = {memberId};";
                MySqlCommand cmd = new MySqlCommand(query, mConexion.getConexion());
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        boxNombre.Text = reader["nombre"].ToString();
                        boxApPaterno.Text = reader["apellido_p"].ToString();
                        boxApMaterno.Text = reader["apellido_m"].ToString();
                    }
                }
                MySqlCommand cmd2 = new MySqlCommand(query2, mConexion.getConexion());
                using (MySqlDataReader reader = cmd2.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        boxFechaPago.Text = reader["Max(fecha_limite)"].ToString();
                        boxFechaCorte.Text = reader["Max(fecha_corte)"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void datagridListaPagos_SelectionChanged(object sender, EventArgs e)
        {

        }
        private void AMRegistrarPagos_FormClosing(object sender, FormClosingEventArgs e)
        {
            Variables.CerrarFormManual(this);
            AdminMenu.MenuAdminPagosAbierto = false;
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
                    string queryNuevoPago = "INSERT INTO Pago (id_miembro, cantidad, fecha_corte, fecha_limite, adeudo) VALUES (@idMiembro, @cantidad, @fechaCorte, @fechaLimite, true)";
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
        private void SearchForMembresia(string matricula)
        {
            int memberId;
            bool activo = true;
            try
            {
                string query = $"SELECT * FROM Miembro WHERE matricula = {matricula}";
                MySqlCommand cmd = new MySqlCommand(query, mConexion.getConexion());
                using (MySqlDataReader readerID = cmd.ExecuteReader()) //Obtiene el id segun la matricula
                {
                    if (readerID.Read())
                    {
                        memberId = (int)readerID["id_miembro"];
                        IDMiembro = memberId.ToString();
                        activo = bool.Parse(readerID["activo"].ToString());
                    }
                }
                if (activo)
                {
                    GetMemberDetails(IDMiembro);
                    LoadDataPagos(IDMiembro);
                }
                else 
                {
                    DialogResult result = MessageBox.Show("ATENCION: El miembro buscado esta inactivo!\n¿Deseas continuar esta acción?",
                        "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        GetMemberDetails(IDMiembro);
                        LoadDataPagos(IDMiembro);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public class nConexion
        {
            private string server = "localhost";
            private string database = "SoftwareGPT_NewDB";
            private string user = "root";
            private string password = "$machenike24";
            private string cadenaConexion;
            public nConexion()
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
        private async void picBuscar_Click(object sender, EventArgs e)
        {
            diseño.CambioColor(picBuscar, "Admin_Buscar_Miembro");
            await Task.Delay(150);
            if (boxMatricula.Text != "")
            {
                SearchForMembresia(boxMatricula.Text);
            }
            else
            {
                MessageBox.Show("Ingrese una matricula!");
            }
        }
        private async void picConfirmarPago_Click(object sender, EventArgs e)
        {
            diseño.CambioColor(picConfirmarPago, "AMConfirmar_Pago");
            await Task.Delay(150);
            if (IDMiembro != "")
            {
                ActualizarPago(IDMiembro);
                LoadDataPagos(IDMiembro);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnConfirmarPago_Click(object sender, EventArgs e)
        {

        }
        private void InitializeComponents()
        {
            // Configuración de DataGridView
            datagridListaMiembros.Location = new System.Drawing.Point(62, 115);
            datagridListaMiembros.Size = new System.Drawing.Size(372, 227);
            datagridListaMiembros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagridListaMiembros.SelectionChanged += new EventHandler(datagridListaMiembros_SelectionChanged);

            datagridListaMiembros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            datagridListaMiembros.MultiSelect = false;
            datagridListaMiembros.ReadOnly = true;

            this.Controls.Add(datagridListaMiembros);

            // Configuración de DataGridView
            datagridListaPagos.Location = new System.Drawing.Point(756, 115);
            datagridListaPagos.Size = new System.Drawing.Size(375, 227);
            datagridListaPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datagridListaPagos.SelectionChanged += new EventHandler(datagridListaPagos_SelectionChanged);
            datagridListaPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            datagridListaPagos.MultiSelect = false;
            datagridListaPagos.ReadOnly = true;
            this.Controls.Add(datagridListaPagos);
            this.Size = new System.Drawing.Size(1207, 429);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void datagridListaMiembros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
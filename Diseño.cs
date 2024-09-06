using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymGTPSoftware
{
    public class Diseño
    {
        public async void CambioColor(PictureBox imagen, string opcion) //Cambia el color de las letras cuando se le da click
        {
            int retardo = 120;
            Cursor.Current = Cursors.WaitCursor;
            switch (opcion)
            {


                case "EMHorarioGuardar":
                    imagen.BackgroundImage = Properties.Resources.MEHorariosGuardar1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.MEHorariosGuardar0;
                break;
                case "EMHorarioActualizar":
                    imagen.BackgroundImage = Properties.Resources.MEHorariosActualizar1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.MEHorariosActualizar0;
                break;

                case "INHorario0":
                    imagen.BackgroundImage = Properties.Resources.IHorarios1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.IHorarios0;
                    break;
                case "EMAgregarEntreno":
                    imagen.BackgroundImage = Properties.Resources.EMAgregarEntreno1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.EMAgregarEntreno0;
                break;

                case "EHorarios":
                    imagen.BackgroundImage = Properties.Resources.Horarios1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.Horarios0;
                    break;

                case "EMEliminarVIP":
                    imagen.BackgroundImage = Properties.Resources.EMRemover1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.EMRemover0;
                break;
                
                case "EMGuardarVIP":
                    imagen.BackgroundImage = Properties.Resources.EMGuardarVIP1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.EMGuardarVIP0;
                break;

                case "EMEditarVIP":
                    imagen.BackgroundImage = Properties.Resources.EMEditarVIP1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.EMEditarVIP0;
                break;

                case "EMInscribir":
                    imagen.BackgroundImage = Properties.Resources.EMInscribir1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.EMInscribir0;
                break;

                case "AMRegistrar_Administrador":
                    imagen.BackgroundImage = Properties.Resources.AMRegistrar_Admin1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.AMRegistrar_Admin0;
                break;
                
                case "AMConfirmar_Pago":
                    imagen.BackgroundImage = Properties.Resources.AMConfirmar_Pago1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.AMConfirmar_Pago0;
                break;

                case "AMDeshabilitar_miembro":
                    imagen.BackgroundImage = Properties.Resources.AMDeshabilitar_miembro1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.AMDeshabilitar_miembro0;
                break;

                case "AMHabilitar_miembro":
                    imagen.BackgroundImage = Properties.Resources.AMHabilitar_miembro1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.AMHabilitar_miembro0;
                break;

                case "AMVer_Inactivos":
                    imagen.BackgroundImage = Properties.Resources.AMVer_Inactivos1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.AMVer_Inactivos0;
                break;

                case "AMVer_Activos":
                    imagen.BackgroundImage = Properties.Resources.AMVer_Activos1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.AMVer_Activos0;
                break;

                case "AMGERegistrar":
                    imagen.BackgroundImage = Properties.Resources.AMGERegistrar1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.AMGERegistrar0;
                break;

                case "AMActualizar_Lista":
                    imagen.BackgroundImage = Properties.Resources.AMActualizar_Lista1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.AMActualizar_Lista0;
                break;

                case "Registrar_Acceso_Entrenador":
                    imagen.BackgroundImage = Properties.Resources.AMRegistrar_Acceso_Entrenador1;
                    
                    await Task.Delay(retardo);
                  
                    imagen.BackgroundImage = Properties.Resources.AMRegistrar_Acceso_Entrenador0;
                break;

                case "EMInscribir_Entrenamientos":
                    imagen.BackgroundImage = Properties.Resources.Entrenamientos1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Entrenamientos0;
                break;

                case "EMInscribir_Miembros":
                    imagen.BackgroundImage = Properties.Resources.Inscribir1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Inscribir0;
                break;

                case "EMGestionar_Miembros":
                    imagen.BackgroundImage = Properties.Resources.Miembros1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Miembros0;
                break;

                case "AMRegistrar_Visita":
                    imagen.BackgroundImage = Properties.Resources.AMRegistrar_Visita1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.AMRegistrar_Visita0;
                break;

                case "AMVer_Visitas":
                    imagen.BackgroundImage = Properties.Resources.AMVerAsistencias1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.AMVerAsistencias0;
                break;

                case "Registrar_Entrenador":
                    imagen.BackgroundImage = Properties.Resources.AMRegistrar_Entrenador1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.AMRegistrar_Entrenador0;
                break;

                case "Admin_Buscar_Miembro":
                    imagen.BackgroundImage = Properties.Resources.buscar1;

                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.buscar0;
                break;

                case "Admin_Editar_Miembro":
                    imagen.BackgroundImage = Properties.Resources.Editar_Miembro0;

                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.Editar_Miembro1;
                break;

                case "Admin_Guardar_Miembro":
                    imagen.BackgroundImage = Properties.Resources.Guardar_Miembro1;

                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.Guardar_Miembro0;
                break;

                case "ILogin": //ANTES "SoyAdmin", AHORA "ILogin" - Nota el nombre del picturebox NO cambio sigue siendo adminpic
                    imagen.BackgroundImage = Properties.Resources.Loguearse1;

                    //Espera imagen de que vuelva a la normalidad
                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Loguearse0;
                break;

                case "ReestablecerContraseña":
                    imagen.BackgroundImage = Properties.Resources.ReestablecerContra1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.ReestablecerContra0;
                break;

                case "OlvideContraseña":
                    imagen.BackgroundImage = Properties.Resources.OlvideMiContraseña1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.OlvideMiContraseña0;
                break;

                case "IMRegistrar":
                    imagen.BackgroundImage= Properties.Resources.IMRegistrarVisita1;
                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.IMRegistrarVisita0;
                break;

                case "IRegistrarV": //Antes "SoyEntrenador", AHORA "IRegistrarV" - Nota el nombre del picturebox NO cambio sigue siendo entrenadorpic
                    imagen.BackgroundImage = Properties.Resources.IRegistrarVisita1;

                    //Espera imagen de que vuelva a la normalidad
                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.IRegistrarVisita0;
                break;

                case "EntrarLogin":
                    imagen.BackgroundImage = Properties.Resources.Entrar1;
                    
                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Entrar0;
                break;

                case "Volver":
                    imagen.BackgroundImage= Properties.Resources.Volver1;

                    await Task.Delay(100);

                    imagen.BackgroundImage = Properties.Resources.Volver0;
                break;

                case "AdminAsistencias":
                    imagen.BackgroundImage = Properties.Resources.Asistencias1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Asistencias0;
                break;

                case "AdminUsuarios":
                    imagen.BackgroundImage = Properties.Resources.Usuarios1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Usuarios0;
                break;

                case "AdminMenuRegistrarMiembroNuevo":
                    imagen.BackgroundImage = Properties.Resources.OPRegistrarUsuario1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.OPRegistrarUsuario0;
                break;

                case "AdminMembresias":
                    imagen.BackgroundImage = Properties.Resources.Membresias1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Membresias0;
                break;

                case "AdminEntrenadores":
                    imagen.BackgroundImage = Properties.Resources.GEntrenador1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.GEntrenador0;
                break;

                case "AdminPagos":
                    imagen.BackgroundImage = Properties.Resources.Pagos1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Pagos0;
                break;

                case "CerrarSesion":
                    imagen.BackgroundImage = Properties.Resources.CerrarSesion1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.CerrarSesion0;
                break;

                case "ModoAdminRegistrarUsuario":
                    imagen.BackgroundImage = Properties.Resources.RegistrarUsuario1;
                    
                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.RegistrarUsuario0;
                break;

                case "ModoAdminRegistrarAdmin":
                    imagen.BackgroundImage = Properties.Resources.RegistrarAdmin1;

                    await Task.Delay(retardo);
                    imagen.BackgroundImage = Properties.Resources.RegistrarAdmin0;
                break;

                case "ModoAdminRegistrarEntrenador":
                    imagen.BackgroundImage = Properties.Resources.RegistrarEntrenador1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.RegistrarEntrenador0;
                break;

                case "Configuracion":
                    imagen.BackgroundImage = Properties.Resources.Configuracion1;

                    await Task.Delay(retardo);

                    imagen.BackgroundImage = Properties.Resources.Configuracion0;
                    break;
                default:

                break;
            }
            Cursor.Current = Cursors.Default;
        }
    }
}

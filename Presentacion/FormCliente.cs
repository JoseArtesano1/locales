using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FormCliente : Form
    {
        ClienteModel clienteModel = new ClienteModel();
        TelefonoModel telefonoModel = new TelefonoModel();
        AlquilerModel alquiler = new AlquilerModel();

        int idCliente,  idTelefCorreo,  idAlquileres;

        public FormCliente()
        {
            InitializeComponent();
            datagridClientes.DataSource = clienteModel.CargarCliente();
            datagridClientes.Columns[0].Visible = false;
            CrearControl(1);

        }


        #region CLIENTE

      

        private void CrearControl(int pag)
        {
            switch (pag)
            {
                case 1:
                    tabControl1.TabPages.Remove(tabPage2);
                    tabControl1.TabPages.Remove(tabPage3);
                    break;

                case 2:
                    tabControl1.TabPages.Add(tabPage2);
                    tabControl1.TabPages.Remove(tabPage3);
                    dataGridcontacto.DataSource = telefonoModel.CargarTablaTef(idCliente);
                    dataGridcontacto.Columns[0].Visible = false;
                    break;

                case 3:
                   
                    tabControl1.TabPages.Add(tabPage3);
                    cargarComboLugar(); cmbNumero.SelectedIndex = -1;
                    dataGridAlquiler.DataSource = alquiler.CargarTablaAlquiler(idCliente);
                    dataGridAlquiler.Columns[0].Visible = false; dataGridAlquiler.Columns[5].Visible = false;
                    break;

                case 4:
                    tabControl1.TabPages.Remove(tabPage3);
                    break;
            }
           

        }


        private void LlamarTab(TabPage page)
        {
            tabControl1.SelectedTab = page;
        }


        private void Limpiar()
        {          
            cmblugar.SelectedIndex = -1; cmbNumero.SelectedIndex = -1;
            txtfianza.Clear(); txtImporte.Clear(); richtextObserv.Clear(); datepickerFechaIn.Value = DateTime.Today;
            checkImp.Checked = false;
        }

        private void txtnombre_Validating(object sender, CancelEventArgs e)
        {
            if (txtnombre.Text != "")
            {
                if (clienteModel.ComprobarString(txtnombre.Text))
                {
                    MessageBox.Show("Introduce un Nombre"); e.Cancel = true;
                }
            }
        }

        private void txtdni_Validating(object sender, CancelEventArgs e)
        {
            if (txtdni.Text != "")
            {
                if (clienteModel.ComprobarString(txtdni.Text)&& txtdni.Text.Length>7)
                {
                    MessageBox.Show("Introduce Dni/Nie"); e.Cancel = true;
                }
            }
        }

        private void txtdireccion_Validating(object sender, CancelEventArgs e)
        {
            if (txtdireccion.Text != "")
            {
                if (clienteModel.ComprobarString(txtdireccion.Text))
                {
                    MessageBox.Show("Introduce una Dirección"); e.Cancel = true;
                }
            }
        }

        private void btnAltaCliente_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text == "") { MessageBox.Show("Introduce el nombre"); txtnombre.Focus();return; }
            if (txtdni.Text == "") { MessageBox.Show("Introduce un dni/nie"); txtdni.Focus(); return; }
            if (txtdireccion.Text == "") { MessageBox.Show("Introducce una Dirección"); txtdireccion.Focus(); return; }
            if (!CheckActivo.Checked) { MessageBox.Show("Activa el Cliente"); CheckActivo.Focus(); return; }
         

            var cliente = new ClienteModel(nombre: txtnombre.Text, dni: txtdni.Text, direccion: txtdireccion.Text, activo: CheckActivo.Checked, tipo: checkboxTipo.Checked);

          string mensaje=  cliente.NuevoCliente();
            if (mensaje.Substring(0, 1) == "G")
            {
                MessageBox.Show(mensaje);
                CrearControl(2); LlamarTab(tabPage2); lblCliente.Text = txtnombre.Text;
            }
            else
            {
                MessageBox.Show(mensaje);
            }
  
            Recarga();
            dataGridcontacto.DataSource = "";
           
        }


        private void datagridClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                
                if (datagridClientes.CurrentRow.Cells[0].Value.ToString() != "")
                {
                     CrearControl(1); 
                    idCliente = int.Parse( datagridClientes.CurrentRow.Cells[0].Value.ToString());
                    txtnombre.Text = datagridClientes.CurrentRow.Cells[1].Value.ToString();
                    lblCliente.Text = datagridClientes.CurrentRow.Cells[1].Value.ToString();
                    txtdni.Text = datagridClientes.CurrentRow.Cells[2].Value.ToString();
                    txtdireccion.Text = datagridClientes.CurrentRow.Cells[3].Value.ToString();
                    if (bool.Parse(datagridClientes.CurrentRow.Cells[4].Value.ToString()))
                    {
                        CheckActivo.Checked = true;
                    }
                    else { CheckActivo.Checked = false; }

                    if (bool.Parse(datagridClientes.CurrentRow.Cells[5].Value.ToString()))
                    {
                        checkboxTipo.Checked = true;
                    }
                    else { checkboxTipo.Checked = false; }
                    btnAltaCliente.Enabled = false;
                   
                    RecargaTelefono(idCliente);
                   
                    Limpiar();
                   
                    CrearControl(2);
                }
                else
                {
                    CrearControl(1); Recarga(); btnAltaCliente.Enabled = true;
                    MessageBox.Show("Selecciona un cliente");
                }
            }
        }


      

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text == "") { MessageBox.Show("Introduce el nombre o selecciona un cliente"); txtnombre.Focus(); return; }
            if (txtdni.Text == "") { MessageBox.Show("Introduce un dni/nie"); txtdni.Focus(); return; }
            if (txtdireccion.Text == "") { MessageBox.Show("Introducce una Dirección"); txtdireccion.Focus(); return; }

            var clienteMod = new ClienteModel(idCliente: idCliente, nombre: txtnombre.Text, dni: txtdni.Text, direccion: txtdireccion.Text, activo: CheckActivo.Checked, tipo: checkboxTipo.Checked);

           string mensaje= clienteMod.EditarCliente();
            MessageBox.Show(mensaje);
            Recarga(); CrearControl(1);
            btnAltaCliente.Enabled = true;
        }


        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text == "") { MessageBox.Show("Selecciona un cliente"); txtnombre.Focus(); return; }
           
            if (MessageBox.Show("Este proceso borra el cliente  de la bd, lo quieres hacer S/N", "CUIDADO",
                MessageBoxButtons.YesNo) == DialogResult.Yes)

            {
                string mensaje = clienteModel.EliminarClientes(idCliente);
                MessageBox.Show(mensaje);
                Recarga(); CrearControl(1);
                btnAltaCliente.Enabled = true;
            }

        }


        private void btnCancelarAlta_Click(object sender, EventArgs e)
        {
            txtnombre.Clear();
            txtdni.Clear();
            txtdireccion.Clear();
            CheckActivo.Checked = false;  checkboxTipo.Checked = false;
            CrearControl(1);
            btnAltaCliente.Enabled = true;
        }


        private void Recarga()
        {
            datagridClientes.DataSource = clienteModel.CargarCliente();
            datagridClientes.Columns[0].Visible = false;
            txtnombre.Clear(); txtnombre.Clear(); txtdireccion.Clear(); txtdni.Clear();
            CheckActivo.Checked = false; checkboxTipo.Checked = false;
        }


        private void RecargaAlquiler(int id)
        {
            dataGridAlquiler.DataSource = alquiler.CargarTablaAlquiler(id);
            
            cmblugar.SelectedIndex = -1;
            cmbNumero.SelectedIndex = -1;
            datepickerFechaIn.Value = DateTime.Today;
            txtfianza.Clear(); txtImporte.Clear();  richtextObserv.Clear(); 
            checkImp.Checked = false; 
        }

        private void RecargaTelefono(int id)
        {
            dataGridcontacto.DataSource = telefonoModel.CargarTablaTef(id);
            txtcorreo.Clear();
            textmovil.Clear();
        }


        #endregion

        #region CONTACTOS

        private void textmovil_Validating(object sender, CancelEventArgs e)
        {
            if (textmovil.Text != "")
            {
                if (!clienteModel.ComprobarTelefono(textmovil.Text))
                {
                    MessageBox.Show("Introduce un teléfono"); e.Cancel = true;
                }
            }
        }

        private void txtcorreo_Validating(object sender, CancelEventArgs e)
        {
            if (textmovil.Text != "")
            {
                if (!telefonoModel.emailOk(txtcorreo.Text))
                {
                    MessageBox.Show("Introduce un correo"); //e.Cancel = true;
                }
            }
        }

        private void btnAltaMovil_Click(object sender, EventArgs e)
        {
            if (textmovil.Text == "") { MessageBox.Show("Introduce Un teléfono"); textmovil.Focus(); return; }
   
            var correoMovil = new TelefonoModel(movil: int.Parse(textmovil.Text), idClient: idCliente, email: txtcorreo.Text);

            string mensaje = correoMovil.NuevoTelefono(txtdni.Text);
            if (mensaje.Substring(0, 1) == "C")
            {
                MessageBox.Show(mensaje);
                CrearControl(3); LlamarTab(tabPage3);
            }
            else { MessageBox.Show(mensaje); }

          
            RecargaTelefono(idCliente);
           
        }



        private void btnmodifmovil_Click(object sender, EventArgs e)
        {
            if (textmovil.Text == "") { MessageBox.Show("Introduce Un teléfono"); textmovil.Focus(); return; }
      
            var correoMovilEdit = new TelefonoModel(idTelefono:idTelefCorreo, movil: int.Parse(textmovil.Text), idClient: idCliente, email: txtcorreo.Text);
             string mensaje=  correoMovilEdit.EditarTelefono();
            MessageBox.Show(mensaje);
            RecargaTelefono(idCliente);
            btnAltaMovil.Enabled = true;
        }



        private void btneliminarMovil_Click(object sender, EventArgs e)
        {
            if (textmovil.Text == "") { MessageBox.Show("Introduce Un teléfono"); textmovil.Focus(); return; }
           

            if (MessageBox.Show("Este proceso borra el Teléfono y el correo del cliente de la bd, lo quieres hacer S/N", "CUIDADO",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje = telefonoModel.EliminarTelefono(idTelefCorreo);
                MessageBox.Show(mensaje);
                RecargaTelefono(idCliente);
                btnAltaMovil.Enabled = true;
            }
        }

     

        private void btnCancelarMovil_Click(object sender, EventArgs e)
        {
            textmovil.Clear();
            txtcorreo.Clear();
            btnAltaMovil.Enabled = true;
            CrearControl(4);
        }


        private void dataGridcontacto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                tabControl1.TabPages.Remove(tabPage3); 
                if (dataGridcontacto.CurrentRow.Cells[0].Value.ToString() != "")
                {                  
                    idTelefCorreo = int.Parse( dataGridcontacto.CurrentRow.Cells[0].Value.ToString());
                    textmovil.Text = dataGridcontacto.CurrentRow.Cells[1].Value.ToString();
                    txtcorreo.Text= dataGridcontacto.CurrentRow.Cells[2].Value.ToString();
                    btnAltaMovil.Enabled = false;
                    RecargaAlquiler(idCliente);
                    Limpiar();
                    
                    CrearControl(3);
                }
                else
                {
                    MessageBox.Show("Selecciona un Teléfono y un correo");
                }
            }
        }

     



        #endregion


        #region ALQUILER

        private void dataGridAlquiler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (dataGridAlquiler.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    idAlquileres = int.Parse( dataGridAlquiler.CurrentRow.Cells[0].Value.ToString());
                     cmbNumero.SelectedValue = int.Parse( dataGridAlquiler.CurrentRow.Cells[5].Value.ToString());
                    cmblugar.SelectedValue = alquiler.GetLugar(int.Parse(dataGridAlquiler.CurrentRow.Cells[5].Value.ToString()));
                 
                    datepickerFechaIn.Value = DateTime.Parse(dataGridAlquiler.CurrentRow.Cells[1].Value.ToString());
                    txtfianza.Text = dataGridAlquiler.CurrentRow.Cells[2].Value.ToString();
                    txtImporte.Text = dataGridAlquiler.CurrentRow.Cells[3].Value.ToString();
                    richtextObserv.Text = dataGridAlquiler.CurrentRow.Cells[4].Value.ToString();

                    if (bool.Parse(dataGridAlquiler.CurrentRow.Cells[6].Value.ToString()))
                    {
                        checkImp.Checked = true;
                    }
                    else { checkImp.Checked = false; }
                    btnaltaAlq.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Selecciona un Alquiler");
                }
            }
        }

      

        private void cargarComboLugar()
        {
            
            cmblugar.DataSource = alquiler.CargarComboLugar();
            cmblugar.DisplayMember = "nombreLugar";
            cmblugar.ValueMember = "nombreLugar";
            cmblugar.SelectedIndex = -1;

        }


        private void cmblugar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmblugar.SelectedIndex != -1)
            {
                cargarComboNumero();
            }
        }

      

        private void cargarComboNumero()
        {               
            cmbNumero.DataSource = alquiler.CargarComboLocal(cmblugar.SelectedValue.ToString());
            cmbNumero.DisplayMember = "numero";
            cmbNumero.ValueMember = "idLocal";
            cmbNumero.SelectedIndex = -1;
        }


        private void txtfianza_Validating(object sender, CancelEventArgs e)
        {
            if (txtfianza.Text != "")
            {
                if (!clienteModel.ComprobarDecimal(txtfianza.Text))
                {
                    MessageBox.Show("Introduce una fianza"); e.Cancel = true;
                }
            }
        }


        private void txtImporte_Validating(object sender, CancelEventArgs e)
        {
            if (txtImporte.Text != "")
            {
                if (!clienteModel.ComprobarDecimal(txtImporte.Text))
                {
                    MessageBox.Show("Introduce un importe"); e.Cancel = true;
                }
            }
        }

        private void richtextObserv_Validating(object sender, CancelEventArgs e)
        {
            if (richtextObserv.Text != "")
            {
                if (clienteModel.ComprobarString(richtextObserv.Text))
                {
                    MessageBox.Show("Introduce un texto"); e.Cancel = true;
                }
            }
        }


        private void btnaltaAlq_Click(object sender, EventArgs e)
        {
            if (cmblugar.SelectedIndex == -1) { MessageBox.Show("Selecciona un lugar"); cmblugar.Focus(); return; }
            if (cmbNumero.SelectedIndex == -1) { MessageBox.Show("Selecciona un número"); cmbNumero.Focus(); return; }
            if (txtfianza.Text == "") { MessageBox.Show("Introduce la fianza"); txtfianza.Focus(); return; }
            if (txtImporte.Text == "") { MessageBox.Show("Introduce un importe"); txtImporte.Focus(); return; }

            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/DocumentosLocales/";
            if (!Directory.Exists(carpeta)) { Directory.CreateDirectory(carpeta); }
            string ruta1 = carpeta + "\\contrato1.docx"; 
            string ruta2 = carpeta + "\\contrato2.docx"; 
           

            var NuevoAlquiler = new AlquilerModel(fechaIni: datepickerFechaIn.Value.Date, fianza: int.Parse (txtfianza.Text), importe: decimal.Parse(txtImporte.Text), observaciones: richtextObserv.Text, idLoc:  cmbNumero.SelectedValue.GetHashCode(), idCl: idCliente, modelo: checkImp.Checked);

              string mensaje = NuevoAlquiler.NuevoAlquiler();
              MessageBox.Show(mensaje);
            

            if (cmblugar.SelectedValue.ToString().StartsWith("P"))
            {
                if (alquiler.isFileOpen(ruta1)) { MessageBox.Show("cierre el documento"); return; }

                string resultado=  NuevoAlquiler.NuevoContrato(lblCliente.Text, datagridClientes.CurrentRow.Cells[3].Value.ToString(), datagridClientes.CurrentRow.Cells[2].Value.ToString(), cmblugar.SelectedValue.ToString(), int.Parse(cmbNumero.SelectedValue.ToString()), int.Parse(dataGridcontacto.CurrentRow.Cells[1].Value.ToString()), dataGridcontacto.CurrentRow.Cells[2].Value.ToString(), ruta1);
                MessageBox.Show(resultado);
            }
            else
            {
                if (alquiler.isFileOpen(ruta2)) { MessageBox.Show("cierre el documento"); return; }
                string resultado1=  NuevoAlquiler.NuevoContrato(lblCliente.Text, datagridClientes.CurrentRow.Cells[3].Value.ToString(), datagridClientes.CurrentRow.Cells[2].Value.ToString(), cmblugar.SelectedValue.ToString(), int.Parse(cmbNumero.SelectedValue.ToString()), int.Parse(dataGridcontacto.CurrentRow.Cells[1].Value.ToString()), dataGridcontacto.CurrentRow.Cells[2].Value.ToString(), ruta2);
                MessageBox.Show(resultado1);
            }
            RecargaAlquiler(idCliente);
            CrearControl(1);
        }



        private void EditarAlquileres(int option)
        {
            if (option == 2)
            { var EditAlquiler = new AlquilerModel(idAlquiler: idAlquileres, fechaIni: datepickerFechaIn.Value, fianza: int.Parse(txtfianza.Text), importe: decimal.Parse(txtImporte.Text), observaciones: richtextObserv.Text, modelo: checkImp.Checked);
              string mensaje = EditAlquiler.EditarAlquiler(2, int.Parse(dataGridAlquiler.CurrentRow.Cells[5].Value.ToString()),idCliente);
              MessageBox.Show(mensaje);
                
            }
            else
            {
                var EditarAlquiler = new AlquilerModel(idAlquiler: idAlquileres, fechaIni: datepickerFechaIn.Value, fianza: int.Parse(txtfianza.Text), importe: decimal.Parse(txtImporte.Text), observaciones: richtextObserv.Text, idLoc: int.Parse(cmbNumero.SelectedValue.ToString()), modelo: checkImp.Checked);
                string mensaje = EditarAlquiler.EditarAlquiler(1, int.Parse(dataGridAlquiler.CurrentRow.Cells[5].Value.ToString()),idCliente);
                MessageBox.Show(mensaje);
                
            }
           
        }

        private void btnmodAlq_Click(object sender, EventArgs e)
        {
            if (cmblugar.SelectedIndex == -1) { MessageBox.Show("Selecciona un lugar"); cmblugar.Focus(); return; }
            if (txtfianza.Text == "") { MessageBox.Show("Introduce la fianza"); txtfianza.Focus(); return; }
            if (txtImporte.Text == "") { MessageBox.Show("Introduce un importe"); txtImporte.Focus(); return; }
           
                if (cmbNumero.SelectedIndex == -1) 
                { 
                  if (MessageBox.Show("¿Quiere comprobar la disponibilidad de locales?", "COMPROBAR", MessageBoxButtons.YesNo) == DialogResult.Yes )
                  {
                    cmbNumero.Focus(); return;

                  }
                  else
                  {
                    if (cmbNumero.SelectedIndex == -1)
                    {
                        EditarAlquileres(2); 
                    }
                    else
                    {
                        EditarAlquileres(1); 
                    }
                  }
               
                }
                else
              {
                EditarAlquileres(1);
              }

            RecargaAlquiler(idCliente);
            btnaltaAlq.Enabled = true;
        }

       

        private void btneliminarAlq_Click(object sender, EventArgs e)
        {
            if (cmblugar.SelectedIndex == -1) { MessageBox.Show("Selecciona un lugar"); cmblugar.Focus(); return; }
      
            if (txtfianza.Text == "") { MessageBox.Show("Introduce la fianza"); txtfianza.Focus(); return; }
            if (txtImporte.Text == "") { MessageBox.Show("Introduce un importe"); txtImporte.Focus(); return; }

            if (MessageBox.Show("Este proceso borra el Alquiler del cliente de la bd, lo quieres hacer S/N", "CUIDADO",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje = alquiler.EliminarAlquiler(idAlquileres, idCliente);
                MessageBox.Show(mensaje);
                RecargaAlquiler(idCliente);
                btnaltaAlq.Enabled = true;
                CrearControl(1);
            }
        }

     

        private void btnCancelarAlq_Click(object sender, EventArgs e)
        {
            cmblugar.SelectedIndex = -1;
            cmbNumero.SelectedIndex = -1;
            txtfianza.Clear();
            txtImporte.Clear();
            richtextObserv.Clear();
            btnaltaAlq.Enabled = true;
        }

        

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        #endregion

    }
}

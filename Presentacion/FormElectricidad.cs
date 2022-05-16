using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FormElectricidad : Form
    {
        ElectricidadModel electricidad = new ElectricidadModel();
        LocalModel localModel = new LocalModel();
        AlquilerModel alquiler = new AlquilerModel();

       public int idElectrico, idLocal, idCliente,numero;
         public string lugar, nombre;
      

        public FormElectricidad()
        {
            InitializeComponent();
            cargarComboLugar();
            tabControl1.TabPages.Remove(tabPage2);
            btnActualiZona.Visible = false;
        }

        private void txtConsumo_Validating(object sender, CancelEventArgs e)
        {
            if (txtConsumo.Text != "")
            {
                if (!electricidad.ComprobarDecimal(txtConsumo.Text))
                {
                    MessageBox.Show("Introduce el consumo total actual"); e.Cancel = true;
                }
            }
        }

       

        private void cargarComboLugar()
        {
            cmbLugar.DataSource = alquiler.CargarComboLugar();
            cmbLugar.DisplayMember = "nombreLugar";
            cmbLugar.ValueMember = "idLugar";
            cmbLugar.SelectedIndex = -1;
        }

        private void LlamarTab(TabPage page)
        {
            tabControl1.SelectedTab = page;
            Recarga();
           
        }

        private void Recarga()
        {            
            datagridElec.DataSource = electricidad.CargaElectricidad(idCliente);
            datagridElec.Columns[0].Visible = false;
            txtConsumo.Clear();
            checkboxEstado.Checked = false;
            dateTimeInicio.Value = DateTime.Today;
            dateTimeFin.Value = DateTime.Today;
        }



        private void datagridLocalCli_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                btnActualiZona.Visible = false;
                tabControl1.TabPages.Remove(tabPage2);
                if (datagridLocalCli.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    idLocal = int.Parse( datagridLocalCli.CurrentRow.Cells[0].Value.ToString());
                    lugar = datagridLocalCli.CurrentRow.Cells[1].Value.ToString();
                    numero= int.Parse(datagridLocalCli.CurrentRow.Cells[2].Value.ToString());
                    idCliente = int.Parse(datagridLocalCli.CurrentRow.Cells[3].Value.ToString());
                    nombre = datagridLocalCli.CurrentRow.Cells[4].Value.ToString();
                  
                      tabControl1.TabPages.Add(tabPage2);
                      LlamarTab(tabPage2);
       
                }
                else
                {
                    MessageBox.Show("Selecciona un local");
                }
            }
        }


      


        private void btnAlta_Click(object sender, EventArgs e)
        {
            if (txtConsumo.Text == "") { MessageBox.Show("Introducce un número"); txtConsumo.Focus(); return; }
            if (checkboxEstado.Checked) { MessageBox.Show("No se debe cambiar el estado"); checkboxEstado.Focus(); return; }
            if (DateTime.Compare(dateTimeInicio.Value.Date, dateTimeFin.Value.Date) > 0) 
            { MessageBox.Show("la fecha del inicio debe ser inferior a la fecha final");dateTimeInicio.Focus(); return; }
            if (DateTime.Compare(alquiler.FechaContrato(idLocal,idCliente), dateTimeInicio.Value.Date) > 0)
            { MessageBox.Show("la fecha del inicio debe ser mayor a la fecha contrato"); dateTimeInicio.Focus(); return; }

            var NuevaElectricidad = new ElectricidadModel(fechaInicio: dateTimeInicio.Value, fechaFin: dateTimeFin.Value, idLoca: idLocal, idCli: idCliente, consumo: decimal.Parse(txtConsumo.Text), estado: checkboxEstado.Checked, importe: 0);

            string mensaje = NuevaElectricidad.NuevaElec();
            
            MessageBox.Show(mensaje);
            Recarga();

        }

     

        private void cmbLugar_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cmbLugar.SelectedIndex != -1)
            {
                datagridLocalCli.DataSource = electricidad.CargaLocalesClientes("", 0, "", 3, int.Parse(cmbLugar.SelectedValue.ToString()));
                datagridLocalCli.Columns[0].Visible = false; datagridLocalCli.Columns[3].Visible = false;
                tabControl1.TabPages.Remove(tabPage2);

                CargarBoton();

            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtConsumo.Clear();
            checkboxEstado.Checked = false;
            dateTimeInicio.Value = DateTime.Today;
            dateTimeFin.Value = DateTime.Today;
            btnAlta.Enabled = true;
        }

       

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void datagridElec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (datagridElec.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    idElectrico = int.Parse(datagridElec.CurrentRow.Cells[0].Value.ToString());
                    dateTimeInicio.Value = DateTime.Parse(datagridElec.CurrentRow.Cells[5].Value.ToString());
                    dateTimeFin.Value= DateTime.Parse(datagridElec.CurrentRow.Cells[6].Value.ToString());
                    txtConsumo.Text = datagridElec.CurrentRow.Cells[7].Value.ToString();
                    if (bool.Parse(datagridElec.CurrentRow.Cells[4].Value.ToString()))
                    {
                        checkboxEstado.Checked = true;
                    }
                    else { checkboxEstado.Checked = false; }
                    btnAlta.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Selecciona un Electricidad");
                }
            }
        }




        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtConsumo.Text == "") { MessageBox.Show("Introducce un número"); txtConsumo.Focus(); return; }
            if (DateTime.Compare(dateTimeInicio.Value.Date, dateTimeFin.Value.Date) > 0)
            { MessageBox.Show("la fecha del inicio debe ser inferior a la fecha final"); dateTimeInicio.Focus(); return; }
            if (DateTime.Compare(alquiler.FechaContrato(idLocal, idCliente), dateTimeInicio.Value.Date) > 0)
            { MessageBox.Show("la fecha del inicio debe ser mayor a la fecha contrato"); dateTimeInicio.Focus(); return; }

            var EditarElectricidad = new ElectricidadModel(idElectricidad: idElectrico,fechaInicio: dateTimeInicio.Value, fechaFin: dateTimeFin.Value, consumo: decimal.Parse(txtConsumo.Text), estado: checkboxEstado.Checked, importe: 0);

            string mensaje = EditarElectricidad.EditarElectricidad();
           
            MessageBox.Show(mensaje);
            Recarga();
            btnAlta.Enabled = true;
        }


        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (txtConsumo.Text == "") { MessageBox.Show("selecciona un registro"); txtConsumo.Focus(); return; }

            if (MessageBox.Show("Este proceso borra la electricidad del cliente de la bd, lo quieres hacer S/N", "CUIDADO",
               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje = electricidad.EliminarElectricidad(idElectrico);
                MessageBox.Show(mensaje);
                Recarga();
                btnAlta.Enabled = true;
            }
        }


        private void btnActualiZona_Click(object sender, EventArgs e)
        {
            if (cmbLugar.SelectedIndex != -1)
            {
                string mensaje = electricidad.EditarEstadoZona(int.Parse(cmbLugar.SelectedValue.ToString()));
                MessageBox.Show(mensaje);
                if (mensaje.Substring(0, 1) == "A")
                {
                    btnActualiZona.Visible = false;
                }
                else { btnActualiZona.Visible = true; }
                Recarga();
            }
            else
            {
                MessageBox.Show("selecciona un Lugar"); cmbLugar.Focus(); return;
            }
        }



        private void CargarBoton()
        {
            if (electricidad.botonesAcumulados(0, 0, int.Parse(cmbLugar.SelectedValue.ToString()), "", 3) && electricidad.NumeroEstados("", int.Parse(cmbLugar.SelectedValue.ToString()), 2) > 1)
            {
                btnActualiZona.Visible = true;
            }
            else
            {
                btnActualiZona.Visible = false;
            }
        }
    }
}

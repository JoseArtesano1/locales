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

       public int idElectrico, idLocal, idCliente,numero;
         public string lugar, nombre;
      

        public FormElectricidad()
        {
            InitializeComponent();
            cargarComboLugar();
            tabControl1.TabPages.Remove(tabPage2);
            // datagridElec.DataSource = electricidad.CargaElectricidad();
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
            cmbLugar.DataSource = electricidad.CargaComboLugar();
            cmbLugar.DisplayMember = "nombreLugar";
            cmbLugar.ValueMember = "nombreLugar";
            // cmbLugar.ValueMember = "idLocal";
            cmbLugar.SelectedIndex = -1;
        }

        private void LlamarTab(TabPage page)
        {
            tabControl1.SelectedTab = page;
        }

        private void datagridLocalCli_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                tabControl1.TabPages.Remove(tabPage2);
                if (datagridLocalCli.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    idLocal = int.Parse( datagridLocalCli.CurrentRow.Cells[0].Value.ToString());
                    lugar = datagridLocalCli.CurrentRow.Cells[1].Value.ToString();
                    numero= int.Parse(datagridLocalCli.CurrentRow.Cells[2].Value.ToString());
                    idCliente = int.Parse(datagridLocalCli.CurrentRow.Cells[3].Value.ToString());
                    nombre = datagridLocalCli.CurrentRow.Cells[4].Value.ToString();
                    datagridElec.DataSource = electricidad.CargaElectricidad(idCliente);
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
         
            var NuevaElectricidad = new ElectricidadModel(fechaInicio: dateTimeInicio.Value, fechaFin: dateTimeFin.Value, idLoca: idLocal, idCli: idCliente, consumo: decimal.Parse(txtConsumo.Text), estado: checkboxEstado.Checked, importe: 0);

            string mensaje = NuevaElectricidad.NuevaElec();
            
            MessageBox.Show(mensaje);
            datagridElec.DataSource = electricidad.CargaElectricidad(idCliente);

        }

        private void cmbLugar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLugar.SelectedIndex != -1 )
            {         
              datagridLocalCli.DataSource = electricidad.CargaLocalesClientes(cmbLugar.SelectedValue.ToString(), 0, "", 3);
            }
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

           
            var EditarElectricidad = new ElectricidadModel(idElectricidad: idElectrico,fechaInicio: dateTimeInicio.Value, fechaFin: dateTimeFin.Value, consumo: decimal.Parse(txtConsumo.Text), estado: checkboxEstado.Checked, importe: 0);

            string mensaje = EditarElectricidad.EditarElectricidad();
           
            MessageBox.Show(mensaje);
            datagridElec.DataSource = electricidad.CargaElectricidad(idCliente);

        }


        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Este proceso borra la electricidad del cliente de la bd, lo quieres hacer S/N", "CUIDADO",
               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje = electricidad.EliminarElectricidad(idElectrico);
                MessageBox.Show(mensaje);
            }
        }




    }
}

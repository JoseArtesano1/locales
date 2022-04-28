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
    public partial class FormImporte : Form
    {

        AlquilerModel alquiler = new AlquilerModel();
        ElectricidadModel electricidad = new ElectricidadModel();
        LocalModel local = new LocalModel();
        int numero=0; string nombre, direccion, dni;
        double importe;

        public FormImporte()
        {
            InitializeComponent();
            CargarCmboxLugar();
            btnIndividual.Visible = false;
            datagridLocC.DataSource = "";
        }


        private void CargarCmboxLugar()
        {
            cmbLugar.DataSource = alquiler.CargarComboLugar();
            cmbLugar.DisplayMember= "nombreLugar";
            cmbLugar.ValueMember = "nombreLugar";
            cmbLugar.SelectedIndex = -1;
        }


        private void cmbLugar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLugar.SelectedIndex != -1)
            {
                btnIndividual.Visible = false; btnzona.Enabled = true;
                datagridLocC.DataSource = electricidad.CargaLocalesClientes(cmbLugar.SelectedValue.ToString(), 0, "", 0, 0);
                datagridLocC.Columns[0].Visible = false; datagridLocC.Columns[2].Visible = false; datagridLocC.Columns[5].Visible = false;
            }
        }

             

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.Text != "")
            {
                if(!local.ComprobarString(textBox1.Text))
                {
                    MessageBox.Show("Introduce un número"); e.Cancel = true;
                }
            }
        }

      

        private void btnzona_Click(object sender, EventArgs e)
        {
            if (cmbLugar.SelectedIndex == -1) { MessageBox.Show("Selecciona un lugar"); cmbLugar.Focus(); return; }
            if (textBox1.Text == "") { MessageBox.Show("Introduce un número de Factura"); textBox1.Focus(); return; }

            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/DocumentosLocales/";
            string ruta = "\\FRA.xlsx";
            string mensaje = alquiler.NuevasFacturas(carpeta, ruta, cmbLugar.SelectedValue.ToString(), dateTimePicker1.Value, int.Parse(textBox1.Text));
            MessageBox.Show(mensaje);
            btnIndividual.Visible = false; btnzona.Enabled = true;
            limpiar();
        }

      

        private void datagridLocC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                btnIndividual.Visible = false;
                if (datagridLocC.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    numero = int.Parse( datagridLocC.CurrentRow.Cells[1].Value.ToString());
                    nombre = datagridLocC.CurrentRow.Cells[3].Value.ToString();
                    direccion= datagridLocC.CurrentRow.Cells[7].Value.ToString();
                    dni= datagridLocC.CurrentRow.Cells[6].Value.ToString();
                    importe= double.Parse(datagridLocC.CurrentRow.Cells[8].Value.ToString());
                    btnIndividual.Visible = true; btnzona.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Selecciona un local-cliente");
                }
            }
        }


        private void btnIndividual_Click(object sender, EventArgs e)
        {
            if (cmbLugar.SelectedIndex == -1) { MessageBox.Show("Selecciona un lugar"); cmbLugar.Focus(); return; }
            if (textBox1.Text == "") { MessageBox.Show("Introduce un número de Factrua"); textBox1.Focus(); return; }
            if(datagridLocC.CurrentRow.Cells[0].Value.ToString() == "") { MessageBox.Show("Selecciona un cliente"); datagridLocC.Focus(); return; }

            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/DocumentosLocales/";
            string ruta = "\\FRAS.xlsx";
            string mensaje = alquiler.NuevaFactura(carpeta, ruta, nombre, direccion, dni, cmbLugar.SelectedValue.ToString(), importe, dateTimePicker1.Value, int.Parse(textBox1.Text));
            MessageBox.Show(mensaje);
            limpiar(); btnIndividual.Visible = false; btnzona.Enabled = true;
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
            btnIndividual.Visible = false; btnzona.Enabled = true;
        }

        private void limpiar()
        {
            cmbLugar.SelectedIndex = -1;
            textBox1.Clear();
            datagridLocC.DataSource = "";
            dateTimePicker1.Value = DateTime.Today;
        }


    }
}

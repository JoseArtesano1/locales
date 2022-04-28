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
    public partial class FormConsulta : Form
    {
        ElectricidadModel electricidad = new ElectricidadModel();
        AlquilerModel alquiler = new AlquilerModel();
        ClienteModel cliente = new ClienteModel();
        int idCliente;
        public FormConsulta()
        {
            InitializeComponent();
            cargarComboLugar();
            CargarCmbConsulta();
            CargarConsultas2();
            OcultarControles(true, false);
            btnInforme.Enabled = false;
        }


        private void OcultarControles(bool verdad, bool falso)
        {
            cmbConsulta.Visible = falso; lblInformar.Visible = falso; cmbConsulta2.Visible = falso;
            lblINFORMACION.Visible = verdad; datagridConsultas.Visible = falso; dataGridDatos.Visible = falso;
            cmbZonas.Visible = falso; lblzona.Visible = falso; lblInfZona.Visible = falso; lblconsulta.Visible = falso; lbldato.Visible = falso;
        }

        public void EditCtl(System.Windows.Forms.ComboBox combo, bool valor, System.Windows.Forms.ComboBox combo1, bool valor1, System.Windows.Forms.Label label1, System.Windows.Forms.Label label2)
        {
            combo.Visible = valor; label1.Visible = valor;
            combo1.Visible = valor1; label2.Visible = valor1;
        }


        private void Recarga()
        {
            dataGridDatos.DataSource = "";
            cmbZonas.SelectedIndex = -1;
            cmbConsulta.SelectedIndex = -1;
        }

        private void cargarComboLugar()
        {
            cmbZonas.DataSource = alquiler.CargarComboLugar();
            cmbZonas.DisplayMember = "nombreLugar";
            cmbZonas.ValueMember = "idLugar";
            cmbZonas.SelectedIndex = -1;

        }

       public void CargarCmbConsulta()
        {
            foreach(var item in cliente.CargaConsulta())
            {
                cmbConsulta.Items.Add(item);
            }
        }

        public void CargarConsultas2()
        {
            foreach(var item in cliente.CargaConsulta2())
            {
                cmbConsulta2.Items.Add(item);
            }
        }



        private void cmbZonas_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbZonas.SelectedIndex != -1)
            {   
                cmbConsulta2.SelectedIndex = -1; dataGridDatos.DataSource = "";
               
                if (rdbtnZona.Checked == true) { cmbConsulta2.Visible = true; lblInfZona.Visible = true;  }
                datagridConsultas.DataSource = alquiler.ConsultaCliente(int.Parse(cmbZonas.SelectedValue.ToString()));
                datagridConsultas.Columns[0].Visible = false;

                btnInforme.Enabled = true;
            }
        }




        private void datagridConsultas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (datagridConsultas.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    dataGridDatos.DataSource = ""; cmbConsulta.SelectedIndex = -1;
                    idCliente = int.Parse(datagridConsultas.CurrentRow.Cells[0].Value.ToString());
                    EditCtl(cmbConsulta, true, cmbConsulta2, false,lblInformar,lblInfZona);
                   
                }
                else
                {
                    MessageBox.Show("Selecciona un Cliente");
                }
            }
        }

       

        private void cmbConsulta_SelectionChangeCommitted(object sender, EventArgs e)
        {

            switch (cmbConsulta.SelectedIndex)
            {                 
                case 0:
                    dataGridDatos.DataSource = alquiler.ConsultaTelefono(idCliente);
                    dataGridDatos.Columns[0].Visible = false; dataGridDatos.Columns[2].Visible = false;
                    break;

                case 1:
                    dataGridDatos.DataSource = alquiler.ConsultaAlquiler(idCliente, int.Parse(cmbZonas.SelectedValue.ToString()));
                    dataGridDatos.Columns[0].Visible = false;
                    break;

                case 2:
                    dataGridDatos.DataSource = alquiler.ConsultaElectricidad(idCliente, int.Parse(cmbZonas.SelectedValue.ToString()));
                    dataGridDatos.Columns[0].Visible = false;
                    break;
               
            }

        }


        private void cmbConsulta2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cmbConsulta2.SelectedIndex)
            {
                case 0:
                    dataGridDatos.DataSource = alquiler.ConsultaInactivo(int.Parse(cmbZonas.SelectedValue.ToString()));
                    dataGridDatos.Columns[0].Visible = false;
                    break;

                case 1:
                    dataGridDatos.DataSource = alquiler.ConsultaCartera(int.Parse(cmbZonas.SelectedValue.ToString()));
                    dataGridDatos.Columns[0].Visible = false;
                    break;
            }
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdbtnZona_CheckedChanged(object sender, EventArgs e)
        {
            lblINFORMACION.Visible = false; btnInforme.Enabled = false;
            datagridConsultas.DataSource = null;
            if (rdbtnZona.Checked == true)
            {
                Recarga(); 
                datagridConsultas.Visible = false; lblconsulta.Visible = false;
                dataGridDatos.Location = new Point(55, 80); lbldato.Location = new Point(32, 58);
                dataGridDatos.Visible = true; lbldato.Visible = true;
                EditCtl(cmbZonas, true, cmbConsulta, false, lblzona, lblInformar);
               
            }
            else
            {
                datagridConsultas.Visible = true; lblconsulta.Visible = true;

            }
        }

        private void rdbtnIndividuo_CheckedChanged(object sender, EventArgs e)
        {
            lblINFORMACION.Visible = false; btnInforme.Enabled = false;
            datagridConsultas.DataSource = null;
            if (rdbtnIndividuo.Checked == true)
            {
                Recarga();
                datagridConsultas.Visible = true; lblconsulta.Visible = true;
                dataGridDatos.Location = new Point(90, 410); lbldato.Location = new Point(65, 385);
                dataGridDatos.Visible = true; lbldato.Visible = true; lbldato.Text = "DATOS";
                EditCtl(cmbZonas, true, cmbConsulta2, false, lblzona, lblInfZona);
               
            }
            else
            {
              
                datagridConsultas.Visible = false; lblconsulta.Visible = false;
            }
        }

        private void btnInforme_Click(object sender, EventArgs e)
        {
            if (cmbZonas.SelectedIndex != -1)
            {                              
                if (alquiler.isFileOpen(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/DocumentosLocales/" + "\\Informe.pdf")) { MessageBox.Show("cierre el documento"); return; }
                string mensaje= electricidad.GenerarInforme( int.Parse(cmbZonas.SelectedValue.ToString()));
                MessageBox.Show(mensaje);
            }
        }
    }
}

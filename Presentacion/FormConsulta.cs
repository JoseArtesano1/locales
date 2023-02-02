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
        DataGridViewButtonColumn Editar = new DataGridViewButtonColumn();
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
                cmbConsulta2.SelectedIndex = -1; dataGridDatos.DataSource = ""; idCliente = 0; cmbConsulta.SelectedIndex = -1;
                AddColumData(1);
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
                    AddColumData(1);
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
                    dataGridDatos.DataSource = alquiler.ConsultaTelefono(idCliente); AddColumData(1);
                    dataGridDatos.Columns[0].Visible = false; dataGridDatos.Columns[2].Visible = false; 
                  
                    break;

                case 1:
                    dataGridDatos.DataSource = alquiler.ConsultaAlquiler(idCliente, int.Parse(cmbZonas.SelectedValue.ToString())); 
                    dataGridDatos.Columns[0].Visible = false; AddColumData(2);
                    break;

                case 2:
                    dataGridDatos.DataSource = alquiler.ConsultaElectricidad(idCliente, int.Parse(cmbZonas.SelectedValue.ToString())); AddColumData(1);
                    dataGridDatos.Columns[0].Visible = false; dataGridDatos.Columns["importe"].DisplayIndex = 7; dataGridDatos.Columns["consumo"].DisplayIndex = 5;

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
                    dataGridDatos.DataSource = alquiler.ConsultaCartera(int.Parse(cmbZonas.SelectedValue.ToString()),false);
                    dataGridDatos.Columns[0].Visible = false; dataGridDatos.Columns["numero"].DisplayIndex = 1;
                    break;

                case 2:
                    dataGridDatos.DataSource = electricidad.CargaListadoElectricidad("", 0, int.Parse(cmbZonas.SelectedValue.ToString()), 3);
                    dataGridDatos.Columns["importe"].DisplayIndex = 6;
                    break;

                case 3:
                    dataGridDatos.DataSource = alquiler.ConsultaCartera(int.Parse(cmbZonas.SelectedValue.ToString()),true);
                     dataGridDatos.Columns[0].Visible = false; dataGridDatos.Columns["numero"].DisplayIndex = 4;
                    break;
            }
        }

        #region EDITAR CON NUEVA COLUMNA
        private void AddColumData(int opcion)
        {            
            Editar.Name = "Modificar";
            Editar.Width = 40;
         
            if (opcion == 1)
            {
                if (dataGridDatos.Columns.Contains("Modificar"))
                {   dataGridDatos.Columns.Remove(Editar);
                }
            }
            else
            {
                if (dataGridDatos.Columns.Contains("Modificar"))
                { dataGridDatos.Columns.Remove(Editar);
                }
                else
                {   dataGridDatos.Columns.Add(Editar);
                    dataGridDatos.Columns["Modificar"].DisplayIndex = 6;
                    dataGridDatos.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridDatos.Columns[6].DefaultCellStyle.Padding= new Padding(7, 0, 7, 0);
                   
                }
            }
     
        }



        private void dataGridDatos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && this.dataGridDatos.Columns[e.ColumnIndex].Name == "Modificar" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                DataGridViewButtonCell celBoton = this.dataGridDatos.Rows[e.RowIndex].Cells["Modificar"] as DataGridViewButtonCell;
                Icon icoAtomico = new Icon(Environment.CurrentDirectory + @"\\cambiar.ico");/////Recuerden colocar su icono en la carpeta debug de su proyecto

                e.Graphics.DrawIcon(icoAtomico, e.CellBounds.Left + 16, e.CellBounds.Top + 1);

                this.dataGridDatos.Rows[e.RowIndex].Height = icoAtomico.Height + 4;
                this.dataGridDatos.Columns[e.ColumnIndex].Width = icoAtomico.Width - 1;

                e.Handled = true;

            }

        }


        private void dataGridDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridDatos.Columns[e.ColumnIndex].Name=="Modificar")
            {
                if (e.RowIndex >= 0)
                {
                   if ( dataGridDatos.CurrentRow.Cells[0].Value?.ToString() != "")
                   {                       
                        string notas = dataGridDatos.CurrentRow.Cells[4]?.Value.ToString();
                        decimal imp = decimal.Parse(dataGridDatos.CurrentRow.Cells[3].Value?.ToString());
                        int idAlqui = int.Parse(dataGridDatos.CurrentRow.Cells[0].Value?.ToString());
                        FormEdit edit = new FormEdit(idAlqui, imp, notas);
                        edit.ShowDialog();
 
                        dataGridDatos.DataSource = alquiler.ConsultaAlquiler(idCliente, int.Parse(cmbZonas.SelectedValue.ToString())); ;
                        if (dataGridDatos.Columns.Contains("Modificar")) //  reordenar el datagrid
                        {
                            dataGridDatos.Columns.Remove(Editar);
                            dataGridDatos.Columns.Add(Editar);
                            dataGridDatos.Columns["Modificar"].DisplayIndex = 6;
                        }
   
                    }
                    else
                    {
                        MessageBox.Show("Selecciona un Alquiler");
                    }

                }
                    
            }
            
        }


        #endregion

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
                dataGridDatos.Height = 325;
                dataGridDatos.Visible = true; lbldato.Visible = true;
                EditCtl(cmbZonas, true, cmbConsulta, false, lblzona, lblInformar);
               
            }
            else
            {
                datagridConsultas.Visible = true; lblconsulta.Visible = true;
                dataGridDatos.Height = 100;
            }
        }

        private void rdbtnIndividuo_CheckedChanged(object sender, EventArgs e)
        {
            lblINFORMACION.Visible = false; btnInforme.Enabled = false;
            datagridConsultas.DataSource = null; AddColumData(1);
            if (rdbtnIndividuo.Checked == true)
            {
                Recarga();
                datagridConsultas.Visible = true; lblconsulta.Visible = true;
                datagridConsultas.Height = 250; dataGridDatos.Height = 100;
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

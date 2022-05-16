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
    public partial class FormCalculos : Form
    {
        ElectricidadModel electricidad = new ElectricidadModel();
        LocalModel localModel = new LocalModel();

        int idElectrico, idLocal, idCliente,  numero, idlugar;
        string zona, milugar;
        decimal consumo, miImporte;
        DateTime fecha1, fecha2;

        public FormCalculos()
        {
            InitializeComponent();
            cargarComboLugar();
            CrearControl(1);
            btnacumulado.Visible = false; btnIndAcum.Visible = false;
        }


        private void Recargar()
        {
            dataGridZona.DataSource = "";
            dataGridZona.Columns.Clear();
            CrearControl(1);
        }

        private void ColumnasOcultas()
        {
            datagridLocalCli.Columns[0].Visible = false; datagridLocalCli.Columns[3].Visible = false;
            datagridLocalCli.Columns[6].Visible = false;
        }

        private void CrearControl(int pag)
        {
            switch (pag)
            {
                case 1:
                    tabControl1.TabPages.Remove(tabpg2);
                    tabControl1.TabPages.Remove(tabPage3);
                    break;

                case 2:
                    tabControl1.TabPages.Add(tabpg2);
                    tabControl1.TabPages.Remove(tabPage3);
                    datagridListado.DataSource = electricidad.CargaListadoElectricidad(zona, idCliente,0, 1);
                    datagridListado.Columns[0].Visible = false;
                    break;

                case 3:
                    tabControl1.TabPages.Add(tabPage3);
                    tabControl1.TabPages.Remove(tabpg2);
                    dataGridZona.DataSource = electricidad.CargaListadoElectricidad(zona, 0,0, 2);
                    dataGridZona.Columns[0].Visible = false;
                    break;
            }

        }

        private void LlamarTab(TabPage page)
        {
            tabControl1.SelectedTab = page;
        }

      

        private void txtNumero_Validating_1(object sender, CancelEventArgs e)
        {
            if (txtNumero.Text != "")
            {
                if (!electricidad.ComprobarInt(txtNumero.Text))
                {
                    MessageBox.Show("Introduce Número de local"); e.Cancel = true;
                }
            }
        }


        private void txtnombre_Validating_1(object sender, CancelEventArgs e)
        {
            if (txtnombre.Text != "")
            {
                if (electricidad.ComprobarString(txtnombre.Text))
                {
                    MessageBox.Show("Introduce Nombre de cliente"); e.Cancel = true;
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

        private void cmbLugar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLugar.SelectedIndex != -1)
            {
                zona = cmbLugar.SelectedValue.ToString();
                CrearControl(1);
            }
        }

        private void btnLimpia_Click(object sender, EventArgs e)
        {
            txtnombre.Clear(); txtNumero.Clear(); cmbLugar.SelectedIndex = -1; datagridLocalCli.DataSource = "";
            CrearControl(1);
        }

      

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            Recargar();
           

            if (!txtnombre.Text.Trim().Equals(""))
            {
                datagridLocalCli.DataSource = electricidad.CargaLocalesClientes("", 0, txtnombre.Text.Trim(), 2,0);
                ColumnasOcultas();
            }

            if (cmbLugar.SelectedIndex != -1)
            {
                if (!txtNumero.Text.Trim().Equals(""))
                {
                    datagridLocalCli.DataSource = electricidad.CargaLocalesClientes(cmbLugar.SelectedValue.ToString(), int.Parse(txtNumero.Text.Trim()), "", 1,0);
                    ColumnasOcultas();
                }
                else
                {
                    btnZona.Enabled = false;
            
                    if (electricidad.CargaListadoElectricidad(zona, 0,0, 2).Rows.Count != 0) 
                    {
                        datagridLocalCli.DataSource = "";
                        btnZona.Enabled = true; 
                         CrearControl(3);
                         LlamarTab(tabPage3);

                    }

                    if (electricidad.botonesAcumulados(0, 0, 0, zona, 2) && electricidad.NumeroEstados(zona, 0, 1) > 1)
                    {
                        btnacumulado.Visible = true;
                    }
                    else { btnacumulado.Visible = false; }

                }
            }
        }

      

        private void cargarComboLugar()
        {
            cmbLugar.DataSource = electricidad.CargaComboLugar();
            cmbLugar.DisplayMember = "nombreLugar";
            cmbLugar.ValueMember = "nombreLugar";
            cmbLugar.SelectedIndex = -1;

        }


        private void datagridLocalCli_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                CrearControl(1);
                if (datagridLocalCli.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    btnIndiv.Enabled = false;
                    idLocal = int.Parse(datagridLocalCli.CurrentRow.Cells[0].Value.ToString());
                    milugar = datagridLocalCli.CurrentRow.Cells[1].Value.ToString();
                    idlugar = int.Parse(datagridLocalCli.CurrentRow.Cells[6].Value.ToString());
                    numero = int.Parse(datagridLocalCli.CurrentRow.Cells[2].Value.ToString());
                    idCliente = int.Parse(datagridLocalCli.CurrentRow.Cells[3].Value.ToString());

                    if(electricidad.CargaListadoElectricidad(zona, idCliente,0, 1).Rows.Count != 0)
                    { 
                      btnIndiv.Enabled = true;
                      CrearControl(2);
                      LlamarTab(tabpg2);
                    }
                    if (electricidad.botonesAcumulados(idLocal, idCliente, 0, "", 1)) { btnIndAcum.Visible = true; } else { btnIndAcum.Visible = false; }
                }
                else
                {
                    MessageBox.Show("Selecciona un local");
                }
            }
        }


     


     

        private void btnIndiv_Click(object sender, EventArgs e)
        {
            if (idElectrico!=0)
            { 
                decimal import = electricidad.ImporteTotalPot(fecha1, fecha2, idLocal, idCliente, milugar, consumo) + electricidad.ImporteTotalEnerg(fecha1, fecha2, idLocal, idCliente, consumo);
                string mensaje = electricidad.EditarImporteIndividual(idElectrico, import);
                if (mensaje.Substring(0, 1) == "I")
                {
                    MessageBox.Show(mensaje);
                    btnIndAcum.Visible = true; LlamarTab(tabPage1);
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
            else
            { MessageBox.Show("Selecciona un registro"); }
        }


        private void datagridListado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (datagridListado.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    idElectrico = int.Parse(datagridListado.CurrentRow.Cells[0].Value.ToString());
                    fecha1 = DateTime.Parse(datagridListado.CurrentRow.Cells[5].Value.ToString());
                    fecha2 = DateTime.Parse(datagridListado.CurrentRow.Cells[6].Value.ToString());
                    consumo = decimal.Parse(datagridListado.CurrentRow.Cells[10].Value.ToString());
                    miImporte = decimal.Parse(datagridListado.CurrentRow.Cells[9].Value.ToString());
                    if (miImporte != 0) { btnIndiv.Enabled = false; btnIndAcum.Visible = true; }
                    
                }
                else
                {
                    MessageBox.Show("Selecciona un registro Eléctrico");
                }
            }
        }



        private void btnZona_Click(object sender, EventArgs e)
        {
            if (cmbLugar.SelectedIndex == -1) { MessageBox.Show("Selecciona un lugar"); cmbLugar.Focus(); return; }

            string mensaje=electricidad.EditarImporteZona(zona);

            if (mensaje.Substring(0, 1) == "A")
            {
                MessageBox.Show(mensaje);
                btnacumulado.Visible = true; LlamarTab(tabPage1);
            }
            else
            {
                MessageBox.Show(mensaje);
            }
            
            Recargar();
        }


        private string Calculos(decimal consum)
        {
            var Localmd = new LocalModel(idLocal: idLocal, acumulado: consum);
            return Localmd.ActualizarAcumulado();
        }

        private void btnIndAcum_Click(object sender, EventArgs e)
        {
            decimal consumo = localModel.GetConsumo(idLocal, idCliente);
            string mensaje2 = Calculos(consumo);
            MessageBox.Show(mensaje2);
            if (mensaje2.Substring(0, 1) == "A")
            {
                btnIndAcum.Visible = false;
            }
            else { btnIndAcum.Visible = true; }
        }


        private void btnacumulado_Click(object sender, EventArgs e)
        {
            string mensaje = electricidad.EditarAcumuladoZona(zona);

            MessageBox.Show(mensaje);
            if (mensaje.Substring(0, 1) == "A")
            {
                btnacumulado.Visible = false;
            }
            else { btnacumulado.Visible = true; }
        }

    }
}

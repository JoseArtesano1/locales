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

        int idElectrico, idLocal, idCliente, idcli, numero, idlugar;
        string zona, milugar;
        decimal consumo;
        DateTime fecha1, fecha2;

        public FormCalculos()
        {
            InitializeComponent();
            cargarComboLugar();
            CrearControl(1);
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
                    break;

                case 3:
                    tabControl1.TabPages.Add(tabPage3);
                    tabControl1.TabPages.Remove(tabpg2);
                    break;
            }

        }

        private void LlamarTab(TabPage page)
        {
            tabControl1.SelectedTab = page;
        }

        private void txtNumero_Validating(object sender, CancelEventArgs e)
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
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
             
            CrearControl(1);

            if (!txtnombre.Text.Trim().Equals(""))
            {
                datagridLocalCli.DataSource = electricidad.CargaLocalesClientes("", 0, txtnombre.Text.Trim(), 2);
            }

            if (cmbLugar.SelectedIndex != -1)
            {
                if (!txtNumero.Text.Trim().Equals(""))
                {
                    datagridLocalCli.DataSource = electricidad.CargaLocalesClientes(cmbLugar.SelectedValue.ToString(), int.Parse(txtNumero.Text.Trim()), "", 1);
                    
                }
                else
                {
                    dataGridZona.DataSource = electricidad.CargaListadoElectricidad(zona, 0, 2);
                   
                    CrearControl(3);
                    LlamarTab(tabPage3);
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


        private void datagridLocalCli_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                CrearControl(1);
                if (datagridLocalCli.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    idLocal = int.Parse(datagridLocalCli.CurrentRow.Cells[0].Value.ToString());
                    milugar = datagridLocalCli.CurrentRow.Cells[1].Value.ToString();
                    idlugar = int.Parse(datagridLocalCli.CurrentRow.Cells[6].Value.ToString());
                    numero = int.Parse(datagridLocalCli.CurrentRow.Cells[2].Value.ToString());
                    idCliente = int.Parse(datagridLocalCli.CurrentRow.Cells[3].Value.ToString());
                  
                    datagridListado.DataSource = electricidad.CargaListadoElectricidad(zona, idCliente, 1);
                    CrearControl(2);
                    LlamarTab(tabpg2);
                }
                else
                {
                    MessageBox.Show("Selecciona un local");
                }
            }
        }


        private string Calculos( decimal consum)
        {
            var Localmd = new LocalModel(idLocal: idLocal, acumulado: consum);
            return Localmd.ActualizarAcumulado();
        }

        private void btnIndiv_Click(object sender, EventArgs e)
        {
            if (datagridListado.CurrentRow.Cells[0].Value.ToString() == "") { MessageBox.Show("Selecciona un registro"); datagridListado.Focus(); return; }

            decimal import= electricidad.ImporteTotalPot(fecha1, fecha2, idLocal, idCliente, milugar, consumo) + electricidad.ImporteTotalEnerg(fecha1, fecha2,idLocal,idCliente,consumo);
            string mensaje= electricidad.EditarImporteIndividual(idElectrico, import);
            MessageBox.Show(mensaje);
            string mensaje2 = Calculos(consumo);
            MessageBox.Show(mensaje2);
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
                    consumo = decimal.Parse(datagridListado.CurrentRow.Cells[11].Value.ToString());
                    
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
            MessageBox.Show(mensaje);
            electricidad.EditarAcumuladoZona(zona);
            
        }



    }
}

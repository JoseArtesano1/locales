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
    public partial class FormModLc : Form
    {
        LocalModel local = new LocalModel();
        AlquilerModel alquiler = new AlquilerModel();
        public FormModLc()
        {
            InitializeComponent();
            
        }


        public FormModLc(int id, int idlo, string lug, int num, decimal acum)
        {
            InitializeComponent();
            Mostrar(id, idlo, lug, num, acum);
            lblID.Visible = false;
            lblIdLoc.Visible = false;
            cargarCmbLugar();
            cmbLugar.Visible = false; txtlugar.Visible = true; txtnumero.Visible = false; txtacumulado.Visible = false;
            btnEdit2.Visible = false; label3.Visible = false; label4.Visible = false;
        }


        public void Mostrar(int id,int idlo,string lug, int num, decimal acum )
        {
            try
            {
                lblID.Text = id.ToString();
                lblIdLoc.Text = idlo.ToString();
                txtacumulado.Text = acum.ToString();
                txtlugar.Text = lug;
                txtnumero.Text = num.ToString();
                cmbLugar.SelectedValue = id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void cargarCmbLugar()
        {
            cmbLugar.DataSource = alquiler.CargarComboLugar();
            cmbLugar.DisplayMember = "nombreLugar";
            cmbLugar.ValueMember = "idLugar";
        
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Quiere modificar el lugar?", "CUIDADO", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (txtlugar.Text == "") { MessageBox.Show("Introducce una Dirección"); txtlugar.Focus(); return; }
                var lugar = new LugarModel(idLugar: int.Parse(lblID.Text), nombreLugar: txtlugar.Text);
                string mensaje = lugar.EditarLugarlocal();
                MessageBox.Show(mensaje);
                cargarCmbLugar(); 
            }

            btnEdit2.Visible = true; btnEdit.Visible = false; label3.Visible = false; label4.Visible = false;
            cmbLugar.Visible = true; txtlugar.Visible = false; txtnumero.Visible = true; txtacumulado.Visible = true;
        }


        private void btnEdit2_Click(object sender, EventArgs e)
        {   

            if (MessageBox.Show("¿Quiere modificar el local?", "CUIDADO", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (txtnumero.Text == "") { MessageBox.Show("Introducce un número"); txtnumero.Focus(); return; }
                if (txtacumulado.Text == "") { MessageBox.Show("Introducce Energía acumulada"); txtacumulado.Focus(); return; }
                if (cmbLugar.SelectedIndex == -1) { MessageBox.Show("Debe seleccionar un lugar"); cmbLugar.Focus(); return; }


                var local = new LocalModel(idLocal: int.Parse(lblIdLoc.Text), idLug: int.Parse(cmbLugar.SelectedValue.ToString()), numero: int.Parse(txtnumero.Text), acumulado: decimal.Parse(txtacumulado.Text));

                string mensaje = local.EditarLocal();
                MessageBox.Show(mensaje);
                this.Close();
            }
            else
            {
                MessageBox.Show("Sin Modificar Local");
                this.Close();
            }
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtacumulado.Clear(); txtlugar.Clear(); txtnumero.Clear(); cmbLugar.SelectedIndex = -1;
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtlugar_Validating(object sender, CancelEventArgs e)
        {
            if (txtlugar.Text != "")
            {
                if (local.ComprobarString(txtlugar.Text))
                {
                    MessageBox.Show("Introduce una Dirección"); e.Cancel = true;
                }
            }
        }

        private void txtnumero_Validating(object sender, CancelEventArgs e)
        {
            if (txtnumero.Text != "")
            {
                if (!local.ComprobarString(txtnumero.Text))
                {
                    MessageBox.Show("Introduce un número"); e.Cancel = true;
                }
            }
        }

        private void txtacumulado_Validating(object sender, CancelEventArgs e)
        {
            if (txtacumulado.Text != "")
            {
                if (!local.ComprobarDecimal(txtacumulado.Text))
                {
                    MessageBox.Show("Introduce una cantidad"); e.Cancel = true;
                }
            }
        }

      
    }
}

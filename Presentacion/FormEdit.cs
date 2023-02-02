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
    public partial class FormEdit : Form
    {
        AlquilerModel alquiler = new AlquilerModel();
        ElectricidadModel electrico = new ElectricidadModel();

        public FormEdit()
        {
            InitializeComponent();

        }

        public FormEdit(int id, decimal import, string ob)
        {
            InitializeComponent();
            Mostrar(id, import, ob);
            lblID.Visible = false;
        }

      
        public void Mostrar(int id, decimal import, string ob)
        {
            try
            {
                lblID.Text = id.ToString();
                txtImp.Text = import.ToString();
                rtxtObserv.Text = ob;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
           
            txtImp.Clear(); rtxtObserv.Clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtImp.Text == "") { MessageBox.Show("sin Importe"); txtImp.Focus(); return; }
            var alquile = new AlquilerModel(idAlquiler: int.Parse( lblID.Text), importe: decimal.Parse(txtImp.Text), observaciones: rtxtObserv.Text);
            string mensaje=alquile.EditarAlquilerConsulta();
            MessageBox.Show(mensaje);
            if (mensaje.Substring(0, 1) == "A")
            {
                this.Close();
            }
            else { MessageBox.Show("Error"); }
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtImp_Validating(object sender, CancelEventArgs e)
        {
            if (txtImp.Text != "")
            {
                if (!electrico.ComprobarDecimal(txtImp.Text))
                {
                    MessageBox.Show("Introduce Importe"); e.Cancel = true;
                }
            }
        }

        private void rtxtObserv_Validating(object sender, CancelEventArgs e)
        {
            if (rtxtObserv.Text != "")
            {
                if (electrico.ComprobarString(rtxtObserv.Text))
                {
                    MessageBox.Show("Introduce Texto"); e.Cancel = true;
                }
            }
        }
    }
}

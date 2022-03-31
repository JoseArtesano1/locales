using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Cache;
using Domain;

namespace Presentacion
{
    public partial class FormUsuarios2 : Form
    {
        UserModel modelo = new UserModel();
        public FormUsuarios2()
        {
            InitializeComponent();
            mostrar();
            txtConfirm.Visible = false; lblconfir.Visible = false; grupboxDatos.Visible = false;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text == "") { MessageBox.Show("Introduce un nombre"); txtnombre.Focus(); return; }
            if (txtusuario.Text == "") { MessageBox.Show("Introduce el usuario"); txtusuario.Focus(); return; }
            if (txtpass.Text == "") { MessageBox.Show("Introduce la contraseña"); txtpass.Focus(); return; }

            if (txtpass.Text == txtConfirm.Text)
            {
                UserModel userModel = new UserModel(idUsuario: UserLoginCache.idUsuario, nombre: txtnombre.Text, passUser: txtpass.Text, rol: null, nombreUsuario: txtusuario.Text);
               string mensaje= userModel.EditaUser();
                MessageBox.Show(mensaje);
                Limpiar();
                grupboxDatos.Visible = false;
                mostrar();

            }
            else
            {
                MessageBox.Show("No coincide la contraseña, repite de nuevo");
                txtpass.Text = ""; txtConfirm.Text = ""; txtpass.Focus();
            }

        }

        private void mostrar()
        {
           
            lblnombre.Text = UserLoginCache.nombre;
            lblusuario.Text = UserLoginCache.nombreUsuario;
            lblpass.Text = UserLoginCache.passUser;
            txtnombre.Text = UserLoginCache.nombre;
            txtusuario.Text = UserLoginCache.nombreUsuario;
            txtpass.Text = UserLoginCache.passUser;
            txtConfirm.Text= UserLoginCache.passUser;

        }

        private void Limpiar()
        {
            txtnombre.Text = "";
            txtusuario.Text = "";
            txtpass.Text = "";
            txtConfirm.Text = "";

        }

        private void linklblperfil_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            grupboxDatos.Visible = true;
           
        }

        private void txtpass_TextChanged(object sender, EventArgs e)
        {
            txtConfirm.Visible = true; lblconfir.Visible = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void txtnombre_Validating(object sender, CancelEventArgs e)
        {
            if (txtnombre.Text != "")
            {
                if (modelo.ComprobarString(txtnombre.Text))
                {
                    MessageBox.Show("Introduce nombre"); e.Cancel = true;
                }
            }
        }

        private void txtusuario_Validating(object sender, CancelEventArgs e)
        {
            if (txtusuario.Text != "")
            {
                if (modelo.ComprobarString(txtusuario.Text))
                {
                    MessageBox.Show("Introduce nombre"); e.Cancel = true;
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain;

namespace Presentacion
{
    public partial class FLogin : Form
    {
        public FLogin()
        {
            InitializeComponent();
            UserModel usuarioInicio = new UserModel();
            usuarioInicio.IniciarBd();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);


        private void btnaceptar_Click(object sender, EventArgs e)
        {
            UserModel usuario = new UserModel();
            var ExisteLog = usuario.LoginUser(txtUsuario.Text, txtPass.Text);
            if (ExisteLog)
            {
                this.Hide();
                FormBienvenida bienvenida = new FormBienvenida();
                bienvenida.ShowDialog();
                FormPrincipal menu = new FormPrincipal();
                menu.Show();
                menu.FormClosed += Logout;
           
            }
            else
            {
                MessageBox.Show("Usuario o Contraseña incorretos");
                txtPass.Clear();
                txtUsuario.Clear();
                txtUsuario.Focus();
            }

        }

        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                txtUsuario.Text = "USUARIO";
                txtUsuario.ForeColor = Color.LightGray;

            }
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (txtPass.Text == "")
            {
                txtPass.Text = "CONTRASEÑA";
                txtPass.UseSystemPasswordChar = false;
                txtPass.ForeColor = Color.LightGray;
            }
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "USUARIO")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.Black;

            }
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "CONTRASEÑA")
            {
                txtPass.Text = "";
                txtPass.UseSystemPasswordChar = true;
                txtPass.ForeColor = Color.Black;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0x0f012, 0);
        }

        private void FLogin_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0x0f012, 0);
        }


        private void Logout(object sender, FormClosedEventArgs e)
        {
            txtUsuario.Clear();
            txtPass.Clear();
            this.Show();
            txtUsuario.Focus();
        }
    }
}

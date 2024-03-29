﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Cache;

namespace Presentacion
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            LoadUserData();
            if (UserLoginCache.rol == Cargos.RolNormal)
            {
                btnCli.Enabled = false;
                button1.Visible = false;
            }
        }

        private void LoadUserData()
        {
            lblnombre.Text = UserLoginCache.nombre;
        }


        #region Funcionalidades del formulario
        //RESIZE METODO PARA REDIMENSIONAR/CAMBIAR TAMAÑO A FORMULARIO EN TIEMPO DE EJECUCION ----------------------------------------------------------
        private int tolerance = 12;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case WM_NCHITTEST:
        //            base.WndProc(ref m);
        //            var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
        //            if (sizeGripRectangle.Contains(hitPoint))
        //                m.Result = new IntPtr(HTBOTTOMRIGHT);
        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }
        //}
        //----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));

            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);

            region.Exclude(sizeGripRectangle);
            this.panelContenedor.Region = region;
            this.Invalidate();
        }
        //----------------COLOR Y GRIP DE RECTANGULO INFERIOR
        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(244, 244, 244));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);

            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }



        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Capturar posicion y tamaño antes de maximizar para restaurar
        int lx, ly;
        int sw, sh;
        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;
            btnMaximizar.Visible = false;
            btnRestaurar. Visible = true;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnMaximizar.Visible = true ;
            btnRestaurar.Visible = false;
            this.Size = new Size(sw,sh);
            this.Location = new Point(lx,ly);
        }

        private void panelBarraTitulo_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //METODO PARA ARRASTRAR EL FORMULARIO---------------------------------------------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormUsuario>();
            button1.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormUsuarios2>();
            button2.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormLocales>();
            button3.BackColor = Color.FromArgb(12, 61, 92);
        }

   
        private void btnElectricidad_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormElectricidad>();
            btnElectricidad.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void btnCli_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormCliente>();
            btnCli.BackColor = Color.FromArgb(12, 61, 92);

        }

        private void btnCalculo_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormCalculos>();
            btnCalculo.BackColor = Color.FromArgb(12, 61, 92);
        }


        private void btnConsulta_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormConsulta>();
            btnConsulta.BackColor = Color.FromArgb(12, 61, 92);
        }


        private void btnfactura_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FormImporte>();
            btnfactura.BackColor= Color.FromArgb(12, 61, 92);
        }

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        //MENU DESLIZANTE
        private void btnMenu_Click(object sender, EventArgs e)
        {
            if (panelMenu.Width >= 180)
            {
                panelMenu.Width = 59;

            }
            else if (panelMenu.Width == 59)
            {
                panelMenu.Width = 180;
            }

        }










        #endregion
        //METODO PARA ABRIR FORMULARIOS DENTRO DEL PANEL
        private void AbrirFormulario<MiForm>() where MiForm : Form, new() {
            Form formulario;
            formulario = panelformularios.Controls.OfType<MiForm>().FirstOrDefault();//Busca en la colecion el formulario
            //si el formulario/instancia no existe
            if (formulario == null)
            {
                formulario = new MiForm();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                panelformularios.Controls.Add(formulario);
                panelformularios.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms );
            }
            //si el formulario/instancia existe
            else {
                formulario.BringToFront();
            }
        }
        private void CloseForms(object sender,FormClosedEventArgs e) {
            if (Application.OpenForms["FormUsuario"] == null)
                button1.BackColor = Color.DarkRed;
            if (Application.OpenForms["FormUsuarios2"] == null)
                button2.BackColor = Color.DarkRed;
            if (Application.OpenForms["FormLocales"] == null)
                button3.BackColor = Color.DarkRed;
            if (Application.OpenForms["FormCliente"] == null)
                btnCli.BackColor = Color.DarkRed;
            if (Application.OpenForms["FormElectricidad"] == null)
                btnElectricidad.BackColor = Color.DarkRed;
            if (Application.OpenForms["FormCalculos"] == null)
                //btnCalculo.BackColor = Color.FromArgb(4, 41, 68);
                btnCalculo.BackColor = Color.DarkRed;
            if (Application.OpenForms["FormConsulta"] == null)
                btnConsulta.BackColor = Color.DarkRed;
            if (Application.OpenForms["FormImporte"] == null)
                btnfactura.BackColor = Color.DarkRed;

        }
    }
}

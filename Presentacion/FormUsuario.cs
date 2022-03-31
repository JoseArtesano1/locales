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
    public partial class FormUsuario : Form
    {
        int id;
        string usuarioClick;
        UserModel modelo = new UserModel();
        

        public FormUsuario()
        {
            InitializeComponent();
            CargaCmbRoles();
            txtConfirm.Visible = false;
            lblconfir.Visible = false;
            btnModificar.Enabled = false;
           
            datagridUsuarios.DataSource = modelo.Carga();
            datagridUsuarios.Columns[0].Visible = false; datagridUsuarios.Columns[2].Visible = false;
        }

        private void CargaCmbRoles()
        {
            cmbAutoriza.Items.Add(Cargos.SuperRol);
            cmbAutoriza.Items.Add(Cargos.RolNormal);
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text == "") { MessageBox.Show("Introduce Nombre"); txtnombre.Focus(); return; }
            if (txtusuario.Text == "") { MessageBox.Show("Introduce el usuario"); txtusuario.Focus(); return; }
            if (txtpass.Text == "") { MessageBox.Show("Introduce la contraseña"); txtpass.Focus(); return; }

            if (cmbAutoriza.SelectedIndex == -1)
            {
                MessageBox.Show("selecciona autorización"); cmbAutoriza.Focus();
                return;
            }
            if (txtpass.Text == txtConfirm.Text)
            {
                var userModel = new UserModel(nombre:txtnombre.Text,passUser:txtpass.Text,rol:cmbAutoriza.SelectedItem.ToString(),nombreUsuario:txtusuario.Text);
                 string mensaje= userModel.NuevoUser();
                  MessageBox.Show(mensaje);
                Recarga();
            }
            else
            {
                MessageBox.Show("No coincide la contraseña, repite de nuevo"); cmbAutoriza.Focus();
                txtpass.Text = ""; txtConfirm.Text = ""; txtpass.Focus();
            }
        }

        private void txtnombre_Validating(object sender, CancelEventArgs e)
        {
            if (txtnombre.Text != "")
            {
                if(modelo.ComprobarString(txtnombre.Text))
                {
                    MessageBox.Show("Introduce nombre");   e.Cancel = true;
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

        private void txtpass_TextChanged(object sender, EventArgs e)
        {
            txtConfirm.Visible = true;
            lblconfir.Visible = true;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text == "") { MessageBox.Show("Selecciona un Usuario"); datagridUsuarios.Focus(); return; }
            if (txtusuario.Text == "") { MessageBox.Show("Introduce el usuario"); txtusuario.Focus(); return; }
            if (txtpass.Text == "") { MessageBox.Show("Introduce la contraseña"); txtpass.Focus(); return; }
            //if (txtpass.Text == txtConfirm.Text)
            //{                                              
            var user = new UserModel(idUsuario:id ,nombre: txtnombre.Text, passUser: txtpass.Text, 
                rol: cmbAutoriza.SelectedItem.ToString(), nombreUsuario: txtusuario.Text);
                
               string mensaje=  user.EditaUser();
                MessageBox.Show(mensaje);
                Recarga();
                ConfPasstext(true, true, true);
            
            //}
            //else
            //{
            //    MessageBox.Show("No coincide la contraseña, repite de nuevo");
            //    txtpass.Text = ""; txtConfirm.Text = ""; txtpass.Focus();
            //}
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text == "") { MessageBox.Show("Selecciona un Usuario"); datagridUsuarios.Focus(); return; }
            if (MessageBox.Show("Este proceso borra el usuario " + datagridUsuarios.CurrentRow.Cells[1].Value.ToString().ToUpper() + " de la bd, lo quieres hacer S/N", "CUIDADO", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                 string mensaje = modelo.EliminarUsuario(id);
                MessageBox.Show(mensaje);
                Recarga();
                ConfPasstext(true, true, true);
            }
            else {
                Recarga();
                ConfPasstext(true, true, true);
            }
               
            
        }
      

        private void datagridUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (datagridUsuarios.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    id = int.Parse(datagridUsuarios.CurrentRow.Cells[0].Value.ToString());
                    usuarioClick = datagridUsuarios.CurrentRow.Cells[4].Value.ToString();
                    txtusuario.Text = usuarioClick;
                    txtnombre.Text = datagridUsuarios.CurrentRow.Cells[1].Value.ToString();
                    
                    cmbAutoriza.SelectedItem = datagridUsuarios.CurrentRow.Cells[3].Value.ToString();
                    txtConfirm.Visible = false; lblconfir.Visible = false;
                    ConfPasstext(false, false, false);    
                   
                    btnModificar.Enabled = true;
                }
                else
                { MessageBox.Show("Selecciona un Usuario");

                }
            }
           
        }

        private void Recarga()
        {
            datagridUsuarios.DataSource = modelo.Carga();
            txtnombre.Text = ""; txtusuario.Text = ""; txtpass.Text = ""; txtConfirm.Text = "";
           // this.Controls.OfType<TextBox>().ToList().ForEach(o => o.Text = string.Empty);
            cmbAutoriza.SelectedIndex = -1;
        }

        private void ConfPasstext(bool text, bool lbl, bool btn)
        {
            txtpass.Visible = text; label4.Visible = lbl; btnAlta.Enabled = btn;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Recarga(); ConfPasstext(true, true, true);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

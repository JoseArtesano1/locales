﻿using Domain;
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
    public partial class FormLocales : Form
    {
        LocalModel localModel = new LocalModel();
        LugarModel lugar = new LugarModel();
        AlquilerModel alquiler = new AlquilerModel();
        PotenciaEnergiaModel energiaModel = new PotenciaEnergiaModel();
        int idLocal , numero, idPotEn, year, idLugar; string dato;
      
        public FormLocales()
        {
            InitializeComponent();
            CargaGridBtn();
            CargaCmbox(); cargarCmbLugar();  cargarGrupbox();
            CrearPag(1);
        }

        private void CargaGridBtn()
        {
            datagridLocales.DataSource = localModel.CargaLocales();
           
                datagridLocales.Columns["idLocal"].DisplayIndex = 0; datagridLocales.Columns["numero"].DisplayIndex = 1;
                datagridLocales.Columns["acumulado"].DisplayIndex = 2; datagridLocales.Columns["idLug"].DisplayIndex = 3;
                datagridLocales.Columns["idLugar"].DisplayIndex = 4;datagridLocales.Columns["nombreLugar"].DisplayIndex = 5;
                datagridLocales.Columns["Edit"].DisplayIndex = 6; datagridLocales.Columns["Borrar"].DisplayIndex = 7;
                datagridLocales.Columns["idLocal"].Visible = false;
                datagridLocales.Columns["idLug"].Visible = false;
                datagridLocales.Columns["idLugar"].Visible = false;
                datagridLocales.Columns["Edit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                datagridLocales.Columns["Borrar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                datagridLocales.Columns["numero"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                datagridLocales.Columns["acumulado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }


        private void CrearPag(int pag)
        {
            if (pag == 1)
            {
                tabControl1.TabPages.Remove(tabPage2);
            }
            else
            {
                tabControl1.TabPages.Add(tabPage2);
                dataGridpotencia.DataSource = energiaModel.CargaPotenciaEnergia(idLugar);
                dataGridpotencia.Columns[0].Visible = false; 
            }
        }

        private void txtlugar_Validating(object sender, CancelEventArgs e)
        {
            if (txtlugar.Text != "")
            {
                if (localModel.ComprobarString(txtlugar.Text))
                {
                    MessageBox.Show("Introduce una Dirección"); e.Cancel = true;
                }
            }
        }

        private void txtnumero_Validating_1(object sender, CancelEventArgs e)
        {
            if (txtnumero.Text != "")
            {
                if (!localModel.ComprobarString(txtnumero.Text))
                {
                    MessageBox.Show("Introduce un número"); e.Cancel = true;
                }
            }
        }


        private void txtacumulado_Validating(object sender, CancelEventArgs e)
        {
            if (txtacumulado.Text != "")
            {
                if (!localModel.ComprobarDecimal(txtacumulado.Text))
                {
                    MessageBox.Show("Introduce una cantidad"); e.Cancel = true;
                }
            }
        }

        private void txtImpEnerg_Validating(object sender, CancelEventArgs e)
        {
            if (txtImpEnerg.Text != "")
            {
                if (!localModel.ComprobarDecimal(txtImpEnerg.Text))
                {
                    MessageBox.Show("Introduce importe de Energía"); e.Cancel = true;
                }
            }
        }

        private void textpot_Validating(object sender, CancelEventArgs e)
        {
            if (textpot.Text != "")
            {
                if (!localModel.ComprobarDecimal(textpot.Text)) { MessageBox.Show("Introduce la potencia"); e.Cancel = true; }
            }
        }

        private void txtenerg_Validating(object sender, CancelEventArgs e)
        {
            if (txtenerg.Text != "")
            {
                if (!localModel.ComprobarDecimal(txtenerg.Text)) { MessageBox.Show("Introduce la energía"); e.Cancel = true; }
            }
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        #region LUGAR

        private void cargarCmbLugar()
        {
            cmbLugar.DataSource = alquiler.CargarComboLugar();
            cmbLugar.DisplayMember = "nombreLugar";
            cmbLugar.ValueMember = "idLugar";
            cmbLugar.SelectedIndex = -1;

        }

        private void cargarGrupbox()
        {
            if (!lugar.ExisteLugar())
            {
                grupboxLocal.Visible = false;
            }
        }

        private  void  btnlugar_Click(object sender, EventArgs e)
        {
            if (txtlugar.Text == "") { MessageBox.Show("Introducce un número"); txtlugar.Focus(); return; }

            var lugar = new LugarModel(nombreLugar: txtlugar.Text);
          string mensaje =  lugar.NuevoLugarlocal();
            MessageBox.Show( mensaje);
           
            cargarCmbLugar(); txtlugar.Clear();
            cargarGrupbox();
        }

        private void btnAltaLocal_Click(object sender, EventArgs e)
        {           
            if (txtnumero.Text == "") { MessageBox.Show("Introducce un número"); txtnumero.Focus(); return; }
            if (txtacumulado.Text == "") { MessageBox.Show("Introducce Energía acumulada"); txtacumulado.Focus(); return; }
            if (cmbLugar.SelectedIndex == -1) { MessageBox.Show("Debe seleccionar un lugar"); cmbLugar.Focus(); return; }

            var local = new LocalModel(numero: int.Parse(txtnumero.Text), acumulado: decimal.Parse(txtacumulado.Text), idLug: int.Parse(cmbLugar.SelectedValue.ToString()));

            string mensaje = local.NuevoLocal();

            MessageBox.Show(mensaje);
            Recarga();
        }

        
        private void datagridLocales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                CrearPag(1); btnAltaLocal.Enabled = false;
                btnlugar.Enabled = false;

                if (datagridLocales.CurrentRow.Cells[2].Value.ToString() != "")
                {
                    dato = datagridLocales.CurrentRow.Cells[7].Value.ToString();
                    lblLugarNum.Text = "Trastero: " + dato;
                    idLugar = int.Parse(datagridLocales.CurrentRow.Cells[5].Value.ToString()); 
                    if (datagridLocales.Columns[e.ColumnIndex].Name == "Edit" || datagridLocales.Columns[e.ColumnIndex].Name == "Borrar")
                    {
                        if (datagridLocales.Columns[e.ColumnIndex].Name == "Edit")
                        {
                            idLocal = int.Parse(datagridLocales.CurrentRow.Cells[2].Value.ToString()); 
                            idLugar = int.Parse(datagridLocales.CurrentRow.Cells[5].Value.ToString()); 
                            numero = int.Parse(datagridLocales.CurrentRow.Cells[3].Value.ToString());
                            decimal acumldo = decimal.Parse(datagridLocales.CurrentRow.Cells[4].Value.ToString());
                            FormModLc formedit = new FormModLc(idLugar, idLocal, dato, numero, acumldo);
                            formedit.ShowDialog();
                            CargaGridBtn();
                           btnAltaLocal.Enabled = true;
                           btnlugar.Enabled = true;
                          
                        }

                        if (datagridLocales.Columns[e.ColumnIndex].Name == "Borrar")
                        {
                            if (MessageBox.Show("Este proceso borra el lugar " + dato.ToUpper() +
                               " de la bd, lo quieres hacer S/N", "CUIDADO", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                string mensaje = lugar.EliminarLugar(idLugar);
                                MessageBox.Show(mensaje);
                                Recarga(); cargarCmbLugar();
                                btnlugar.Enabled = true; CrearPag(1);
                            }

                            if (MessageBox.Show("Este proceso borra el local " + numero + " de " + dato.ToUpper() +
                                " de la bd, lo quieres hacer S/N", "CUIDADO", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                string mensaje = localModel.EliminarLocal(idLocal);
                                MessageBox.Show(mensaje);
                                Recarga();
                                btnAltaLocal.Enabled = true; CrearPag(1);
                            }

                            btnAltaLocal.Enabled = true; btnlugar.Enabled = true;

                        }
                    }
                    else
                    {
                        btnAltaLocal.Enabled = true; btnlugar.Enabled = true;  btnAltaPot.Enabled = true;
                        Recarga2(idLugar);
                        CrearPag(2);
                    }
       
                }
                else
                {
                    CrearPag(1); Recarga(); btnlugar.Enabled = true; btnAltaLocal.Enabled = true;
                    MessageBox.Show("Selecciona un local");
                }
               
            }
                
        }

       

        private void btnCancelarAlta_Click(object sender, EventArgs e)
        {
            Limpiar(); CrearPag(1);
            btnAltaLocal.Enabled = true;
            btnlugar.Enabled = true;
        }

        private void Recarga()
        {
            CargaGridBtn();
            Limpiar();
        }


        private void Limpiar()
        {
            cmbLugar.SelectedIndex = -1; txtnumero.Clear(); txtacumulado.Clear(); txtlugar.Clear();
        }

        #endregion


        #region POTENCIA Y ENERGIA

        private void btnAltaPot_Click(object sender, EventArgs e)
        {
            if (textpot.Text == "") { MessageBox.Show("Introduce Potencia"); textpot.Focus(); return; }
            if (txtenerg.Text == "") { MessageBox.Show("Introduce Energía"); txtenerg.Focus(); return; }
            if (cmbanno.SelectedIndex == -1) { MessageBox.Show("Selecciona un año"); cmbanno.Focus(); return; }

            var potEnerg = new PotenciaEnergiaModel(anno: int.Parse(cmbanno.SelectedItem.ToString()), Energia:decimal.Parse(txtenerg.Text) , Potencia: decimal.Parse( textpot.Text), idLu: idLugar, importeEnergia:decimal.Parse(txtImpEnerg.Text));
            string mensaje= potEnerg.NuevoPotEng();
            MessageBox.Show(mensaje);
            Recarga2(idLugar); 
        }


        private void btnCancelarPot_Click(object sender, EventArgs e)
        {
            textpot.Clear(); txtenerg.Clear(); cmbanno.SelectedIndex = -1; txtImpEnerg.Clear();
            btnAltaPot.Enabled = true;
        }


        private void btneliminarPot_Click(object sender, EventArgs e)
        {
            if (textpot.Text == "") { MessageBox.Show("Seleccione Potencia y Energía"); textpot.Focus(); return; }
            if (MessageBox.Show("Este proceso borra el la potencia y la energía del año " + year + " de la bd, lo quieres hacer S/N", "CUIDADO", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mensaje = energiaModel.EliminarPotEnergia(year,idLugar);
                MessageBox.Show(mensaje);
                Recarga2(idLugar);
                btnAltaPot.Enabled = true; 
            }

        }

       

        private void btnmodifPot_Click(object sender, EventArgs e)
        {
            if (textpot.Text == "") { MessageBox.Show("Introduce Potencia"); textpot.Focus(); return; }
            if (txtenerg.Text == "") { MessageBox.Show("Introduce Energía"); txtenerg.Focus(); return; }
            if (cmbanno.SelectedIndex == -1) { MessageBox.Show("Selecciona un año"); cmbanno.Focus(); return; }

            var potEnerg = new PotenciaEnergiaModel(idPotEnergy: idPotEn, anno: int.Parse(cmbanno.SelectedItem.ToString()), Energia: decimal.Parse(txtenerg.Text), Potencia: decimal.Parse(textpot.Text), idLu:idLugar, importeEnergia: decimal.Parse(txtImpEnerg.Text));

           string mensaje=   potEnerg.EditarPotEnergia();
            MessageBox.Show(mensaje);
            Recarga2(idLugar);
            btnAltaPot.Enabled = true; 
        }


        private void dataGridpotencia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (dataGridpotencia.CurrentRow.Cells[0].Value.ToString() != "")
                {
                    idPotEn = int.Parse(dataGridpotencia.CurrentRow.Cells[0].Value.ToString());
                    year= int.Parse(dataGridpotencia.CurrentRow.Cells[1].Value.ToString());
                    cmbanno.SelectedItem = year;
                    txtenerg.Text = dataGridpotencia.CurrentRow.Cells[2].Value.ToString();
                    textpot.Text= dataGridpotencia.CurrentRow.Cells[3].Value.ToString();
                    txtImpEnerg.Text= dataGridpotencia.CurrentRow.Cells[4].Value.ToString();
                    btnAltaPot.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Selecciona valores de Potencia y Energía");
                }
            }

        }

        private void Recarga2(int id)
        {
            dataGridpotencia.AutoResizeColumns();
            dataGridpotencia.DataSource = energiaModel.CargaPotenciaEnergia(id);
            textpot.Clear(); txtenerg.Clear(); cmbanno.SelectedIndex = -1; txtImpEnerg.Clear();
            
        }

        public void CargaCmbox()
        {
            foreach (var item in energiaModel.CargaYears())
            {
                cmbanno.Items.Add(item);
            }
        }

        #endregion
    }
}

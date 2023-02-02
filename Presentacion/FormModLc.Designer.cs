
namespace Presentacion
{
    partial class FormModLc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModLc));
            this.btncerrar = new System.Windows.Forms.PictureBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lblID = new System.Windows.Forms.Label();
            this.txtlugar = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbLugar = new System.Windows.Forms.ComboBox();
            this.txtacumulado = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtnumero = new System.Windows.Forms.TextBox();
            this.lblIdLoc = new System.Windows.Forms.Label();
            this.btnEdit2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.btncerrar)).BeginInit();
            this.SuspendLayout();
            // 
            // btncerrar
            // 
            this.btncerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btncerrar.BackColor = System.Drawing.Color.LightCoral;
            this.btncerrar.Image = ((System.Drawing.Image)(resources.GetObject("btncerrar.Image")));
            this.btncerrar.Location = new System.Drawing.Point(669, 1);
            this.btncerrar.Name = "btncerrar";
            this.btncerrar.Size = new System.Drawing.Size(28, 28);
            this.btncerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btncerrar.TabIndex = 15;
            this.btncerrar.TabStop = false;
            this.btncerrar.Click += new System.EventHandler(this.btncerrar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(439, 162);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(116, 41);
            this.btnCancelar.TabIndex = 17;
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(133, 162);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(110, 41);
            this.btnEdit.TabIndex = 16;
            this.btnEdit.Text = "GUARDAR";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(23, 45);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(51, 20);
            this.lblID.TabIndex = 18;
            this.lblID.Text = "label3";
            // 
            // txtlugar
            // 
            this.txtlugar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtlugar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtlugar.Location = new System.Drawing.Point(84, 68);
            this.txtlugar.Name = "txtlugar";
            this.txtlugar.Size = new System.Drawing.Size(254, 30);
            this.txtlugar.TabIndex = 36;
            this.txtlugar.Validating += new System.ComponentModel.CancelEventHandler(this.txtlugar_Validating);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(90, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 25);
            this.label2.TabIndex = 37;
            this.label2.Text = "LUGAR";
            // 
            // cmbLugar
            // 
            this.cmbLugar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLugar.FormattingEnabled = true;
            this.cmbLugar.Location = new System.Drawing.Point(84, 70);
            this.cmbLugar.Name = "cmbLugar";
            this.cmbLugar.Size = new System.Drawing.Size(245, 28);
            this.cmbLugar.TabIndex = 61;
            // 
            // txtacumulado
            // 
            this.txtacumulado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtacumulado.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtacumulado.Location = new System.Drawing.Point(483, 68);
            this.txtacumulado.Name = "txtacumulado";
            this.txtacumulado.Size = new System.Drawing.Size(141, 30);
            this.txtacumulado.TabIndex = 63;
            this.txtacumulado.Validating += new System.ComponentModel.CancelEventHandler(this.txtacumulado_Validating);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(483, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 25);
            this.label4.TabIndex = 65;
            this.label4.Text = "ACUMULADO";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(363, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 25);
            this.label3.TabIndex = 64;
            this.label3.Text = "NUMERO";
            // 
            // txtnumero
            // 
            this.txtnumero.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtnumero.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnumero.Location = new System.Drawing.Point(368, 68);
            this.txtnumero.Name = "txtnumero";
            this.txtnumero.Size = new System.Drawing.Size(49, 30);
            this.txtnumero.TabIndex = 62;
            this.txtnumero.Validating += new System.ComponentModel.CancelEventHandler(this.txtnumero_Validating);
            // 
            // lblIdLoc
            // 
            this.lblIdLoc.AutoSize = true;
            this.lblIdLoc.Location = new System.Drawing.Point(23, 78);
            this.lblIdLoc.Name = "lblIdLoc";
            this.lblIdLoc.Size = new System.Drawing.Size(51, 20);
            this.lblIdLoc.TabIndex = 66;
            this.lblIdLoc.Text = "label1";
            // 
            // btnEdit2
            // 
            this.btnEdit2.Location = new System.Drawing.Point(250, 162);
            this.btnEdit2.Name = "btnEdit2";
            this.btnEdit2.Size = new System.Drawing.Size(110, 41);
            this.btnEdit2.TabIndex = 67;
            this.btnEdit2.Text = "GUARDAR";
            this.btnEdit2.UseVisualStyleBackColor = true;
            this.btnEdit2.Click += new System.EventHandler(this.btnEdit2_Click);
            // 
            // FormModLc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkRed;
            this.ClientSize = new System.Drawing.Size(698, 228);
            this.Controls.Add(this.btnEdit2);
            this.Controls.Add(this.lblIdLoc);
            this.Controls.Add(this.txtacumulado);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtnumero);
            this.Controls.Add(this.cmbLugar);
            this.Controls.Add(this.txtlugar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btncerrar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormModLc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormModLc";
            ((System.ComponentModel.ISupportInitialize)(this.btncerrar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox btncerrar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox txtlugar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbLugar;
        private System.Windows.Forms.TextBox txtacumulado;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtnumero;
        private System.Windows.Forms.Label lblIdLoc;
        private System.Windows.Forms.Button btnEdit2;
    }
}
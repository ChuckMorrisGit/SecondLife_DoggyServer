namespace DoggyClient
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl_Com = new System.Windows.Forms.TabControl();
            this.listView_Clients = new System.Windows.Forms.ListView();
            this.textBox_Message = new System.Windows.Forms.TextBox();
            this.button_addText = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tabControl_Com
            // 
            this.tabControl_Com.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_Com.Location = new System.Drawing.Point(191, 3);
            this.tabControl_Com.Name = "tabControl_Com";
            this.tabControl_Com.SelectedIndex = 0;
            this.tabControl_Com.Size = new System.Drawing.Size(682, 326);
            this.tabControl_Com.TabIndex = 1;
            // 
            // listView_Clients
            // 
            this.listView_Clients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView_Clients.FullRowSelect = true;
            this.listView_Clients.Location = new System.Drawing.Point(3, 3);
            this.listView_Clients.MultiSelect = false;
            this.listView_Clients.Name = "listView_Clients";
            this.listView_Clients.Size = new System.Drawing.Size(182, 326);
            this.listView_Clients.TabIndex = 2;
            this.listView_Clients.UseCompatibleStateImageBehavior = false;
            this.listView_Clients.SelectedIndexChanged += new System.EventHandler(this.listView_Clients_SelectedIndexChanged);
            // 
            // textBox_Message
            // 
            this.textBox_Message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Message.Location = new System.Drawing.Point(191, 335);
            this.textBox_Message.Name = "textBox_Message";
            this.textBox_Message.Size = new System.Drawing.Size(601, 20);
            this.textBox_Message.TabIndex = 3;
            // 
            // button_addText
            // 
            this.button_addText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_addText.Location = new System.Drawing.Point(798, 333);
            this.button_addText.Name = "button_addText";
            this.button_addText.Size = new System.Drawing.Size(75, 23);
            this.button_addText.TabIndex = 4;
            this.button_addText.Text = "LOS";
            this.button_addText.UseVisualStyleBackColor = true;
            this.button_addText.Click += new System.EventHandler(this.button_addText_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 360);
            this.Controls.Add(this.button_addText);
            this.Controls.Add(this.textBox_Message);
            this.Controls.Add(this.listView_Clients);
            this.Controls.Add(this.tabControl_Com);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_Com;
        private System.Windows.Forms.ListView listView_Clients;
        private System.Windows.Forms.TextBox textBox_Message;
        private System.Windows.Forms.Button button_addText;
    }
}


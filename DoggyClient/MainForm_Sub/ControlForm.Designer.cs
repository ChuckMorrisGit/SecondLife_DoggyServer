namespace DoggyClient.MainForm_Sub
{
    partial class ControlForm
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
            this.checkBox_alice_chat = new System.Windows.Forms.CheckBox();
            this.checkBox_alice_im = new System.Windows.Forms.CheckBox();
            this.checkBox_alice_g_im = new System.Windows.Forms.CheckBox();
            this.checkBox_follow = new System.Windows.Forms.CheckBox();
            this.checkBox_chatfollow = new System.Windows.Forms.CheckBox();
            this.button_relog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBox_alice_chat
            // 
            this.checkBox_alice_chat.AutoSize = true;
            this.checkBox_alice_chat.Location = new System.Drawing.Point(12, 12);
            this.checkBox_alice_chat.Name = "checkBox_alice_chat";
            this.checkBox_alice_chat.Size = new System.Drawing.Size(74, 17);
            this.checkBox_alice_chat.TabIndex = 2;
            this.checkBox_alice_chat.Text = "Alice Chat";
            this.checkBox_alice_chat.UseVisualStyleBackColor = true;
            // 
            // checkBox_alice_im
            // 
            this.checkBox_alice_im.AutoSize = true;
            this.checkBox_alice_im.Location = new System.Drawing.Point(85, 12);
            this.checkBox_alice_im.Name = "checkBox_alice_im";
            this.checkBox_alice_im.Size = new System.Drawing.Size(64, 17);
            this.checkBox_alice_im.TabIndex = 3;
            this.checkBox_alice_im.Text = "Alice IM";
            this.checkBox_alice_im.UseVisualStyleBackColor = true;
            // 
            // checkBox_alice_g_im
            // 
            this.checkBox_alice_g_im.AutoSize = true;
            this.checkBox_alice_g_im.Location = new System.Drawing.Point(161, 12);
            this.checkBox_alice_g_im.Name = "checkBox_alice_g_im";
            this.checkBox_alice_g_im.Size = new System.Drawing.Size(75, 17);
            this.checkBox_alice_g_im.TabIndex = 4;
            this.checkBox_alice_g_im.Text = "Alice G IM";
            this.checkBox_alice_g_im.UseVisualStyleBackColor = true;
            // 
            // checkBox_follow
            // 
            this.checkBox_follow.AutoSize = true;
            this.checkBox_follow.Location = new System.Drawing.Point(12, 35);
            this.checkBox_follow.Name = "checkBox_follow";
            this.checkBox_follow.Size = new System.Drawing.Size(56, 17);
            this.checkBox_follow.TabIndex = 5;
            this.checkBox_follow.Text = "Follow";
            this.checkBox_follow.UseVisualStyleBackColor = true;
            // 
            // checkBox_chatfollow
            // 
            this.checkBox_chatfollow.AutoSize = true;
            this.checkBox_chatfollow.Location = new System.Drawing.Point(12, 58);
            this.checkBox_chatfollow.Name = "checkBox_chatfollow";
            this.checkBox_chatfollow.Size = new System.Drawing.Size(81, 17);
            this.checkBox_chatfollow.TabIndex = 6;
            this.checkBox_chatfollow.Text = "Chat Follow";
            this.checkBox_chatfollow.UseVisualStyleBackColor = true;
            // 
            // button_relog
            // 
            this.button_relog.Location = new System.Drawing.Point(12, 92);
            this.button_relog.Name = "button_relog";
            this.button_relog.Size = new System.Drawing.Size(75, 23);
            this.button_relog.TabIndex = 7;
            this.button_relog.Text = "relog";
            this.button_relog.UseVisualStyleBackColor = true;
            this.button_relog.Click += new System.EventHandler(this.button_relog_Click);
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button_relog);
            this.Controls.Add(this.checkBox_chatfollow);
            this.Controls.Add(this.checkBox_follow);
            this.Controls.Add(this.checkBox_alice_g_im);
            this.Controls.Add(this.checkBox_alice_im);
            this.Controls.Add(this.checkBox_alice_chat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ControlForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ControlForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_alice_chat;
        private System.Windows.Forms.CheckBox checkBox_alice_im;
        private System.Windows.Forms.CheckBox checkBox_alice_g_im;
        private System.Windows.Forms.CheckBox checkBox_follow;
        private System.Windows.Forms.CheckBox checkBox_chatfollow;
        private System.Windows.Forms.Button button_relog;
    }
}
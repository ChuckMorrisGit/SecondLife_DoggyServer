using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoggyClient.MainForm_Sub
{
    public partial class ControlForm : Form
    {
        public static DoggyMCP.SL_Client_Sub.SL_Client sl_Client = new DoggyMCP.SL_Client_Sub.SL_Client();

        public ControlForm(DoggyMCP.SL_Client_Sub.SL_Client sl_Client_new)
        {
            sl_Client = sl_Client_new;
            InitializeComponent();
        }

        private void ControlForm_Load(object sender, EventArgs e)
        {
            if (!sl_Client.online) this.Enabled = false;
            checkBox_alice_chat.Checked = sl_Client.alice_chat;
            checkBox_alice_chat.CheckedChanged += new EventHandler(checkBox_alice_chat_CheckedChanged);
            checkBox_alice_im.Checked = sl_Client.alice_im;
            checkBox_alice_im.CheckedChanged += new EventHandler(checkBox_alice_im_CheckedChanged);
            checkBox_alice_g_im.Checked = sl_Client.alice_g_im;
            checkBox_alice_g_im.CheckedChanged += new EventHandler(checkBox_alice_g_im_CheckedChanged);

            checkBox_follow.Checked = sl_Client.follow;
            checkBox_follow.Text = "Following: " + sl_Client.current_master;
            checkBox_follow.CheckedChanged += new EventHandler(checkBox_follow_CheckedChanged);

            checkBox_chatfollow.Checked = sl_Client.chat_follow;
            checkBox_chatfollow.CheckedChanged += new EventHandler(checkBox_chatfollow_CheckedChanged);
        }

        void checkBox_chatfollow_CheckedChanged(object sender, EventArgs e)
        {
            sl_Client.chat_follow = checkBox_chatfollow.Checked;
            update_SL_client();
        }

        void checkBox_follow_CheckedChanged(object sender, EventArgs e)
        {
            sl_Client.follow = checkBox_follow.Checked;
            if ((sl_Client.current_master == "") && (sl_Client.follow))
            {
                sl_Client.current_master = "Walter Ginsburg";
                checkBox_follow.Text = "Following: " + sl_Client.current_master;
            }

            if (!sl_Client.follow) sl_Client.current_master = "";
            update_SL_client();
        }

        void checkBox_alice_g_im_CheckedChanged(object sender, EventArgs e)
        {
            sl_Client.alice_g_im = checkBox_alice_g_im.Checked;
            update_SL_client();
        }

        void checkBox_alice_im_CheckedChanged(object sender, EventArgs e)
        {
            sl_Client.alice_im = checkBox_alice_im.Checked;
            update_SL_client();
        }

        void checkBox_alice_chat_CheckedChanged(object sender, EventArgs e)
        {
            sl_Client.alice_chat = checkBox_alice_chat.Checked;
            update_SL_client();
        }

        void update_SL_client()
        {
            MCP_Sub.init.service.set_SL_Client(sl_Client, true);
        }

        private void button_relog_Click(object sender, EventArgs e)
        {
            
            //MCP_Sub.init.service.set_command(DoggyMCP.MCP_Data.SetCommand.restart);
        }

    }
}

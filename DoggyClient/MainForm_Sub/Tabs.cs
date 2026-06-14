using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using OpenMetaverse;

namespace DoggyClient.MainForm_Sub
{
    class Tabs
    {
        public static string tabpage_chat_name = "chat";
        public static string tabpage_control_name = "control";

        public static TabControl tabControl_mainform;

        private static TabPage generate_tab(string name, string text, List<DoggyMCP.Comunication_Sub.Com_data> message)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.AppendText(text + " " + name + "\n");
            int counter = 0;

            while(counter < message.Count)
            {
                rtb.AppendText(message[counter].line + "\n");
                counter++;
            }
            rtb.Dock = DockStyle.Fill;
            rtb.BorderStyle = BorderStyle.None;
            rtb.ReadOnly = true;
            rtb.ScrollToCaret();

            TabPage tabPage1 = new TabPage();
            tabPage1.Name = name;
            tabPage1.Text = text;
            //tabPage1.BackColor = Color.Green;
            //tabPage1.ForeColor = Color.White;
            tabPage1.Font = new Font("Verdana", 8);
            tabPage1.Width = 150;
            tabPage1.Height = 100;
            tabPage1.Controls.Add(rtb);

            return (tabPage1);
        }

        public static void add_chat(UUID ava_key, TabControl tabControl_Com)
        {
            TabPage tabPage = generate_tab(tabpage_chat_name, "Chat", Comunikation_Sub.Messages.chats[ava_key]);
            tabControl_Com.TabPages.Add(tabPage);
            tabControl_Com.Name = ava_key.ToString();
            tabControl_mainform = tabControl_Com;
        }

        public static void add_IM(UUID ava_key, TabControl tabControl_Com)
        {
            Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>> message_per_sission_id = new Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>>();

            foreach (DoggyMCP.Comunication_Sub.Com_data com_data in Comunikation_Sub.Messages.IMs[ava_key])
            {
                if (!message_per_sission_id.ContainsKey(com_data.session_id)) message_per_sission_id.Add(com_data.session_id, new List<DoggyMCP.Comunication_Sub.Com_data>());
                message_per_sission_id[com_data.session_id].Add(com_data);
            }

            foreach (UUID id in message_per_sission_id.Keys)
            {
                string title = "";
                if (id == UUID.Zero) title = "Second Life";
                else
                {
                    title = id.ToString().Substring(0, 6);

                }
                TabPage tabPage = generate_tab(id.ToString(), title, message_per_sission_id[id]);
                tabControl_Com.TabPages.Add(tabPage);
            }
            tabControl_Com.Name = ava_key.ToString();
            tabControl_mainform = tabControl_Com;
        }

        public static void add_G_IM(UUID ava_key, TabControl tabControl_Com)
        {
            Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>> message_per_sission_id = new Dictionary<UUID, List<DoggyMCP.Comunication_Sub.Com_data>>();

            foreach (DoggyMCP.Comunication_Sub.Com_data com_data in Comunikation_Sub.Messages.G_IMs[ava_key])
            {
                if (!message_per_sission_id.ContainsKey(com_data.session_id)) message_per_sission_id.Add(com_data.session_id, new List<DoggyMCP.Comunication_Sub.Com_data>());
                message_per_sission_id[com_data.session_id].Add(com_data);
            }

            foreach (UUID id in message_per_sission_id.Keys)
            {
                string text = "G:";
                if (Comunikation_Sub.Messages.groups.ContainsKey(id)) text += Comunikation_Sub.Messages.groups[id].group_name;
                else text += id.ToString().Substring(0, 6);

                TabPage tabPage = generate_tab(id.ToString(), text, message_per_sission_id[id]);
                tabControl_Com.TabPages.Add(tabPage);
            }
            tabControl_Com.Name = ava_key.ToString();
            tabControl_mainform = tabControl_Com;
        }

        public static void add_Controler(UUID ava_key, TabControl tabControl_Com)
        {
            DoggyMCP.SL_Client_Sub.SL_Client sl_Client = MCP_Sub.init.service.get_SL_Client(ava_key);

            ControlForm controlForm = new ControlForm(sl_Client);
            controlForm.Dock = DockStyle.Fill;
            controlForm.TopLevel = false;
            controlForm.Visible = true;
            controlForm.FormBorderStyle = FormBorderStyle.None;
            //controlForm.Show();

            TabPage tabPage1 = new TabPage();
            tabPage1.Name = tabpage_control_name;
            tabPage1.Text = "Control";
            //tabPage1.BackColor = Color.Green;
            //tabPage1.ForeColor = Color.White;
            tabPage1.Font = new Font("Verdana", 8);
            tabPage1.Width = 100;
            tabPage1.Height = 100;
            tabPage1.Controls.Add(controlForm);
            tabControl_Com.TabPages.Add(tabPage1);

            tabControl_Com.Name = ava_key.ToString();
            tabControl_mainform = tabControl_Com;

        }

        public static void add_message_line(DoggyMCP.SL_Client_Sub.SL_Message message)
        {
            tabControl_mainform.Invoke((MethodInvoker)delegate()
            {
                if (tabControl_mainform.TabPages.Count > 0)
                {
                    tabControl_mainform.BackColor = Color.Black;
                    if (tabControl_mainform.Name == message.agent_id.ToString())
                    {
                        switch (message.message_art)
                        {
                            case DoggyMCP.SL_Client_Sub.SL_Message.Message_art.chat:
                                foreach (TabPage tabpage in tabControl_mainform.TabPages)
                                {
                                    if (tabpage.Name == tabpage_chat_name)
                                    {
                                        RichTextBox rtb = tabpage.Controls[0] as RichTextBox;
                                        rtb.AppendText(message.message + "\n");
                                        rtb.ScrollToCaret();
                                    }
                                }
                                break;

                            default:
                                foreach (TabPage tabpage in tabControl_mainform.TabPages)
                                {
                                    if (tabpage.Name == message.session_id.ToString())
                                    {
                                        RichTextBox rtb = tabpage.Controls[0] as RichTextBox;
                                        rtb.AppendText(message.message + "\n");
                                        rtb.ScrollToCaret();
                                    }
                                }
                                break;
                        }
                    }
                }
            });
        }
    }
}

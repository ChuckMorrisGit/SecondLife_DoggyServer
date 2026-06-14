using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using OpenMetaverse;

namespace DoggyClient.MainForm_Sub
{
    class Clients
    {
        private static ListView listView_Clients;

        public static void init(ListView listview)
        {
            listView_Clients = listview;
        }

        public static void Fill_Clients()
        {
            listView_Clients.Invoke((MethodInvoker)delegate()
            {
                listView_Clients.Items.Clear();

                foreach (UUID key in MCP_Sub.init.names.Keys)
                {
                    string[] item = new string[] { MCP_Sub.init.names[key].name, MCP_Sub.init.names[key].online.ToString() };
                    ListViewItem listItem = new ListViewItem(item, -1);
                    listItem.Name = key.ToString();
                    if (MCP_Sub.init.names[key].online) listItem.BackColor = Color.LightGreen;
                    else listItem.BackColor = Color.LightPink;

                    listView_Clients.Items.Add(listItem);
                }
            });
        }
    }
}

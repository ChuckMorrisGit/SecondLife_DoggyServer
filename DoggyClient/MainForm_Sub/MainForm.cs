using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;

namespace DoggyClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MCP_Sub.init.all();

            listView_Clients.View = View.Details;
            listView_Clients.Columns.Add(new ColHeader("Name", 120, HorizontalAlignment.Left, true));
            listView_Clients.Columns.Add(new ColHeader("Status", 50, HorizontalAlignment.Left, true));
            MCP_Sub.init.get_clients_all();
            
            this.listView_Clients.ColumnClick += new ColumnClickEventHandler(listView_Clients_ColumnClick);

            textBox_Message.KeyDown += new KeyEventHandler(textBox_Message_KeyDown);

            textBox_Message.Focus();

            tabControl_Com.SelectedIndexChanged += new EventHandler(tabControl_Com_SelectedIndexChanged);

            MainForm_Sub.Tabs.tabControl_mainform = tabControl_Com;

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            MainForm_Sub.Clients.init(listView_Clients);
            MainForm_Sub.Clients.Fill_Clients();
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MCP_Sub.CallBack.close();
        }

        void tabControl_Com_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl_check();
        }
         
        void tabControl_check()
        {
            if (tabControl_Com.SelectedTab != null)
            {
                if (tabControl_Com.SelectedTab.Name == "control") textBox_Message.Enabled = false;
                else textBox_Message.Enabled = true;
            }
        }


        #region Texteingabe
        void textBox_Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) chat_im_reply();
        }

        private void button_addText_Click(object sender, EventArgs e)
        {
            chat_im_reply();
        }

        private void chat_im_reply()
        {
            UUID ava_key = UUID.Parse(listView_Clients.SelectedItems[0].Name);
            if (tabControl_Com.SelectedTab.Name == MainForm_Sub.Tabs.tabpage_chat_name)Comunikation_Sub.Messages.say_chat(ava_key, textBox_Message.Text);
            else Comunikation_Sub.Messages.say_session_id(ava_key, UUID.Parse(tabControl_Com.SelectedTab.Name), textBox_Message.Text);
            textBox_Message.Text = "";
        }
        #endregion

        void listView_Clients_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            // Create an instance of the ColHeader class.
            ColHeader clickedCol = (ColHeader)this.listView_Clients.Columns[e.Column];



            // Set the ascending property to sort in the opposite order.
            clickedCol.ascending = !clickedCol.ascending;

            // Get the number of items in the list.
            int numItems = this.listView_Clients.Items.Count;

            // Turn off display while data is repoplulated.
            this.listView_Clients.BeginUpdate();

            // Populate an ArrayList with a SortWrapper of each list item.
            ArrayList SortArray = new ArrayList();
            for (int i = 0; i < numItems; i++)
            {
                SortArray.Add(new SortWrapper(this.listView_Clients.Items[i], e.Column));
            }

            // Sort the elements in the ArrayList using a new instance of the SortComparer
            // class. The parameters are the starting index, the length of the range to sort,
            // and the IComparer implementation to use for comparing elements. Note that
            // the IComparer implementation (SortComparer) requires the sort
            // direction for its constructor; true if ascending, othwise false.
            SortArray.Sort(0, SortArray.Count, new SortWrapper.SortComparer(clickedCol.ascending));

            // Clear the list, and repopulate with the sorted items.
            this.listView_Clients.Items.Clear();
            for (int i = 0; i < numItems; i++)
                this.listView_Clients.Items.Add(((SortWrapper)SortArray[i]).sortItem);

            // Turn display back on.
            this.listView_Clients.EndUpdate();

        }

        

        private void listView_Clients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_Clients.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView_Clients.Items)
                {
                    if (MCP_Sub.init.names[UUID.Parse(item.Name)].online) item.BackColor = Color.LightGreen;
                    else item.BackColor = Color.LightPink;
                }
                if (MCP_Sub.init.names[UUID.Parse(listView_Clients.SelectedItems[0].Name)].online) listView_Clients.SelectedItems[0].BackColor = Color.Green;
                else listView_Clients.SelectedItems[0].BackColor = Color.Red;

                tabControl_Com.TabPages.Clear();
                UUID ava_key = UUID.Parse(listView_Clients.SelectedItems[0].Name);

                if (MCP_Sub.init.names.ContainsKey(ava_key)) this.Text = MCP_Sub.init.names[ava_key].name;

                DoggyClient.MainForm_Sub.Tabs.add_Controler(ava_key, tabControl_Com);

                DoggyClient.MainForm_Sub.Tabs.add_chat(ava_key, tabControl_Com);
                DoggyClient.MainForm_Sub.Tabs.add_IM(ava_key, tabControl_Com);
                DoggyClient.MainForm_Sub.Tabs.add_G_IM(ava_key, tabControl_Com);


                tabControl_check();
            }
        }
    }

    public class ColHeader : ColumnHeader
    {
        public bool ascending;
        public ColHeader(string text, int width, HorizontalAlignment align, bool asc)
        {
            this.Text = text;
            this.Width = width;
            this.TextAlign = align;
            this.ascending = asc;
        }
    }

    public class SortWrapper
    {
        internal ListViewItem sortItem;
        internal int sortColumn;


        // A SortWrapper requires the item and the index of the clicked column.
        public SortWrapper(ListViewItem Item, int iColumn)
        {
            sortItem = Item;
            sortColumn = iColumn;
        }

        // Text property for getting the text of an item.
        public string Text
        {
            get
            {
                return sortItem.SubItems[sortColumn].Text;
            }
        }

        // Implementation of the IComparer
        // interface for sorting ArrayList items.
        public class SortComparer : IComparer
        {
            bool ascending;

            // Constructor requires the sort order;
            // true if ascending, otherwise descending.
            public SortComparer(bool asc)
            {
                this.ascending = asc;
            }

            // Implemnentation of the IComparer:Compare
            // method for comparing two objects.
            public int Compare(object x, object y)
            {
                SortWrapper xItem = (SortWrapper)x;
                SortWrapper yItem = (SortWrapper)y;

                string xText = xItem.sortItem.SubItems[xItem.sortColumn].Text;
                string yText = yItem.sortItem.SubItems[yItem.sortColumn].Text;
                return xText.CompareTo(yText) * (this.ascending ? 1 : -1);
            }
        }
    }

}

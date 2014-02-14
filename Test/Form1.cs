using Framework;
using Framework.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WoWAPI;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var items = await Item.SearchByNameAsync(textBox1.Text);
            foreach (Item i in items)
            {
                listBox1.Items.Add(i);
            }
        }

        private void itemTest()
        {
            var items = new List<Item>();
            string json = Request.GetJSON("http://www.wowhead.com/search?q=" + textBox1.Text.Replace(" ", "+") + "&opensearch", 5000);
            dynamic results = Framework.Data.JSON.Deserialize(json);
            for (int i = 0; i < results[1].Count; i++)
            {
                if (results[1][i].Value.IndexOf("(Item)") != -1)
                {
                    items.Add(new Item()
                    {
                        Name = results[1][i].Value,
                        ID = int.Parse(results[7][i][1].Value.ToString()),
                        Icon = results[7][i][2].Value
                    });
                }
            }
            MessageBox.Show("First item\nName: " + items[0].Name + "\nID:" + items[0].ID.ToString() + "\nIcon: " + items[0].Icon);
        }

        private void jsonTest()
        {
            string json = Request.GetJSON("http://us.battle.net/api/wow/character/mugthol/makeitdrizle", 5000, DateTime.Now);
            MessageBox.Show(json);
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = await Item.PopAsync(((sender as ListBox).SelectedItem as Item).ID);
            pictureBox1.ImageLocation = item.GetIconPath();
            label1.Text = item.Name;
            label2.Text = item.Slot.ToString();
            label3.Text = item.ItemLevel.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            jsonTest();
            //string json = await Request.GetJSONAsync("http://www.wowhead.com/search?q=" + textBox1.Text.Replace(" ", "+") + "&opensearch", 1000);
            //MessageBox.Show(json);
        }
    }
}

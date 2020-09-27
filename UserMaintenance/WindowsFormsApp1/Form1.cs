﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Entities;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<Entities.User>();
        public Form1()
        {
            InitializeComponent();
            label1.Text = Resource1.Fullname;
            button2.Text = Resource1.Write;
            button1.Text = Resource1.Add;
            button3.Text = Resource1.Delete;
            listBox1.DataSource = users;
            listBox1.ValueMember = "ID";
            listBox1.DisplayMember = "FullName";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var u = new User()
            {
                FullName = textBox1.Text,
               
            };
            users.Add(u);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Comma Seperated Values (*.csv)|*.csv";
            sfd.DefaultExt = "csv";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() != DialogResult.OK) return;
            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                foreach (var g in users)
                {
                    sw.Write(g.ID);
                    sw.Write(" ");
                    sw.Write(g.FullName);
                    sw.WriteLine();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var u = new User();
           
            u = (User)(listBox1.SelectedItem);
            users.Remove(u);
        }
    }
}

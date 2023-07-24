using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HostEditor
{
    public partial class Form1 : Form
    {

        String filepath = String.Empty; 
        public Form1()
        {
            InitializeComponent();
            openHOSTFile();
            label5.Text = checkOS(); 
        }


        #region["OS 버전 확인"]
        public String checkOS()
        {
            string HKLMWinNTCurrent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            string osName = Registry.GetValue(HKLMWinNTCurrent, "productName", "").ToString();
            string osBuild = Registry.GetValue(HKLMWinNTCurrent, "CurrentBuildNumber", "").ToString();
            String label;
            String[] osName_arr = osName.Split(' ');
            if (osName_arr[1].Equals("10"))
            {
                if (Int32.Parse(osBuild) > 22000) //Windows 11 및 Windows 10/Server 2016~2022 구분 
                {
                    label = "Your Operating System : Windows 11 " + osName_arr[2] + " Build: " + osBuild;
                }
                else
                {
                    label = "Your Operating System : " + osName + " Build: " + osBuild;
                }
            }
            else
            {
                label = "Your Operating System : " + osName + " Build: " + osBuild;
            }
            return label;
        }
        #endregion

        public void openHOSTFile()
        {
            try
            { 
                String windowspath = Interaction.InputBox("Input your Current Windows Directory.(For example. C:\\WINDOWS)", "HostEditor", "C:\\WINDOWS");
                filepath = windowspath + "\\System32\\drivers\\etc\\hosts"; 
                String text = File.ReadAllText(filepath);
                String[] text_list = text.Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
                for(int i=0; i < text_list.Length; i++)
                {
                    listBox1.Items.Add(text_list[i] + "\r\n"); 
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String ipaddress = textBox2.Text;
                String dnsaddress = textBox3.Text;

                if (radioButton1.Checked) //Add 
                {
                    listBox1.Items.Add("\r\n" + ipaddress + " " + dnsaddress + "\r\n");
                }
                else //Remove
                {
                    listBox1.Items.Remove(listBox1.Text); 
                }

                String text_new = String.Empty; 
                for(int i=0; i<listBox1.Items.Count; i++)
                {
                    text_new += listBox1.Items[i].ToString();  
                }

                File.WriteAllText(filepath, text_new); 
                MessageBox.Show("Successfully Changed"); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Refresh();  
        }

        #region["Remove Mode"] 
        private void radioButton2_CheckChanged(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }
        #endregion

        #region["Add Mode => Default "] 
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            listBox1.SelectedIndex = 0;
        }
        #endregion

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

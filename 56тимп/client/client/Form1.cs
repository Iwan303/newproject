using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace client
{
    public partial class Form1 : Form
    {
        static IPHostEntry ipHost = Dns.GetHostEntry("localhost");
        static IPAddress ipAddr = ipHost.AddressList[0];
        static IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 7770);
        Socket req = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        public Form1()
        {
            InitializeComponent();
            //Form2 f = new Form2();
            //f.ShowDialog();
            try
            {
                byte[] buff = new byte[1024];
                req.Connect(ipEndPoint);
                byte[] msg = Encoding.UTF8.GetBytes("asd");
                req.Send(msg);
                int countbyte = req.Receive(buff);
                string threatening = Encoding.UTF8.GetString(buff, 0, countbyte);
                string[] asd = threatening.Split(new Char[] { '/' });
                for (int i = 0; i < asd.Length; i++) { comboBox1.Items.Add(asd[i]); }
                comboBox1.SelectedIndex = 0;
                req.Shutdown(SocketShutdown.Both);
                req.Close();
            }
            catch (SocketException) { MessageBox.Show("111111111Ошибка подключения к серверу!!!"); }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                Socket qr = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                byte[] buffer = new byte[1024];
                qr.Connect(ipEndPoint);
                string message = comboBox1.SelectedItem + " " + textBox1.Text + " " + textBox2.Text;
                byte[] ms = Encoding.UTF8.GetBytes(message);
                qr.Send(ms);
                int countbyte = qr.Receive(buffer);
                string a = Encoding.UTF8.GetString(buffer, 0, countbyte);
                //string[] fs = a.Split(new Char[] { '/' });
                MessageBox.Show("Площадь ровна : " + a);
            }
            catch (SocketException) { MessageBox.Show("Ошибка подключения к серверу!!!"); }
        }
    }
}

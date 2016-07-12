using System;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;

namespace client
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] buff = new byte[1024];
                IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 7770);
                Socket send = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                send.Connect(ipEndPoint);
                byte[] Bmsg = Encoding.UTF8.GetBytes("B");
                send.Send(Bmsg);
                int cobyte = send.Receive(buff);
                if (Encoding.UTF8.GetString(buff, 0, cobyte) == "md5")
                {
                    send.Shutdown(SocketShutdown.Both);
                    send.Close();
                    Socket sendData = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    string hashPass = null; byte[] buff2 = new byte[1024];
                    using (MD5 md5Hash = MD5.Create())
                    {
                        hashPass = GetMd5Hash(md5Hash, textBox2.Text);
                    }
                    string message = "R" + " " + textBox1.Text + " " + hashPass;
                    byte[] msg = Encoding.UTF8.GetBytes(message);
                    sendData.Connect(ipEndPoint);
                    sendData.Send(msg);
                    int countbyte = sendData.Receive(buff2);
                    if (Encoding.UTF8.GetString(buff2, 0, countbyte) == "good")
                    {
                        MessageBox.Show("Вы вошли в систему.");
                        sendData.Shutdown(SocketShutdown.Both);
                        sendData.Close();
                        Hide();
                        Form1 f = new Form1();
                        f.ShowDialog();
                        this.Close();
                    }
                    else if (Encoding.UTF8.GetString(buff2, 0, countbyte) == "bad") { MessageBox.Show("Неверный логин или пароль!!!"); }
                    else { MessageBox.Show("Ошибка!!!"); }
                    }
                else { MessageBox.Show("111"); }     
                }
            catch (SocketException ex) { MessageBox.Show(ex.ToString()); }
        }

        string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}

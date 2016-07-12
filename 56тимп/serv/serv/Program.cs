using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;

namespace serv
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 7770);
                Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                while (true)
                {
                    Socket handler = sListener.Accept();
                    byte[] buff = new byte[1024];
                    int countbyte = handler.Receive(buff);
                    string msg = Encoding.UTF8.GetString(buff, 0, countbyte);
                    if (msg.Substring(0, 1) == "B") 
                    {
                        string ms = "md5";
                        byte[] ansms = Encoding.UTF8.GetBytes(ms);
                        handler.Send(ansms);
                    }
                    else if (msg.Substring(0, 1) == "R")
                    {
                        
                        byte[] answer = null;
                        if (val_data(msg)) 
                        { answer = Encoding.UTF8.GetBytes("good"); handler.Send(answer); }
                        else { answer = Encoding.UTF8.GetBytes("bad"); handler.Send(answer); }
                        Console.WriteLine(Encoding.UTF8.GetString(answer, 0, answer.Length));
                    }
                    else if (msg.Substring(0, 1) == "т")
                    {
                        string[] asd = msg.Split(new Char[] { ' ' });
                        double h = Convert.ToDouble(asd[1]); double w = Convert.ToDouble(asd[2]);
                        double S = h * w * 0.5;
                        string ms = S.ToString();
                        byte[] ansms = Encoding.UTF8.GetBytes(ms);
                        handler.Send(ansms);
                    }
                    else if (msg.Substring(0, 1) == "п")
                    {
                        string[] asd = msg.Split(new Char[] { ' ' });
                        double h = Convert.ToDouble(asd[1]); double w = Convert.ToDouble(asd[2]);
                        double S = h * w;
                        string ms = S.ToString();
                        byte[] ansms = Encoding.UTF8.GetBytes(ms);
                        handler.Send(ansms);
                    }
                    else if (msg.Substring(0, 1) == "к")
                    {
                        string[] asd = msg.Split(new Char[] { ' ' });
                        double h = Convert.ToDouble(asd[1]); double w = Convert.ToDouble(asd[2]); double S = 0;
                        string ms = null;
                        if (h != 1 & w == 1)
                        { S = h * h * 3.14; ms = S.ToString(); }
                        else if (h == 1 & w != 1)
                        { S = (w * w) * (3.14 / 4); ms = S.ToString(); }
                        else
                        { ms = "ошибка"; }
                        byte[] ansms = Encoding.UTF8.GetBytes(ms);
                        handler.Send(ansms);
                    }
                    else if (msg.Substring(0, 1) == "ш")
                    {
                        string[] asd = msg.Split(new Char[] { ' ' });
                        double h = Convert.ToDouble(asd[1]); double w = Convert.ToDouble(asd[2]);
                        double S = h * h * 4 * 3.14;
                        string ms = S.ToString();
                        byte[] ansms = Encoding.UTF8.GetBytes(ms);
                        handler.Send(ansms);
                    }
                    else
                    {
                        string dsa = "треугольник" + "/"
                            + "прямоугольник" + "/"
                            + "круг" + "/"
                            + "шар";
                        byte[] ansdsa = Encoding.UTF8.GetBytes(dsa);
                        handler.Send(ansdsa);
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); Console.ReadLine(); }
        }

         static bool val_data(string data)
         {
             string[,] bd = new string[,] { {"user1","user2","user3"}, 
                                                     {"qwert1","qwert2","qwert3"} };
             using (MD5 md5Hash = MD5.Create())
             {
                 string[] asd = data.Split(new Char[] { ' ' });
                 for (int i = 0; i < 3; i++)
                 {
                     if (asd[1] == bd[0, i])
                     {
                         if (asd[2] == GetMd5Hash(md5Hash, bd[1, i])) { return true; }
                     }
                 }
             }
             return false;
         }
         static string GetMd5Hash(MD5 md5Hash, string input)
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
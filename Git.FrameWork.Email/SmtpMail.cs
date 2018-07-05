using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;

namespace Git.Framework.Email
{
    public class SmtpMail
    {
        private NetworkStream networkStream;
        private StreamReader sr;
        private StreamWriter sw;
        private TcpClient tcpClient;

        public void CloseConnection()
        {
            this.SendDataToServer("QUIT");
            this.sr.Close();
            this.sw.Close();
            this.networkStream.Close();
            this.tcpClient.Close();
        }

        public string DeleteEmail(string str)
        {
            if (this.SendDataToServer("DELE " + str))
            {
                return "成功删除";
            }
            return "Error";
        }

        private string ReadDataFromServer()
        {
            string message = null;
            try
            {
                message = this.sr.ReadLine();
                if (message[0] == '-')
                {
                    message = null;
                }
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        public string ReadEmail(string str)
        {
            if (!this.SendDataToServer("RETR " + str))
            {
                return "Error";
            }
            return this.sr.ReadToEnd();
        }

        public ArrayList ReceiveMail(string uid, string pwd)
        {
            ArrayList list = new ArrayList();
            int index = uid.IndexOf('@');
            string hostname = "pop3." + uid.Substring(index + 1);
            this.tcpClient = new TcpClient(hostname, 110);
            this.networkStream = this.tcpClient.GetStream();
            this.sr = new StreamReader(this.networkStream);
            this.sw = new StreamWriter(this.networkStream);
            if (this.ReadDataFromServer() != null)
            {
                if (!this.SendDataToServer("USER " + uid))
                {
                    return list;
                }
                if (this.ReadDataFromServer() == null)
                {
                    return list;
                }
                if (!this.SendDataToServer("PASS " + pwd))
                {
                    return list;
                }
                if (this.ReadDataFromServer() == null)
                {
                    return list;
                }
                if (!this.SendDataToServer("LIST"))
                {
                    return list;
                }
                string str = this.ReadDataFromServer();
                if (str == null)
                {
                    return list;
                }
                int num2 = int.Parse(str.Split(new char[] { ' ' })[1]);
                if (num2 > 0)
                {
                    for (int i = 0; i < num2; i++)
                    {
                        str = this.ReadDataFromServer();
                        if (str == null)
                        {
                            return list;
                        }
                        string[] strArray = str.Split(new char[] { ' ' });
                        list.Add(string.Format("第{0}封邮件，{1}字节", strArray[0], strArray[1]));
                    }
                    return list;
                }
            }
            return list;
        }

        private bool SendDataToServer(string str)
        {
            try
            {
                this.sw.WriteLine(str);
                this.sw.Flush();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}


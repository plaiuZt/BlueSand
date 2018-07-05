using Git.Framework.Resource;
using System;
using System.Net;
using System.Net.Mail;

namespace Git.Framework.Email
{
    public class MailServer
    {
        private string _fromName;
        private string _fromPassword;
        private MailMessageItem _mailMessage;
        private string _smtpPort;
        private string _smtpServer;

        public MailServer() : this(ResourceManager.GetSettingEntity("Setting.SMTP_SERVER").Value, ResourceManager.GetSettingEntity("Setting.SMTP_SERVER_PORT").Value, ResourceManager.GetSettingEntity("Setting.MAIL_USERNAME").Value, ResourceManager.GetSettingEntity("Setting.MAIL_PASSWORD").Value)
        {
        }

        public MailServer(string smtpServer, string smtpPort, string fromName, string fromPassword)
        {
            this._smtpServer = string.Empty;
            this._smtpPort = string.Empty;
            this._fromName = string.Empty;
            this._fromPassword = string.Empty;
            this._smtpPort = smtpPort;
            this._smtpServer = smtpServer;
            this._fromName = fromName;
            this._fromPassword = fromPassword;
        }

        public void SendMail()
        {
            if (this.MailMessage != null)
            {
                using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(this._fromName, this.MailMessage.Recipients[0], this.MailMessage.Subject, this.MailMessage.Body))
                {
                    SmtpClient client = new SmtpClient(this._smtpServer) {
                        Credentials = new NetworkCredential(this._fromName, this._fromPassword)
                    };
                    if ((this.MailMessage.Attachments != null) && (this.MailMessage.Attachments.Attachments != null))
                    {
                        foreach (string str in this.MailMessage.Attachments.Attachments)
                        {
                            message.Attachments.Add(new Attachment(str));
                        }
                    }
                    client.Send(message);
                }
            }
        }

        public MailMessageItem MailMessage
        {
            get
            {
                return this._mailMessage;
            }
            set
            {
                this._mailMessage = value;
            }
        }
    }
}


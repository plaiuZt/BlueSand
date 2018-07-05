using System;
using System.Collections.Generic;

namespace Git.Framework.Email
{

    public class MailMessageItem
    {
        private MailAttachments _Attachments = new MailAttachments();
        private string _Body;
        private MailFormat _BodyFormat = MailFormat.HTML;
        private string _Charset = "GB2312";
        private string _From;
        private string _FromName;
        private int _MaxRecipientNum = 30;
        private MailPriority _Priority = MailPriority.Normal;
        private IList<string> _Recipients = new List<string>();
        private string _Subject;

        public MailMessageItem()
        {
            this._Charset = "GB2312";
        }

        public void AddRecipients(string recipient)
        {
            if (this._Recipients.Count < this.MaxRecipientNum)
            {
                this._Recipients.Add(recipient);
            }
        }

        public void AddRecipients(params string[] recipient)
        {
            if (recipient == null)
            {
                throw new ArgumentException("收件人不能为空.");
            }
            for (int i = 0; i < recipient.Length; i++)
            {
                this.AddRecipients(recipient[i]);
            }
        }

        public MailAttachments Attachments
        {
            get
            {
                return this._Attachments;
            }
            set
            {
                this._Attachments = value;
            }
        }

        public string Body
        {
            get
            {
                return this._Body;
            }
            set
            {
                this._Body = value;
            }
        }

        public MailFormat BodyFormat
        {
            get
            {
                return this._BodyFormat;
            }
            set
            {
                this._BodyFormat = value;
            }
        }

        public string Charset
        {
            get
            {
                return this._Charset;
            }
            set
            {
                this._Charset = value;
            }
        }

        public string From
        {
            get
            {
                return this._From;
            }
            set
            {
                this._From = value;
            }
        }

        public string FromName
        {
            get
            {
                return this._FromName;
            }
            set
            {
                this._FromName = value;
            }
        }

        public int MaxRecipientNum
        {
            get
            {
                return this._MaxRecipientNum;
            }
            set
            {
                this._MaxRecipientNum = value;
            }
        }

        public MailPriority Priority
        {
            get
            {
                return this._Priority;
            }
            set
            {
                this._Priority = value;
            }
        }

        public IList<string> Recipients
        {
            get
            {
                return this._Recipients;
            }
        }

        public string Subject
        {
            get
            {
                return this._Subject;
            }
            set
            {
                this._Subject = value;
            }
        }
    }
}


namespace Git.Framework.Controller
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web.UI;

    public class HtmlHelper
    {
        private static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private static System.Net.CookieContainer cc = new System.Net.CookieContainer();
        private static string contentType = "application/x-www-form-urlencoded";
        private static int currentTry = 0;
        private static int delay = 0x3e8;
        private static System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("utf-8");
        private static int maxTry = 300;
        private static string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";

        public static string CSS(string cssPath, Page p)
        {
            return ("<link href=\"" + p.ResolveUrl(cssPath) + "\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
        }

        public static string DelHtml(string s_TextStr, string html_Str)
        {
            string str = "";
            if (!string.IsNullOrEmpty(s_TextStr))
            {
                str = Regex.Replace(Regex.Replace(s_TextStr, "<" + html_Str + "[^>]*>", "", RegexOptions.IgnoreCase), "</" + html_Str + ">", "", RegexOptions.IgnoreCase);
            }
            return str;
        }

        public static string File(string Path, Page p)
        {
            return p.ResolveUrl(Path);
        }

        public static string Get_Http(string tUrl)
        {
            string message;
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(tUrl);
                request.Timeout = 0x4c90;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                StringBuilder builder = new StringBuilder();
                while (-1 != reader.Peek())
                {
                    builder.Append(reader.ReadLine() + "\r\n");
                }
                message = builder.ToString();
                response.Close();
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        public static string Get_Http(string tUrl, System.Text.Encoding returnEncode)
        {
            string message;
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(tUrl);
                request.Timeout = 0x4c90;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), returnEncode);
                StringBuilder builder = new StringBuilder();
                while (-1 != reader.Peek())
                {
                    builder.Append(reader.ReadLine() + "\r\n");
                }
                message = builder.ToString();
                response.Close();
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        public string GetHref(string HtmlCode)
        {
            string str = "";
            string pattern = "(h|H)(r|R)(e|E)(f|F) *= *('|\")?((\\w|\\\\|\\/|\\.|:|-|_)+)[\\S]*";
            foreach (Match match in Regex.Matches(HtmlCode, pattern))
            {
                str = str + match.Value.ToLower().Replace("href=", "").Trim() + "|";
            }
            return str;
        }

        public static string GetHtml(string url, System.Net.CookieContainer cookieContainer)
        {
            Thread.Sleep(NetworkDelay);
            currentTry++;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest) WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                request.ContentType = contentType;
                request.ServicePoint.ConnectionLimit = maxTry;
                request.Referer = url;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = "GET";
                response = (HttpWebResponse) request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, encoding);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                currentTry--;
                request.Abort();
                response.Close();
                return str;
            }
            catch (Exception)
            {
                if (currentTry <= maxTry)
                {
                    GetHtml(url, cookieContainer);
                }
                currentTry--;
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public static string GetHtml(string url, string postData, bool isPost, System.Net.CookieContainer cookieContainer)
        {
            if (string.IsNullOrEmpty(postData))
            {
                return GetHtml(url, cookieContainer);
            }
            Thread.Sleep(NetworkDelay);
            currentTry++;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                byte[] bytes = System.Text.Encoding.Default.GetBytes(postData);
                request = (HttpWebRequest) WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                request.ContentType = contentType;
                request.ServicePoint.ConnectionLimit = maxTry;
                request.Referer = url;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = isPost ? "POST" : "GET";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                response = (HttpWebResponse) request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, encoding);
                string str2 = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                currentTry = 0;
                request.Abort();
                response.Close();
                return str2;
            }
            catch (Exception)
            {
                if (currentTry <= maxTry)
                {
                    GetHtml(url, postData, isPost, cookieContainer);
                }
                currentTry--;
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public string GetImg(string ImgString, string imgHttp)
        {
            string str = "";
            string pattern = @"src=.+\.(bmp|jpg|gif|png|)";
            foreach (Match match in Regex.Matches(ImgString.ToLower(), pattern))
            {
                str = str + match.Value.ToLower().Trim().Replace("src=", "");
            }
            if (((((str.IndexOf(".net") != -1) || (str.IndexOf(".com") != -1)) || ((str.IndexOf(".org") != -1) || (str.IndexOf(".cn") != -1))) || (((str.IndexOf(".cc") != -1) || (str.IndexOf(".info") != -1)) || (str.IndexOf(".biz") != -1))) || (str.IndexOf(".tv") != -1))
            {
                return str;
            }
            return (imgHttp + str);
        }

        public string GetImgSrc(string HtmlCode, string imgHttp)
        {
            string str = "";
            string pattern = "<img.+?>";
            foreach (Match match in Regex.Matches(HtmlCode.ToLower(), pattern))
            {
                str = str + this.GetImg(match.Value.ToLower().Trim(), imgHttp) + "|";
            }
            return str;
        }

        public static Stream GetStream(string url, System.Net.CookieContainer cookieContainer)
        {
            currentTry++;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest) WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                request.ContentType = contentType;
                request.ServicePoint.ConnectionLimit = maxTry;
                request.Referer = url;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = "GET";
                response = (HttpWebResponse) request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                currentTry--;
                return responseStream;
            }
            catch (Exception)
            {
                if (currentTry <= maxTry)
                {
                    GetHtml(url, cookieContainer);
                }
                currentTry--;
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return null;
            }
        }

        public static string JS(string jsPath, Page p)
        {
            return ("<script type=\"text/javascript\" src=\"" + p.ResolveUrl(jsPath) + "\"></script>\r\n");
        }

        public static string NoHTML(string Htmlstring)
        {
            Htmlstring = Regex.Replace(Htmlstring, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = new Regex("<.+?>", RegexOptions.IgnoreCase).Replace(Htmlstring, "");
            Htmlstring = Regex.Replace(Htmlstring, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            return Htmlstring;
        }

        public static string Post_Http(string url, string postData, string encodeType)
        {
            try
            {
                byte[] bytes = System.Text.Encoding.GetEncoding(encodeType).GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                return reader.ReadToEnd();
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static string Post_Http(string url, string postData, string encodeType, System.Text.Encoding returnEncode)
        {
            try
            {
                byte[] bytes = System.Text.Encoding.GetEncoding(encodeType).GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), returnEncode);
                return reader.ReadToEnd();
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static string Post_Http(string url, string postData, System.Text.Encoding encodeType, System.Text.Encoding returnEncode)
        {
            try
            {
                byte[] bytes = encodeType.GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), returnEncode);
                return reader.ReadToEnd();
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static string ZipHtml(string Html)
        {
            Html = Regex.Replace(Html, @">\s+?<", "><");
            Html = Regex.Replace(Html, @"\r\n\s*", "");
            Html = Regex.Replace(Html, @"<body([\s|\S]*?)>([\s|\S]*?)</body>", "<body$1>$2</body>", RegexOptions.IgnoreCase);
            return Html;
        }

        public static System.Net.CookieContainer CookieContainer
        {
            get
            {
                return cc;
            }
        }

        public static System.Text.Encoding Encoding
        {
            get
            {
                return encoding;
            }
            set
            {
                encoding = value;
            }
        }

        public static int MaxTry
        {
            get
            {
                return maxTry;
            }
            set
            {
                maxTry = value;
            }
        }

        public static int NetworkDelay
        {
            get
            {
                Random random = new Random();
                return random.Next(delay, delay * 2);
            }
            set
            {
                delay = value;
            }
        }
    }
}


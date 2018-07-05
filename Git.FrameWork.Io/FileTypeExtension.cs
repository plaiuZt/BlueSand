using System;
using System.Runtime.CompilerServices;

namespace Git.Framework.Io
{
    public static class FileTypeExtension
    {
        public static short FormatFileType(this EFileType fileType, string subffex)
        {
            short num = -1;
            if (EFileType.Aspx.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Aspx.FormatValue();
            }
            if (EFileType.Asp.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Asp.FormatValue();
            }
            if (EFileType.Ashx.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Ashx.FormatValue();
            }
            if (EFileType.Master.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Master.FormatValue();
            }
            if (EFileType.Config.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Config.FormatValue();
            }
            if (EFileType.Asax.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Asax.FormatValue();
            }
            if (EFileType.Txt.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Txt.FormatValue();
            }
            if (EFileType.Html.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Html.FormatValue();
            }
            if (EFileType.Htm.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Htm.FormatValue();
            }
            if (EFileType.Css.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Css.FormatValue();
            }
            if (EFileType.Js.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Js.FormatValue();
            }
            if (EFileType.Bmp.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Bmp.FormatValue();
            }
            if (EFileType.Gif.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Gif.FormatValue();
            }
            if (EFileType.Jpeg.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Jpeg.FormatValue();
            }
            if (EFileType.Jpeg2000.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Jpeg2000.FormatValue();
            }
            if (EFileType.Psd.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Psd.FormatValue();
            }
            if (EFileType.Png.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Png.FormatValue();
            }
            if (EFileType.Swf.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Swf.FormatValue();
            }
            if (EFileType.Doc.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Doc.FormatValue();
            }
            if (EFileType.Docx.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Docx.FormatValue();
            }
            if (EFileType.Xls.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Xls.FormatValue();
            }
            if (EFileType.Xlsx.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Xlsx.FormatValue();
            }
            if (EFileType.Pdf.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Pdf.FormatValue();
            }
            if (EFileType.Jpg.FormatString().ToLower() == subffex.ToLower())
            {
                num = EFileType.Jpg.FormatValue();
            }
            return num;
        }

        public static string FormatString(this EFileType fileType)
        {
            string str = string.Empty;
            switch (fileType)
            {
                case EFileType.Aspx:
                    return "Aspx";

                case EFileType.Asp:
                    return "Asp";

                case EFileType.Ashx:
                    return "Ashx";

                case EFileType.Master:
                    return "Master";

                case EFileType.Config:
                    return "Config";

                case EFileType.Asax:
                    return "Asax";

                case EFileType.Txt:
                    return "Txt";

                case EFileType.Html:
                    return "Html";

                case EFileType.Htm:
                    return "Htm";

                case EFileType.Css:
                    return "Css";

                case EFileType.Js:
                    return "Js";

                case EFileType.Bmp:
                    return "Bmp";

                case EFileType.Gif:
                    return "Gif";

                case EFileType.Jpeg:
                    return "Jpeg";

                case EFileType.Jpeg2000:
                    return "Jpeg2000";

                case EFileType.Psd:
                    return "Psd";

                case EFileType.Png:
                    return "Png";

                case EFileType.Swf:
                    return "Swf";

                case EFileType.Doc:
                    return "Doc";

                case EFileType.Docx:
                    return "Docx";

                case EFileType.Xls:
                    return "Xls";

                case EFileType.Xlsx:
                    return "Xlsx";

                case EFileType.Pdf:
                    return "Pdf";

                case EFileType.Jpg:
                    return "Jpg";
            }
            return str;
        }

        public static short FormatValue(this EFileType fileType)
        {
            return (short) fileType;
        }
    }
}


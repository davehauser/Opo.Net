using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Mime;

namespace Opo.Net.Mime
{
    public static class MediaType
    {
        public static class Application
        {
            public const string Excel = "application/msexcel";
            public const string Flash = "application/s-shockwave-flash";
            public const string Gzip = "application/gzip";
            public const string Javascript = "application/javascript";
            public const string Octet = "application/octet-stream";
            public const string Pdf = "application/pdf";
            public const string Powerpoint = "application/mspowerpoint";
            public const string Rtf = "application/rtf";
            public const string Soap = "application/soap+xml";
            public const string Word = "application/msword";
            public const string XHtml = "application/xhtml+xml";
            public const string Xml = "application/xml";
            public const string Zip = "application/zip";
        }

        public static class Audio
        {
            public const string Aiff = "audio/x-aiff";
            public const string Midi = "audio/x-midi";
            public const string Mpeg = "audio/x-mpeg";
            public const string RealAudio = "audio/x-pn-realaudio";
            public const string Wave = "audio/x-wav";
        }

        public static class Image
        {
            public const string Gif = "image/gif";
            public const string Jpeg = "image/jpeg";
            public const string Png = "image/png";
            public const string Tiff = "image/tiff";
        }

        public static class Message
        {
            public const string Rfc822 = "message/rfc822";
        }

        public static class Multipart
        {
            public const string Alternative = "multipart/alternative";
            public const string ByteRanges = "multipart/byteranges";
            public const string Digest = "multipart/digest";
            public const string Encrypted = "multipart/encrypted";
            public const string Mixed = "multipart/mixed";
            public const string VoiceMessage = "multipart/voice-message";
        }

        public static class Text
        {
            public const string CommaSeparated = "text/comma-separated-values";
            public const string Css = "text/css";
            public const string Html = "text/html";
            public const string Javascript = "text/javascript";
            public const string Plain = "text/plain";
            public const string RichText = "text/richtext";
            public const string Xml = "text/xml";
        }

        public class Video
        {
            public const string Avi = "video/x-msvideo";
            public const string Mpeg = "video/mpeg";
            public const string Quicktime = "video/quicktime";
            public const string Wmv = "video/x-ms-wmv";
        }

        public static string GetMediaType(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            switch (extension)
            {
                #region Application
                case "xls":
                case "xlsx":
                case "xlsm":
                    return Application.Excel;
                case "swf":
                case "fla":
                    return Application.Flash;
                case "gz":
                    return Application.Gzip;
                case "js":
                    return Application.Javascript;
                case "pdf":
                    return Application.Pdf;
                case "ppt":
                case "pps":
                case "pptx":
                case "ppsx":
                    return Application.Powerpoint;
                case "rtf":
                    return Application.Rtf;
                case "soap":
                    return Application.Soap;
                case "doc":
                case "docx":
                case "docm":
                    return Application.Word;
                case "xhtml":
                    return Application.XHtml;
                case "zip":
                    return Application.Zip;
                #endregion
                #region Audio
                case "aiff":
                    return Audio.Aiff;
                case "mid":
                    return Audio.Midi;
                case "mp3":
                case "mp4":
                    return Audio.Mpeg;
                case "ra":
                case "rm":
                    return Audio.RealAudio;
                case "wav":
                    return Audio.Wave;
                #endregion
                #region Image
                case "gif":
                    return Image.Gif;
                case "jpg":
                case "jpeg":
                    return Image.Jpeg;
                case "png":
                    return Image.Png;
                case "tiff":
                case "tif":
                    return Image.Tiff;
                #endregion
                #region Text
                case "csv":
                    return Text.CommaSeparated;
                case "css":
                    return Text.Css;
                case "html":
                case "htm":
                    return Text.Html;
                case "txt":
                    return Text.Plain;
                case "xml":
                    return Text.Xml;
                #endregion
                #region Video
                case "avi":
                    return Video.Avi;
                case "mpg":
                case "mpeg":
                    return Video.Mpeg;
                case "mov":
                case "qt":
                    return Video.Quicktime;
                case "wmv":
                    return Video.Wmv;
                #endregion
                default:
                    return Application.Octet;
            }
        }
    }
}

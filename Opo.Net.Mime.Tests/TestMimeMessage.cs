using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mime
{
    public static class TestMimeMessage
    {
        public static string[] emailAdresses = new string[]
        {
            "\"Address 1\" <email1@example.org>",
            "Umlaut äöü <umlaut@example.org>",
            "email3@example.org",
            "\"Address 4\" <email4@example.org>"
        };

        public static string from = emailAdresses[0];
        public static string to = String.Format("{0},\r\n\t=?iso-8859-1?Q?Umlaut =E4=F6=FC?= <umlaut@example.org>, {2},\r\n {3}", emailAdresses);
        public static string cc = to;
        public static string bcc = to;
        public static string messageID = "testmessage_MessageID";
        public static string subject = "Message Subject";
        public static string messageDate = "Sat, 01 Jan 2001 00:00:00 +0100";
        public static string mimeVersion = "1.0";
        public static string contentType = "multipart/mixed";
        public static string boundaryLevel1 = "--=_NextPart001";
        public static string priority = "3";
        public static string textBody = @"This is the text body of the mail message. =
Content-Transfer-Encoding=3Dquoted-printable";
        public static string textBodyContentType = "text/plain";
        public static string textBodyCharset = "iso-8859-1";
        public static string textBodyContentTransferEncoding = "quoted-printable";
        public static string htmlBody = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html><head>
<meta http-equiv=3DContent-Type content=3D""text/html; =
charset=3Diso-8859-1"">
</head>
<body>
<h1>Hello Reader</h1>
<p>This is the text body of the mail message. =
Content-Transfer-Encoding=3Dquoted-printable</p>
</body>
</html>";
        public static string htmlBodyContentType = "text/html";
        public static string htmlBodyCharset = "iso-8859-1";
        public static string htmlBodyContentTransferEncoding = "quoted-printable";
        public static string boundaryLevel2 = "--=_NextPart002";
        public static string attachmentContentType = "image/gif";
        public static string attachmentName = "attachment.gif";
        public static string attachmentContentTransferEncoding = "base64";
        public static string attachmentContentDisposition = "attachment";
        #region _attachmentContent
        public static string attachmentContent = @"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";
        #endregion

        public static string mimeData = BuildMessage();
        public static string content = BuildContent();

        private static string BuildContent()
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("Content-Type: multipart/alternative;");
            content.AppendLine("\tboundary=\"" + boundaryLevel2 + "\"");
            content.AppendLine();
            content.AppendLine("--" + boundaryLevel2);
            content.AppendLine("Content-Type: " + textBodyContentType + ";");
            content.AppendLine("\tcharset=\"" + textBodyCharset + "\"");
            content.AppendLine("Content-Transfer-Encoding: " + textBodyContentTransferEncoding);
            content.AppendLine();
            content.AppendLine(textBody);
            content.AppendLine("--" + boundaryLevel2);
            content.AppendLine("Content-Type: " + htmlBodyContentType);
            content.AppendLine("\tcharset=\"" + htmlBodyCharset + "\"");
            content.AppendLine("Content-Transfer-Encoding: " + htmlBodyContentTransferEncoding);
            content.AppendLine();
            content.AppendLine(htmlBody);
            content.AppendLine("--" + boundaryLevel2);
            content.AppendLine();
            content.AppendLine("--" + boundaryLevel2);

            content.AppendLine("Content-Type: " + attachmentContentType + ";");
            content.AppendLine("\tname=\"" + attachmentName + "\"");
            content.AppendLine("Content-Transfer-Encoding: " + attachmentContentTransferEncoding);
            content.AppendLine("Content-Disposition: " + attachmentContentDisposition + ";");
            content.AppendLine("\tfilename=\"" + attachmentName + "\"");
            content.AppendLine();
            content.AppendLine(attachmentContent);
            content.AppendLine();
            return content.ToString();
        }
        private static string BuildMessage()
        {
            StringBuilder mimeData = new StringBuilder();
            mimeData.AppendLine("From: " + from);
            mimeData.AppendLine("To: " + to);
            mimeData.AppendLine("Cc: " + cc);
            mimeData.AppendLine("Bcc: " + bcc);
            mimeData.AppendLine("Subject: " + subject);
            mimeData.AppendLine("Date: " + messageDate);
            mimeData.AppendLine("Message-ID: <" + messageID + ">");
            mimeData.AppendLine("MIME-Version: " + mimeVersion + "(produced by opoMail)");
            mimeData.AppendLine("Content-Type: " + contentType + ";");
            mimeData.AppendLine("\tboundary=\"" + boundaryLevel1 + "\"");
            mimeData.AppendLine("X-Priority: " + priority);
            mimeData.AppendLine();
            mimeData.AppendLine("This is a multi-part message in MIME format.");
            mimeData.AppendLine();
            mimeData.AppendLine("--" + boundaryLevel1);
            mimeData.AppendLine(BuildContent());
            mimeData.AppendLine("--" + boundaryLevel1);

            return mimeData.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents an attachment
    /// </summary>
    public class Attachment : IAttachment
    {
        private string _content;
        private Stream _contentStream;
        private string _filePath;
        private AttachmentType _type;

        /// <summary>
        /// Gets or sets the size of the attachment in bytes
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Gets or sets the Content-Type of the attachment
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the Name of the attachment
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Content-Transfer-Encoding
        /// </summary>
        public string TransferEncoding { get; set; }
        /// <summary>
        /// Gets or sets the Content-Disposition
        /// </summary>
        public ContentDisposition ContentDisposition { get; set; }

        /// <summary>
        /// Initializes a new instance of the Attachment class setting the content as a String
        /// </summary>
        /// <param name="name">Name of the attachment</param>
        /// <param name="content">A String containing the content</param>
        /// <param name="contentType">Content-Type of the attachment (e.g. "image/gif")</param>
        /// <param name="transferEncoding">Content-Transfer-Encoding (e.g. "base64")</param>
        public Attachment(string name, string content, string contentType, string transferEncoding)
        {
            Name = name;
            Size = Encoding.UTF8.GetByteCount(content);
            _content = content;
            ContentType = contentType;
            TransferEncoding = transferEncoding;
            ContentDisposition = new ContentDisposition
            {
                CreationDate = DateTime.Now,
                DispositionType = "attachment",
                FileName = name,
                Size = Size
            };
            _type = AttachmentType.String;
        }
        /// <summary>
        /// Initializes a new instance of the Attachment class setting the content as a Stream
        /// </summary>
        /// <param name="name">Name of the attachment</param>
        /// <param name="content">A Stream containing the content</param>
        /// <param name="transferEncoding">Content-Transfer-Encoding (e.g. "base64")</param>
        public Attachment(string name, Stream content, string transferEncoding)
        {
            Name = name;
            _contentStream = content;
            TransferEncoding = transferEncoding;
            ContentDisposition = new ContentDisposition
            {
                CreationDate = DateTime.Now,
                DispositionType = "attachment",
                FileName = name,
                Size = content.Length
            };
            _type = AttachmentType.Stream;
        }
        /// <summary>
        /// Initializes a new instance of the Attachment class setting the content as a file. TransferEncoding is set to "base64"
        /// </summary>
        /// <param name="filePath">Absolute path to the attachment file</param>
        public Attachment(string filePath)
        {
            new Attachment(filePath, Mime.ContentTransferEncoding.Base64);
        }
        /// <summary>
        /// Initializes a new instance of the Attachment class setting the content as a file
        /// </summary>
        /// <param name="filePath">Absolute path to the attachment file</param>
        /// <param name="transferEncoding">Content-Transfer-Encoding (e.g. "base64")</param>
        public Attachment(string filePath, string transferEncoding)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("[" + filePath + "] could not be found.");
            _filePath = filePath;
            Name = Path.GetFileName(filePath);
            FileInfo fi = new FileInfo(filePath);
            Size = fi.Length;
            ContentType = MediaType.GetMediaType(fi.Name);
            TransferEncoding = transferEncoding;
            ContentDisposition = new ContentDisposition
            {
                CreationDate = fi.CreationTime,
                FileName = fi.Name,
                Size = fi.Length
            };
            _type = AttachmentType.File;
        }

        /// <summary>
        /// Returns the name of the attachment
        /// </summary>
        /// <returns>A String representing the name of the attachment</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Gets the attachments content as a Stream
        /// </summary>
        /// <returns>A Stream containing the attachments content</returns>
        public Stream GetContentStream()
        {
            switch (_type)
            {
                case AttachmentType.String:
                    if (_content.IsNullOrEmpty())
                    {
                        throw new Exception("No content specified.");
                    }
                    return new MemoryStream(Encoding.UTF8.GetBytes(_content));
                case AttachmentType.File:
                    try
                    {
                        return new FileStream(_filePath, FileMode.Open, FileAccess.Read);
                    }
                    catch (FileNotFoundException) { }
                    break;
                case AttachmentType.Stream:
                    return _contentStream;
            }
            return null;
        }
        /// <summary>
        /// Saves the attachment to a file
        /// </summary>
        /// <param name="path">Absolute path where the file is saved. Filename is set to the name of the attachment</param>
        /// <returns>A String containing the files path</returns>
        public string SaveToFile(string path)
        {
            return SaveToFile(path, Guid.NewGuid().ToString());
        }
        /// <summary>
        /// Saves the attachment to a file
        /// </summary>
        /// <param name="path">Absolute path where the file is saved</param>
        /// <param name="fileName">Filename for the attachment file</param>
        /// <returns>A string containing the files path</returns>
        public string SaveToFile(string path, string fileName)
        {
            string savePath = Path.Combine(path, fileName);
            if (_type == AttachmentType.File)
            {
                if (File.Exists(_filePath))
                    File.Copy(_filePath, savePath);
            }
            else
            {
                if (_type == AttachmentType.String)
                {
                    _contentStream = new MemoryStream(Encoding.UTF8.GetBytes(_content));
                }
                using (Stream stream = File.Create(savePath))//new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryReader br = new BinaryReader(_contentStream))
                    {
                        using (BinaryWriter bw = new BinaryWriter(stream))
                        {
                            bw.Write(br.ReadBytes((int)stream.Length));
                            bw.Flush();
                        }
                    }
                    //byte[] buffer = new byte[4096];
                    //int readCount;
                    //while ((readCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                    //{
                    //    stream.Write(buffer, 0, buffer.Length);
                    //}
                }
            }
            return savePath;
        }
    }
}

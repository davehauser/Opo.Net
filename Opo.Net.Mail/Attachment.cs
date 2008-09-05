using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    public class Attachment : IAttachment
    {
        private string _content;
        private Stream _contentStream;
        private string _filePath;
        private AttachmentType _type;

        public long Size { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
        public string TransferEncoding { get; set; }
        public ContentDisposition ContentDisposition { get; set; }

        // string content
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
        // stream content
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
        // file content
        public Attachment(string filePath)
        {
            new Attachment(filePath, Mime.TransferEncoding.Base64);
        }
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

        public override string ToString()
        {
            return Name;
        }

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
        public string SaveToFile(string path)
        {
            return SaveToFile(path, Guid.NewGuid().ToString());
        }
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

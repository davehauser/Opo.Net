using System.IO;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents an attachment
    /// </summary>
    public interface IAttachment
    {
        /// <summary>
        /// Gets or sets the size of the attachment in bytes
        /// </summary>
        long Size { get; set; }
        /// <summary>
        /// Gets or sets the Content-Type of the attachment
        /// </summary>
        string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the Name of the attachment
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the Content-Transfer-Encoding
        /// </summary>
        string TransferEncoding { get; set; }
        /// <summary>
        /// Gets or sets the Content-Disposition
        /// </summary>
        ContentDisposition ContentDisposition { get; set; }

        /// <summary>
        /// Gets the attachments content as a Stream
        /// </summary>
        /// <returns>A Stream containing the attachments content</returns>
        Stream GetContentStream();
        /// <summary>
        /// Saves the attachment to a file
        /// </summary>
        /// <param name="path">Absolute path where the file is saved. Filename is set to the name of the attachment</param>
        /// <returns>A String containing the files path</returns>
        string SaveToFile(string path, string filename);
    }
}

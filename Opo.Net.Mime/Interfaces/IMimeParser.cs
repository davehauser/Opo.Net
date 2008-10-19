using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mime
{
    public interface IMimeParser
    {
        /// <summary>
        /// Extracts the Message-ID from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String containing the Message-ID of the MIME message</returns>
        string ParseMessageID(string mimeData);

        /// <summary>
        /// Extracts the Subject from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String containing the Subject of the MIME message</returns>
        string ParseSubject(string mimeData);

        /// <summary>
        /// Extracts the From address from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String containing the From address of the MIME message</returns>
        string ParseFrom(string mimeData);
        /// <summary>
        /// Extracts the To addresses from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the To addresses of the MIME message</returns>
        string[] ParseTo(string mimeData);
        /// <summary>
        /// Extracts the CC addresses from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the CC addresses of the MIME message</returns>
        string[] ParseCC(string mimeData);
        /// <summary>
        /// Extracts the BCC addresses from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the BCC addresses of the MIME message</returns>
        string[] ParseBCC(string mimeData);

        /// <summary>
        /// Extracts the Date from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the date of the MIME message</returns>
        string ParseDate(string mimeData);

        /// <summary>
        /// Extracts the MIME-Version from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the MIME-Version of the MIME message</returns>
        string ParseMimeVersion(string mimeData);

        /// <summary>
        /// Extracts the X-Priority from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array the X-Priority of the MIME message</returns>
        string ParsePriority(string mimeData);

        /// <summary>
        /// Extracts the Content-Type from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the Content-Type of the MIME message</returns>
        string ParseContentType(string mimeData);

        /// <summary>
        /// Extracts the charset from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the charset of the MIME message</returns>
        string ParseCharset(string mimeData);

        /// <summary>
        /// Extracts the boundary string from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the boundary string of the MIME message</returns>
        string ParseBoundary(string mimeData);

        /// <summary>
        /// Extracts the Content part from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the Content part of the MIME message</returns>
        string ParseContent(string mimeData);

        /// <summary>
        /// Extracts the value of a header from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <param name="headerName">A string containing the name of the header to parse</param>
        /// <returns>A String array containing the value of the specific header of the MIME message</returns>
        string ParseHeader(string mimeData, string headerName); 
    }
}

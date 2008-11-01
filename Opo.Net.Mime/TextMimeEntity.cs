using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.ProjectBase;

namespace Opo.Net.Mime
{
    /// <summary>
    /// MIME Entity for text content
    /// </summary>
    public class TextMimeEntity : MimeEntityBase, IMimeEntity
    {
        /// <summary>
        /// Initializes a new instance of the TextMimeEntity class
        /// </summary>
        /// <param name="mimeParser">An IMimeParser instance which is used for parsing the MIME data</param>
        /// <param name="mimeData">A string containing the MIME data for the MIME entity</param>
        public TextMimeEntity(IMimeParser mimeParser, string mimeData)
            : base(mimeParser, mimeData) { }

        /// <summary>
        /// Returns the text content
        /// </summary>
        /// <returns>A String containing the text of the TextMimeEntity</returns>
        public string GetContent()
        {
            return Content;
        }
    }
}

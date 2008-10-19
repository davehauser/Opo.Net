using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.ProjectBase;
using System.Text.RegularExpressions;

namespace Opo.Net.Mime
{
    /// <summary>
    /// MIME Entity which holds other entities (e.g. "multipart/mixed", "multipart/alternative") 
    /// </summary>
    public class MultipartMimeEntity : MimeEntityBase, IMimeEntity
    {
        private string _boundary;
        private string _header;

        /// <summary>
        /// Gets or sets the MIME data
        /// </summary>
        public override string MimeData
        {
            get
            {
                StringBuilder mimeData = new StringBuilder();
                mimeData.Append(_header);
                if (HasEntities)
                    mimeData.Append("--" + _boundary);
                foreach (var entity in Entities)
                {
                    mimeData.Append(entity.MimeData);
                    mimeData.Append("--" + _boundary);
                }
                return mimeData.ToString();
            }
            set
            {
                ParseMimeData(value);
            }
        }
        /// <summary>
        /// Gets a value indicating whether there are any items in the Entities collection
        /// </summary>
        public new bool HasEntities { get { return Entities.Count > 0; } }

        /// <summary>
        /// Initializes a new instance of the MultipartMimeEntity class
        /// </summary>
        /// <param name="mimeParser">An IMimeParser instance which is used for parsing the MIME data</param>
        /// <param name="mimeData">A string containing the MIME data for the MIME entity</param>
        public MultipartMimeEntity(IMimeParser mimeParser, string mimeData)
            : base(mimeParser, mimeData) { }
 
        /// <summary>
        /// Get the value of a MIME header
        /// </summary>
        /// <param name="headerName">The name of the header (e.g. "MIME-Version")</param>
        /// <returns>A String containing the value of the header or an empty string if the header is not present</returns>
        public new string GetHeaderValue(string headerName)
        {
            return _mimeParser.ParseHeader(_header, headerName);
        }

        /// <summary>
        /// Returns the content of the MultipartMimeEntity. This is normally MIME data which contains different parts separated by a boundary string.
        /// </summary>
        /// <returns>A String containing the content of the Entity</returns>
        public string GetContent()
        {
            return _mimeParser.ParseContent(MimeData);
        }

        /// <summary>
        /// Extracts header data from a MIME data string and creates IMimeEntity instances which are added to the Entities collection
        /// </summary>
        /// <param name="mimeData">MIME data to process</param>
        private void ParseMimeData(string mimeData)
        {
            _boundary = _mimeParser.ParseBoundary(mimeData);
            System.Diagnostics.Debug.WriteLine("Boundary: " + _boundary);
            Regex r = new Regex(@"--" + _boundary, RegexOptions.IgnoreCase);
            string[] parts = r.Split(mimeData);
            _header = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].Trim().Length > 0)
                {
                    string contentType = _mimeParser.ParseContentType(parts[i]);
                    System.Diagnostics.Debug.WriteLine("Content-Type: " + contentType);
                    IMimeEntity mimeEntity = MimeEntityFactory.GetInstance(_mimeParser, contentType);
                    System.Diagnostics.Debug.WriteLine("*** New MimeEntity: " + mimeEntity.GetType() + "\r\n");
                    mimeEntity.MimeData = parts[i];
                    System.Diagnostics.Debug.WriteLine("*** MimeEntity MimeData: " + mimeEntity.MimeData);
                    System.Diagnostics.Debug.WriteLine("");
                    Entities.Add(mimeEntity);
                }
            }
        }
    }
}

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
        /// Gets the MIME data.
        /// </summary>
        public override string GetMimeData()
        {
            StringBuilder mimeData = new StringBuilder();
            mimeData.Append(_header);
            foreach (var entity in Entities)
            {
                mimeData.AppendLine("--" + _boundary);
                mimeData.Append(entity.GetMimeData());
            }
            if (HasEntities)
                mimeData.Append("--" + _boundary);
            return mimeData.ToString();
        }
        /// <summary>
        /// Sets the MIME data. This changes the properties of the IMimeEntity
        /// </summary>
        public override void SetMimeData(string mimeData)
        {
            _boundary = _mimeParser.ParseBoundary(mimeData);
            Regex r = new Regex(@"--" + _boundary, RegexOptions.IgnoreCase);
            string[] parts = r.Split(mimeData);
            _header = parts[0];
            SetHeaders(_header);
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].Trim().Length > 0)
                {
                    string contentType = _mimeParser.ParseContentType(parts[i]);
                    IMimeEntity mimeEntity = MimeEntityFactory.GetInstance(_mimeParser, contentType);
                    mimeEntity.SetMimeData(parts[i]);
                    Entities.Add(mimeEntity);
                }
            }
        }
        /// <summary>
        /// Gets a value indicating whether there are any items in the Entities collection
        /// </summary>
        public new bool HasEntities { get { return Entities.Count > 0; } }

        /// <summary>
        /// Initializes a new instance of the MultipartMimeEntity class with empty MIME data
        /// </summary>
        /// <param name="mimeParser">An IMimeParser instance which is used for parsing the MIME data</param>
        public MultipartMimeEntity(IMimeParser mimeParser) : this(mimeParser, String.Empty) { }
        /// <summary>
        /// Initializes a new instance of the MultipartMimeEntity class
        /// </summary>
        /// <param name="mimeParser">An IMimeParser instance which is used for parsing the MIME data</param>
        /// <param name="mimeData">A string containing the MIME data for the MIME entity</param>
        public MultipartMimeEntity(IMimeParser mimeParser, string mimeData)
            : base(mimeParser, mimeData) { }

        /// <summary>
        /// Returns the content of the MultipartMimeEntity. This is normally MIME data which contains different parts separated by a boundary string.
        /// </summary>
        /// <returns>A String containing the content of the Entity</returns>
        public string GetContent()
        {
            return _mimeParser.ParseContent(this.GetMimeData());
        }
    }
}

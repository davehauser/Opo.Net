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

        /// <summary>
        /// Gets the MIME data.
        /// </summary>
        public override string GetMimeData()
        {
            StringBuilder mimeData = new StringBuilder();
            mimeData.Append(GetHeaders().TrimEnd());
            mimeData.Append(Content);
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
            string headersAndContent = String.Empty;
            Regex r;

            _boundary = _mimeParser.ParseBoundary(mimeData);
            if (!String.IsNullOrEmpty(_boundary))
            {
                r = new Regex(@"--" + _boundary, RegexOptions.IgnoreCase);
                string[] entities = r.Split(mimeData);
                headersAndContent = entities[0];

                // Entities
                for (int i = 1; i < entities.Length; i++)
                {
                    if (entities[i].Trim().Length > 0)
                    {
                        string contentType = _mimeParser.ParseContentType(entities[i]);
                        IMimeEntity mimeEntity = MimeEntityFactory.GetInstance(_mimeParser, contentType);
                        mimeEntity.SetMimeData(entities[i]);
                        Entities.Add(mimeEntity);
                    }
                }
            }

            // Headers & Content
            r = new Regex(@"(\r\n\s*){2}");
            Match m = r.Match(headersAndContent);
            if (m.Index > 1)
            {
                SetHeaders(headersAndContent.Substring(0, m.Index));
                Content = headersAndContent.Substring(m.Index);
            }
            else
            {
                SetHeaders(headersAndContent);
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mime
{
    /// <summary>
    /// Collection of IMimeEntity instances
    /// </summary>
    public class MimeEntityCollection : List<IMimeEntity>
    {
        /// <summary>
        /// Adds a new IMimeEntity to the collection. The IMimeEntity is parsed from the mimeData and its type is determined by the Content-Type that mimeData contains
        /// </summary>
        /// <param name="mimeParser">An IMimeParser instance which is used to parse the mimeData</param>
        /// <param name="mimeData">A string containing the MIME data</param>
        public void Add(IMimeParser mimeParser, string mimeData)
        {
            this.Add(MimeEntityFactory.GetInstance(mimeParser, mimeData));
            System.Diagnostics.Debug.WriteLine("MimeEntityCollection.Count: " + this.Count);
        }
    }
}

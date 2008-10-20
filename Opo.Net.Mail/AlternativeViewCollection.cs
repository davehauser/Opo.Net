using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.ProjectBase;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represent a collection of IAlternativeView
    /// </summary>
    public class AlternativeViewCollection : List<IAlternativeView>
    {
        /// <summary>
        /// Adds a new AlternativeView to the collection
        /// </summary>
        /// <param name="content">Content of the alternative view</param>
        /// <param name="contentType">Content-Type of the new alternative view</param>
        public void Add(string content, string contentType)
        {
            this.Add(new AlternativeView(content, contentType));
        }
    }
}

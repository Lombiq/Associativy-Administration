using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Associativy.Administration.Models
{
    public class GraphSettings
    {
        public int NeighboursDisplayedMaxCount { get; set; }
        public string ImplicitlyCreatableContentType { get; set; }

        private static readonly GraphSettings _default = new GraphSettings();
        public static GraphSettings Default { get { return _default; } }


        public GraphSettings()
        {
            NeighboursDisplayedMaxCount = 50;
            ImplicitlyCreatableContentType = null;
        }
    }
}
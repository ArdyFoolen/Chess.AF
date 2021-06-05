using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.Tests.Helpers
{
    public class ResourceHelper
    {
        public static string ReadEmbeddedRessource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return Using(assembly.GetManifestResourceStream(resourceName), s => ReadFromStream(s));
        }

        private static string ReadFromStream(Stream stream)
            => (stream != null) ? Using(new StreamReader(stream), s => ReadFromStream(s)) : string.Empty;
        private static string ReadFromStream(StreamReader reader)
            => (reader != null) ? reader.ReadToEnd() : string.Empty;

    }
}

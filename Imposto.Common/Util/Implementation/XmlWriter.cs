using Imposto.Common.Util.Contract;
using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace Imposto.Common.Util.Implementation
{
    public class XmlWriter<T> : IXmlWriter<T> where T : class
    {
        public bool Record(string file, T @object)
        {
            try
            {
                var writer = new XmlSerializer(@object.GetType());

                var path = Path.GetDirectoryName(ConfigurationManager.AppSettings["FolderNotasEmitidas"]);
                var stream = File.Create(FormatPath(path, file));

                writer.Serialize(stream, @object);
                stream.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string FormatPath(string path, string file) => path + "\\" + file + ".xml";
    }
}

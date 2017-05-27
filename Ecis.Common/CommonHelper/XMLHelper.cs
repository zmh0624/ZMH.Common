using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ZMH.Common.CommonHelper
{
    public class XmlHelper
    {
        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(o.GetType());

            XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Indent = true;
            //settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            //settings.IndentChars = "    ";
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                //Create our own namespaces for the output
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //Add an empty namespace and empty value
                ns.Add("", "");
                serializer.Serialize(writer, o, ns);
                writer.Close();
            }
        }

        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="xml">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string xml, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentNullException("xml");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            try
            {
                using (MemoryStream ms = new MemoryStream(encoding.GetBytes(xml)))
                {
                    using (StreamReader sr = new StreamReader(ms, encoding))
                    {
                        return (T)mySerializer.Deserialize(sr);
                    }
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        ///  实体类序列化成xml
        /// </summary>
        /// <param name="enitities">The enitities.</param>
        /// <param name="headtag">The headtag.</param>
        /// <returns></returns>
        public static string ToXml<T>(List<T> enitities, string root = "Score")
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] propinfos = null;
            foreach (T obj in enitities)
            {
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type bjtype = obj.GetType();
                    propinfos = bjtype.GetProperties();
                }

                sb.AppendLine("<" + root + ">");
                foreach (PropertyInfo propinfo in propinfos)
                {
                    sb.Append("<");
                    sb.Append(propinfo.Name);
                    sb.Append(">");
                    sb.Append(propinfo.GetValue(obj, null));
                    sb.Append("</");
                    sb.Append(propinfo.Name);
                    sb.AppendLine(">");
                }
                sb.AppendLine("</" + root + ">");
            }
            return sb.ToString();
        }

        /// <summary>
        ///  使用XML初始化实体类容器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typename">The typename.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="headtag">The headtag.</param>
        /// <returns></returns>
        public static List<T> XmlToObjList<T>(string xml, string root = "Score")
            where T : new()
        {
            List<T> list = new List<T>();
            XmlDocument doc = new XmlDocument();
            PropertyInfo[] propinfos = null;
            doc.LoadXml(xml);
            XmlNodeList nodelist = doc.SelectNodes(root);
            foreach (XmlNode node in nodelist)
            {
                T entity = new T();
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type bjtype = entity.GetType();
                    propinfos = bjtype.GetProperties();
                }
                //填充entity类的属性
                foreach (PropertyInfo propinfo in propinfos)
                {
                    XmlNode cnode = node.SelectSingleNode(propinfo.Name);
                    if (cnode == null) continue;
                    string v = cnode.InnerText;
                    if (v != null)
                        propinfo.SetValue(entity, Convert.ChangeType(v, propinfo.PropertyType), null);
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        ///  使用XML初始化实体类容器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typename">The typename.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="headtag">The headtag.</param>
        /// <returns></returns>
        public static List<T> XmlToObj<T>(string xml, string root = "Item")
            where T : new()
        {
            List<T> list = new List<T>();
            XmlDocument doc = new XmlDocument();
            PropertyInfo[] propinfos = null;
            doc.LoadXml(xml);
            XmlNodeList nodelist = doc.GetElementsByTagName(root);
            foreach (XmlNode node in nodelist)
            {
                T entity = new T();
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type bjtype = entity.GetType();
                    propinfos = bjtype.GetProperties();
                }
                //填充entity类的属性
                foreach (PropertyInfo propinfo in propinfos)
                {
                    foreach (XmlNode sNode in node.ChildNodes)
                    {
                        if (sNode == null) continue;
                        if (sNode.Name == propinfo.Name)
                        {
                            string v = sNode.InnerText;
                            if (v != null)
                            {
                                propinfo.SetValue(entity, Convert.ChangeType(v, propinfo.PropertyType), null);
                                break;
                            }
                        }
                    }
                    //XmlNode cnode = node.(propinfo.Name);
                    //if (cnode == null) continue;
                    //string v = cnode.InnerText;
                    //if (v != null)
                    //    propinfo.SetValue(entity, Convert.ChangeType(v, propinfo.PropertyType), null);
                }
                list.Add(entity);
            }
            return list;
        }
    }
}
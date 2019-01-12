using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common
{
    public class SerializeHelper
    {
        //序列化对象
        public static string Serialize(object obj)
        {
            string result = string.Empty;
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream ms=new MemoryStream())
                {
                    binaryFormatter.Serialize(ms,obj);
                    byte[] bs = ms.ToArray();
                    result = Convert.ToBase64String(bs);
                    Array.Clear(bs,0,bs.Length);
                }
                binaryFormatter = null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }
        //反序列化对象
        public static T Deserialize<T>(string str)
        {
            T t = default(T);
            if (str==string.Empty)
            {
                return t;
            }
            byte[] data = Convert.FromBase64String(str);
            t = DeserializeByBytes<T>(data);
            return t;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="data">字节数组</param>
        /// <param name="isClearData">压缩完成后，是否清除待压缩字节数组里面的内容</param>
        /// <returns>指定类型的对象</returns>
        public static T DeserializeByBytes<T>(byte[] data, bool isClearData = true)
        {
            T t = default(T);
            if (data == null)
                return t;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(data))
                {
                    t = (T)formatter.Deserialize(ms);
                }
                formatter = null;
                if (isClearData)
                    Array.Clear(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }
    }
}

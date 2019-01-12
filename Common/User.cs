using System;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace Common
{
    [Serializable]
    public class User : IIdentity, ISerializable
    {
        public string Name { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", this.Name);
        }

        /// <summary>
        /// 序列化登录人的信息
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return SerializeHelper.Serialize(this);
        }
        /// <summary>
        /// 反序列化登录人的信息
        /// </summary>
        /// <param name="userContextData"></param>
        /// <returns></returns>
        public User Desrialize(string userContextData)
        {
            User user = null;
            try
            {
                user = SerializeHelper.Deserialize<User>(userContextData);
            }
            catch
            {
                user = null;
            }
            return user;
        }
    }
}

using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取属性的Description
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetPropertyDescription(this Type type, string propertyName)
        {
            DescriptionAttribute desc = (DescriptionAttribute)type?.GetProperty(propertyName)?
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault();

            return desc?.Description;
        }

        /// <summary>获取Type的 Description 属性</summary>
        /// <param name="obj">枚举变量</param>
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
        public static string Description(this Type type)
        {
            try
            {
                return type.GetCustomAttribute<DescriptionAttribute>()?.Description;
            }
            catch
            {
                //ignore
            }
            return type.ToString();
        }
    }
}

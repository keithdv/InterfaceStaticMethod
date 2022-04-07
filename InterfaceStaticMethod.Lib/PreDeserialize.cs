using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading;

namespace InterfaceStaticMethod.Lib
{

    public interface IPreDeserialize
    {
        static abstract string Pre(string s);
    }

    public class ClassA : IPreDeserialize
    {
        public string A { get; set; }
        public static string Pre(string s)
        {
            var result = Regex.Replace(s, "\n", "");
            return result;
        }
    }

    public class ClassB : IPreDeserialize
    {
        public static string Pre(string s)
        {
            return s.Trim();
        }
    }

    public static class StaticClass
    {

        public static T Deserialize<T>(string s)
        {

            if (typeof(T).IsAssignableTo(typeof(IPreDeserialize)))
            {
                // If only!!!
                //((IPreDeserialize) T).Pre();
                // IPreDeserialize.Pre(s);
          
                var methods = typeof(T).GetMethods();

                // Isn't T guarenteed to have this method??
                var method = typeof(T).GetMethod(nameof(IPreDeserialize.Pre), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                s = (string)method.Invoke(null, new object[] { s });
            }

            return JsonConvert.DeserializeObject<T>(s);

        }
    }

}

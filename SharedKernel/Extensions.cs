using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using SharedKernel.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using SharedKernel.Data;

namespace System
{
    public static class Extensions
    {
        public static Type FindClassOf<T>(this Assembly assembly, params object[] args) where T : class
        {
            return assembly.GetTypes()
                .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .FirstOrDefault();
        }
        public static string AppendDir(this string path, string new_dir) =>
            path.EndsWith('\\') ? path + new_dir : path + "\\" + new_dir;
        public static string New(this Guid guid, int length) => Guid.NewGuid().ToString("N").Substring(0, length);
        public static bool IsNullOrEmptyGuid(this Guid? guid)
        {
            bool isNullorEmptyGuid = false;

            if (guid == null || guid == Guid.Empty)
                isNullorEmptyGuid = true;

            return isNullorEmptyGuid;
        }

        public static T Retry<T>(this Func<T> func, Func<T, bool> condition, int retryCount = 3)
        {
            for (int i = 0; i < retryCount; i++)
            {
                var x = func();
                if (condition(x)) return x;
            }

            return default(T);
        }



        public static bool NotNull(this object obj) => obj != null;
        public static bool IsNull(this object obj) => obj == null;
        public static T FromJson<T>(this T obj, string json) => JsonConvert.DeserializeObject<T>(FileManager.ReadAllText(json));
        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented,
            new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

        public static void SaveToFile(this object obj, string filename)
        {
            string filePath = Environment.CurrentDirectory + @"\\" + filename;
            FileManager.CreateFile(filePath);
            //save data to the file
            File.WriteAllText(filePath, obj.ToJson(), Encoding.UTF8);
        }


      
    }

    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure middleware checking whether database is installed
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseThis<M>(this IApplicationBuilder application)
        {
            application.UseMiddleware<M>();
        }
    }
}

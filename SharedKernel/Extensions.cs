using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using SharedKernel.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using SharedKernel.Data;

namespace System
{
    public static class Extensions
    {
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
        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented,
            new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

        public static void SaveToFile(this object obj, string filename)
        {
            string filePath = Environment.CurrentDirectory + @"\\" + filename;
            FileManager.CreateFile(filePath);
            //save data to the file
            File.WriteAllText(filePath, obj.ToJson(), Encoding.UTF8);
        }


        /// <summary>
        /// Execute commands from the SQL script against the context database
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="sql">SQL script</param>
        public static void ExecuteSqlScript(this IEmoContext context, string sql)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var sqlCommands = GetCommandsFromScript(sql);
            foreach (var command in sqlCommands)
                context.ExecuteSqlCommand(command);
        }

        private static IList<string> GetCommandsFromScript(string sql)
        {
            var commands = new List<string>();

            //origin from the Microsoft.EntityFrameworkCore.Migrations.SqlServerMigrationsSqlGenerator.Generate method
            sql = Regex.Replace(sql, @"\\\r?\n", string.Empty);
            var batches = Regex.Split(sql, @"^\s*(GO[ \t]+[0-9]+|GO)(?:\s+|$)",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);

            for (var i = 0; i < batches.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(batches[i]) ||
                    batches[i].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                    continue;

                var count = 1;
                if (i != batches.Length - 1 && batches[i + 1].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                {
                    var match = Regex.Match(batches[i + 1], "([0-9]+)");
                    if (match.Success)
                        count = int.Parse(match.Value);
                }

                var builder = new StringBuilder();
                for (var j = 0; j < count; j++)
                {
                    builder.Append(batches[i]);
                    if (i == batches.Length - 1)
                        builder.AppendLine();
                }

                commands.Add(builder.ToString());
            }

            return commands;
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

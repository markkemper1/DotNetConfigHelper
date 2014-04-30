using System;
using System.Collections.Generic;
using System.IO;
using DotNetConfigHelper.Providers;

namespace DotNetConfigHelper
{
    public class EnvConfigFileProvider : BaseSettingsProvider, ISettingsProvider
    {
        public DirectoryInfo Dir { get; set; }
        public bool WalkParentDirectories { get; set; }
        public string Filename { get; set; }

        public EnvConfigFileProvider(DirectoryInfo dir = null, bool walkParentDirectories = true, string filename = "env.config")
            : base("%([^%]*)%")
        {
            dir = dir ?? new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            if (filename == null) throw new ArgumentNullException("filename");

            if (!dir.Exists) throw new ArgumentException("The direcotry does not exist: {0}".Fmt(dir.FullName));

            Dir = dir;
            WalkParentDirectories = walkParentDirectories;
            Filename = filename;
        }

        public IDictionary<string, string> Load()
        {
            //Environment file overrides
            var configFiles = new Stack<FileInfo>();
            //Put into a list 
            ProcessDir(configFiles, Dir, Filename);

            return ProcessFiles(configFiles);
        }

        private static void ProcessDir(Stack<FileInfo> envConfigsFound, DirectoryInfo dir, string environmentFileName)
        {
            var envFile = new FileInfo(Path.Combine(dir.FullName, environmentFileName));

            if (envFile.Exists)
                envConfigsFound.Push(envFile);

            if (dir.Parent != null && dir.Parent.Exists)
                ProcessDir(envConfigsFound, dir.Parent, environmentFileName);
        }

        private static Dictionary<string, string> ProcessFiles(Stack<FileInfo> envConfigsFound)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            foreach (var item in envConfigsFound)
            {
                foreach (var line in File.ReadAllLines(item.FullName))
                {
                    var split = line.Split(new char[] { '=' }, 2);

                    if (split.Length != 2)
                        continue;

                    var key = split[0];

                    if (String.IsNullOrWhiteSpace(key)) continue;

                    results[key.Trim()] = split[1].Trim();
                }
            }

            return results;
        }
    }
}

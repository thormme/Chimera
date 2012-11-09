using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace GameConstructLibrary
{
    public static class DirectoryManager
    {

        public static string GetRoot()
        {

            string path = Directory.GetCurrentDirectory();
            string[] split = path.Split('\\');

            for (int steps = 0; steps < split.Count(); steps++)
            {
                string newPath = String.Empty;
                for (int parts = 0; parts < split.Count() - steps; parts++)
                {
                    newPath = newPath + split[parts] + "\\";
                }

                DirectoryInfo di = new DirectoryInfo(newPath);
                FileInfo[] info = di.GetFiles("*.root");

                if (info.Count() == 1)
                {
                    path = newPath;
                    break;
                }

            }
            return path;
        }

    }
}

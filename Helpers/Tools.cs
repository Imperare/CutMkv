using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CutMkv.Helpers
{
    public static class Tools
    {
        public static void OuvrirExplorerRepertoire(string repertoire)
        {
            if (Directory.Exists(repertoire))
            {
                Process.Start("explorer.exe", repertoire);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}

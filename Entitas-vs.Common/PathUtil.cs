﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Entitas_vs.Common
{
    public static class PathUtil
    {
        public const string SettingsName = "entitas-vs.cfg";

        public static string GetSettingsPath(string directory)
        {
            return $@"{directory}\{SettingsName}";
        }

        public static IEnumerable<string> ToAbsolutePaths(this IEnumerable<string> paths, string appendDirectory)
        {
            if (appendDirectory == null)
                throw new ArgumentNullException(nameof(appendDirectory));
            if (paths == null)
                throw new ArgumentNullException(nameof(paths));
            return paths.Select(path => Path.IsPathRooted(path) ? path : $@"{appendDirectory}\{path}");
        }

        public static string AbsoluteToRelativePath(string subPath, string fullPath)
        {
            if (string.IsNullOrWhiteSpace(subPath))
                throw new ArgumentException("Cannot be null or empty/whitespace", nameof(subPath));
            if (string.IsNullOrWhiteSpace(fullPath))
                throw new ArgumentException("Cannot be null or empty/whitespace", nameof(fullPath));
            return fullPath.Replace($@"{subPath}\", "");
        }

        public static IEnumerable<string> GetDeletedFileNames(this IEnumerable<string> oldFileNames, IEnumerable<string> allFileNames)
        {
            return oldFileNames.Except(allFileNames);
        }

        public static bool IsSubpathOf(this string subPath, string fullPath)
        {
            if (subPath is null)
                throw new ArgumentNullException(nameof(subPath));
            if (fullPath is null)
                throw new ArgumentNullException(nameof(fullPath));

            int result = string.CompareOrdinal($@"{subPath}\", 0, fullPath, 0, subPath.Length + 1); // + 1 because of the added \
            if (result == 0) return true;
            else return false;
        }
    }
}

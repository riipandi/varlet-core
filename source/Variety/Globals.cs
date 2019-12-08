﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Variety
{
    public static class Globals
    {
        public static string HttpServiceName { get; }
        public static string SmtpServiceName { get; }

        static Globals()
        {
            HttpServiceName = "VarletHttpd";
            SmtpServiceName = "VarletMailhog";
        }

        public static string AppVersion
        {
            get {
                var asm = Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return $"{fvi.ProductMajorPart}.{fvi.ProductMinorPart}.{fvi.ProductBuildPart}";
            }
        }

        public static string AppBuildNumber
        {
            get {
                var asm = Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return $"{fvi.ProductPrivatePart}";
            }
        }

        public static string AppConfigFile
        {
            get {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return path + @"\varlet.json";
            }
        }

        public static string WwwDirectory
        {
            get { return Common.GetAppPath() + @"\www"; }
        }
    }
}

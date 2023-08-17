using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Moddding
{
    public static class Directorys
    {
        public const string companyName = "TValle Games";
        public const string productName = "Some Modeling Agency";
        public const string modsFolderName = "Mods";
        public const string clothingFolderName = "Clothing";
        public const string scriptingFolderName = "Scripts";

        public static readonly string clothingModsPath;
        public static readonly string scriptingModsPath;

        public static readonly string clothingModsTypePath;

        static readonly char[] m_invalid;
        static Directorys()
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            clothingModsPath = Path.Combine(myDocuments, companyName, productName, modsFolderName, clothingFolderName);
            scriptingModsPath = Path.Combine(myDocuments, companyName, productName, modsFolderName, scriptingFolderName);

            clothingModsTypePath = string.Format("{{{0}.{1}}}", typeof(Directorys).FullName, nameof(clothingModsPath));

            m_invalid = Path.GetInvalidPathChars();
        }

        public static string RemoveInvalid(string srt)
        {
            return srt.Trim(m_invalid);
        }

    }
}

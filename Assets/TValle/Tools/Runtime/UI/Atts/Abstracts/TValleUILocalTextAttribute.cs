using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.UI
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public abstract class TValleUILocalTextAttribute : TValleUIAttribute
    {
        public TValleUILocalTextAttribute() { }
        public TValleUILocalTextAttribute(string text, Language localizationID)
            : this(text)
        {
            this.localizationID = localizationID;
        }
        public TValleUILocalTextAttribute(string text)
        {
            if(text == null)
                throw new ArgumentNullException("text", "text null reference.");
            this.text = text;
        }


        public Language localizationID { get; set; } = Language.en;
        public string text { get; set; }

        static Dictionary<ValueTuple<Type, Type, int>, Dictionary</*Local*/Language, /*text*/(string, string)>> m_localizedStrings = new Dictionary<ValueTuple<Type, Type, int>, Dictionary<Language, (string, string)>>();


        public static string Localizado<Tenum, TLocal>(Tenum valor, Language localizacion)
            where Tenum : Enum, IConvertible
            where TLocal : TValleUILocalTextAttribute
        {
            var enumType = typeof(Tenum);
            var atributeType = typeof(TLocal);
            InitLocalStrings(enumType, valor, out Dictionary<Language, (string, string)> dicc, atributeType);
            return GetLocalizado(enumType, valor, dicc, localizacion,false);
        }
        public static string Localizado<Tenum>(Tenum valor, Language localizacion)
            where Tenum : Enum, IConvertible
        {
            var enumType = typeof(Tenum);
            InitLocalStrings(enumType, valor, out Dictionary<Language, (string, string)> dicc);
            return GetLocalizado(enumType, valor, dicc, localizacion, false);
        }
        public static string Localizado(Type enumType, object enumValue, Language localizacion)
        {
            InitLocalStrings(enumType, enumValue, out Dictionary<Language, (string, string)> dicc);
            return GetLocalizado(enumType, enumValue, dicc, localizacion, false);
        }
        public static string Localizado(Type enumType, Type localType, object enumValue, Language localizacion)
        {
            InitLocalStrings(enumType, enumValue, out Dictionary<Language, (string, string)> dicc, localType);
            return GetLocalizado(enumType, enumValue, dicc, localizacion, false);
        }
        public static string LocalizadoFirstCharToUpper<Tenum>(Tenum valor, Language localizacion)
         where Tenum : Enum, IConvertible
        {
            var enumType = typeof(Tenum);
            InitLocalStrings(enumType, valor, out Dictionary<Language, (string, string)> dicc);
            return GetLocalizado(enumType, valor, dicc, localizacion, true);
        }




        static string GetLocalizado(Type enumType, object enumValue, Dictionary<Language, (string, string)> dicc, Language localizacion, bool upperFirst)
        {
            (string, string) localizado;
            if(dicc.TryGetValue(localizacion, out localizado))
                return upperFirst ? localizado.Item1 : localizado.Item2;
            else if(dicc.TryGetValue(Language.en, out localizado))
                return upperFirst ? localizado.Item1 : localizado.Item2;
            else
                return Enum.GetName(enumType, enumValue);
        }
        static void InitLocalStrings(Type enumType, object enumValue, out Dictionary<Language, (string, string)> dicc, Type localType = null)
        {
            var enumValor = Convert.ToInt32(enumValue);
            if(localType == null)
                localType = typeof(TValleUILocalTextAttribute);
            var key1 = new ValueTuple<Type, Type, int>(enumType, localType, enumValor);

            if(!m_localizedStrings.TryGetValue(key1, out dicc))
            {
                dicc = new Dictionary<Language, (string, string)>();
                m_localizedStrings.Add(key1, dicc);
                InitLocal(enumType, localType, enumValue, dicc);
            }
        }
        static void InitLocal(Type enumType, Type localType, object enumValue, Dictionary<Language, (string, string)> dicc)
        {
            var enumName = Enum.GetName(enumType, enumValue);
            var atts = enumType.GetField(enumName).GetCustomAttributes(false).OfType<TValleUILocalTextAttribute>().ToArray();
            if(localType != typeof(TValleUILocalTextAttribute))
                atts = atts.Where(a => a.GetType() == localType).ToArray();
            if(atts.Length > 0)
            {
                foreach(var item in atts)
                {
                    if(dicc.ContainsKey(item.localizationID))
                        continue;
                    dicc.Add(item.localizationID, (item.text, FirstCharToUpper(item.text)));
                }
            }
            else
            {
                dicc.Add(Language.en, (enumName, FirstCharToUpper(enumName)));
            }
        }
        public static string FirstCharToUpper(string input)
        {
            switch(input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

    }
}

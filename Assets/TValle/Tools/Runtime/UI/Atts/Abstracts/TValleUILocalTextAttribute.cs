using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.UI
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public abstract class TValleUILocalTextAttribute: TValleUIAttribute
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

        static Dictionary<ValueTuple<Type, Type, int>, Dictionary</*Local*/Language, /*text*/string>> m_localizedStrings = new Dictionary<ValueTuple<Type, Type, int>, Dictionary<Language, string>>();

        public static string Localizado<Tenum, TLocal>(Tenum valor, Language localizacion)
            where Tenum : Enum, IConvertible
            where TLocal : TValleUILocalTextAttribute
        {
            var enumType = typeof(Tenum);
            var atributeType = typeof(TLocal);
            InitLocalStrings(enumType, valor, out Dictionary<Language, string> dicc, atributeType);
            return GetLocalizado(enumType, valor, dicc, localizacion);
        }
        public static string Localizado<Tenum>(Tenum valor, Language localizacion)
            where Tenum : Enum, IConvertible
        {
            var enumType = typeof(Tenum);
            InitLocalStrings(enumType, valor, out Dictionary<Language, string> dicc);
            return GetLocalizado(enumType, valor, dicc, localizacion);
        }
        public static string Localizado(Type enumType, object enumValue, Language localizacion)
        {
            InitLocalStrings(enumType, enumValue, out Dictionary<Language, string> dicc);
            return GetLocalizado(enumType, enumValue, dicc, localizacion);
        }
        public static string Localizado(Type enumType, Type localType, object enumValue, Language localizacion)
        {
            InitLocalStrings(enumType, enumValue, out Dictionary<Language, string> dicc, localType);
            return GetLocalizado(enumType, enumValue, dicc, localizacion);
        }




        static string GetLocalizado(Type enumType, object enumValue, Dictionary<Language, string> dicc, Language localizacion)
        {
            string localizado;
            if(dicc.TryGetValue(localizacion, out localizado))
                return localizado;
            else if(dicc.TryGetValue(Language.en, out localizado))
                return localizado;
            else
                return Enum.GetName(enumType, enumValue);
        }
        static void InitLocalStrings(Type enumType, object enumValue, out Dictionary<Language, string> dicc, Type localType = null)
        {
            var enumValor = Convert.ToInt32(enumValue);
            if(localType == null)
                localType = typeof(TValleUILocalTextAttribute);
            var key1 = new ValueTuple<Type, Type, int>(enumType, localType, enumValor);

            if(!m_localizedStrings.TryGetValue(key1, out dicc))
            {
                dicc = new Dictionary<Language, string>();
                m_localizedStrings.Add(key1, dicc);
                InitLocal(enumType, localType, enumValue, dicc);
            }
        }
        static void InitLocal(Type enumType, Type localType, object enumValue, Dictionary<Language, string> dicc)
        {
            var enumName = Enum.GetName(enumType, enumValue);
            var atts = enumType.GetField(enumName).GetCustomAttributes(false).OfType<TValleUILocalTextAttribute>();
            if(localType != typeof(TValleUILocalTextAttribute))
                atts = atts.Where(a => a.GetType() == localType);
            foreach(var item in atts)
            {
                if(dicc.ContainsKey(item.localizationID))
                    continue;
                dicc.Add(item.localizationID, item.text);
            }
        }

    }
}

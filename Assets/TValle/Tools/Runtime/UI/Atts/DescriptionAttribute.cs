using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.UI
{
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Method, AllowMultiple = true)]
    public class DescriptionAttribute : TValleUILocalTextAttribute
    {
        public DescriptionAttribute() { }
        public DescriptionAttribute(string text) : base(text)
        {
        }

        public DescriptionAttribute(string text, Language localizationID) : base(text, localizationID)
        {
        }

    }
}

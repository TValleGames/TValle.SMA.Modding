using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.UI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class LabelAttribute : TValleUILocalTextAttribute
    {
        public LabelAttribute() { }
        public LabelAttribute(string text) : base(text)
        {
        }

        public LabelAttribute(string text, Language localizationID) : base(text, localizationID)
        {
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.UI
{
    public abstract class TValleUIAttribute : Attribute
    {
        private readonly int order_;
        public TValleUIAttribute([CallerLineNumber] int order = 0)
        {
            order_ = order;
        }

        public int Order { get { return m_orderOverriden.HasValue ? m_orderOverriden.Value : order_; } }

        int? m_orderOverriden;
        public int orderOverriden { get { return m_orderOverriden.GetValueOrDefault(); } set { m_orderOverriden = value; } }

    }
}

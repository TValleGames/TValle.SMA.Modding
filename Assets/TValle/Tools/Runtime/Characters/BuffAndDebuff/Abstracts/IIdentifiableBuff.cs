using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public interface IIdentifiableBuff
    {
        ITuple id { get; }
        string stringId { get; }
    }
    public interface IIdentifiableBuff<T_ID> : IIdentifiableBuff
        where T_ID : struct, ITuple
    {
        T_ID valueId { get; }
    }
}

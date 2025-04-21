using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public interface IFloatValuableBuff : IValuableBuff<float>
    {
        void InverseValue();

    }
}

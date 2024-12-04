using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public interface IStackableBuff<T>
    {

        bool IsStackableWith(ref T Other);
        T StackToNew(ref T Other);
        void StackToSelf(ref T Other);
    }
}

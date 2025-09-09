using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public interface IStackableBuff<T> : IStackableBuff
    {
        bool IsStackableWith(ref T Other);
        //T StackToNew(ref T Other);
        void StackToSelf(ref T Other);
    }
    public interface IStackableBuff
    {
        bool IsStackableWith(object Other);
        void StackToSelf(object Other);
    }

}

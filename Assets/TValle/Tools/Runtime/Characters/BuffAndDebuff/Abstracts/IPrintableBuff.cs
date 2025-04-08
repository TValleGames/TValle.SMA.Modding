using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.BuffAndDebuff
{
    public interface IPrintableBuff
    {
        string DebugPrint();
        string RichPrint(Func<string, string> characterNameGetter, Language language);
    }
    public static class IPrintableBuffEXT
    {
        public static char GetOperationSymbol(this Operation op, float value)
        {
            switch(op)
            {
                case Operation.None:
                    return 'e';
                case Operation.add:
                    return value < 0 ? '-' : '+';
                case Operation.mult:
                    return '×';
                default:
                    throw new ArgumentOutOfRangeException(op.ToString());
            }
        }
        public static char GetOperationSymbol(this AddOperation op, float value)
        {
            switch(op)
            {
                case AddOperation.None:
                    return 'e';
                case AddOperation.add:
                    return value < 0 ? '-' : '+';
                default:
                    throw new ArgumentOutOfRangeException(op.ToString());
            }
        }
        public static char GetOperationSymbol(this ProductOperation op, float value)
        {
            switch(op)
            {
                case ProductOperation.None:
                    return 'e';
                case ProductOperation.mult:
                    return '×';
                default:
                    throw new ArgumentOutOfRangeException(op.ToString());
            }
        }
    }
}

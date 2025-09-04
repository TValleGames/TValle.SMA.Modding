using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Clothing
{
    public interface ITValleOutfit
    {
        string name { get; }
        IReadOnlyList<ITVallePiecesOfClothing> pieces { get; }
    }
    public interface ITVallePiecesOfClothing
    {
        string ID { get; }
        IReadOnlyList<ITValleClothingMaterial> slotMaterial { get; }
    }
    public interface ITValleClothingMaterial
    {
        string ID { get; }
        int slotIndex { get; }
        Color color { get; }
    }
}
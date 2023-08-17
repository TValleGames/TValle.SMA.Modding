

using System.Collections.Generic;
using UnityEngine;

public interface ICustomClothingItemScript
{
    /// <summary>
    /// called when this Clothing Item is initiated
    /// </summary>
    /// <param name="yourClothingItemInstance"></param>
    /// <param name="yourClothingItemCustomArmatureInstance">optional, only if you declared in the map</param>
    void OnInit(GameObject yourClothingItemInstance, GameObject yourClothingItemCustomArmatureInstance);
    /// <summary>
    /// called when this Clothing Item has custom colliders and they are instantiated
    /// </summary>
    /// <param name="yourClothingItemInstantiatedColliders">optional, only if you declared them in the map</param>
    void OnCollidersInit(IReadOnlyList<GameObject> yourClothingItemInstantiatedColliders);
    /// <summary>
    /// called just after materials are added
    /// </summary>
    void OnMaterialsAdded();

    /// <summary>
    /// called just after this Clothing Item is added to an avatar
    /// </summary>
    void OnAdded();

    /// <summary>
    /// called just after this Clothing Item is worn
    /// <para>Warning: maybe call twice</para>
    /// </summary>
    void OnShown();
    /// <summary>
    /// called just after this item of clothing is undressed and hidden
    
    /// </summary>
    void OnHidden();


    /// <summary>
    /// called just after this Clothing Item is removed from an avatar
    /// </summary>
    void OnRemoved();
}


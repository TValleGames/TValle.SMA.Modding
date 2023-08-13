using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// REMEMBER: in unity the file of this cript name MUST be the same name as this type name 
///<para>ex: this type is CustomClothingItemScriptExample, this file in unity must be name the same name CustomClothingItemScriptExample</para>
/// </summary>
public class CustomClothingItemScriptExample : MonoBehaviour, ICustomClothingItemScript
{
    [SerializeField]
    bool m_equiped;
    [SerializeField]
    bool m_shown;


    SkinnedMeshRenderer m_renderer;
    Material m_firstMaterial;

    // Awake is just after this Behaviour instance is created
    private void Awake()
    {

    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while(!m_equiped || !m_shown)//wait till this item is equiped and shown
            yield return null;//if not, try again the next frame


        //this item is squipen and shown, so will get the frist material
        m_renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_firstMaterial = m_renderer?.sharedMaterials.FirstOrDefault();
    }
    // Update is called once per frame
    void Update()
    {
        if(m_equiped && m_shown && m_firstMaterial != null)//we'll change the main color to a Random color
            m_firstMaterial.SetColor("_BaseColor", UnityEngine.Random.ColorHSV());
    }
    // OnDestroy is called once just before this Behaviour instance is destroyed
    private void OnDestroy()
    {
        //clear anything you have created, here
    }


    void ICustomClothingItemScript.OnInit(GameObject yourClothingItemInstance, GameObject yourClothingItemCustomArmatureInstance)
    {
        
    }
    void ICustomClothingItemScript.OnCollidersInit(IReadOnlyList<GameObject> yourClothingItemInstantiatedColliders)
    {
        
    }

    void ICustomClothingItemScript.OnAdded()
    {
        m_equiped = true;
    }

    void ICustomClothingItemScript.OnShown()
    {
        m_shown = true;
    }

    void ICustomClothingItemScript.OnHidden()
    {
        m_shown = false;
    }

    void ICustomClothingItemScript.OnRemoved()
    {
        m_equiped = false;
    }

    
}

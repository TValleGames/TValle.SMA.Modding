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
    [SerializeField]
    bool m_materialsAdded;
    [SerializeField]
    SkinnedMeshRenderer m_renderer;
    [SerializeField]
    Material m_firstMaterial;//


    [SerializeField]
    float m_coolDown;

    // Awake is just after this Behaviour instance is created
    private void Awake()
    {

    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while(!m_equiped || !m_shown || !m_materialsAdded)//wait till this item is equiped and shown and the new materials are added
            yield return null;//if not, try again the next frame


        //this item is squipen and shown, so will get the frist material
        m_renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_firstMaterial = m_renderer?.sharedMaterials.FirstOrDefault();
    }
    // Update is called once per frame
    void Update()
    {
        m_coolDown = m_coolDown - Time.deltaTime;
        if(m_coolDown < 0)
        {
            if(m_equiped && m_shown && m_materialsAdded && m_firstMaterial != null)//we'll change the main color to a Random color
            {
                m_firstMaterial.SetColor("_BaseColor", UnityEngine.Random.ColorHSV());
                Debug.Log("Im a script changin the color of this material: " + m_firstMaterial.name + " every second, its not a bug ¬¬");
            }
            m_coolDown = 1f;
        }

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
    void ICustomClothingItemScript.OnMaterialsAdded()
    {
        m_materialsAdded = true;
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

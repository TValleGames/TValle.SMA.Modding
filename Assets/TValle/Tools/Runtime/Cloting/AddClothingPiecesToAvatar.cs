using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Cloting
{
    public class AddClothingPiecesToAvatar : MonoBehaviour
    {
        [SerializeField]
        Animator m_avatar;
        [SerializeField]
        SkinnedMeshRenderer[] m_clothingAssets;

        SkinnedMeshRenderer[] m_clothingInstances;

        private void OnEnable()
        {
            if(m_avatar == null || m_clothingAssets == null || m_clothingAssets.Length == 0)
                return;

            var avatarSkins = m_avatar.GetComponentsInChildren<SkinnedMeshRenderer>();
            var avatarBones = avatarSkins.SelectMany(s => s.bones).Distinct().ToDictionary(b => b.name);

            m_clothingInstances = m_clothingAssets.Select(a => Instantiate(a, m_avatar.transform.position, m_avatar.transform.rotation, m_avatar.transform)).ToArray();

            foreach(var clothing in m_clothingInstances)
            {
                if(clothing == null)
                    continue;
                var clothingBones = clothing.bones;
                for(int i = 0; i < clothingBones.Length; i++)
                {
                    var clothingBone = clothingBones[i];
                    if(clothingBone == null || !avatarBones.TryGetValue(clothingBone.name, out Transform avatarBone))
                        continue;
                    clothingBones[i] = avatarBone;
                }
                clothing.bones = clothingBones;
            }
        }
        private void OnDisable()
        {
            if(m_clothingInstances == null || m_clothingInstances.Length == 0)
                return;
            foreach(var clothing in m_clothingInstances)
                if(clothing != null)
                    Destroy(clothing);
            m_clothingInstances = null;
        }

    }
}

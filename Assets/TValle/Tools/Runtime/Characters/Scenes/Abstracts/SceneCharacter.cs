using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Characters.Scenes
{
    public abstract class SceneCharacter : MonoBehaviour
    {
        public abstract Guid ID { get; }
        public abstract string stringID { get; }
        public abstract bool isLoaded { get; }
        public abstract string fullName { get; }
        public abstract void Teleport(Vector3 position, Quaternion rotation);
    }

    public class SceneCharacterWrapper
    {
        public SceneCharacterWrapper(SceneCharacter Character)
        {
            if(Character == null)
                throw new ArgumentNullException("Character", "Character null reference.");

            m_reference = new WeakReference<SceneCharacter>(Character);
        }
        WeakReference<SceneCharacter> m_reference;

        public SceneCharacter sceneCharacter => m_reference.TryGetTarget(out SceneCharacter character) ? character : null;
    }

}

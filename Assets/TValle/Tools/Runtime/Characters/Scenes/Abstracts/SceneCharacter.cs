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
        public abstract bool isLoaded { get; }
        public abstract string fullName { get; }
        public abstract void Teleport(Vector3 position, Quaternion rotation);
    }
}

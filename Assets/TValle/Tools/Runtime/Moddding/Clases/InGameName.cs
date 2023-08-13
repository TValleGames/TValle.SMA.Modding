using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Moddding
{
    [Serializable]
    public class InGameName
    {
        [Tooltip("Right now, the game is only compatible with English.")]
        public Language language = Language.en;
        [Tooltip("How this item will be called in the game by the player, UI, or NPCs.")]
        public string name;
        [Tooltip("Whether it's a pair of gloves, shoes, glasses, or just a ring or a neckless")]
        public bool isPlural;

        public enum Language
        {
            None = 0,
            en,
        }
    }
}

using Assets.TValle.Tools.Runtime.Moddding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.SMA.Moddding.Jobs.Maps
{
    [CreateAssetMenu(fileName = "SMAJobObjectivesTextForLevelMap", menuName = "TValle/SMA/Modding/Jobs/SMAJobObjectivesTextForLevelMap")]
    public class SMAJobObjectivesTextForLevelMap : ScriptableObject
    {
        [Tooltip("You can get this data in-game with the id. the logic for each Objective must be set in-Game")]
        public List<ObjectiveText> text = new List<ObjectiveText>();



        [Serializable]
        public class ObjectiveText
        {
            [Tooltip("Ex: tvalle.photoshoot.lvl1.assPicture")]
            public string id;
            [Tooltip("How this Objective will be shown in the game in the UI. set 'name' field for Ex: 'Take a Phooto of her glutes', set 'desciption' field for Ex: 'press NUM 3 to show the camera and take a photo of her lower back' ")]
            public List<InGameObjectiveText> inGameDescription;
        }
    }
}

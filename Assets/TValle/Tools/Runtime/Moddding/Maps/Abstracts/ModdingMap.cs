using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Moddding
{
    public abstract class ModdingMap : ScriptableObject
    {
        public string displayId => m_displayId;
        public string id => m_id;

        [Space]
        [Header("--- Basic Info -----------------------------------------------------------------------")]
        [Space]
        [JustToReadUI]
        [SerializeField]
        string m_displayId;
        [JustToReadUI]
        [SerializeField]
        string m_id;


        [Tooltip("the name of your company, or just your artistic name.")]
        public string organization;
        [Tooltip("It's clothes, or makeup, or shapes...")]
        public string category;
        [Tooltip("full name with details")]
        public string fullName;
        [Tooltip("How this item will be called in the game by the player, UI, or NPCs.")]
        public List<InGameName> inGameNames;
        public string version;

        [Tooltip("When this mod is used in the game, notifications will be displayed with the name of the author and the link.")]
        public bool displayAutorsOnUsed;
        public List<Autor> autors = new List<Autor>();




        protected virtual void OnValidate()
        {
            TryInitID();
            if((inGameNames == null || inGameNames.Count == 0) && !string.IsNullOrWhiteSpace(fullName)&& fullName.Trim().Any(char.IsWhiteSpace))
            {
                if(inGameNames == null)
                    inGameNames = new List<InGameName>();
                if(inGameNames.Count == 0)
                    inGameNames.Add(new InGameName()
                    {
                        language = InGameName.Language.en,
                        name = fullName.Trim().Split(' ').FirstOrDefault()
                    });
            }
        }
        public bool TryInitID()
        {
            return TryGenerateID(out m_id, out m_displayId);
        }
        protected virtual bool TryGenerateID(out string ID, out string displayID)
        {
            displayID = string.Empty;
            ID = string.Empty;
            if(string.IsNullOrWhiteSpace(organization))
                return false;
            if(string.IsNullOrWhiteSpace(category))
                return false;
            if(string.IsNullOrWhiteSpace(fullName))
                return false;

            displayID = organization + "." +
                category + "." +
                fullName;

            ID = RemoveSpecialCharacters(displayID).Trim().ToLower();

            return true;
        }
        static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach(char c in str)
            {

                if((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}

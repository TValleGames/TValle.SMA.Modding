using Assets.TValle.Tools.Runtime.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.TValle.Tools.Runtime.Characters.Atts.Emotions
{
    public enum SensitiveBodyPart
    {
        All = -1,
        None = 0,

        head,
        temples,
        forehead,
        nose,
        cheeks,
        eyes,
        eyeballs,
        eyebrows,
        jaw,
        lips,
        tongue,
        ears,



        neck,
        shoulders,
        armpits,
        arms,
        forearms,
        hands,


        chest,
        breasts,
        nipples,


        back,
        abdomen,
        waist,
        hips,
        belly,
        navel,
        coccyx,

        buttocks,
        crotch,
        [Label("vaginal Lips Or Balls", Language.en)]
        vaginalLipsOrBalls, 
        [Label("clitoris Or Penis", Language.en)]
        clitorisOrPenis,
        perineum,


        legs,
        knees,
        calf,
        feet,


        throat,
        [Label("back of the Throat", Language.en)]
        throatBottom,//depth damage
        [Label("tonsils", Language.en)]
        throatWalls,//stretch damage

        [Label("vagina", Language.en)]
        vag,
        [Label("fornix", Language.en)]
        vagBottom,//depth damage
        [Label("vagina", Language.en)]
        vagWalls,//stretch damage
        
        anus,
        [Label("guts", Language.en)]
        anusBottom,//depth damage
        [Label("rectum", Language.en)]
        anusWalls,//stretch damage
    }
    public enum SensitiveFemaleHoleType
    {
        None = 0,

        hole,
        walls,
        bottom
    }
    public enum SensitiveFemaleHole
    {
        None = 0,

        throat = SensitiveBodyPart.throat,
        [Label("vagina", Language.en)]
        vag = SensitiveBodyPart.vag,
        anus = SensitiveBodyPart.anus,
    }
    public enum SensitiveFemaleHoleWalls
    {
        None = 0,
        [Label("tonsils", Language.en)]
        throatWalls = SensitiveBodyPart.throatWalls,
        [Label("vagina", Language.en)]
        vagWalls = SensitiveBodyPart.vagWalls,
        [Label("rectum", Language.en)]
        anusWalls = SensitiveBodyPart.anusWalls,
    }
    public enum SensitiveFemaleHoleBottom
    {
        None = 0,
        [Label("back of the Throat", Language.en)]
        throatBottom = SensitiveBodyPart.throatBottom,
        [Label("fornix", Language.en)]
        vagBottom = SensitiveBodyPart.vagBottom,
        [Label("guts", Language.en)]
        anusBottom = SensitiveBodyPart.anusBottom,
    }

    public static class SensitiveBodyPartHelper
    {
        static SensitiveBodyPartHelper()
        {
            {
                var p = new List<SensitiveBodyPart>();
                p.Add(SensitiveBodyPart.jaw);
                p.Add(SensitiveBodyPart.nose);
                p.Add(SensitiveBodyPart.cheeks);
                p.Add(SensitiveBodyPart.eyebrows);
                p.Add(SensitiveBodyPart.temples);
                p.Add(SensitiveBodyPart.forehead);
                p.Add(SensitiveBodyPart.ears);

                p.Add(SensitiveBodyPart.lips);
                p.Add(SensitiveBodyPart.tongue);
                p.Add(SensitiveBodyPart.eyeballs);
                p.Add(SensitiveBodyPart.eyes);
                p.Add(SensitiveBodyPart.head);

                p.Add(SensitiveBodyPart.throat);
                p.Add(SensitiveBodyPart.throatBottom);
                p.Add(SensitiveBodyPart.throatWalls);

                facialParts = p;
            }
            {
                var p = new List<SensitiveBodyPart>();
                p.Add(SensitiveBodyPart.breasts);
                p.Add(SensitiveBodyPart.nipples);
                breastParts = p;
            }
            {
                var p = new List<SensitiveBodyPart>();
                p.Add(SensitiveBodyPart.coccyx);
                p.Add(SensitiveBodyPart.buttocks);
                p.Add(SensitiveBodyPart.perineum);

                p.Add(SensitiveBodyPart.anus);
                p.Add(SensitiveBodyPart.anusBottom);
                p.Add(SensitiveBodyPart.anusWalls);

                assParts = p;
            }
            {
                var p = new List<SensitiveBodyPart>();
                p.Add(SensitiveBodyPart.legs);
                p.Add(SensitiveBodyPart.crotch);
                p.Add(SensitiveBodyPart.clitorisOrPenis);
                p.Add(SensitiveBodyPart.vaginalLipsOrBalls);

                p.Add(SensitiveBodyPart.vag);
                p.Add(SensitiveBodyPart.vagBottom);
                p.Add(SensitiveBodyPart.vagWalls);

                crotchParts = p;
            }
        }

        public static readonly IReadOnlyList<SensitiveBodyPart> facialParts;
        public static readonly IReadOnlyList<SensitiveBodyPart> assParts;
        public static readonly IReadOnlyList<SensitiveBodyPart> crotchParts;
        public static readonly IReadOnlyList<SensitiveBodyPart> breastParts;



        public static bool CanBePenetrated(this SensitiveBodyPart parte)
        {
            switch(parte)
            {
                case SensitiveBodyPart.vag:
                case SensitiveBodyPart.vagBottom:
                case SensitiveBodyPart.vagWalls:

                case SensitiveBodyPart.anus:
                case SensitiveBodyPart.anusBottom:
                case SensitiveBodyPart.anusWalls:

                case SensitiveBodyPart.throat:
                case SensitiveBodyPart.throatBottom:
                case SensitiveBodyPart.throatWalls:
                    return true;
                default:
                    return false;
            }
        }
    }
}

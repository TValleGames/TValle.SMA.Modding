using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.MeshBindPosesCorrector.Maps
{
    [CreateAssetMenu(fileName = "MapOfFemaleAvatarBindPoses", menuName = "TValle/Maps/CorrectBindPoses/Map Of Female Avatar Bind Poses")]
    public class MapOfFemaleAvatarBindPoses : ScriptableObject
    {
        //just to remove the anoying alert
        internal string msg => m_msg;

        [SerializeField]
        [JustToReadUI]
        string m_msg = "DO NOT modify this data";

        [HideInInspector]
        [SerializeField]
        [JustToReadUI]
        string[] m_names;
        [HideInInspector]
        [SerializeField]
        [JustToReadUI]
        Matrix[] m_poses;

        internal void SetData(Dictionary<string, (Transform, Matrix4x4)> data)
        {
            m_names = data.Keys.ToArray();
            m_poses = data.Values.Select(par => (Matrix)par.Item2).ToArray();
        }
        internal IReadOnlyDictionary<string, Matrix4x4> GetData()
        {
            return m_names.Select((n, i) => (n, m_poses[i])).ToDictionary(par => par.n, par => (Matrix4x4)par.Item2);
        }

        [Serializable]
        public struct Matrix
        {
            public float m00;
            public float m33;
            public float m23;
            public float m13;
            public float m03;
            public float m32;
            public float m22;
            public float m02;
            public float m12;
            public float m21;
            public float m11;
            public float m01;
            public float m30;
            public float m20;
            public float m10;
            public float m31;

            public static explicit operator Matrix(Matrix4x4 m)
            {
                Matrix matrix = new Matrix();


                matrix.m00 = m.m00;
                matrix.m33 = m.m33;
                matrix.m23 = m.m23;
                matrix.m13 = m.m13;
                matrix.m03 = m.m03;
                matrix.m32 = m.m32;
                matrix.m22 = m.m22;
                matrix.m02 = m.m02;
                matrix.m12 = m.m12;
                matrix.m21 = m.m21;
                matrix.m11 = m.m11;
                matrix.m01 = m.m01;
                matrix.m30 = m.m30;
                matrix.m20 = m.m20;
                matrix.m10 = m.m10;
                matrix.m31 = m.m31;

                return matrix;
            }
            public static explicit operator Matrix4x4(Matrix m)
            {
                Matrix4x4 matrix = new Matrix4x4();


                matrix.m00 = m.m00;
                matrix.m33 = m.m33;
                matrix.m23 = m.m23;
                matrix.m13 = m.m13;
                matrix.m03 = m.m03;
                matrix.m32 = m.m32;
                matrix.m22 = m.m22;
                matrix.m02 = m.m02;
                matrix.m12 = m.m12;
                matrix.m21 = m.m21;
                matrix.m11 = m.m11;
                matrix.m01 = m.m01;
                matrix.m30 = m.m30;
                matrix.m20 = m.m20;
                matrix.m10 = m.m10;
                matrix.m31 = m.m31;

                return matrix;
            }
        }

    }
}

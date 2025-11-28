using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.MeshNormalImporter
{
    public class MeshCustomNormals : MonoBehaviour, IMeshCustomNormalsImporter
    {
        [SerializeField]
        [HideInInspector]
        float3[] m_calculedNormalsFromImporter;
        [SerializeField]
        [HideInInspector]
        float3[] m_customNormalsFromImporter;

        NativeArray<float3> m_calculedNormals;
        NativeArray<float3> m_customNormals;

        public NativeArray<float3>.ReadOnly calculedNormals => m_calculedNormals.AsReadOnly();
        public NativeArray<float3>.ReadOnly customNormals => m_customNormals.AsReadOnly();




        /// <summary>
        /// calculedNormals[i] + customNormals[i] = importedNormals[i];  to calculate mesh normals the custom normals need to be calculed and then added every time the mesh normals are recaculated
        /// </summary>
        void IMeshCustomNormalsImporter.Import(NativeArray<float3>.ReadOnly CalculedNormals, NativeArray<float3>.ReadOnly CustomNormals)
        {
            m_calculedNormalsFromImporter = CalculedNormals.ToArray();
            m_customNormalsFromImporter = CustomNormals.ToArray();
        }


        public void Awake()
        {
            if(!m_calculedNormals.IsCreated)
                m_calculedNormals = new NativeArray<float3>(m_calculedNormalsFromImporter, Allocator.Persistent);
            if(!m_customNormals.IsCreated)
                m_customNormals = new NativeArray<float3>(m_customNormalsFromImporter, Allocator.Persistent);

        }
        private void OnDestroy()
        {
            m_calculedNormals.Dispose();
            m_customNormals.Dispose();
        }
    }
    public interface IMeshCustomNormalsImporter
    {
        void Import(NativeArray<float3>.ReadOnly CalculedNormals, NativeArray<float3>.ReadOnly CustomNormals);
    }

}

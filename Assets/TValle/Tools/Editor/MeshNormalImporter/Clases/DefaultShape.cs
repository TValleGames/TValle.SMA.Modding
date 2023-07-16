using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.TValle.Tools.Editor.MeshNormalImporter.Clases
{
    public class DefaultShape
    {


        public JobHandle Init(NativeArray<int> triangles, JobHandle dependency)
        {

        }

        [BurstCompile]
        public struct LoadPointsJob : IJob
        {
            [ReadOnly]
            public NativeArray<float3>.ReadOnly vertices;
            /// <summary>
            /// the real vertices, a vertex may have a copy of it self in the mesh, becouse of seams or becouse of materials
            /// </summary>
            [WriteOnly]
            public NativeArray<float3> points;
            /// <summary>
            /// holds a colection of mesh vertices indexes of a single point index
            /// <para>from a point index get many vertex indexes</para>
            /// </summary>
            [WriteOnly]
            public NativeParallelMultiHashMap<int, int> verticesIndexesOfPointIndex;
            /// <summary>
            /// from a vertex index get the point index
            /// </summary>
            [WriteOnly]
            public NativeParallelHashMap<int, int> pointIndexOfVertexIndex;


            public void Execute()
            {
                NativeParallelHashMap<float3, int> pointIndexesOfPointPosition = new NativeParallelHashMap<float3, int>(vertices.Length, Allocator.Temp);
                int lastPointIndex = 0;
                for(int i = 0; i < vertices.Length; i++)
                {
                    var vert = vertices[i];
                    if(!pointIndexesOfPointPosition.TryGetValue(vert, out int pointIndex))//if not existe
                    {
                        pointIndex = lastPointIndex;
                        pointIndexesOfPointPosition.Add(vert, pointIndex);

                        points[pointIndex] = vert;
                        lastPointIndex = lastPointIndex + 1;
                    }
                    pointIndexOfVertexIndex.Add(i, pointIndex);
                    verticesIndexesOfPointIndex.Add(pointIndex, i);
                }
            }
        }

        /// <summary>
        /// only compatible with meshes with triangles as faces
        /// </summary>
        [BurstCompile]
        public struct LoadTriangleFacesJob : IJob
        {
            [ReadOnly]
            public NativeArray<int>.ReadOnly triangles;
            /// <summary>
            /// each face is three vertex indices, Length is triangles.Length/3
            /// </summary>
            [WriteOnly]
            public NativeArray<int3> faces;

            public void Execute()
            {
                int faceIndex = 0;
                for(int i = 0; i < triangles.Length; i = i + 3)
                {
                    var v1 = triangles[i];
                    var v2 = triangles[i + 1];
                    var v3 = triangles[i + 2];

                    faces[faceIndex] = new int3(v1, v2, v3);

                }

            }
        }


        [BurstCompile]
        public struct CalculeFaceNormalJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<int3>.ReadOnly faces;
            [ReadOnly]
            public NativeArray<float3>.ReadOnly points;
            [ReadOnly]
            public NativeParallelHashMap<int, int>.ReadOnly pointIndexOfVertexIndex;
            /// <summary>
            /// a normalized normal for each faces
            /// </summary>
            [WriteOnly]
            public NativeArray<float3> faceNormals;

            public void Execute(int index)
            {
                var face = faces[index];
                int p1 = pointIndexOfVertexIndex[face.x];
                int p2 = pointIndexOfVertexIndex[face.y];
                int p3 = pointIndexOfVertexIndex[face.z];

                float3 v1Pos = points[p1];
                float3 v2Pos = points[p2];
                float3 v3Pos = points[p3];

                var U = v2Pos - v1Pos;
                var V = v3Pos - v1Pos;
                float3 normal = math.normalize(math.cross(U, V));
                faceNormals[index] = normal;
            }
        }

    }
}

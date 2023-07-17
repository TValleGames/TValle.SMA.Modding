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

    //[BurstCompile]
    public struct LoadBasisPointsJob : IJob
    {
        [ReadOnly]
        public NativeArray<float3>.ReadOnly vertices;
        //[ReadOnly]
        //public NativeArray<float3>.ReadOnly deltaVertices;



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
        /// <summary>
        /// the real vertices, a vertex may have a copy of it self in the mesh, becouse of seams or becouse of materials
        /// </summary>
        [WriteOnly]
        public NativeList<float3> points;


        public void Execute()
        {
            //NativeArray<float3> shapedVertices = new NativeArray<float3>(vertices.Length, Allocator.Temp);
            //for(int i = 0; i < vertices.Length; i++)
            //    shapedVertices[i] = vertices[i] + deltaVertices[i];


            NativeParallelHashMap<float3, int> pointIndexesOfPointPosition = new NativeParallelHashMap<float3, int>(vertices.Length, Allocator.Temp);
            int lastPointIndex = 0;
            for(int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                if(!pointIndexesOfPointPosition.TryGetValue(vertex, out int pointIndex))//if not existe
                {
                    pointIndex = lastPointIndex;
                    pointIndexesOfPointPosition.Add(vertex, pointIndex);

                    points.Add(vertex);
                    lastPointIndex = lastPointIndex + 1;
                }
                pointIndexOfVertexIndex.Add(i, pointIndex);
                verticesIndexesOfPointIndex.Add(pointIndex, i);
            }

            //shapedVertices.Dispose();
            pointIndexesOfPointPosition.Dispose();
        }
    }
    public struct LoadShapePointsJob : IJob
    {
        [ReadOnly]
        public NativeArray<float3>.ReadOnly vertices;
        [ReadOnly]
        public NativeArray<float3>.ReadOnly deltaVertices;
        /// <summary>
        /// from a vertex index get the point index
        /// </summary>
        [ReadOnly]
        public NativeParallelHashMap<int, int>.ReadOnly pointIndexOfVertexIndex;


        /// <summary>
        /// the real vertices, a vertex may have a copy of it self in the mesh, becouse of seams or becouse of materials
        /// </summary>
        [WriteOnly]
        public NativeArray<float3> points;


        public void Execute()
        {
            NativeArray<float3> shapedVertices = new NativeArray<float3>(vertices.Length, Allocator.Temp);
            for(int i = 0; i < vertices.Length; i++)
                shapedVertices[i] = vertices[i] + deltaVertices[i];

            for(int i = 0; i < shapedVertices.Length; i++)
            {
                var shapedVertex = shapedVertices[i];
                points[pointIndexOfVertexIndex[i]] = shapedVertex;
            }

            shapedVertices.Dispose();
        }
    }

    /// <summary>
    /// only compatible with meshes with triangles as faces
    /// </summary>
    //[BurstCompile]
    public struct LoadTriangleFacesJob : IJob
    {
        [ReadOnly]
        public NativeArray<int>.ReadOnly triangles;
        /// <summary>
        /// from a vertex index get the point index
        /// </summary>
        [ReadOnly]
        public NativeParallelHashMap<int, int>.ReadOnly pointIndexOfVertexIndex;
        /// <summary>
        /// holds a colection of mesh vertices indexes of a single point index
        /// <para>from a point index get many vertex indexes</para>
        /// </summary>
        [ReadOnly]
        public NativeParallelMultiHashMap<int, int>.ReadOnly verticesIndexesOfPointIndex;
        /// <summary>
        /// each face is three vertex indices, Length is triangles.Length/3
        /// </summary>
        [WriteOnly]
        public NativeArray<int3> faces;
        /// <summary>
        /// holds a colection of mesh vertices indexes of a single face index
        /// </summary>
        [WriteOnly]
        public NativeParallelMultiHashMap<int, int> verticesIndexesOfFaceIndex;

        public void Execute()
        {
            int faceIndex = 0;
            for(int i = 0; i < triangles.Length; i = i + 3)
            {
                var v1 = triangles[i];
                var v2 = triangles[i + 1];
                var v3 = triangles[i + 2];

                faces[faceIndex] = new int3(v1, v2, v3);

                var p1 = pointIndexOfVertexIndex[v1];
                var p2 = pointIndexOfVertexIndex[v2];
                var p3 = pointIndexOfVertexIndex[v3];

                if(verticesIndexesOfPointIndex.TryGetFirstValue(p1, out int vertexIndexOfPoint1, out NativeParallelMultiHashMapIterator<int> iterator1))
                {
                    do
                    {
                        verticesIndexesOfFaceIndex.Add(faceIndex, vertexIndexOfPoint1);

                    } while(verticesIndexesOfPointIndex.TryGetNextValue(out vertexIndexOfPoint1, ref iterator1));
                }
                if(verticesIndexesOfPointIndex.TryGetFirstValue(p2, out int vertexIndexOfPoint2, out NativeParallelMultiHashMapIterator<int> iterator2))
                {
                    do
                    {
                        verticesIndexesOfFaceIndex.Add(faceIndex, vertexIndexOfPoint2);

                    } while(verticesIndexesOfPointIndex.TryGetNextValue(out vertexIndexOfPoint2, ref iterator2));
                }
                if(verticesIndexesOfPointIndex.TryGetFirstValue(p3, out int vertexIndexOfPoint3, out NativeParallelMultiHashMapIterator<int> iterator3))
                {
                    do
                    {
                        verticesIndexesOfFaceIndex.Add(faceIndex, vertexIndexOfPoint3);

                    } while(verticesIndexesOfPointIndex.TryGetNextValue(out vertexIndexOfPoint3, ref iterator3));
                }
                faceIndex = faceIndex + 1;
            }

        }
    }


    //[BurstCompile]
    public struct CalculeFaceNormalsJob : IJobParallelFor
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
        public NativeArray<float3> calculedFaceNormals;

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
            calculedFaceNormals[index] = normal;
        }
    }
    //[BurstCompile]
    public struct CalculeVertexNormalsJob : IJob
    {
        /// <summary>
        /// holds a colection of mesh vertices indexes of a single face index
        /// </summary>
        [ReadOnly]
        public NativeParallelMultiHashMap<int, int>.ReadOnly verticesIndexesOfFaceIndex;
        [ReadOnly]
        public NativeArray<float3>.ReadOnly calculedFaceNormals;

       
        public NativeArray<float3> calculedVertexNormals;


        public void Execute()
        {
            //for(int i = 0; i < triangles.Length; i++)
            //{
            //    var vertIndex = triangles[i];
            //    var pointIndex = pointIndexOfVertexIndex[vertIndex];

            //    if(verticesIndexesOfPointIndex.TryGetFirstValue(pointIndex, out int vertexIndexOfPoint, out NativeParallelMultiHashMapIterator<int> iterator))
            //    {
            //        do
            //        {
            //            var faceIndex = faceIndexOfVertexIndex[vertexIndexOfPoint];
            //            var faceNormal = calculedFaceNormals[faceIndex];
            //            calculedVertexNormals[vertexIndexOfPoint] = calculedVertexNormals[vertexIndexOfPoint] + faceNormal;

            //        } while(verticesIndexesOfPointIndex.TryGetNextValue(out vertexIndexOfPoint, ref iterator));
            //    }
            //}

            for(int i = 0; i < calculedFaceNormals.Length; i++)
            {
                var faceNormal = calculedFaceNormals[i];
                if(verticesIndexesOfFaceIndex.TryGetFirstValue(i, out int vertexIndexOfFace, out NativeParallelMultiHashMapIterator<int> iterator))
                {
                    do
                    {
                        calculedVertexNormals[vertexIndexOfFace] = calculedVertexNormals[vertexIndexOfFace] + faceNormal;

                    } while(verticesIndexesOfFaceIndex.TryGetNextValue(out vertexIndexOfFace, ref iterator));
                }
            }

            for(int i = 0; i < calculedVertexNormals.Length; i++)
                calculedVertexNormals[i] = math.normalize(calculedVertexNormals[i]);

        }
    }
    //[BurstCompile]
    public struct CalculeCustomVertexNormalsJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<float3>.ReadOnly calculedVertexNormals;
        [ReadOnly]
        public NativeArray<float3>.ReadOnly importedVertexNormals;

        [WriteOnly]
        public NativeArray<float3> customVertexNormals;

        public void Execute(int index)
        {
            customVertexNormals[index] = importedVertexNormals[index] - calculedVertexNormals[index];
        }
    }
    //[BurstCompile]
    public struct CalculeCorrectedVertexNormalsJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<float3>.ReadOnly calculedVertexNormals;
        [ReadOnly]
        public NativeArray<float3>.ReadOnly customVertexNormals;

        [WriteOnly]
        public NativeArray<float3> correctedVertexNormals;

        public void Execute(int index)
        {
            correctedVertexNormals[index] = calculedVertexNormals[index] + customVertexNormals[index];
        }
    }
    //[BurstCompile]
    public struct CalculeCorrectedDeltaNormalsJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<float3>.ReadOnly importedVertexNormals;
        [ReadOnly]
        public NativeArray<float3>.ReadOnly correctedVertexNormals;

        [WriteOnly]
        public NativeArray<float3> correctedDeltaNormals;

        public void Execute(int index)
        {
            correctedDeltaNormals[index] = importedVertexNormals[index] - correctedVertexNormals[index];
        }
    }

}

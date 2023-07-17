using Assets.TValle.Tools.Editor.MeshNormalImporter.Clases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.TValle.Tools.Editor.MeshNormalImporter
{
    public class AssetPostprocessorNormalImporter : AssetPostprocessor
    {
        public const string label = "CorrectBlendShapeNormals";
        void OnPostprocessModel(GameObject g)
        {
            var lables = AssetDatabase.GetLabels(assetImporter);
            if(lables.Contains(label, StringComparer.Ordinal))
            {
                Debug.Log("****Fixing Blend Shape Normals on: " + g.name);
                var renderers = g.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach(var item in renderers)
                {
                    FixBlendShapeNormals(item);
                }
                Debug.Log("****Terminado*****");
            }
        }
        void FixBlendShapeNormals(SkinnedMeshRenderer renderer)
        {
            var shapes = new BlendShapes(renderer);

            Debug.Log("building blend shapes: " + renderer.name);
            shapes.BuildBlendShapes();

            Debug.Log("init blend shapes correctors: " + renderer.name);
            shapes.InitBlendShapesCorrectors();

            Debug.Log("removing old blend shapes: " + renderer.name);
            shapes.RemoveShapesFromMesh();

            Debug.Log("calculing corrected normals: " + renderer.name);
            shapes.CalculeNormals();

            Debug.Log("loading currected normals to mesh: " + renderer.name);
            shapes.CorrectShapes();

            Debug.Log("Disposing used data: " + renderer.name);
            shapes.Dispose();
        }

        public class BlendShapes
        {
            public BlendShapes(SkinnedMeshRenderer renderer)
            {
                this.mesh = renderer.sharedMesh;
                this.renderer = renderer;

                importedTriangles = new NativeArray<int>(mesh.triangles, Allocator.Persistent);
                importedVertices = new NativeArray<Vector3>(mesh.vertices, Allocator.Persistent).Reinterpret<float3>();
                importedNormals = new NativeArray<Vector3>(mesh.normals, Allocator.Persistent).Reinterpret<float3>();

                m_corrector = new ImportedCorrectorBasis(importedTriangles.AsReadOnly(), importedVertices.AsReadOnly(), importedNormals.AsReadOnly());

            }

            public readonly NativeArray<int> importedTriangles;
            public readonly NativeArray<float3> importedVertices;
            public readonly NativeArray<float3> importedNormals;

            ImportedCorrectorBasis m_corrector;

            public List<BlendShape> blendShapes { get; private set; }
            public readonly SkinnedMeshRenderer renderer;
            public readonly Mesh mesh;

            public void BuildBlendShapes()
            {
                blendShapes = new List<BlendShape>(mesh.blendShapeCount);
                for(int shapeIndex = 0; shapeIndex < mesh.blendShapeCount; shapeIndex++)
                {
                    var shape = new BlendShape(this, shapeIndex);
                    blendShapes.Add(shape);
                }
            }


            public void InitBlendShapesCorrectors()
            {
                JobHandle jobHandle = m_corrector.Init(default);
                jobHandle.Complete();
                NativeArray<JobHandle> handles = new NativeArray<JobHandle>(blendShapes.Count, Allocator.Temp);
                for(int i = 0; i < blendShapes.Count; i++)
                {
                    handles[i] = blendShapes[i].InitCorrector(m_corrector, default);
                }
                JobHandle.CompleteAll(handles);
                handles.Dispose();
            }
            public void RemoveShapesFromMesh()
            {
                mesh.ClearBlendShapes();
            }
            public void CalculeNormals()
            {
                JobHandle jobHandle = m_corrector.CalculeNormals(default);
                jobHandle = m_corrector.CalculeCustomNormals(jobHandle);
                NativeArray<JobHandle> handles = new NativeArray<JobHandle>(blendShapes.Count, Allocator.Temp);
                for(int i = 0; i < blendShapes.Count; i++)
                {
                    handles[i] = blendShapes[i].CalculeNormals(m_corrector, jobHandle);
                }
                jobHandle.Complete();
                JobHandle.CompleteAll(handles);
                handles.Dispose();

                for(int i = 0; i < blendShapes.Count; i++)
                {
                    blendShapes[i].ModCorrectedDeltas();
                }

            }
            public void CorrectShapes()
            {
                if(mesh.blendShapeCount > 0)
                    throw new InvalidOperationException("first you must delete the shapes from the mesh, RemoveShapeFromMesh()");

                foreach(var item in blendShapes)
                {
                    item.Correct();
                }
            }




            public void Dispose()
            {
                foreach(var item in blendShapes)
                {
                    item.Dispose();
                }

                importedTriangles.Dispose();
                importedVertices.Dispose();
                importedNormals.Dispose();

            }

        }

        public class BlendShape
        {
            public BlendShape(BlendShapes blendShapes, int shapeIndex)
            {
                this.blendShapes = blendShapes;
                var frames = blendShapes.mesh.GetBlendShapeFrameCount(shapeIndex);
                if(frames != 1)
                    throw new NotSupportedException();
                const int frameIndex = 0;
                deltaVertices = new Vector3[blendShapes.mesh.vertexCount];
                deltaNormals = new Vector3[blendShapes.mesh.vertexCount];
                deltaTangents = new Vector3[blendShapes.mesh.vertexCount];
                this.shapeIndex = shapeIndex;
                this.shapeName = blendShapes.mesh.GetBlendShapeName(shapeIndex);
                w = blendShapes.mesh.GetBlendShapeFrameWeight(shapeIndex, frameIndex);
                blendShapes.mesh.GetBlendShapeFrameVertices(shapeIndex, frameIndex, deltaVertices, deltaNormals, deltaTangents);
                m_corrector = new ImportedCorrectorBlendShape(blendShapes.importedTriangles.AsReadOnly(), blendShapes.importedVertices.AsReadOnly(), blendShapes.importedNormals.AsReadOnly());
            }
            readonly public BlendShapes blendShapes;
            readonly public int shapeIndex;
            readonly public string shapeName;
            readonly public float w;
            public Vector3[] deltaVertices;
            public Vector3[] deltaNormals;
            public Vector3[] deltaTangents;
            ImportedCorrectorBlendShape m_corrector;
            public JobHandle InitCorrector(ImportedCorrectorBasis basisCorrector, JobHandle dependency)
            {
                NativeArray<float3> DeltaVertices = new NativeArray<Vector3>(deltaVertices, Allocator.TempJob).Reinterpret<float3>();
                dependency = m_corrector.Init(DeltaVertices.AsReadOnly(), basisCorrector.points, basisCorrector.pointIndexOfVertexIndex, dependency);
                DeltaVertices.Dispose(dependency);
                return dependency;
            }

            public JobHandle CalculeNormals(ImportedCorrectorBasis basisCorrector, JobHandle dependency)
            {
                dependency = m_corrector.CalculeNormals(basisCorrector.faces, basisCorrector.pointIndexOfVertexIndex, basisCorrector.verticesIndexesOfFaceIndex, dependency);
                dependency = m_corrector.CalculeCorrectedNormals(basisCorrector.customNormals, dependency);
                dependency = m_corrector.CalculeCorrectedDeltaNormals(dependency);
                return dependency;
            }
            public void ModCorrectedDeltas()
            {
                deltaNormals = m_corrector.correctedDeltaNormals.Reinterpret<Vector3>().ToArray();
            }
            public void Correct()
            {
                if(shapeIndex < blendShapes.mesh.blendShapeCount)
                    throw new InvalidOperationException("a shape can only be updated if it does not exist");

                if(shapeIndex != blendShapes.mesh.blendShapeCount)
                    throw new InvalidOperationException("shapes have to be updated in a correct order");

                blendShapes.mesh.AddBlendShapeFrame(shapeName, w, deltaVertices, deltaNormals, deltaTangents);
            }
            public void Dispose()
            {
                m_corrector.Dispose();

            }
        }
    }
}

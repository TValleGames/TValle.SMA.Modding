using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.TValle.Tools.MeshNormalImporter.Clases
{
    public class ImportedCorrectorBlendShape : ImportedCorrectorBase
    {
        public ImportedCorrectorBlendShape(NativeArray<int>.ReadOnly importedTriangles, NativeArray<float3>.ReadOnly importedVertices, NativeArray<float3>.ReadOnly importedNormals) : base(importedTriangles, importedVertices, importedNormals)
        {
        }
        /// <summary>
        /// the real vertices, a vertex may have a copy of it self in the mesh, becouse of seams or becouse of materials
        /// <para>already shaped</para>
        /// </summary>
        NativeArray<float3> m_points;
        /// <summary>
        /// re calculated normals
        /// </summary>
        NativeArray<float3> m_correctedNormals;
        /// <summary>
        /// re calculated normals, ready to create a new blend shape
        /// </summary>
        NativeArray<float3> m_correctedDeltaNormals;


        /// <summary>
        /// loaded after calling CorrectNormals()
        /// </summary>
        public NativeArray<float3>.ReadOnly correctedNormals => m_correctedNormals.AsReadOnly();
        /// <summary>
        /// loaded after calling CalculeCorrectedDeltaNormals()
        /// </summary>
        public NativeArray<float3>.ReadOnly correctedDeltaNormals => m_correctedDeltaNormals.AsReadOnly();


        public JobHandle Init(NativeArray<float3>.ReadOnly deltaVertices, NativeArray<float3>.ReadOnly points, NativeParallelHashMap<int, int>.ReadOnly pointIndexOfVertexIndex, JobHandle dependency)
        {
            dependency = InitBase(dependency);
            m_points = new NativeArray<float3>(points.Length, Allocator.Persistent);
            m_correctedNormals = new NativeArray<float3>(importedVertices.Length, Allocator.Persistent);
            m_correctedDeltaNormals = new NativeArray<float3>(importedVertices.Length, Allocator.Persistent);



            LoadShapePointsJob loadPointsJob = new LoadShapePointsJob();
            loadPointsJob.vertices = importedVertices;
            loadPointsJob.deltaVertices = deltaVertices;

            loadPointsJob.points = m_points;
            loadPointsJob.pointIndexOfVertexIndex = pointIndexOfVertexIndex;

            dependency = loadPointsJob.Schedule(dependency);
            return dependency;

        }
        /// <summary>
        /// all blend shapes require Normals to be calculed
        /// </summary>      
        public JobHandle CalculeNormals(NativeArray<int3>.ReadOnly faces, NativeParallelHashMap<int, int>.ReadOnly pointIndexOfVertexIndex, NativeParallelMultiHashMap<int, int>.ReadOnly verticesIndexesOfFaceIndex, JobHandle dependency)
        {
            CalculeFaceNormalsJob calculeFaceNormalsJob = new CalculeFaceNormalsJob();
            calculeFaceNormalsJob.faces = faces;
            calculeFaceNormalsJob.points = m_points.AsReadOnly();
            calculeFaceNormalsJob.pointIndexOfVertexIndex = pointIndexOfVertexIndex;

            calculeFaceNormalsJob.calculedFaceNormals = m_calculedFaceNormals;

            dependency = calculeFaceNormalsJob.Schedule(faces.Length, faces.Length.BatchCountAllProcessors(), dependency);



            CalculeVertexNormalsJob calculeVertexNormalsJob = new CalculeVertexNormalsJob();
            calculeVertexNormalsJob.verticesIndexesOfFaceIndex = verticesIndexesOfFaceIndex;
            calculeVertexNormalsJob.calculedFaceNormals = m_calculedFaceNormals.AsReadOnly();

            calculeVertexNormalsJob.calculedVertexNormals = m_calculedNormals;

            dependency = calculeVertexNormalsJob.Schedule(dependency);

            return dependency;
        }
        /// <summary>
        /// only required for the Non Basis shapes
        /// </summary>
        public JobHandle CalculeCorrectedNormals(NativeArray<float3>.ReadOnly customNormals, JobHandle dependency)
        {

            CalculeCorrectedVertexNormalsJob calculeCorrectedVertexNormalsJob = new CalculeCorrectedVertexNormalsJob();
            calculeCorrectedVertexNormalsJob.calculedVertexNormals = m_calculedNormals.AsReadOnly();
            calculeCorrectedVertexNormalsJob.customVertexNormals = customNormals;

            calculeCorrectedVertexNormalsJob.correctedVertexNormals = m_correctedNormals;
            dependency = calculeCorrectedVertexNormalsJob.Schedule(m_calculedNormals.Length, m_calculedNormals.Length.BatchCountAllProcessors(), dependency);

            return dependency;
        }
        /// <summary>
        /// only required for the Non Basis shapes
        /// </summary>
        public JobHandle CalculeCorrectedDeltaNormals(JobHandle dependency)
        {
            CalculeCorrectedDeltaNormalsJob calculeCorrectedDeltaNormalsJob = new CalculeCorrectedDeltaNormalsJob();
            calculeCorrectedDeltaNormalsJob.importedVertexNormals = importedNormals;
            calculeCorrectedDeltaNormalsJob.correctedVertexNormals = m_correctedNormals.AsReadOnly();

            calculeCorrectedDeltaNormalsJob.correctedDeltaNormals = m_correctedDeltaNormals;
            dependency = calculeCorrectedDeltaNormalsJob.Schedule(importedNormals.Length, importedNormals.Length.BatchCountAllProcessors(), dependency);

            return dependency;
        }
        public override void Dispose()
        {
            base.Dispose();
            m_points.Dispose();
            m_correctedNormals.Dispose();
            m_correctedDeltaNormals.Dispose();
        }

    }
    public class ImportedCorrectorBasis : ImportedCorrectorBase
    {
        public ImportedCorrectorBasis(NativeArray<int>.ReadOnly importedTriangles, NativeArray<float3>.ReadOnly importedVertices, NativeArray<float3>.ReadOnly importedNormals) : base(importedTriangles, importedVertices, importedNormals)
        {
        }

        /// <summary>
        /// the real vertices, a vertex may have a copy of it self in the mesh, becouse of seams or becouse of materials
        /// <para>already shaped</para>
        /// </summary>
        NativeList<float3> m_points;
        /// <summary>
        /// holds a colection of mesh vertices indexes of a single point index
        /// <para>from a point index get many vertex indexes</para>
        /// </summary>
        NativeParallelMultiHashMap<int, int> m_verticesIndexesOfPointIndex;
        /// <summary>
        /// from a vertex index get the point index
        /// </summary>
        NativeParallelHashMap<int, int> m_pointIndexOfVertexIndex;
        /// <summary>
        /// an imported mesh may have custom normals
        /// </summary>
        NativeArray<float3> m_customNormals;
        /// <summary>
        /// holds a colection of mesh vertices indexes of a single face index
        /// </summary>
        NativeParallelMultiHashMap<int, int> m_verticesIndexesOfFaceIndex;
        /// <summary>
        /// each face is three vertex indices, Length is triangles.Length/3
        /// </summary>
        NativeArray<int3> m_faces;


        /// <summary>
        /// loaded after calling Init()
        /// </summary>
        public NativeArray<float3>.ReadOnly points => m_points.AsReadOnly();
        /// <summary>
        /// loaded after calling Init()
        /// </summary>
        public NativeParallelMultiHashMap<int, int>.ReadOnly verticesIndexesOfPointIndex => m_verticesIndexesOfPointIndex.AsReadOnly();
        /// <summary>
        /// loaded after calling Init()
        /// </summary>
        public NativeParallelHashMap<int, int>.ReadOnly pointIndexOfVertexIndex => m_pointIndexOfVertexIndex.AsReadOnly();
        /// <summary>
        /// loaded after calling Init()
        /// </summary>       
        public NativeParallelMultiHashMap<int, int>.ReadOnly verticesIndexesOfFaceIndex => m_verticesIndexesOfFaceIndex.AsReadOnly();
        /// <summary>
        /// loaded after calling Init()
        /// </summary>       
        public NativeArray<int3>.ReadOnly faces => m_faces.AsReadOnly();

        /// <summary>
        /// loaded after calling CalculeCustomNormals()
        /// </summary>
        public NativeArray<float3>.ReadOnly customNormals => m_customNormals.AsReadOnly();



        public JobHandle Init(JobHandle dependency)
        {
            dependency = InitBase(dependency);
            //m_points = new NativeList<float3>(importedVertices.Length, Allocator.Persistent);
            m_customNormals = new NativeArray<float3>(importedVertices.Length, Allocator.Persistent);

            //
            m_points = new NativeList<float3>(2, Allocator.Persistent);
            m_verticesIndexesOfPointIndex = new NativeParallelMultiHashMap<int, int>(2, Allocator.Persistent);
            m_pointIndexOfVertexIndex = new NativeParallelHashMap<int, int>(2, Allocator.Persistent);


            LoadBasisPointsJob loadPointsJob = new LoadBasisPointsJob();
            loadPointsJob.vertices = importedVertices;
            loadPointsJob.points = m_points;
            loadPointsJob.verticesIndexesOfPointIndex = m_verticesIndexesOfPointIndex;
            loadPointsJob.pointIndexOfVertexIndex = m_pointIndexOfVertexIndex;

            dependency = loadPointsJob.Schedule(dependency);

            //
            m_faces = new NativeArray<int3>(importedTriangles.Length / 3, Allocator.Persistent);
            m_verticesIndexesOfFaceIndex = new NativeParallelMultiHashMap<int, int>(2, Allocator.Persistent);

            LoadTriangleFacesJob loadTriangleFacesJob = new LoadTriangleFacesJob();
            loadTriangleFacesJob.triangles = importedTriangles;
            loadTriangleFacesJob.pointIndexOfVertexIndex = m_pointIndexOfVertexIndex.AsReadOnly();
            loadTriangleFacesJob.verticesIndexesOfPointIndex = m_verticesIndexesOfPointIndex.AsReadOnly();
            loadTriangleFacesJob.faces = m_faces;
            loadTriangleFacesJob.verticesIndexesOfFaceIndex = m_verticesIndexesOfFaceIndex;

            dependency = loadTriangleFacesJob.Schedule(dependency);
            return dependency;
        }
        /// <summary>
        /// all blend shapes require Normals to be calculed
        /// </summary>      
        public JobHandle CalculeNormals(JobHandle dependency)
        {
            CalculeFaceNormalsJob calculeFaceNormalsJob = new CalculeFaceNormalsJob();
            calculeFaceNormalsJob.faces = m_faces.AsReadOnly();
            calculeFaceNormalsJob.points = m_points.AsReadOnly();
            calculeFaceNormalsJob.pointIndexOfVertexIndex = m_pointIndexOfVertexIndex.AsReadOnly();

            calculeFaceNormalsJob.calculedFaceNormals = m_calculedFaceNormals;

            dependency = calculeFaceNormalsJob.Schedule(m_faces.Length, m_faces.Length.BatchCountAllProcessors(), dependency);


            CalculeVertexNormalsJob calculeVertexNormalsJob = new CalculeVertexNormalsJob();
            calculeVertexNormalsJob.verticesIndexesOfFaceIndex = m_verticesIndexesOfFaceIndex.AsReadOnly();
            calculeVertexNormalsJob.calculedFaceNormals = m_calculedFaceNormals.AsReadOnly();

            calculeVertexNormalsJob.calculedVertexNormals = m_calculedNormals;

            dependency = calculeVertexNormalsJob.Schedule(dependency);

            return dependency;
        }

        /// <summary>
        /// only required for the Basis shape, or better said, just the mesh
        /// </summary>
        public JobHandle CalculeCustomNormals(JobHandle dependency)
        {
            CalculeCustomVertexNormalsJob calculeCustomVertexNormalsJob = new CalculeCustomVertexNormalsJob();
            calculeCustomVertexNormalsJob.calculedVertexNormals = m_calculedNormals.AsReadOnly();
            calculeCustomVertexNormalsJob.importedVertexNormals = importedNormals;

            calculeCustomVertexNormalsJob.customVertexNormals = m_customNormals;
            dependency = calculeCustomVertexNormalsJob.Schedule(m_calculedNormals.Length, m_calculedNormals.Length.BatchCountAllProcessors(), dependency);

            return dependency;
        }
        public override void Dispose()
        {
            base.Dispose();
            m_points.Dispose();
            m_verticesIndexesOfPointIndex.Dispose();
            m_pointIndexOfVertexIndex.Dispose();
            m_verticesIndexesOfFaceIndex.Dispose();
            m_faces.Dispose();
            m_customNormals.Dispose();
        }
    }
    public abstract class ImportedCorrectorBase
    {

        public ImportedCorrectorBase(NativeArray<int>.ReadOnly importedTriangles, NativeArray<float3>.ReadOnly importedVertices, NativeArray<float3>.ReadOnly importedNormals)
        {
            this.importedTriangles = importedTriangles;
            this.importedVertices = importedVertices;
            this.importedNormals = importedNormals;
        }


        public readonly NativeArray<int>.ReadOnly importedTriangles;
        public readonly NativeArray<float3>.ReadOnly importedVertices;
        public readonly NativeArray<float3>.ReadOnly importedNormals;




        /// <summary>
        /// a normalized normal for each faces
        /// </summary>
        protected NativeArray<float3> m_calculedFaceNormals;
        /// <summary>
        /// a normalized normal for each vertex
        /// </summary>
        protected NativeArray<float3> m_calculedNormals;


        public NativeArray<float3>.ReadOnly calculedFaceNormals => m_calculedFaceNormals.AsReadOnly();
        public NativeArray<float3>.ReadOnly calculedNormals => m_calculedNormals.AsReadOnly();




        protected JobHandle InitBase(JobHandle dependency)
        {
            m_calculedFaceNormals = new NativeArray<float3>(importedTriangles.Length / 3, Allocator.Persistent);
            m_calculedNormals = new NativeArray<float3>(importedVertices.Length, Allocator.Persistent);

            return dependency;
        }







        public virtual void Dispose()
        {
            m_calculedFaceNormals.Dispose();
            m_calculedNormals.Dispose();
        }
    }
}

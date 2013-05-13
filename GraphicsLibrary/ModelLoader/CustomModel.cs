#region File Description
//-----------------------------------------------------------------------------
// CustomModel.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using AnimationUtilities;
using grendgine_collada;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GraphicsLibrary.ModelLoader
{
    /// <summary>
    /// Custom class that can be used as a replacement for the built-in Model type.
    /// This provides functionality roughly similar to Model, but simplified as far
    /// as possible while still being able to correctly render data from arbitrary
    /// X or FBX files. This can be used as a starting point for building up your
    /// own more sophisticated Model replacements.
    /// </summary>
    public class CustomModel
    {
        #region Fields

        // Disable compiler warning that we never initialize these fields.
        // That's ok, because the XNB deserializer initialises them for us!
#pragma warning disable 649

        [ContentSerializer]
        public SkinningData SkinningData;

        // Internally our custom model is made up from a list of model parts.
        [ContentSerializer]
        public List<Mesh> Meshes;


        // Each model part represents a piece of geometry that uses one
        // single effect. Multiple parts are needed for models that use
        // more than one effect.
        public class MeshPart
        {
            public int TriangleCount;
            public int VertexCount;

            public VertexBuffer VertexBuffer;
            public IndexBuffer IndexBuffer;

            public Matrix BindPose;
            public MeshPart ParentPart;

            [ContentSerializer(SharedResource = true)]
            public Effect Effect;

            public MeshPart(GraphicsDevice graphicsDevice, VertexPositionNormalTexture[] vertices, short[] indices, Effect effect)
                : this(
                    new VertexBuffer(
                        graphicsDevice,
                        vertices.GetType().GetElementType(),
                        vertices.Length,
                        BufferUsage.None),
                    new IndexBuffer(
                        graphicsDevice,
                        typeof(short),
                        indices.Length,
                        BufferUsage.None),
                    effect)
            {
                VertexBuffer.SetData(vertices);
                IndexBuffer.SetData(indices);
            }

            public MeshPart(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, Effect effect)
            {
                VertexBuffer = vertexBuffer;
                IndexBuffer = indexBuffer;
                VertexCount = VertexBuffer.VertexCount;
                TriangleCount = IndexBuffer.IndexCount / 3;
                Effect = effect;
            }
        }

        public class Mesh
        {
            public ObservableCollection<MeshPart> Parts = new ObservableCollection<MeshPart>();
            public Mesh ParentMesh = null;
            public Matrix BindPose = Matrix.Identity;

            public BoundingSphere BoundingSphere = new BoundingSphere(Vector3.Zero, 0);

            public Matrix AbsoluteWorldTransform
            {
                get
                {
                    Matrix transform = BindPose;
                    Mesh parent = ParentMesh;
                    while (parent != null)
                    {
                        transform = parent.BindPose * transform;
                        parent = parent.ParentMesh;
                    }
                    return transform;
                }
            }

            public Mesh(ObservableCollection<MeshPart> parts, Mesh parentMesh, Matrix bindPose)
            {
                ParentMesh = parentMesh;
                BindPose = bindPose;
                Parts = parts;
                Parts.CollectionChanged += Parts_CollectionChanged<VertexPositionNormalTexture>;
            }

            void Parts_CollectionChanged<T>(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) where T : VertexPositionNormalTexture
            {
                BoundingSphere.Center = Vector3.Zero;
                int numVertices = 0;
                foreach (var part in Parts)
                {
                    T[] vertices = new T[part.VertexBuffer.VertexCount];
                    part.VertexBuffer.GetData(vertices);
                    foreach (var vertex in vertices)
                    {
                        BoundingSphere.Center += vertex.Position;
                    }
                    numVertices += vertices.Length;
                }
                BoundingSphere.Center /= numVertices;

                BoundingSphere.Radius = 0;
                foreach (var part in Parts)
                {
                    // TODO: don't extract verts twice
                    T[] vertices = new T[part.VertexBuffer.VertexCount];
                    part.VertexBuffer.GetData(vertices);
                    foreach (var vertex in vertices)
                    {
                        BoundingSphere.Radius = (float)Math.Max((double)BoundingSphere.Radius, (double)(vertex.Position - BoundingSphere.Center).Length());
                    }
                }
            }
        }


#pragma warning restore 649

        #endregion


        /// <summary>
        /// Private constructor, for use by the XNB deserializer.
        /// </summary>
        private CustomModel()
        {
        }

        public CustomModel(GraphicsDevice graphicsDevice, string fileName)
        {// TODO: perhaps.. this: https://github.com/Bunkerbewohner/ColladaXna/
            Meshes = new List<Mesh>();

            //-----------------
            fileName = "Content/animcubetexbaked.dae";
            //-----------------
            FileInfo info = new FileInfo(fileName);
            Grendgine_Collada model = Grendgine_Collada.Grendgine_Load_File(fileName);

            Dictionary<string, Effect> effects = GetEffects(graphicsDevice, model, fileName);
            Dictionary<string, Grendgine_Collada_Geometry> geometryLibrary = new Dictionary<string, Grendgine_Collada_Geometry>();
            foreach (var geometry in model.Library_Geometries.Geometry)
            {
                geometryLibrary.Add(geometry.ID, geometry);
            }
            Dictionary<string, Grendgine_Collada_Controller> controllerLibrary = new Dictionary<string, Grendgine_Collada_Controller>();
            if (model.Library_Controllers != null)
            {
                foreach (var controller in model.Library_Controllers.Controller)
                {
                    controllerLibrary.Add(controller.ID, controller);
                }
            }
            Dictionary<string, Grendgine_Collada_Animation> animationLibrary = new Dictionary<string, Grendgine_Collada_Animation>();
            if (model.Library_Animations != null)
            {
                foreach (var animation in model.Library_Animations.Animation)
                {
                    animationLibrary.Add(animation.ID != null ? animation.ID : "default", animation);
                }
            }
            BoneData boneData = new BoneData();
            WalkScene(graphicsDevice, model, boneData, geometryLibrary, effects, controllerLibrary);
            boneData.AnimationClips = GetAnimationClips(model, animationLibrary, boneData);
            
            SkinningData = new SkinningData(
                boneData.AnimationClips,
                boneData.BindPose,
                boneData.InverseBindPose,
                boneData.SkeletonHierarchy,
                boneData.BoneIndices);
        }

        #region ModelLoadingHelpers

        private static Dictionary<string, AnimationClip> GetAnimationClips(Grendgine_Collada model, Dictionary<string, Grendgine_Collada_Animation> animationLibrary, BoneData boneData)
        {
            Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();
            if (model.Library_Animation_Clips != null)
            {
                foreach (var animationClip in model.Library_Animation_Clips.Animation_Clip)
                {
                    foreach (var instanceAnimation in animationClip.Instance_Animation)
                    {
                        animationClips.Add(
                            animationClip.ID,
                            new AnimationClip(
                                TimeSpan.FromSeconds(animationClip.End - animationClip.Start),
                                GetAnimationKeyframes(
                                    animationLibrary[instanceAnimation.URL.TrimStart('#')],
                                    boneData,
                                    (float)animationClip.Start,
                                    (float)animationClip.End)));
                    }
                }
            }
            return animationClips;
        }

        private static List<Keyframe> GetAnimationKeyframes(Grendgine_Collada_Animation animation, BoneData boneData, float startTime, float endTime)
        {
            List<Keyframe> keyframes = new List<Keyframe>();

            Dictionary<string, Grendgine_Collada_Source> sources = new Dictionary<string, Grendgine_Collada_Source>();
            foreach (var source in animation.Source)
            {
                sources.Add(source.ID, source);
            }

            Dictionary<string, Grendgine_Collada_Sampler> samplers = new Dictionary<string, Grendgine_Collada_Sampler>();
            foreach (var sampler in animation.Sampler)
            {
                samplers.Add(sampler.ID, sampler);
            }

            foreach (var channel in animation.Channel)
            {
                string[] target = channel.Target.Split('/');
                string node = target[0];
                string property = target[1];
                if (property == "matrix")
                {
                    Grendgine_Collada_Input_Unshared inputInput = null;
                    Grendgine_Collada_Input_Unshared outputInput = null;
                    Grendgine_Collada_Input_Unshared interpolationInput = null;
                    foreach (var input in samplers[channel.Source.TrimStart('#')].Input)
                    {
                        if (input.Semantic == Grendgine_Collada_Input_Semantic.INPUT)
                        {
                            inputInput = input;
                        }
                        else if (input.Semantic == Grendgine_Collada_Input_Semantic.OUTPUT)
                        {
                            outputInput = input;
                        }
                        else if (input.Semantic == Grendgine_Collada_Input_Semantic.INTERPOLATION)
                        {
                            interpolationInput = input;
                        }
                    }
                    int boneIndex = boneData.BoneIndices[node];
                    float[] times = sources[inputInput.source.TrimStart('#')].Float_Array.Value();
                    Matrix[] matrices = GetMatricesFromSource(sources[outputInput.source.TrimStart('#')]);

                    int currentIndex = Array.BinarySearch(times, startTime);
                    if (currentIndex < 0)
                    {
                        currentIndex = ~currentIndex;
                        Matrix interpolatedMatrix = InterpolateMatrices(matrices[currentIndex - 1], matrices[currentIndex], (startTime - times[currentIndex - 1]) / (times[currentIndex] - times[currentIndex - 1]));
                        keyframes.Add(new Keyframe(boneIndex, TimeSpan.FromSeconds(startTime), interpolatedMatrix));
                    }
                    for (; currentIndex < times.Length && times[currentIndex] <= endTime; currentIndex++)
                    {
                        keyframes.Add(new Keyframe(boneIndex, TimeSpan.FromSeconds(times[currentIndex]), matrices[currentIndex]));
                    }
                    if (currentIndex < times.Length && times[currentIndex - 1] < endTime)
                    {
                        Matrix interpolatedMatrix = InterpolateMatrices(matrices[currentIndex - 1], matrices[currentIndex], (endTime - times[currentIndex - 1]) / (times[currentIndex] - times[currentIndex - 1]));
                        keyframes.Add(new Keyframe(boneIndex, TimeSpan.FromSeconds(endTime), interpolatedMatrix));
                    }
                }
            }
            keyframes.Sort(new KeyframeComparison());
            return keyframes;
        }

        class KeyframeComparison : Comparer<Keyframe>
        {
            public override int Compare(Keyframe x, Keyframe y)
            {
                double diff = x.Time.TotalSeconds - y.Time.TotalSeconds;
                if (diff < 0)
                {
                    return -1;
                }
                if (diff == 0)
                {
                    return 0;
                }
                return 1;
            }
        }

        private static Matrix InterpolateMatrices(Matrix matrixBegin, Matrix matrixEnd, float amount)
        {
            Vector3 scaleBegin;
            Vector3 scaleEnd;
            Vector3 positionBegin;
            Vector3 positionEnd;
            Quaternion rotationBegin;
            Quaternion rotationEnd;
            matrixBegin.Decompose(out scaleBegin, out rotationBegin, out positionBegin);
            matrixEnd.Decompose(out scaleEnd, out rotationEnd, out positionEnd);
            Matrix interpolatedMatrix = Matrix.CreateFromQuaternion(Quaternion.Slerp(rotationBegin, rotationEnd, amount));
            interpolatedMatrix *= Matrix.CreateScale(Vector3.Lerp(scaleBegin, scaleEnd, amount));
            interpolatedMatrix *= Matrix.CreateTranslation(Vector3.Lerp(positionBegin, positionEnd, amount));

            return interpolatedMatrix;
        }

        private static Matrix[] GetMatricesFromSource(Grendgine_Collada_Source source)
        {
            float[] matrixValues = source.Float_Array.Value();
            Matrix[] matrices = new Matrix[source.Technique_Common.Accessor.Count];
            for (int i = 0; i < source.Technique_Common.Accessor.Count; i++)
            {
                float[] values = new float[16];
                Array.Copy(matrixValues, i * 16, values, 0, 16);
                matrices[i] = GetMatrixFromArray(values);
            }
            return matrices;
        }

        private static Matrix GetMatrixFromArray(float[] matrixArray)
        {
            return new Matrix(
                matrixArray[0], matrixArray[4], matrixArray[8], matrixArray[12],
                matrixArray[1], matrixArray[5], matrixArray[9], matrixArray[13],
                matrixArray[2], matrixArray[6], matrixArray[10], matrixArray[14],
                matrixArray[3], matrixArray[7], matrixArray[11], matrixArray[15]);
        }

        private void WalkScene(GraphicsDevice graphicsDevice, Grendgine_Collada model, BoneData boneData, Dictionary<string, Grendgine_Collada_Geometry> geometryLibrary, Dictionary<string, Effect> effects, Dictionary<string, Grendgine_Collada_Controller> controllerLibrary)
        {
            foreach (var node in model.Library_Visual_Scene.Visual_Scene[0].Node)
            {
                WalkNode(graphicsDevice, node, -1, boneData, null, Matrix.Identity, geometryLibrary, effects, controllerLibrary);
            }
        }

        private void WalkNode(GraphicsDevice graphicsDevice, Grendgine_Collada_Node node, int parentBoneIndex, BoneData boneData, Mesh parentMesh, Matrix parentWorldTransform, Dictionary<string, Grendgine_Collada_Geometry> geometryLibrary, Dictionary<string, Effect> effects, Dictionary<string, Grendgine_Collada_Controller> controllerLibrary)
        {
            // TODO: These transformations should actually be performed in the order (or reverse) of that found in the file
            Matrix worldTransform = Matrix.Identity;
            if (node.Scale != null)
            {
                foreach (var scale in node.Scale)
                {
                    var scaleArray = scale.Value();
                    worldTransform *= Matrix.CreateScale(scaleArray[0], scaleArray[1], scaleArray[2]);
                }
            }
            if (node.Rotate != null)
            {
                Matrix rotationMatrix = Matrix.Identity;
                foreach (var rotation in node.Rotate)
                {
                    var rotationArray = rotation.Value();
                    rotationMatrix = Matrix.CreateFromAxisAngle(new Vector3(rotationArray[0], rotationArray[1], rotationArray[2]), rotationArray[3] / 180.0f * (float)Math.PI) * rotationMatrix;
                }
                worldTransform *= rotationMatrix;
            }
            if (node.Translate != null)
            {
                foreach (var translation in node.Translate)
                {
                    var translationArray = translation.Value();
                    worldTransform *= Matrix.CreateTranslation(translationArray[0], translationArray[1], translationArray[2]);
                }
            }
            if (node.Translate != null)
            {
                foreach (var translation in node.Translate)
                {
                    var translationArray = translation.Value();
                    worldTransform *= Matrix.CreateTranslation(translationArray[0], translationArray[1], translationArray[2]);
                }
            }
            if (node.Matrix != null)
            {
                foreach (var matrix in node.Matrix)
                {
                    var matrixArray = matrix.Value();
                    worldTransform *= GetMatrixFromArray(matrixArray);
                }
            }
            Mesh modelMesh = null;
            if (node.Type == Grendgine_Collada_Node_Type.NODE && (node.Instance_Geometry != null || node.Instance_Controller != null))
            {
                modelMesh = new Mesh(new ObservableCollection<MeshPart>(), parentMesh, worldTransform);
                if (node.Instance_Geometry != null)
                {
                    Dictionary<string, Effect> mappedEffects = GetNodeMappedEffects(node.Instance_Geometry[0].Bind_Material, effects);
                    LoadPolylist(graphicsDevice, geometryLibrary[node.Instance_Geometry[0].URL.TrimStart('#')].Mesh, modelMesh, worldTransform, mappedEffects);
                    LoadTriangles(graphicsDevice, geometryLibrary[node.Instance_Geometry[0].URL.TrimStart('#')].Mesh, modelMesh, worldTransform, mappedEffects);
                }
                else if (node.Instance_Controller != null)
                {
                    Dictionary<string, Effect> mappedEffects = GetNodeMappedEffects(node.Instance_Controller[0].Bind_Material, effects);
                    string sourceGeometry = controllerLibrary[node.Instance_Controller[0].URL.TrimStart('#')].Skin.SourceGeometry;
                    Grendgine_Collada_Mesh mesh = geometryLibrary[sourceGeometry.TrimStart('#')].Mesh;
                    LoadPolylist(graphicsDevice, mesh, modelMesh, worldTransform, mappedEffects);
                    LoadTriangles(graphicsDevice, mesh, modelMesh, worldTransform, mappedEffects);
                }
                Meshes.Add(modelMesh);
            }

            int boneIndex = -1;
            if (node.Type == Grendgine_Collada_Node_Type.JOINT)
            {
                boneData.BindPose.Add(worldTransform);
                boneData.InverseBindPose.Add(Matrix.Invert(worldTransform));
                boneData.SkeletonHierarchy.Add(parentBoneIndex);
                boneIndex = boneData.BoneIndices.Count;
                boneData.BoneIndices.Add(node.ID, boneIndex);
            }

            if (node.node != null)
            {
                foreach (var childNode in node.node)
                {
                    WalkNode(graphicsDevice, childNode, boneIndex, boneData, modelMesh, worldTransform, geometryLibrary, effects, controllerLibrary);
                }
            }
        }

        class BoneData
        {
            public List<Matrix> BindPose = new List<Matrix>();
            public List<Matrix> InverseBindPose = new List<Matrix>();
            public List<int> SkeletonHierarchy = new List<int>();
            public Dictionary<string, int> BoneIndices = new Dictionary<string, int>();

            public Dictionary<string, AnimationClip> AnimationClips;

            public BoneData() { }
        }

        private static Dictionary<string, Effect> GetNodeMappedEffects(Grendgine_Collada_Bind_Material[] bindMaterials, Dictionary<string, Effect> effects)
        {
            Dictionary<string, Effect> mappedEffects = new Dictionary<string, Effect>();
            foreach (var bindMaterial in bindMaterials)
            {
                foreach (var materialInstance in bindMaterial.Technique_Common.Instance_Material)
                {
                    mappedEffects.Add(materialInstance.Symbol, effects[materialInstance.Target.TrimStart('#')]);
                }
            }
            return mappedEffects;
        }

        private static Dictionary<string, Texture2D> GetTextures(GraphicsDevice graphicsDevice, Grendgine_Collada_Image[] images, string fileName)
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
            foreach (var image in images)
            {
                Texture2D fileTexture;
                using (FileStream fileStream = new FileStream(Path.Combine(Path.GetDirectoryName(fileName), image.File_Path.Replace("file://", "").Replace("file:///", "")), FileMode.Open))
                {
                    fileTexture = Texture2D.FromStream(graphicsDevice, fileStream);
                    textures.Add(image.ID, fileTexture);
                }
            }
            return textures;
        }

        private static Dictionary<string, Effect> GetEffects(GraphicsDevice graphicsDevice, Grendgine_Collada model, string fileName)
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
            if (model.Library_Images != null)
            {
                textures = GetTextures(graphicsDevice, model.Library_Images.Image, fileName);
            }

            Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
            foreach (var material in model.Library_Materials.Material)
            {
                foreach (var effect in model.Library_Effects.Effect)
                {
                    if (material.Instance_Effect.URL.EndsWith(effect.ID))
                    {
                        BasicEffect basicEffect = new BasicEffect(graphicsDevice);

                        Grendgine_Collada_FX_Common_Color_Or_Texture_Type textureType = effect.Profile_COMMON[0].Technique.Phong != null ?
                            effect.Profile_COMMON[0].Technique.Phong.Diffuse : effect.Profile_COMMON[0].Technique.Blinn.Diffuse;
                        if (textureType.Texture != null)
                        {
                            basicEffect.Texture = textures[textureType.Texture.Texture.Replace("-sampler", "")]; // All the hacks
                        }

                        effects.Add(material.ID, basicEffect);
                    }
                }
            }
            return effects;
        }

        private void LoadPolylist(GraphicsDevice graphicsDevice, Grendgine_Collada_Mesh mesh, Mesh modelMesh, Matrix worldTransform, Dictionary<string, Effect> effects)
        {
            if (mesh.Polylist != null)
            {
                foreach (var polylist in mesh.Polylist)
                {
                    Vector3[] vertexValues;
                    Vector3[] normalValues;
                    Vector2[] textureCoordinateValues;
                    int vertexOffset;
                    int normalOffset;
                    int textureCoordinateOffset;

                    GetVertexValuesAndOffsets(
                        mesh,
                        polylist.Input,
                        worldTransform,
                        out vertexValues,
                        out normalValues,
                        out textureCoordinateValues,
                        out vertexOffset,
                        out normalOffset,
                        out textureCoordinateOffset);

                    List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
                    List<short> indices = new List<short>();
                    int[] vertexIndexList = polylist.P.Value();
                    int vertexNumIndicies = polylist.Input.Length;
                    int nextPolyIndex = 0;
                    foreach (int numVerts in polylist.VCount.Value())
                    {
                        ExtractPolygonTriangles(
                            vertexIndexList,
                            vertexNumIndicies,
                            nextPolyIndex,
                            numVerts,
                            vertexValues,
                            normalValues,
                            textureCoordinateValues,
                            vertexOffset,
                            normalOffset,
                            textureCoordinateOffset,
                            vertices,
                            indices);

                        nextPolyIndex += vertexNumIndicies * numVerts;
                    }

                    string materialName = polylist.Material;
                    modelMesh.Parts.Add(new MeshPart(graphicsDevice, vertices.ToArray(), indices.ToArray(), effects[materialName]));
                }
            }
        }

        private void LoadTriangles(GraphicsDevice graphicsDevice, Grendgine_Collada_Mesh mesh, Mesh modelMesh, Matrix worldTransform, Dictionary<string, Effect> effects)
        {
            if (mesh.Triangles != null)
            {
                foreach (var triangle in mesh.Triangles)
                {
                    Vector3[] vertexValues;
                    Vector3[] normalValues;
                    Vector2[] textureCoordinateValues;
                    int vertexOffset;
                    int normalOffset;
                    int textureCoordinateOffset;

                    GetVertexValuesAndOffsets(
                        mesh,
                        triangle.Input,
                        worldTransform,
                        out vertexValues,
                        out normalValues,
                        out textureCoordinateValues,
                        out vertexOffset,
                        out normalOffset,
                        out textureCoordinateOffset);

                    List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
                    List<short> indices = new List<short>();
                    int[] vertexIndexList = triangle.P.Value();
                    int vertexNumIndicies = triangle.Input.Length;
                    int nextPolyIndex = 0;
                    for (int i = 0; i < triangle.Count; i++) // Changed this line
                    {
                        ExtractPolygonTriangles(
                            vertexIndexList,
                            vertexNumIndicies,
                            nextPolyIndex,
                            3,
                            vertexValues,
                            normalValues,
                            textureCoordinateValues,
                            vertexOffset,
                            normalOffset,
                            textureCoordinateOffset,
                            vertices,
                            indices);

                        nextPolyIndex += vertexNumIndicies * 3;
                    }

                    string materialName = triangle.Material;
                    modelMesh.Parts.Add(new MeshPart(graphicsDevice, vertices.ToArray(), indices.ToArray(), effects[materialName]));
                }
            }
        }

        private static void ExtractPolygonTriangles(
            int[] vertexIndexList,
            int vertexNumIndicies,
            int polyIndex,
            int numVerts,
            Vector3[] vertexValues,
            Vector3[] normalValues,
            Vector2[] textureCoordinateValues,
            int vertexOffset,
            int normalOffset,
            int textureCoordinateOffset,
            List<VertexPositionNormalTexture> vertices,
            List<short> indices)
        {
            int startVert = 0;
            while (startVert < numVerts - 2)
            {
                startVert++;
                int vertIndex0 = polyIndex + vertexNumIndicies * 0;
                int vertIndex1 = polyIndex + vertexNumIndicies * startVert;
                int vertIndex2 = polyIndex + vertexNumIndicies * (startVert + 1);
                vertices.Add(
                    new VertexPositionNormalTexture(
                        vertexValues[vertexIndexList[vertIndex2 + vertexOffset]],
                        normalValues[vertexIndexList[vertIndex2 + normalOffset]],
                        textureCoordinateValues[vertexIndexList[vertIndex2 + textureCoordinateOffset]]));
                indices.Add((short)(vertices.Count - 1));
                vertices.Add(
                    new VertexPositionNormalTexture(
                        vertexValues[vertexIndexList[vertIndex1 + vertexOffset]],
                        normalValues[vertexIndexList[vertIndex1 + normalOffset]],
                        textureCoordinateValues[vertexIndexList[vertIndex1 + textureCoordinateOffset]]));
                indices.Add((short)(vertices.Count - 1));
                vertices.Add(
                    new VertexPositionNormalTexture(
                        vertexValues[vertexIndexList[vertIndex0 + vertexOffset]],
                        normalValues[vertexIndexList[vertIndex0 + normalOffset]],
                        textureCoordinateValues[vertexIndexList[vertIndex0 + textureCoordinateOffset]]));
                indices.Add((short)(vertices.Count - 1));
            }
        }

        private static void GetVertexValuesAndOffsets(Grendgine_Collada_Mesh mesh, Grendgine_Collada_Input_Shared[] inputs, Matrix worldTransform, out Vector3[] vertexValues, out Vector3[] normalValues, out Vector2[] textureCoordinateValues, out int vertexOffset, out int normalOffset, out int textureCoordinateOffset)
        {
            vertexValues = new Vector3[1];
            normalValues = new Vector3[1];
            textureCoordinateValues = new Vector2[1];
            vertexOffset = 0;
            normalOffset = 0;
            textureCoordinateOffset = 0;
            foreach (var input in inputs)
            {
                foreach (var source in mesh.Source)
                {
                    if (input.source.EndsWith(source.ID) ||
                        (input.source.EndsWith(mesh.Vertices.ID) &&
                        mesh.Vertices.Input[0].source.EndsWith(source.ID))) // This is bollucks
                    {
                        if (input.Semantic == Grendgine_Collada_Input_Semantic.VERTEX)
                        {
                            vertexValues = GetVector3ArrayFromSource(source.Float_Array.Value(), worldTransform * Matrix.CreateRotationX(-(float)Math.PI / 2));
                            vertexOffset = input.Offset;
                        }
                        if (input.Semantic == Grendgine_Collada_Input_Semantic.NORMAL)
                        {
                            normalValues = GetVector3ArrayFromSource(source.Float_Array.Value(), worldTransform * Matrix.CreateRotationX(-(float)Math.PI / 2));
                            normalOffset = input.Offset;
                        }
                        if (input.Semantic == Grendgine_Collada_Input_Semantic.TEXCOORD)
                        {
                            textureCoordinateValues = GetUVArrayFromSource(source.Float_Array.Value(), source.Technique_Common.Accessor.Stride);
                            textureCoordinateOffset = input.Offset;
                        }
                    }
                }
            }
        }

        private static Vector3[] GetVector3ArrayFromSource(float[] values, Matrix worldTransform)
        {
            Vector3[] vectors = new Vector3[values.Length / 3];
            int vectorArrayIndex = 0;
            for (int i = 0; i < values.Length; i += 3)
            {
                vectors[vectorArrayIndex++] = Vector3.Transform(new Vector3(values[i], values[i + 1], values[i + 2]), worldTransform);
            }
            return vectors;
        }

        private static Vector2[] GetUVArrayFromSource(float[] values, uint stride)
        {
            Vector2[] vectors = new Vector2[values.Length / stride];
            int vectorArrayIndex = 0;
            for (int i = 0; i < values.Length; i += (int)stride)
            {
                vectors[vectorArrayIndex++] = new Vector2(values[i], 1f - values[i + 1]); // TODO: Why is this necessary?
            }
            return vectors;
        }

        #endregion


        /// <summary>
        /// Draws the model using the specified camera matrices.
        /// </summary>
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            foreach (var mesh in Meshes)
            {
                foreach (var meshPart in mesh.Parts)
                {
                    // Look up the effect, and set effect parameters on it. This sample
                    // assumes the model will only be using BasicEffect, but a more robust
                    // implementation would probably want to handle custom effects as well.
                    if (meshPart.Effect is AnimationUtilities.SkinnedEffect)
                    {
                        AnimationUtilities.SkinnedEffect effect = (AnimationUtilities.SkinnedEffect)meshPart.Effect;

                        effect.EnableDefaultLighting();

                        effect.World = world * mesh.AbsoluteWorldTransform;
                        effect.View = view;
                        effect.Projection = projection;
                    }
                    else if (meshPart.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)meshPart.Effect;

                        effect.EnableDefaultLighting();

                        effect.World = world * mesh.AbsoluteWorldTransform;
                        effect.View = view;
                        effect.Projection = projection;
                        effect.TextureEnabled = true;
                    }

                    // Set the graphics device to use our vertex declaration,
                    // vertex buffer, and index buffer.
                    GraphicsDevice device = meshPart.Effect.GraphicsDevice;

                    device.SetVertexBuffer(meshPart.VertexBuffer);

                    device.Indices = meshPart.IndexBuffer;

                    // Loop over all the effect passes.
                    foreach (EffectPass pass in meshPart.Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        // Draw the geometry.
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                                                     0, 0, meshPart.VertexCount,
                                                     0, meshPart.TriangleCount);
                    }
                }
            }
        }
    }
}

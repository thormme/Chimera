using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Collidables;

namespace GameConstructLibrary
{
    public class HeightMapBlock : IGameObject, IStaticCollidableOwner
    {
        public static CollisionGroup HeightMapPhysicsGroup = new CollisionGroup();

        public HeightMapRenderable Renderable { get { return mRenderable; } }
        private HeightMapRenderable mRenderable;

        private string mName;

        public HeightMapBlock(string name, Vector3 position, Quaternion orientation, Vector3 scale)
        {
            Position = position;
            XNAOrientationMatrix = Matrix.CreateFromQuaternion(orientation);
            OrientationQuaternion = orientation;
            Scale = scale;

            mName = name;

            CreateStaticCollidable();

            mRenderable = new HeightMapRenderable(mName);
        }

        #region Properties

        public void Render()
        {
            mRenderable.Render(Position, XNAOrientationMatrix, Scale);
        }

        public Vector3 Position
        {
            get;
            private set;
        }

        public World World
        {
            protected get;
            set;
        }

        public Matrix XNAOrientationMatrix
        {
            get;
            private set;
        }

        public Quaternion OrientationQuaternion
        {
            get;
            private set;
        }

        public Vector3 Scale
        {
            get;
            private set;
        }

        public StaticCollidable StaticCollidable
        {
            get;
            protected set;
        }

        #endregion

        private void CreateStaticCollidable()
        {
            float[,] heights = AssetLibrary.LookupHeightMap(mName).Mesh.Heights;
            Vector3 collidableScale = new Vector3(Scale.X / (float)(heights.GetLength(0) - 1), Scale.Y, Scale.Z / (float)(heights.GetLength(1) - 1));

            StaticCollidable = new Terrain(heights, new BEPUphysics.MathExtensions.AffineTransform(collidableScale, OrientationQuaternion, Position));
            StaticCollidable.CollisionRules.Group = HeightMapPhysicsGroup;
            StaticCollidable.Tag = this;
        }

        #region HeightMap Modification

        public void SetTerrain(Vector3 centerPosition, float radius, float intensity, bool isFeathered, bool isBlock)
        {
            HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName).Mesh;

            mesh.SetTerrain(centerPosition, radius, intensity, isFeathered, isBlock);

            CreateStaticCollidable();
        }

        public void SmoothTerrain(Vector3 centerPosition, float radius, bool isFeathered, bool isBlock)
        {
            HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName).Mesh;

            mesh.SmoothTerrain(centerPosition, radius, isFeathered, isBlock);

            CreateStaticCollidable();
        }

        public void FlattenTerrain(Vector3 centerPosition, float radius, bool isFeathered, bool isBlock)
        {
            HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName).Mesh;

            mesh.FlattenTerrain(centerPosition, radius, isFeathered, isBlock);

            CreateStaticCollidable();
        }

        public void LowerTerrain(Vector3 centerPosition, float radius, float intensity, bool isFeathered, bool isBlock)
        {
            HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName).Mesh;

            mesh.LowerTerrain(centerPosition, radius, intensity, isFeathered, isBlock);

            CreateStaticCollidable();
        }

        public void RaiseTerrain(Vector3 centerPosition, float radius, float intensity, bool isFeathered, bool isBlock)
        {
            HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName).Mesh;

            mesh.RaiseTerrain(centerPosition, radius, intensity, isFeathered, isBlock);

            CreateStaticCollidable();
        }

        #endregion

        #region Texture Modification

        public void PaintTexture(Vector3 centerPosition, float radius, float alpha, HeightMapMesh.TextureLayer layer, string texture, Vector2 UVOffset, Vector2 UVScale, bool isFeathered, bool isBlock)
        {
            HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName).Mesh;
            mesh.PaintTerrain(centerPosition, radius, alpha, layer, texture, UVOffset, UVScale, isFeathered, isBlock);
        }

        public void EraseTexture(Vector3 centerPosition, float radius, float alpha, HeightMapMesh.TextureLayer layer, string texture, Vector2 UVOffset, Vector2 UVScale, bool isFeathered, bool isBlock)
        {
            HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName).Mesh;
            mesh.EraseTerrain(centerPosition, radius, alpha, layer, texture, UVOffset, UVScale, isFeathered, isBlock);
        }

        public void BlendTexture(Vector3 centerPosition, float radius, bool isFeathered, bool isBlock)
        {
            HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName).Mesh;
            mesh.SmoothPaint(centerPosition, radius, isFeathered, isBlock);
        }

        #endregion
    }
}

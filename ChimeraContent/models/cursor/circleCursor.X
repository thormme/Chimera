xof 0303txt 0032
template KeyValuePair {
 <26e6b1c3-3d4d-4a1d-a437-b33668ffa1c2>
 STRING key;
 STRING value;
}

template Frame {
 <3d82ab46-62da-11cf-ab39-0020af71e433>
 [...]
}

template Matrix4x4 {
 <f6f23f45-7686-11cf-8f52-0040333594a3>
 array FLOAT matrix[16];
}

template FrameTransformMatrix {
 <f6f23f41-7686-11cf-8f52-0040333594a3>
 Matrix4x4 frameMatrix;
}

template ObjectMatrixComment {
 <95a48e28-7ef4-4419-a16a-ba9dbdf0d2bc>
 Matrix4x4 objectMatrix;
}

template Vector {
 <3d82ab5e-62da-11cf-ab39-0020af71e433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template MeshFace {
 <3d82ab5f-62da-11cf-ab39-0020af71e433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template Mesh {
 <3d82ab44-62da-11cf-ab39-0020af71e433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

template MeshNormals {
 <f6f23f43-7686-11cf-8f52-0040333594a3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}

template Coords2d {
 <f6f23f44-7686-11cf-8f52-0040333594a3>
 FLOAT u;
 FLOAT v;
}

template MeshTextureCoords {
 <f6f23f40-7686-11cf-8f52-0040333594a3>
 DWORD nTextureCoords;
 array Coords2d textureCoords[nTextureCoords];
}

template VertexElement {
 <f752461c-1e23-48f6-b9f8-8350850f336f>
 DWORD Type;
 DWORD Method;
 DWORD Usage;
 DWORD UsageIndex;
}

template DeclData {
 <bf22e553-292c-4781-9fea-62bd554bdd93>
 DWORD nElements;
 array VertexElement Elements[nElements];
 DWORD nDWords;
 array DWORD data[nDWords];
}

template ColorRGBA {
 <35ff44e0-6c7c-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <d3e16e81-7835-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template Material {
 <3d82ab4d-62da-11cf-ab39-0020af71e433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template MeshMaterialList {
 <f6f23f42-7686-11cf-8f52-0040333594a3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material <3d82ab4d-62da-11cf-ab39-0020af71e433>]
}

template TextureFilename {
 <a42790e1-7810-11cf-8f52-0040333594a3>
 STRING filename;
}


KeyValuePair {
 "Date";
 "2013-03-17 16:53:05";
}

KeyValuePair {
 "File";
 "D:\\Documents\\GitHub\\eecs494FinalProject\\Chimera\\ChimeraContent\\workInProgress\\models\\Cursor\\cursor.max";
}

KeyValuePair {
 "User";
 "401";
}

KeyValuePair {
 "CoreTime";
 "0";
}

Frame Plane001 {
 

 FrameTransformMatrix relative {
  1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,-0.000317,0.000000,0.000792,1.000000;;
 }

 ObjectMatrixComment object {
  1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000;;
 }

 Mesh mesh_Plane001 {
  4;
  -0.012500;0.000000;0.012500;,
  -0.012500;0.000000;-0.012500;,
  0.012500;0.000000;0.012500;,
  0.012500;0.000000;-0.012500;;
  2;
  3;0,1,2;,
  3;3,2,1;;

  MeshNormals normals {
   4;
   0.000000;1.000000;0.000000;,
   0.000000;1.000000;0.000000;,
   0.000000;1.000000;0.000000;,
   0.000000;1.000000;0.000000;;
   2;
   3;0,1,2;,
   3;3,2,1;;
  }

  MeshTextureCoords tc0 {
   4;
   0.000000;0.000000;,
   0.000000;1.000000;,
   1.000000;0.000000;,
   1.000000;1.000000;;
  }

  DeclData {
   2;
   2;0;7;0;,
   2;0;6;0;;
   24;
   0,
   0,
   1065353216,
   1065353216,
   0,
   0,
   0,
   0,
   1065353216,
   1065353216,
   0,
   0,
   0,
   0,
   1065353216,
   1065353216,
   0,
   0,
   0,
   0,
   1065353216,
   1065353216,
   0,
   0;
  }

  MeshMaterialList mtls {
   1;
   2;
   0,
   0;

   Material _01___Default {
    0.588235;0.588235;0.588235;1.000000;;
    9.999999;
    0.900000;0.900000;0.900000;;
    0.000000;0.000000;0.000000;;

    TextureFilename Diffuse {
     "cursorTexture.png";
    }
   }
  }
 }
}
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
 "2013-05-02 13:38:10";
}

KeyValuePair {
 "File";
 "D:\\Users\\Josh\\My Documents\\GitHub\\eecs494FinalProject\\ChimeraContent\\workInProgress\\models\\Gizmo\\rotateGizmo_thick.max";
}

KeyValuePair {
 "User";
 "Josh";
}

KeyValuePair {
 "CoreTime";
 "0";
}

Frame Torus002 {
 

 FrameTransformMatrix relative {
  0.903812,0.000000,0.000000,0.000000,0.000000,-0.000000,-0.903812,0.000000,0.000000,0.903812,-0.000000,0.000000,0.000000,0.000000,0.000000,1.000000;;
 }

 ObjectMatrixComment object {
  1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,-0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000;;
 }

 Mesh mesh_Torus002 {
  20;
  -0.257768;0.000079;0.000000;,
  -0.215206;0.000290;0.019942;,
  -0.257768;0.000079;0.023886;,
  -0.215206;0.000290;0.000000;,
  -0.207878;0.000290;0.059146;,
  -0.248990;0.000079;0.070844;,
  -0.193470;0.000290;0.096337;,
  -0.231733;0.000079;0.115389;,
  -0.172474;0.000290;0.130246;,
  -0.206584;0.000079;0.156005;,
  -0.145605;0.000290;0.159721;,
  -0.174401;0.000079;0.191309;,
  -0.113777;0.000290;0.183756;,
  -0.136278;0.000079;0.220097;,
  -0.078074;0.000290;0.201534;,
  -0.093515;0.000079;0.241391;,
  -0.039713;0.000290;0.212448;,
  -0.047567;0.000079;0.254464;,
  -0.000000;0.000290;0.216128;,
  -0.000000;0.000079;0.258872;;
  18;
  3;0,2,1;,
  3;0,1,3;,
  3;2,5,4;,
  3;2,4,1;,
  3;5,7,6;,
  3;5,6,4;,
  3;7,9,8;,
  3;7,8,6;,
  3;9,11,10;,
  3;9,10,8;,
  3;11,13,12;,
  3;11,12,10;,
  3;13,15,14;,
  3;13,14,12;,
  3;15,17,16;,
  3;15,16,14;,
  3;17,19,18;,
  3;17,18,16;;

  MeshNormals normals {
   20;
   -0.005465;1.106412;-0.000000;,
   -0.005419;1.106412;0.000502;,
   -0.005419;1.106412;0.000502;,
   -0.005465;1.106412;0.000000;,
   -0.005234;1.106412;0.001489;,
   -0.005234;1.106412;0.001489;,
   -0.004871;1.106412;0.002426;,
   -0.004871;1.106412;0.002426;,
   -0.004343;1.106412;0.003279;,
   -0.004343;1.106412;0.003279;,
   -0.003666;1.106412;0.004022;,
   -0.003666;1.106412;0.004022;,
   -0.002865;1.106412;0.004627;,
   -0.002865;1.106412;0.004627;,
   -0.001966;1.106412;0.005074;,
   -0.001966;1.106412;0.005074;,
   -0.001000;1.106412;0.005349;,
   -0.001000;1.106412;0.005349;,
   -0.000504;1.106412;0.005442;,
   -0.000504;1.106412;0.005442;;
   18;
   3;0,2,1;,
   3;0,1,3;,
   3;2,5,4;,
   3;2,4,1;,
   3;5,7,6;,
   3;5,6,4;,
   3;7,9,8;,
   3;7,8,6;,
   3;9,11,10;,
   3;9,10,8;,
   3;11,13,12;,
   3;11,12,10;,
   3;13,15,14;,
   3;13,14,12;,
   3;15,17,16;,
   3;15,16,14;,
   3;17,19,18;,
   3;17,18,16;;
  }

  MeshTextureCoords tc0 {
   20;
   0.000000;0.264706;,
   0.333333;0.235294;,
   0.000000;0.235294;,
   0.333333;0.264706;,
   0.333333;0.205882;,
   0.000000;0.205882;,
   0.333333;0.176471;,
   0.000000;0.176471;,
   0.333333;0.147059;,
   0.000000;0.147059;,
   0.333333;0.117647;,
   0.000000;0.117647;,
   0.333333;0.088235;,
   0.000000;0.088235;,
   0.333333;0.058824;,
   0.000000;0.058824;,
   0.333333;0.029412;,
   0.000000;0.029412;,
   0.333333;0.000000;,
   0.000000;0.000000;;
  }

  MeshMaterialList mtls {
   1;
   18;
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0;

   Material _01___Default {
    0.588235;0.588235;0.588235;1.000000;;
    9.999999;
    0.900000;0.900000;0.900000;;
    0.000000;0.000000;0.000000;;

    TextureFilename Diffuse {
     "gizmoTexture.png";
    }
   }
  }
 }
}
xof 0303txt 0032
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

template TextureFilename {
 <a42790e1-7810-11cf-8f52-0040333594a3>
 STRING filename;
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

template MeshMaterialList {
 <f6f23f42-7686-11cf-8f52-0040333594a3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material <3d82ab4d-62da-11cf-ab39-0020af71e433>]
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


Material _01_-_Default {
 1.000000;1.000000;1.000000;1.000000;;
 9.999999;
 0.000000;0.000000;0.000000;;
 0.000000;0.000000;0.000000;;

 TextureFilename {
  "bricks.png";
 }
}

Frame Plane001 {
 

 FrameTransformMatrix {
  1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000;;
 }

 Mesh Plane001 {
  15;
  -20.000000;0.000000;-10.000000;,
  -10.000000;0.000000;-10.000000;,
  -20.000000;0.000000;-20.000000;,
  -20.000000;0.000000;0.000000;,
  -10.000000;0.000000;0.000000;,
  0.000000;0.000000;0.000000;,
  -20.000000;0.000000;10.000000;,
  -10.000000;0.000000;10.000000;,
  0.000000;0.000000;10.000000;,
  10.000000;0.000000;10.000000;,
  -20.000000;0.000000;20.000000;,
  -10.000000;0.000000;20.000000;,
  0.000000;0.000000;20.000000;,
  10.000000;0.000000;20.000000;,
  20.000000;0.000000;20.000000;;
  16;
  3;0,1,2;,
  3;3,4,0;,
  3;1,0,4;,
  3;4,5,1;,
  3;6,7,3;,
  3;4,3,7;,
  3;7,8,4;,
  3;5,4,8;,
  3;8,9,5;,
  3;10,11,6;,
  3;7,6,11;,
  3;11,12,7;,
  3;8,7,12;,
  3;12,13,8;,
  3;9,8,13;,
  3;13,14,9;;

  MeshMaterialList {
   1;
   16;
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
   { _01_-_Default }
  }

  DeclData {
   2;
   1;0;5;0;,
   2;0;3;0;;
   75;
   0,
   1061158912,
   0,
   1065353216,
   0,
   1048576000,
   1061158912,
   0,
   1065353216,
   0,
   0,
   1065353216,
   0,
   1065353216,
   0,
   0,
   1056964608,
   0,
   1065353216,
   0,
   1048576000,
   1056964608,
   0,
   1065353216,
   0,
   1056964608,
   1056964608,
   0,
   1065353216,
   0,
   0,
   1048576000,
   0,
   1065353216,
   0,
   1048576000,
   1048576000,
   0,
   1065353216,
   0,
   1056964608,
   1048576000,
   0,
   1065353216,
   0,
   1061158912,
   1048576000,
   0,
   1065353216,
   0,
   0,
   0,
   0,
   1065353216,
   0,
   1048576000,
   0,
   0,
   1065353216,
   0,
   1056964608,
   0,
   0,
   1065353216,
   0,
   1061158912,
   0,
   0,
   1065353216,
   0,
   1065353216,
   0,
   0,
   1065353216,
   0;
  }
 }
}
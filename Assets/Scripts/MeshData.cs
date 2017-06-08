using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MeshData {
	public List<Vector3> vertices; 
	public List<int> triangles;
	public List<Vector2> uv;
	public List<List<Vector2>> edges;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshGenerator))]

public class Chunk : MonoBehaviour {

	public float[ , ] densityMatrix = new float[chunkSize,chunkSize];
	public static int chunkSize = 32;
	public bool update = true;
	public Position position;
	public Entity entity;
	public bool renderforCollider;

	private MeshGenerator meshGenerator;
	private MeshFilter filter;
	private List<List<Vector2>> edges;
	private PolygonCollider2D coll;

	void Start () {
		filter = GetComponent<MeshFilter> ();
		meshGenerator = GetComponent<MeshGenerator> ();
		coll = GetComponent<PolygonCollider2D> ();
	}
	

	void Update () {
		if (update) {
			UpdateMesh ();
			update = false;
		}
	}

	public virtual void UpdateMesh(){
		MeshData meshData = new MeshData ();

		meshData = meshGenerator.GenerateMesh (densityMatrix, Entity.scale, renderforCollider, meshData);

		filter.mesh.Clear();
		filter.mesh.vertices = meshData.vertices.ToArray();
		filter.mesh.triangles = meshData.triangles.ToArray();
		filter.mesh.uv = meshData.uv.ToArray();
		filter.mesh.RecalculateNormals();
		coll.enabled = false;

		if (renderforCollider) {
			edges = meshData.edges;
			coll.pathCount = edges.Count;
			int n = 0;
			foreach (List<Vector2> edge in edges) {
				coll.SetPath (n++, edge.ToArray ());
			}
			coll.enabled = true;
		}
	}

	/*
	void OnDrawGizmos()
	{
		if (edges != null) {
			Gizmos.color = Color.yellow;
			foreach (List<Vector2> edge in edges) {
				int n = edge.Count;
				Vector2[] points = new Vector2[n];
				points = edge.ToArray ();
				Vector3 start = new Vector3();
				start.x = points [0].x + transform.position.x;
				start.y = points [0].y + transform.position.y;
				start.z = 0;
				for (int i = 1; i <= n; i++){
					Gizmos.DrawLine (start, new Vector3 (points [i%points.Length].x + transform.position.x, points [i%points.Length].y +  transform.position.y, 0));
					start.x = points [i%points.Length].x + transform.position.x;
					start.y = points [i%points.Length].y + transform.position.y;
				}
			}	
		}
				
	}
	*/
}

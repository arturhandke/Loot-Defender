using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour {

	public SquareGrid squareGrid;
	List<Vector3> vertices;
	List<int> triangles;
	List<Vector2> uvs;
	List<List<Vector2>> colliderPolygons;

	public MeshData GenerateMesh(float[,] map, float squareSize, bool renderforCollider, MeshData meshData) {
		squareGrid = new SquareGrid(map, squareSize);
		vertices = new List<Vector3>();
		triangles = new List<int>();
		uvs = new List<Vector2> ();
		if (renderforCollider)
			colliderPolygons = new List<List<Vector2>> ();

		for (int x = 0; x < squareGrid.squares.GetLength(0); x ++) {
			for (int y = 0; y < squareGrid.squares.GetLength(1); y ++) {
				TriangulateSquare(squareGrid.squares[x,y]);
				if(renderforCollider) edgeFinder (squareGrid.squares [x, y]);
			}
		}
		meshData.vertices = vertices;
		meshData.triangles = triangles;
		meshData.uv = uvs;
		if (renderforCollider)
			meshData.edges = colliderPolygons;

		return meshData;
	}

	void TriangulateSquare(Square square) {
		switch (square.configuration) {
		case 0:
			break;

			// 1 points:
		case 1:
			MeshFromPoints(square.centreBottom, square.bottomLeft, square.centreLeft);
			break;
		case 2:
			MeshFromPoints(square.centreRight, square.bottomRight, square.centreBottom);
			break;
		case 4:
			MeshFromPoints(square.centreTop, square.topRight, square.centreRight);
			break;
		case 8:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
			break;

			// 2 points:
		case 3:
			MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
			break;
		case 6:
			MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
			break;
		case 9:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
			break;
		case 12:
			MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
			break;
		case 5:
			MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
			break;
		case 10:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
			break;

			// 3 point:
		case 7:
			MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
			break;
		case 11:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
			break;
		case 13:
			MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
			break;
		case 14:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
			break;

			// 4 point:
		case 15:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
			break;
		}
	}



	void MeshFromPoints(params Node[] points) {
		AssignVertices(points);

		if (points.Length >= 3)
			CreateTriangle(points[0], points[1], points[2]);
		if (points.Length >= 4)
			CreateTriangle(points[0], points[2], points[3]);
		if (points.Length >= 5) 
			CreateTriangle(points[0], points[3], points[4]);
		if (points.Length >= 6)
			CreateTriangle(points[0], points[4], points[5]);
	}

	void AssignVertices(Node[] points) {
		for (int i = 0; i < points.Length; i ++) {
			if (points[i].vertexIndex == -1) {
				points[i].vertexIndex = vertices.Count;
				vertices.Add(points[i].position);
				uvs.Add (new Vector2 (points [i].position.x, points [i].position.y));
			}
		}
	}

	void CreateTriangle(Node a, Node b, Node c) {
		triangles.Add(a.vertexIndex);
		triangles.Add(b.vertexIndex);
		triangles.Add(c.vertexIndex);
	}

	void edgeFinder(Square square)
	{
					switch (square.configuration) {
					case 0:
						break;
					case 1:
						colliderPolygons.Add (ColliderFromPoints (square.centreBottom, square.bottomLeft, square.centreLeft));
						break;
					case 2:
						colliderPolygons.Add (ColliderFromPoints (square.centreRight, square.bottomRight, square.centreBottom));
						break;
					case 3:
						colliderPolygons.Add (ColliderFromPoints (square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft));
						break;
					case 4:
						colliderPolygons.Add (ColliderFromPoints (square.centreTop, square.topRight, square.centreRight));
						break;
					case 5:
						colliderPolygons.Add (ColliderFromPoints (square.centreTop, square.topRight, square.bottomRight));
						colliderPolygons.Add (ColliderFromPoints (square.centreBottom, square.bottomLeft, square.centreLeft));
						break;
					case 6:
						colliderPolygons.Add (ColliderFromPoints (square.centreTop, square.topRight, square.bottomRight, square.centreBottom));
						break;
					case 7:
						colliderPolygons.Add (ColliderFromPoints (square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft));	
						break;
					case 8:
						colliderPolygons.Add (ColliderFromPoints (square.centreLeft, square.topLeft, square.centreTop));
						break;
					case 9:
						colliderPolygons.Add (ColliderFromPoints (square.centreTop, square.centreBottom, square.bottomLeft, square.topLeft));
						break;
					case 10:
						colliderPolygons.Add (ColliderFromPoints (square.centreLeft, square.topLeft, square.centreTop));
						colliderPolygons.Add (ColliderFromPoints (square.centreRight, square.bottomRight, square.centreBottom));
						break;
					case 11:
						colliderPolygons.Add (ColliderFromPoints (square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft, square.topLeft));	
						break;
					case 12:
						colliderPolygons.Add (ColliderFromPoints (square.centreRight, square.centreLeft, square.topLeft, square.topRight));
						break;
					case 13:
						colliderPolygons.Add (ColliderFromPoints (square.centreRight, square.centreBottom, square.bottomLeft, square.topLeft, square.topRight));	
						break;
					case 14:
						colliderPolygons.Add (ColliderFromPoints (square.centreBottom, square.centreLeft, square.topLeft, square.topRight, square.bottomRight));	
						break;
					case 15:
						break;
					}
	}

	List<Vector2> ColliderFromPoints(params Node[] points) {
		List<Vector2> edge = new List<Vector2> ();

		for (int i = 0; i < points.Length; i++) {
			edge.Add (new Vector2 (points [i].position.x, points [i].position.y));
		}

		return edge;
	}

	public class SquareGrid {
		public Square[,] squares;

		public SquareGrid(float[,] map, float squareSize) {
			int nodeCountX = map.GetLength(0);
			int nodeCountY = map.GetLength(1);

			ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

			for (int x = 0; x < nodeCountX; x++) {
				for (int y = 0; y < nodeCountY; y++) {
					Vector3 pos = new Vector3(x * squareSize, y * squareSize, 0);
					controlNodes[x,y] = new ControlNode(pos, map[x,y]);
				}
			}

			squares = new Square[nodeCountX -1,nodeCountY -1];
			for (int x = 0; x < nodeCountX-1; x ++) {
				for (int y = 0; y < nodeCountY-1; y ++) {
					squares[x,y] = new Square(controlNodes[x,y+1], controlNodes[x+1,y+1], controlNodes[x+1,y], controlNodes[x,y], squareSize);
				}
			}

		}
	}

	public class Square {

		public ControlNode topLeft, topRight, bottomRight, bottomLeft;
		public Node centreTop, centreRight, centreBottom, centreLeft;
		public int configuration;

		public Square (ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft, float squareSize) {
			topLeft = _topLeft;
			topRight = _topRight;
			bottomRight = _bottomRight;
			bottomLeft = _bottomLeft;

			centreTop = topLeft.right;
			centreRight = bottomRight.above;
			centreBottom = bottomLeft.right;
			centreLeft = bottomLeft.above;

			centreTop.position = topLeft.position + Vector3.right * squareSize * ( - topLeft.density)/(topRight.density - topLeft.density);
			centreRight.position = bottomRight.position + Vector3.up * squareSize * ( - bottomRight.density)/(topRight.density - bottomRight.density);
			centreBottom.position = bottomLeft.position + Vector3.right * squareSize * ( - bottomLeft.density)/(bottomRight.density - bottomLeft.density);
			centreLeft.position = bottomLeft.position + Vector3.up * squareSize * ( - bottomLeft.density)/(topLeft.density - bottomLeft.density);

			if (topLeft.active)
				configuration += 8;
			if (topRight.active)
				configuration += 4;
			if (bottomRight.active)
				configuration += 2;
			if (bottomLeft.active)
				configuration += 1;
		}
	}

	public class Node {
		public Vector3 position;
		public int vertexIndex = -1;

		public Node(Vector3 _pos) {
			position = _pos;
		}
	}

	public class ControlNode : Node {

		public bool active;
		public float density;
		public Node above, right;

		public ControlNode(Vector3 _pos, float _density) : base(_pos) {
			density = _density;
			if(density>0) active = true;
				else active = false;
			above = new Node(position);
			right = new Node(position);
		}
	}

}
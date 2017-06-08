using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class Entity : MonoBehaviour {
	private Dictionary<Position, Chunk> chunks = new Dictionary<Position, Chunk>();
	public GameObject chunkPrefab;
	public int sizeX, sizeY;
	public static float scale = 0.2f;
	public float noiseScale;
	[Range(0, 10)]
	public float noiseOffsetX, noiseOffsetY;
	[Range(0,1)]
	public float threshold;
	public bool renderForCollider;
	public Camera cam;

	[HideInInspector]
	public int realSizeX, realSizeY;
	public float[,] densityMatrix;

	private int topLimit, bottomLimit, leftLimit, rightLimit;
	private int chunkStep = Chunk.chunkSize - 1;

	void Start () {
		realSizeX = sizeX * Chunk.chunkSize - sizeX + 1;
		realSizeY = sizeY * Chunk.chunkSize - sizeY + 1;
		densityMatrix = new float[realSizeX, realSizeY];

		CreateDensityMatrix ();
		GenerateChunks ();
	}

	void Update () {
		Wrapp ();
	}

	public virtual void CreateDensityMatrix()
	{
		for (int x = 0; x < realSizeX; x++)
			for (int y = 0; y < realSizeY; y++) {
				densityMatrix [x, y] = threshold - Mathf.PerlinNoise (x * noiseScale + noiseOffsetX, y * noiseScale + noiseOffsetY);
			}

	}

	public void GenerateChunks()
	{
		int cx = 0, cy = 0;

		for (cx = 0; cx < realSizeX -1; cx+= chunkStep)
			for (cy = 0; cy < realSizeY -1; cy+= chunkStep) {

				Position pos = new Position ();
				pos.x = cx;
				pos.y = cy;

				GameObject newChunkObject = Instantiate(
					chunkPrefab, new Vector3(cx * scale, cy * scale),
					Quaternion.Euler(Vector3.zero), this.transform
				) as GameObject;

				Chunk newChunk = newChunkObject.GetComponent<Chunk>();
				newChunk.position = pos;
				newChunk.entity = this;
				newChunk.renderforCollider = renderForCollider;

				for (int x = 0; x < Chunk.chunkSize; x++)
					for (int y = 0; y < Chunk.chunkSize; y++) {
						newChunk.densityMatrix [x, y] = densityMatrix[cx + x, cy + y];
					}

				chunks.Add (pos, newChunk);
			}
		topLimit = cy;
		rightLimit = cx;
		bottomLimit = 0;
		leftLimit = 0;
	}

	public virtual void Wrapp()
	{
		float camLeftLimit, camRightLimit, camTopLimit, camBottomLimit;
		float deltaLeft, deltaRight, deltaTop, deltaBottom;

		camLeftLimit = cam.ViewportToWorldPoint (new Vector3 (0, 0)).x;
		camRightLimit = cam.ViewportToWorldPoint (new Vector3 (1, 1)).x;
		camBottomLimit = cam.ViewportToWorldPoint (new Vector3 (0, 0)).y;
		camTopLimit = cam.ViewportToWorldPoint (new Vector3 (1, 1)).y;

		deltaLeft = Mathf.Abs(camLeftLimit - leftLimit * scale);
		deltaRight = Mathf.Abs(camRightLimit - rightLimit * scale);
		deltaBottom = Mathf.Abs(camBottomLimit - bottomLimit * scale);
		deltaTop = Mathf.Abs(camTopLimit - topLimit * scale);

		if (deltaLeft < chunkStep * scale) {
			for (int y = bottomLimit; y < topLimit - 1; y += chunkStep) {
				Chunk containerChunk = null;
				containerChunk = GetChunk (rightLimit - chunkStep, y);
				if (containerChunk != null) {
					containerChunk.transform.position -= new Vector3 ((realSizeX - 1) * scale, 0, 0);
					chunks.Remove (containerChunk.position);
					containerChunk.position.x -= realSizeX - 1;
					chunks.Add (containerChunk.position, containerChunk);
				}
			}
			leftLimit -= chunkStep;
			rightLimit -= chunkStep;
		}

		if (deltaRight < chunkStep * scale) {
			for (int y = bottomLimit; y < topLimit - 1; y += chunkStep) {
				Chunk containerChunk = null;
				containerChunk = GetChunk (leftLimit, y);
				if (containerChunk != null) {
					containerChunk.transform.position += new Vector3 ((realSizeX - 1) * scale, 0, 0);
					chunks.Remove (containerChunk.position);
					containerChunk.position.x += realSizeX - 1 ;
					chunks.Add (containerChunk.position, containerChunk);
				}
			}
			leftLimit += chunkStep;
			rightLimit += chunkStep;
		}

		if (deltaBottom < chunkStep * scale) {
			for (int x = leftLimit; x < rightLimit - 1; x += chunkStep) {
				Chunk containerChunk = null;
				containerChunk = GetChunk (x, topLimit - chunkStep);
				if (containerChunk != null) {
					containerChunk.transform.position -= new Vector3 (0, (realSizeY - 1) * scale, 0);
					chunks.Remove (containerChunk.position);
					containerChunk.position.y -= realSizeY - 1;
					chunks.Add (containerChunk.position, containerChunk);
				}
			}
			bottomLimit -= chunkStep;
			topLimit -= chunkStep;
		}

		if (deltaTop < chunkStep * scale) {
			for (int x = leftLimit; x < rightLimit - 1; x += chunkStep) {
				Chunk containerChunk = null;
				containerChunk = GetChunk (x, bottomLimit);
				if (containerChunk != null) {
					containerChunk.transform.position += new Vector3 (0, (realSizeY - 1) * scale, 0);
					chunks.Remove (containerChunk.position);
					containerChunk.position.y += realSizeY - 1;
					chunks.Add (containerChunk.position, containerChunk);
				}
			}
			bottomLimit += chunkStep;
			topLimit += chunkStep;
		}
	}

	public Chunk GetChunk(int x, int y)
	{
		Position pos = new Position();
		pos.x = Mathf.FloorToInt((float)x / chunkStep) * chunkStep;
		pos.y = Mathf.FloorToInt((float)y / chunkStep) * chunkStep;
		Chunk containerChunk = null;
		chunks.TryGetValue(pos, out containerChunk);

		return containerChunk;
	}

//	void OnDrawGizmos(){
//		Gizmos.color = Color.white;
//		Gizmos.DrawLine (new Vector3 (leftLimit, bottomLimit) * scale, new Vector3 (leftLimit, topLimit) * scale); 
//		Gizmos.DrawLine (new Vector3 (leftLimit, bottomLimit) * scale, new Vector3 (rightLimit, bottomLimit) * scale); 
//		Gizmos.DrawLine (new Vector3 (leftLimit, topLimit) * scale, new Vector3 (rightLimit, topLimit) * scale); 
//		Gizmos.DrawLine (new Vector3 (rightLimit, bottomLimit) * scale, new Vector3 (rightLimit, topLimit) * scale); 
//	}

	/*
	private bool LoadBlocks(string fileName)
	{
		int x=0, y=0;
		try
		{
			string line;
			fileName = Application.dataPath + "/Resources/" + fileName;
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			using (theReader){
				line = theReader.ReadLine();
				if(line != null){
					do
					{
						string[] entries = line.Split(' ');
						x = entries.Length;
						if (x > 0){
							for(int i=0; i<x; i++)
							{
								Position pos = new Position();
								pos.x = i;
								pos.y = y;
								float d = float.Parse(entries[i]);
								blocks.Add(pos, new Block(d));
							}
						}
						y++;
						line = theReader.ReadLine();
					}
					while (line != null);
				} 
				theReader.Close();
				sizeX = x;
				sizeY = y;
				return true;
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError (e.Message);
			return false;
		}
	}
	*/
}

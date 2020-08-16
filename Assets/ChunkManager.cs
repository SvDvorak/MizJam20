using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
	public ChunkSettings ChunkSettings;
	public static int ChunkSize;
	private Transform gameCamera;
	private readonly Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

	void Start()
	{
		var cam = Camera.main;
		gameCamera = cam.transform;
		ChunkSize = 31;
		//float height = Camera.main.orthographicSize * 2.0f;
		//float width = height * Screen.width / Screen.height;
		//ChunkSize = (int)(Mathf.Max(height, width) + 1f);
	}

	void Update()
	{

		foreach (var chunk in chunks.Values)
		{
			chunk.IsActive = false;
		}

		var cameraChunkPosition = WorldToChunk(gameCamera.position);

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				var pos = new Vector2Int(x, y) + cameraChunkPosition;
				if (!chunks.ContainsKey(pos))
					chunks.Add(pos, new Chunk(ChunkSettings, transform, ChunkToWorld(pos)));
				else
					chunks[pos].IsActive = true;
			}
		}

		List<Vector2Int> ToDestroy = new List<Vector2Int>();
		foreach (var chunkKeyPair in chunks)
		{
			var chunk = chunkKeyPair.Value;
			if (!chunk.IsActive)
			{
				chunk.Destroy();
				ToDestroy.Add(chunkKeyPair.Key);
			}
		}

		foreach (var chunkPos in ToDestroy)
		{
			chunks.Remove(chunkPos);
		}
	}

	Vector2Int WorldToChunk(Vector3 position)
	{
		return position.ToV2Int() / ChunkSize;
	}

	Vector3 ChunkToWorld(Vector2Int position)
	{
		return position.ToV3() * ChunkSize;
	}
}
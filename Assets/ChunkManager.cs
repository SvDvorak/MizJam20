using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	public GameObject GameObject;
	public bool IsActive = true;

	public Chunk(Transform parent, Vector3 pos)
	{
		Debug.Log($"Created chunk at {pos}");
		GameObject = new GameObject("Chunk");
		GameObject.transform.SetParent(parent);
		GameObject.transform.position = pos;
		var collider = GameObject.AddComponent<BoxCollider2D>();
		collider.isTrigger = true;
		collider.size = new Vector2(ChunkManager.ChunkSize, ChunkManager.ChunkSize);
	}

	public void Destroy()
	{
		Debug.Log($"Destroyed chunk at {GameObject.transform.position}");
		Object.Destroy(GameObject);
	}
}

public class ChunkManager : MonoBehaviour
{
	public GameObject Prefab;
	//public int TreeCount;
	//public List<WindAnimation> Trees = new List<WindAnimation>();
	public static int ChunkSize;
	private Transform gameCamera;
	private readonly Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

	void Start()
	{
		var cam = Camera.main;
		gameCamera = cam.transform;
		float height = Camera.main.orthographicSize * 2.0f;
		float width = height * Screen.width / Screen.height;
		ChunkSize = (int)Mathf.Max(height, width);
		Debug.Log(ChunkSize);
		//for(int i = 0; i < TreeCount; i++)
		//{
		//	var tree = Instantiate(Prefab, transform);
		//	tree.transform.position = new Vector3(Random.Range(-13, 13), Random.Range(-7, 7));
		//	Trees.Add(tree.GetComponent<WindAnimation>());
		//}
	}

	void Update()
	{
		//foreach (var tree in Trees)
		//{
		//	tree.UpdateAnimation();
		//}


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
					chunks.Add(pos, new Chunk(transform, ChunkToWorld(pos)));
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

public static class Extensions
{
	public static Vector3 ToV3(this Vector2Int vector)
	{
		return new Vector3(vector.x, vector.y, 0);
	}

	public static Vector2Int ToV2Int(this Vector3 vector)
	{
		return new Vector2Int((int)vector.x, (int)vector.y);
	}
}

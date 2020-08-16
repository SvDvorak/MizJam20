using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	public GameObject GameObject;
	public bool IsActive = true;
	private readonly ChunkSettings chunkSettings;
	private readonly List<GameObject> objects = new List<GameObject>();

	public Chunk(ChunkSettings chunkSettings, Transform parent, Vector3 pos)
	{
		this.chunkSettings = chunkSettings;
		GameObject = new GameObject("Chunk");
		GameObject.transform.SetParent(parent);
		GameObject.transform.position = pos;

		CreateTrees();
	}

	public void CreateTrees()
	{
		for(int i = 0; i < chunkSettings.TreeCount; i++)
		{
			var tree = CreateChunkObject(chunkSettings.TreeTemplate, true);
			tree.GetComponent<SpriteRenderer>().sprite = chunkSettings.TreeVariations.GetRandom();
			var foundSpot = FindSpot();
			if(foundSpot.HasValue)
				tree.transform.position = foundSpot.Value;
		}
	}

	private Vector3? FindSpot()
	{
		var stuckLoopCount = 0;
		while(stuckLoopCount < 100)
		{
			var s = ChunkManager.ChunkSize / 2f;
			var pos = GameObject.transform.position;
			var foundSpot = new Vector2(pos.x + Random.Range(-s, s), pos.y + Random.Range(-s, s));
			var angle = 0f;
			var overlap = Physics2D.OverlapBox(foundSpot, new Vector2(1, 1), angle);
			if (overlap == null)
			{
				return foundSpot;
			}
			else
			{
				//Debug.Log($"Overlap at {foundSpot}!");
				stuckLoopCount += 1;
			}
		}

		Debug.Log("CANT PLACE!");
		return null;
	}

	private GameObject CreateChunkObject(GameObject template, bool changesSortOrderBasedOnPlayer)
	{
		var newObject = Object.Instantiate(template, GameObject.transform);
		if(changesSortOrderBasedOnPlayer)
			newObject.AddComponent<PlayerCorrectSortOrder>();
		objects.Add(newObject);
		return newObject;
	}

	public void Destroy()
	{
		//Debug.Log($"Destroyed chunk at {GameObject.transform.position}");
		foreach (var o in objects)
		{
			Object.Destroy(o);
		}
		Object.Destroy(GameObject);
	}
}
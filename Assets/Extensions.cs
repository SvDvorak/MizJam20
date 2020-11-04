using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

	public static Vector2Int Normalized(this Vector2Int vector)
	{
		if(Math.Abs(vector.x) > Math.Abs(vector.y))
			return new Vector2Int(Math.Sign(vector.x), 0);
		return new Vector2Int(0, Math.Sign(vector.y));
	}

	public static int ManhattanDistance(this Vector2Int vector)
	{
		return Math.Abs(vector.x) + Math.Abs(vector.y);
	}

	public static T GetRandom<T>(this T[] list) where T : class
	{
		if (list.Length == 0)
			return null;
		return list[Random.Range(0, list.Length)];
	}

	public static List<Transform> GetChildren(this Transform transform)
	{
		var children = new List<Transform>();
		foreach (Transform child in transform)
		{
			children.Add(child);
		}

		return children;
	}
}
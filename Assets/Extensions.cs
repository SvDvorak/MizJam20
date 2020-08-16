using UnityEngine;

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

	public static T GetRandom<T>(this T[] list) where T : class
	{
		if (list.Length == 0)
			return null;
		return list[Random.Range(0, list.Length)];
	}
}
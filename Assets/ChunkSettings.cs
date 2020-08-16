using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Anwilc/Chunk Settings", order = 1)]
public class ChunkSettings : ScriptableObject
{
	public GameObject TreeTemplate;
	public Sprite[] TreeVariations;
	public int TreeCount;
}
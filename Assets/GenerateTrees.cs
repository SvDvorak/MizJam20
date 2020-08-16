using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrees : MonoBehaviour
{
	public GameObject Prefab;
	public int TreeCount;
	public List<WindAnimation> Trees = new List<WindAnimation>();

	void Start()
    {
	    for (int i = 0; i < TreeCount; i++)
	    {
			var tree = Instantiate(Prefab, transform);
			tree.transform.position = new Vector3(Random.Range(-13, 13), Random.Range(-7, 7));
			Trees.Add(tree.GetComponent<WindAnimation>());
	    }
    }

	void Update()
	{
		foreach (var tree in Trees)
		{
			tree.UpdateAnimation();
		}
	}
}

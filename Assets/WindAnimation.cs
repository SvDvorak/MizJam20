using System;
using System.Collections.Generic;
using UnityEngine;

public interface Animateable
{
	void Update();
}

public class WindAnimation : MonoBehaviour
{
	[Serializable]
	public class BoneData
	{
		public Transform Bone;
		public Vector3 InitialPos;
		public Quaternion InitialRot;
	}

	[HideInInspector]
	public List<BoneData> Bones = new List<BoneData>();

	public void Start()
	{
		foreach (var bone in GetComponentsInChildren<Transform>())
		{
			if (bone == transform)
				continue;

			Bones.Add(new BoneData()
			{
				Bone = bone,
				InitialPos = bone.localPosition,
				InitialRot = bone.localRotation
			});
		}
	}

	public void Update()
	{
		var noise = NoiseFunctions.windNoise(transform.position / 100) * 10;
		for (int i = 1; i < Bones.Count; i++)
		{
			Bones[i].Bone.localPosition = Bones[i].InitialPos + Bones[i].Bone.right * noise * 0.035f;
			Bones[i].Bone.localRotation = Bones[i].InitialRot * Quaternion.Euler(0, 0, noise * 5);
		}
	}
}

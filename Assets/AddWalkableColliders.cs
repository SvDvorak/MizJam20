using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWalkableColliders : MonoBehaviour
{
    void Start()
    {
	    var sprites = GetComponentsInChildren<SpriteRenderer>();

	    foreach (var sprite in sprites)
	    {
		    var spriteGO = sprite.gameObject;
		    spriteGO.isStatic = true;
		    spriteGO.layer = LayerMask.NameToLayer("Walkable");
			var collider = spriteGO.AddComponent<BoxCollider2D>();
			collider.size = sprite.size;
	    }
    }
}

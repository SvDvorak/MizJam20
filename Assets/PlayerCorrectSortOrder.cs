using UnityEngine;

public class PlayerCorrectSortOrder : MonoBehaviour
{
	private BoxCollider2D col;
	private SpriteRenderer spr;

	void Start()
	{
		col = GetComponent<BoxCollider2D>();
		spr = GetComponent<SpriteRenderer>();
	}

    void Update()
    {
	    var playerPos = PlayerControls.Player.transform.position;
	    if ((playerPos - transform.position).sqrMagnitude > 10)
		    return;

	    var colliderTopEdge = transform.position.y + col.offset.y + col.size.y / 2;
	    spr.sortingOrder = playerPos.y > colliderTopEdge ? 1 : -1;
    }
}

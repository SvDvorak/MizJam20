using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackNode
{
	public List<TrackNode> Connections;
	public Vector2Int Position;

	public Vector2Int? BranchDirection
	{
		get
		{
			if (Connections.Count == 0)
				return null;
			return (Connections[0].Position - Position).Normalized();
		}
	}
}

public class Step
{
	public Vector2Int Position;
	public Vector2Int Direction;
	public bool ChangedDirection;

	public Step()
	{
		Position = new Vector2Int(0, 0);
		Direction = new Vector2Int(-1, 0);
		ChangedDirection = false;
	}

	public Step(TrackNode startNode, Step lastStep)
	{
		Position = startNode.Position;
		Direction = (Position - lastStep.Position).Normalized();
		ChangedDirection = lastStep.Direction != Direction;
	}

	public Step(Vector2Int position, Vector2Int direction, Vector2Int lastDirection)
	{
		Position = position;
		Direction = direction;
		ChangedDirection = lastDirection != Direction;
	}
}

public class TrackGenerator : MonoBehaviour
{
	public Transform TracksParent;
	public GameObject TrackTemplate;

	public Sprite Straight;
	public Sprite Turn;

    [ContextMenu("Regenerate")]
    public void Regenerate()
    {
		foreach(var child in TracksParent.GetChildren())
			DestroyImmediate(child.gameObject);

	    var tree = GetTrackNodes(transform);

	    GenerateTracks(null, null, tree.Connections);
    }

    private TrackNode GetTrackNodes(Transform t)
    {
	    return new TrackNode
	    {
		    Connections = t.GetChildren().Select(GetTrackNodes).ToList(),
			Position = t.position.ToV2Int()
	    };
    }

    private void GenerateTracks(Step lastStep, TrackNode startNode, List<TrackNode> connections)
    {
	    var i = 0;
	    if (startNode == null)
	    {
		    startNode = connections[i];
		    i += 1;
	    }

		if(lastStep == null)
		{
			lastStep = new Step();
		}

		//CreateTrackPiece(new Step(startNode, lastStep));

		for (; i < connections.Count; i++)
	    {
		    var endNode = connections[i];

		    var steps = GetSteps(lastStep.Direction, startNode, endNode).ToList();
		    foreach (var step in steps)
		    {
			    CreateTrackPiece(step);
		    }

			//if(endNode.Connections.Count > 0)
				GenerateTracks(steps.Last(), endNode, endNode.Connections);
			//else
			//	CreateTrackPiece(new Step(endNode, steps.Last()));

			startNode = endNode;
	    }
    }

    private IEnumerable<Step> GetSteps(Vector2Int lastStep, TrackNode startNode, TrackNode endNode)
    {
	    var currentPosition = startNode.Position;
	    var endPosition = endNode.Position;

	    while (true)
	    {
		    var toEnd = endPosition - currentPosition;
		    var step = toEnd.Normalized();
		    var distance = (toEnd + step).ManhattanDistance();
		    if (distance == 0)
			    break;

		    currentPosition += step;
		    yield return new Step(currentPosition, step, lastStep);
			lastStep = step;
		}
    }

    private void CreateTrackPiece(Step step)
    {
		var track = Instantiate(TrackTemplate, TracksParent, false);
		SetTrackSprite(track, step);
		track.transform.position = step.Position.ToV3();
    }

    private void SetTrackSprite(GameObject track, Step step)
    {
	    var spriteRenderer = track.GetComponent<SpriteRenderer>();
	    spriteRenderer.sprite = !step.ChangedDirection ? Straight : Turn;
	    var rotation = Math.Sign(step.Direction.x) * 90;
		track.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}

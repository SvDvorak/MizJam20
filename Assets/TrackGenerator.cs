using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackNode
{
	public List<TrackNode> Connections;
	public Vector2Int Position;

	//public Vector2Int? BranchDirection
	//{
	//	get
	//	{
	//		if (Connections.Count == 0)
	//			return null;
	//		return (Connections[0].Position - Position).Normalized();
	//	}
	//}
}

public class Step
{
	public Vector2Int Position;
	public Vector2Int PreviousDirection;
	public Vector2Int NextDirection;
	public bool ChangedDirection;

	public Step(Vector2Int current, Vector2Int previous, Vector2Int next)
	{
		Position = current;
		PreviousDirection = current - previous;
		NextDirection = next - current;
		ChangedDirection = NextDirection != PreviousDirection && !IsZero(NextDirection) && !IsZero(PreviousDirection);
	}

	private bool IsZero(Vector2Int v) => v == Vector2Int.zero;
}

public class StepChain : List<Vector2Int>
{
	public List<StepChain> ChildChains = new List<StepChain>();
}

public class TrackGenerator : MonoBehaviour
{
	public Transform TracksParent;
	public GameObject TrackTemplate;

	public Sprite Straight;
	public Sprite Turn;

	private readonly Dictionary<Vector2Int, float> _turnRotations = new Dictionary<Vector2Int, float>()
	{
		{new Vector2Int(1, -1), 0},
		{new Vector2Int(1, 1), 90},
		{new Vector2Int(-1, 1), 180},
		{new Vector2Int(-1, -1), 270},
	};

    [ContextMenu("Regenerate")]
    public void Regenerate()
    {
		foreach(var child in TracksParent.GetChildren())
			DestroyImmediate(child.gameObject);

	    var tree = GetTrackNodes(transform);
	    var steps = GetStepChains(null, tree.Connections);

		GenerateTracks(steps, false);
	}

    private TrackNode GetTrackNodes(Transform t)
    {
	    return new TrackNode
	    {
		    Connections = t.GetChildren().Select(GetTrackNodes).ToList(),
		    Position = t.position.ToV2Int()
	    };
    }

    private StepChain GetStepChains(TrackNode startNode, List<TrackNode> connections)
    {
	    var steps = new StepChain();
	    var i = 0;
	    if (startNode == null)
	    {
		    startNode = connections[i];
		    i += 1;
	    }

		steps.Add(startNode.Position);

		for (; i < connections.Count; i++)
	    {
		    var endNode = connections[i];

		    steps.AddRange(GetSteps(startNode, endNode));

			if(endNode.Connections.Count > 0)
				steps.ChildChains.Add(GetStepChains(endNode, endNode.Connections));

			startNode = endNode;
	    }

		return steps;
    }

    private IEnumerable<Vector2Int> GetSteps(TrackNode startNode, TrackNode endNode)
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
		    yield return currentPosition;
	    }
    }

    private void GenerateTracks(StepChain stepChain, bool skipFirst = true)
    {
	    for (var i = skipFirst ? 1 : 0; i < stepChain.Count; i++)
	    {
		    var current = stepChain[i];
		    var previous = current;
		    var next = current;
		    if (i != 0)
			    previous = stepChain[i - 1];
			if(i != stepChain.Count - 1)
				next = stepChain[i + 1];
		    CreateTrackPiece(new Step(current, previous, next));
	    }

	    foreach (var childChain in stepChain.ChildChains)
	    {
			GenerateTracks(childChain);
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
	    var rotation = step.ChangedDirection ? _turnRotations[step.NextDirection - step.PreviousDirection] : Math.Sign(step.NextDirection.x) * 90;
		track.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}

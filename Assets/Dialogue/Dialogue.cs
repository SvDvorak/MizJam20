using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Dialogue
{
	public List<StoryChapter> Story;
	public List<Interaction> Interactions;

	public Dialogue(InputDialogue inputDialogue)
	{
		Story = inputDialogue.Story
			.Select(x => new StoryChapter(x))
			.ToList();

		Interactions = inputDialogue.Interactions
			.Select(x => new Interaction(x))
			.ToList();
	}
}

[Serializable]
public class StoryChapter
{
	public float Distance;
	public List<Line> Lines;

	public StoryChapter(InputStoryChapter inputChapter)
	{
		Distance = inputChapter.Distance;
		Lines = inputChapter.Lines
			.Select(x => new Line(x))
			.ToList();
	}
}

[Serializable]
public class Interaction
{
	public string Name;
	public List<Line> Lines;

	public Interaction(InputInteraction inputInteraction)
	{
		Name = inputInteraction.Name;
		Lines = inputInteraction.Lines
			.Select(x => new Line(x))
			.ToList();
	}
}

[Serializable]
public class Line
{
	public string Text;
	public string[] Mods;

	public Line(string text)
	{
		var mods = new List<string>();
		var modStartIndex = text.IndexOf("--", 0, StringComparison.Ordinal);
		while(modStartIndex != -1)
		{
			var modEndIndex = text.IndexOf("--", modStartIndex + 2, StringComparison.Ordinal);
			mods.Add(text.Substring(modStartIndex + 2, modEndIndex - modStartIndex - 2));
			text = text.Remove(modStartIndex, modEndIndex - modStartIndex + 2);

			modStartIndex = text.IndexOf("--", 0, StringComparison.Ordinal);
		}

		Mods = mods.ToArray();
		Text = text;
	}
}

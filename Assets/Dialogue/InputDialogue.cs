using System;
using System.Collections.Generic;

[Serializable]
public class InputDialogue
{
	public List<InputStoryChapter> Story;
	public List<InputInteraction> Interactions;
}

[Serializable]
public class InputStoryChapter
{
	public float Distance;
	public List<string> Lines;
}

[Serializable]
public class InputInteraction
{
	public string Name;
	public List<string> Lines;
}
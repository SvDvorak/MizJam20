using UnityEngine;
using UnityEngine.UI;

public class NoiseFunctions : MonoBehaviour
{
	public RenderTexture DebugRenderTexture;
	public SpriteRenderer Renderer;

	private const int size = 512;
	private int zoom = 4;
	private Color[] pixels;
	private Texture2D debugTexture;

	private float waitTime;

	public void Start()
	{
		pixels = new Color[size * size];
		debugTexture = new Texture2D(size, size, TextureFormat.RGB24, true)
		{
			filterMode = FilterMode.Point
		};

		Sprite sprite = Sprite.Create(debugTexture, new Rect(0, 0, 256, 256), Vector2.zero);
		Renderer.sprite = sprite;
	}

	public void Update()
	{
		if (Time.time > waitTime)
			UpdateNoise();
	}

	private void UpdateNoise()
	{
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				var v = windNoise(new Vector3(x / (float) size * zoom, y / (float) size * zoom));
				pixels[y * size + x] = new Color(Mathf.Clamp(v, 0, 1), Mathf.Abs(Mathf.Clamp(v, -1, 0)), 0, 1);
			}
		}

		debugTexture.SetPixels(pixels);
		debugTexture.Apply();

		waitTime = Time.time + 1;
	}

	public static float noise(Vector3 pos, float offset)
	{
		return Mathf.PerlinNoise(pos.x + offset, pos.y + offset) * 2 - 1;
	}

	public static float windNoise(Vector3 pos)
	{
		float val = Time.time / 10;
		float offset = val + Mathf.Sin(val) * 0.5f;
		//float offset = val +
		//               Mathf.Sin(val * 0.25f) * 0.5f +
		//               Mathf.Sin(val * 2) * 0.25f +
		//               Mathf.Sin(val * 4 + 0.6f) * 0.12f +
		//               Mathf.Sin(val * 8) * 0.06f;
		var n = noise(pos * 2, offset);
		var nSign = Mathf.Sign(n);
		var edge = 0.5f;
		n = Mathf.Clamp(Mathf.Abs(n) - edge, 0, 1 - edge) / (1 - edge) * nSign;
		return n * 0.9f + noise(pos * 40, offset * 10) * 0.1f;
		// + noise(pos / 16, offset) * 0.25f + noise(pos / 32, offset) * 0.25f;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour {

	private Image image;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		StartCoroutine(FadeIn());
	}

	private IEnumerator FadeIn()
	{
		Color color = image.color;
		for (float i = 255; i >= 0; i -= 15)
		{
			color.a = i / 255;
			image.color = color;
			yield return null;
		}

		Destroy(gameObject);
	}
}

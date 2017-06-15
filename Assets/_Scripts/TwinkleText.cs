using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TwinkleText : MonoBehaviour {

	[Range(0, 10)]
	public float speed = 1f;

	Text targetText;
	Outline outline;
	bool hasOutline = false;
	bool IsStarted = true;

	void Start()
	{
		targetText = GetComponent<Text>();
		outline = GetComponent<Outline>();

		if (outline != null)
			hasOutline = true;
	}

	private void Update()
	{
		if (IsStarted)
		{
			targetText.color = new Color(targetText.color.r, targetText.color.g, targetText.color.b, Mathf.PingPong(Time.timeSinceLevelLoad * speed, 1f)); //Color.Lerp(StartText.color, Color.clear, FadeTime * Time.deltaTime);
			if (hasOutline)
				outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, Mathf.PingPong(Time.timeSinceLevelLoad * speed, 1f));
		}
	}

	public void Stop(bool isShow = true)
	{
		IsStarted = false;
		targetText.color = new Color(targetText.color.r, targetText.color.g, targetText.color.b, isShow ? 1f : 0f); 
	}

	public void Play()
	{
		IsStarted = true;
	}
}

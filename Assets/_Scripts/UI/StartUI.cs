using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum StageScene
{
	START,
	MAIN,
	GAMEOVER,
}


public class StartUI : MonoBehaviour
{

	StageScene CurStage = StageScene.START;

	//Button StartButton = null;
	public Text StartText = null;
	Outline StartTextOutline = null;
	//Transform StartUI = null;
	public Color FadeColor;
	public float FadeTime = 10f;


	//private void Awake()
	//{
	//	StartTextOutline = StartText.GetComponent<Outline>();

	//	Transform tempTrans = null;
	//	tempTrans = this.transform.FindChild("StartButton");

	//	if (tempTrans == null)
	//	{
	//		Debug.Log("Not Founded StartBtn");
	//		return;
	//	}

	//	StartButton = tempTrans.GetComponent<Button>();
	//	StartButton.onClick.AddListener(OnClickStartButton);


	//	//StartText = this.gameObject.GetComponentInChildren<Text>();
	//}

	private void Start()
	{
		StartCoroutine(TextFlash());

	}

	void Update()
	{
		TouchScreen();
		
	}
	public void OnClickStartButton()
	{
		CurStage = StageScene.MAIN;
		SceneManager.LoadScene(CurStage.ToString());
	}
	public void TouchScreen()
	{
		if (Input.GetMouseButtonDown(0))
		{
			CurStage = StageScene.MAIN;
			SceneManager.LoadScene(CurStage.ToString());
		}
	}
	IEnumerator TextFlash()
	{
		//float tempTime = 0;
		while (true)
		{
			//tempTime += Time.deltaTime*FadeTime;
			//StartText.color.a=	Mathf.PingPong(Time.deltaTime, 1);	
			//print(Mathf.PingPong(Time.deltaTime, 1f));
			StartText.color = new Color(StartText.color.r, StartText.color.g, StartText.color.b, Mathf.PingPong(Time.timeSinceLevelLoad * FadeTime, 1f)); //Color.Lerp(StartText.color, Color.clear, FadeTime * Time.deltaTime);
			StartTextOutline.effectColor = new Color(StartTextOutline.effectColor.r, StartTextOutline.effectColor.g, StartTextOutline.effectColor.b, Mathf.PingPong(Time.timeSinceLevelLoad * FadeTime, 1f));
			yield return new WaitForEndOfFrame();
		}
	}





}
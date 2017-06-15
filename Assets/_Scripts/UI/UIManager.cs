using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum UI_STATE
{
	NEN,
	HOME,
	GAME,
	GAMEOVER,
	MAX
}


public class UIManager : MonoBehaviour
{
	GameObject DamagePrefab = null;
	static UIManager instance;
	public static UIManager Instance
	{
		// get; set;
		get
		{
			return instance;
		}
	}

	UI_STATE CurrState = UI_STATE.GAME;

	//public TheStack StackScript = null;

	// HOME UI
	Transform HomeUI = null;
	UIButton StartBtn = null;

	// GAME UI
	Transform GameUI = null;
	Transform ScoreLabel = null;
	UILabel ScoreCount = null;
	Transform ComboRoot = null;
	Text ComboText = null;

	// GAMEOVER UI
	Transform PopupUI = null;
	Text MaxComboText = null;
	Transform GameOverUI = null;
	 Button RetryButton = null;
	 Button QuitButton = null;

	private void Awake()
	{
		instance = this;

		HomeUI = this.transform.FindChild("HomeUI");
		GameUI = this.transform.FindChild("GameUI");
		GameOverUI = this.transform.FindChild("GameOverUI");

		ComboRoot = GameUI.FindChild("Combo");
		ComboText = ComboRoot.FindChild("ComboText").GetComponent<Text>();

		PopupUI = GameOverUI.FindChild("PopupUI");
		MaxComboText = PopupUI.FindChild("MaxComboText").GetComponent<Text>();


		RetryButton = PopupUI.FindChild("RetryButton").gameObject.GetComponent<Button>();
		QuitButton = PopupUI.FindChild("QuitButton").gameObject.GetComponent<Button>();

		DamagePrefab = Resources.Load("UI/DamageText")as GameObject;
		if (DamagePrefab == null)
		{
			Debug.Log("제발 UI/ DamageText 있나 확인먼저 하세요");
			return;
		}
	
	}

	private void Start()
	{
		// HomeUI
		//StartBtn.onClick.Add(new EventDelegate(this,"OnClickedStartBtn"));

		//EventDelegate.Add(StartBtn.onClick, new EventDelegate(this, "OnClickedStartBtn"));// 주권을 어디에 주냐의 차이 EventDelegate냐 StartBtn이냐


		// GameUI
		//ScoreLabel = GameUI.FindChild("Score");
		//ScoreCount = ScoreLabel.FindChild("ScoreCount").GetComponent<UILabel>();
		//ComboRoot = GameUI.FindChild("Combo");
		//ComboText = ComboRoot.FindChild("ComboText").GetComponent<Text>();

		//// ScoreUI
		//MaxComboText = GameOverUI.FindChild("MaxComboText").GetComponent<Text>();


		//StartButton = tempTrans.GetComponent<Button>();
		//StartButton.onClick.AddListener(OnClickStartButton);
		QuitButton.onClick.AddListener(OnClickedQuitBtn);
		RetryButton.onClick.AddListener(OnClickedReStartBtn);


		//EventDelegate.Add(GameOverUI.FindChild("QuitButton").GetComponent<Button>().onClick, new EventDelegate(this, "OnClickedQuitBtn"));
		//EventDelegate.Add(ReStartBtn.onClick, new EventDelegate(this, "OnClickedReStartBtn"));
		//EventDelegate.Add(GameOverUI.FindChild("RetryButton").GetComponent<Button>().onClick, new EventDelegate(this, "OnClickedReStartBtn"));

		ChangeState(UI_STATE.GAME);
	}



	// 게임오버시 넣는다.
	public void SetMaxScore(int MaxCombo)
	{
		//ScoreUI.FindChild("BestScore/Score/ScoreCount").GetComponent<UILabel>().text = bestS.ToString();
		//ScoreUI.FindChild("BestScore/Combo/ComboCount").GetComponent<UILabel>().text = bestC.ToString();
		//ScoreUI.FindChild("CurrScore/Score/ScoreCount").GetComponent<UILabel>().text = s.ToString();
		MaxComboText.text = MaxCombo.ToString();
		ChangeState(UI_STATE.GAMEOVER);
		//GameOverUI.Find("ComboText").GetComponent<Text>().text = MaxCombo.ToString();
	}
	public void SetScore(int combo)
	{
		//ScoreCount.text = score.ToString();

		//if (combo == 0)
		//{
		//	ComboRoot.gameObject.SetActive(false);
		//}
		//else
		//{
		//	if (ComboRoot.gameObject.activeSelf == false)
		//		ComboRoot.gameObject.SetActive(true);

		ComboText.text = combo.ToString();

	}

	public void ChangeState(UI_STATE state)
	{
		//GameUI.gameObject.SetActive(false);
		GameOverUI.gameObject.SetActive(false);

		switch (state)
		{
			case UI_STATE.GAME:
				GameUI.gameObject.SetActive(true);
				break;
			case UI_STATE.GAMEOVER:

				GameOverUI.gameObject.SetActive(true);
				break;
		}
		CurrState = state;

	}




	public void OnClickedReStartBtn()
	{
		//StackScript.Restart();
		//ChangeState(UI_STATE.GAME);
		SceneManager.LoadScene("StartScene");
	}

	public void OnClickedQuitBtn()
	{
		Application.Quit();
	}

	public void CreateDamageText(string text, float showTime,Vector3	pos)
	{
		GameObject go = Instantiate (
			DamagePrefab, pos,Quaternion.identity);

		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;

		DamageText dt = go.GetComponent<DamageText>();
		dt.Set(text, showTime);
	}




	//Color GetRandomColor()
	//{
	//	float r = Random.Range(80f, 200f) / 255f;
	//	float g = Random.Range(80f, 200f) / 255f;
	//	float b = Random.Range(80f, 200f) / 255f;

	//	return new Color(r, g, b);
	//}

}

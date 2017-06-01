using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AniState
{
	None,
	Idle,
	Walk,
	Attack,
	Damage,
	Dead,
}

public delegate void EndEvent();

public class AnimationCtrl : MonoBehaviour
{


	EndEvent End = null;
	AniState CurState = AniState.None;
	Animation Anim = null;

	public AniState State
	{
		get
		{
			return CurState;
		}
	}

	public void Set(EndEvent _endEvent)
	{
		End = _endEvent;
		Anim = this.GetComponent<Animation>();
		if (Anim == null)
		{
			Debug.Log("not found animation in" + gameObject.name);
			return;
		}
	}

	public void AnimationEndEvent()
	{
		if (End != null)
			End();
	}

	public void Play(AniState _state, bool bForce = false)
	{

		//Player -> false상태로 사용, Enemy는 true
		if (bForce == true)
		{
			Anim.Stop();
		}
		else if (CurState == _state)
			return;

		//if (bForce == false&&CurState == _state)
		//	return;

		CurState = _state;
		Anim.Play(CurState.ToString());
	}
}

using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {
	
	public GameObject mDot = null;
	public GameObject mStart = null;
	public GameObject mEnd = null;
	public GameObject mHead = null;
	
	public GameObject mPrev = null;
	public GameObject mNext = null;
	
	
	public void Start () {
		if (mPrev) {
			float angle = Mathf.Atan2 ((mPrev.gameObject.transform.position.x - gameObject.transform.position.x),
			                           (gameObject.transform.position.y - mPrev.gameObject.transform.position.y));
			
			mStart.transform.rotation = Quaternion.Euler(0,0, angle * 57.2957795f + 90.0f);
			mStart.transform.localScale = Vector3.one;
			mHead.transform.localScale = Vector3.zero;
		} else {
			mStart.transform.localScale = Vector3.zero;
		}

		
		if (mNext) {
			float angle = Mathf.Atan2 ((mNext.gameObject.transform.position.x - gameObject.transform.position.x),
			                           (gameObject.transform.position.y - mNext.gameObject.transform.position.y));
			
			mEnd.transform.rotation = Quaternion.Euler(0,0, angle * 57.2957795f + 90.0f);
			mEnd.transform.localScale = Vector3.one;
		} else {
			mEnd.transform.localScale = Vector3.zero;
			
			mHead.transform.localScale = Vector3.one;
			mStart.transform.localScale = Vector3.zero;
			mHead.transform.rotation = mStart.transform.rotation;
			
		}
		
		if (mPrev && !mNext) {
			mDot.transform.localScale = Vector3.zero;
		} else {
			mDot.transform.localScale = Vector3.one;
		}
		
		if (!mPrev && !mNext) {
			mHead.transform.localScale = Vector3.zero;
			mStart.transform.localScale = Vector3.zero;
			mEnd.transform.localScale = Vector3.zero;
		}
	}
}

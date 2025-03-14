using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_HumanDemoControl : MonoBehaviour {

	public Animator[] animators;					// Human animators
	[SerializeField] private Transform cameraFollow;
	[SerializeField] private Transform male;
	[SerializeField] private Transform female;

	public void SetRightHandWeight(float newValue){
		for (int i = 0; i < animators.Length; i++) {
			animators [i].SetLayerWeight (3, newValue);
		}
	}

	public void SetLeftHandWeight(float newValue){
		for (int i = 0; i < animators.Length; i++) {
			animators [i].SetLayerWeight (2, newValue);
		}
	}

	public void SetLocomotion(float newValue){
		for (int i = 0; i < animators.Length; i++) {
			animators [i].SetFloat ("locomotion", newValue);
		}
	}

	public void TriggerAnimation(string newTrigger){
		for (int i = 0; i < animators.Length; i++) {
			animators [i].SetTrigger (newTrigger);
		}
	}

	public void FollowMale()
	{
		Follow(male);
	}

	public void FollowFemale()
	{
		Follow(female);
	}

	public void Follow(Transform target)
	{
		cameraFollow.parent = target;
		cameraFollow.localPosition = Vector3.zero;
		cameraFollow.localRotation = target.rotation;
	}
}

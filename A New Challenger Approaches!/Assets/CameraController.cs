using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {

	// Constants
	protected const float CAMERA_SHAKE_INTERVAL = 0.25f;

	// Runtime variables
	protected Transform cameraTransform;
	public Transform CameraTransform { get { return cameraTransform; } }
	protected Vector3 cameraInitialLocalPosition;
	protected List<ShakeInstance> shakeInstances;

	protected void Awake () {
		cameraTransform = Camera.main.transform;
		cameraInitialLocalPosition = cameraTransform.localPosition;
		shakeInstances = new List<ShakeInstance> ();
	}

	public void ShakeCamera(float intensity, float duration) {
		shakeInstances.Add (new ShakeInstance (intensity, duration));
	}

	protected void Update() {
		float currentShakeIntensity = 0;
		for (int i = 0; i < shakeInstances.Count; i++) {
			ShakeInstance currentShakeInstance = shakeInstances [i];

			if (currentShakeInstance.currentShakeDuration <= 0) {
				shakeInstances.RemoveAt (i);
			} else {
				float currentShakeInstanceIntensity = currentShakeInstance.shakeIntensity * currentShakeInstance.currentShakeDuration / currentShakeInstance.shakeDuration;
				currentShakeIntensity = Mathf.Max (currentShakeInstanceIntensity, currentShakeIntensity);
				float currentShakeInstanceDuration = currentShakeInstance.currentShakeDuration;
				currentShakeInstance.currentShakeDuration = currentShakeInstanceDuration - Time.deltaTime;
			}
		}
		cameraTransform.localPosition = cameraInitialLocalPosition + Random.insideUnitSphere * currentShakeIntensity;
	}

}

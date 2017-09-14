using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {

	// Constants
	protected const float CAMERA_SHAKE_INTERVAL = 0.25f;

    // References
    [SerializeField]
    protected List<Transform> trackedTransforms;

    // Fields
    [SerializeField]
    protected float cameraBuffer;
    [SerializeField]
    protected float cameraScaleSpeed;
    [SerializeField]
    protected Vector2 cameraOffset;

    // Runtime variables
    protected Camera mainCamera;
	protected Transform cameraTransform;
	public Transform CameraTransform { get { return cameraTransform; } }
    protected float cameraZValue;
    protected Vector3 cameraInitialLocalPosition;
	protected List<ShakeInstance> shakeInstances;

	protected void Awake () {
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        cameraZValue = cameraTransform.position.z;
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

        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        Vector3 averagePosition = Vector2.zero;
        for (int i = 0; i < trackedTransforms.Count; i++) {
            float currentTrackedTransformPositionX = trackedTransforms[i].position.x;
            float currentTrackedTransformPositionY = trackedTransforms[i].position.y;
            if (currentTrackedTransformPositionX > maxX) { maxX = currentTrackedTransformPositionX; }
            if (currentTrackedTransformPositionX < minX) { minX = currentTrackedTransformPositionX; }
            if (currentTrackedTransformPositionY > maxY) { maxY = currentTrackedTransformPositionY; }
            if (currentTrackedTransformPositionY < minY) { minY = currentTrackedTransformPositionY; }
            averagePosition += trackedTransforms[i].position;
        }
        averagePosition /= trackedTransforms.Count;
        //Debug.Log(minX + " " + maxX + " " + minY + " " + maxY);
        float charactersHeightDifference = maxY - minY;
        float charactersWidthDifference = maxX - minX;
        //Debug.Log(charactersHeightDifference + " " + charactersWidthDifference);
        float idealHeight = charactersHeightDifference + 2 * cameraBuffer;
        float idealWidth = charactersWidthDifference + 2 * cameraBuffer;
        float calculatedHeightUsingIdealWidth = idealWidth / mainCamera.aspect;
        //Debug.Log(idealHeight + " " + calculatedHeightUsingIdealWidth);
        idealHeight = Mathf.Max(idealHeight, calculatedHeightUsingIdealWidth);
        float distanceFromAveragePositionToCameraPositionOnZAxis = Mathf.Abs(averagePosition.z - transform.position.z);
        float idealFieldOfView = 2 * Mathf.Atan2(idealHeight, 2 * distanceFromAveragePositionToCameraPositionOnZAxis) * Mathf.Rad2Deg;
        averagePosition.z = cameraZValue;
        averagePosition += (Vector3)cameraOffset;
        transform.position = Vector3.Lerp(transform.position, averagePosition, cameraScaleSpeed * Time.deltaTime);

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, idealFieldOfView, cameraScaleSpeed * Time.deltaTime);

    }

    public void AddToCameraTracker(Transform newTrackedTransform) {
        if (!trackedTransforms.Contains(newTrackedTransform)) {
            trackedTransforms.Add(newTrackedTransform);
        }
    }

}

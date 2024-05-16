using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityCursorControl;

public class CameraControl : MonoBehaviour {
	public static CameraControl instance = null;

	// This is the game object that the camera actively follows
	public GameObject followTarget;

	// The distance and height offsets from the followTarget's position
	public float followDistance = 10.0f;
	public float maxFollowDistance = 20.0f;
	public float followHeight = 2.4f;
	public float maxfollowHeight = 7.2f;
	public float fLookDistance = .5f;
	public float fLookHeight = 1f;
	float originalFollowDistance = 10;
	float originalFollowHeight = 2.4f;

	// These are used for the camera orbit and first person mechanics.
	private float rotationOffsetFromMouseX = 0.0f;
	private float rotationOffsetFromMouseY = 0.0f;
	// How fast using the scroll wheel zooms you in towards the followTarget
	public float mouseScrollSpeed = 4.0f;

	// This flag is used in various conditional behaviours throughout the script
	bool mouseHeld = false;

	// This Vector2 is used to store the mouse position in CatchMouse() when the cursor is locked and hidden
	//   When ReleaseMouse() is called, the cursor will reposition to this saved position.
	//   - This is necessary because Cursor.LockState positions the mouse in the center of the screen and it causes the mouse to jump in a disorienting way.
	Vector2 savedMousePos = new Vector2();

	// The first person mode boolean flag for conditional behaviors
	bool dialogMode = false;

	// Use this for initialization
	void Awake () {
		//Singleton coding to ensure main cameras do not duplicate themselves
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		// This likely isn't necessary but I'm not going to try to fix what's not broken
		//DontDestroyOnLoad(gameObject);
	}

    private void Start()
    {
		CatchMouse();
		originalFollowDistance = followDistance;
		originalFollowHeight = followHeight;
    }
    // Update is called once per frame
    void Update ()
	{
		// If the camera has a follow target
		if (followTarget != null)
		{
			// If the camera has been scrolled in as close as possible, trigger and/or run first person mode


			// Sets the camera to the player's position
			MatchFollowTargetTransforms();

			// If either mouse button is down, the Mouse movements are tracked for camera angle modification against the player's transform
			//Scrolling the mouse will zoom the camera in and out
			if(!dialogMode) ReadMouseForCameraControls();

			// Rotates and translates the camera to its final position
			RePositionCameraAgainstPlayer();

			//The player re-orients to match the camera
			
			PlayerYToCameraY();



		}


	}

	/// <summary>
	/// Reset the position camera against player.
	///  - First the camera is rotated based on its offsets, then its new position is calculated
	///  - There is a raycast sent to check for collisions with environmental objects
	///    - There is also a layer mask used on the raycast to avoid the camera being blocked by certain objects
	/// </summary>
	void RePositionCameraAgainstPlayer ()
	{
		Vector3 origin = transform.position;
		//rotates and translates the camera to its final position
		transform.Rotate (-rotationOffsetFromMouseY * 2, rotationOffsetFromMouseX * 2, 0);
		transform.Translate (0, followHeight, -followDistance);
		Vector3 newPos = new Vector3();

		// This creates the mask to be used in making the raycast shoot through living entities
		int livingEntitiesLayer = 8;
		int livingEntitiesMask = 1 << livingEntitiesLayer;

		// Cast the ray from the player to the camera's point to check for obstructions
		RaycastHit hit;
		// If the ray hit something
		if (Physics.Raycast (origin, (transform.position - origin), out hit, Mathf.Infinity, ~livingEntitiesMask)) {
			// If the point if collision with the ray is closer to the player than the camera
			if ( Vector3.Distance(transform.position, origin) >= Vector3.Distance(origin, hit.point))
				// Move the camera to the point of collision
				transform.position = hit.point;
				// And inch it towards the player so that it doesn't sit in a wall
				newPos = Vector3.MoveTowards(transform.position, followTarget.transform.position, .5f);
				transform.position = newPos;
		}
	}

	void ReadMouseForCameraControls()
	{
		// The Mouse movements are tracked for camera angle modification against the player's transform

		rotationOffsetFromMouseX += Input.GetAxis("Mouse X");
		rotationOffsetFromMouseY += Input.GetAxis("Mouse Y");

		if (rotationOffsetFromMouseY < -35)
			rotationOffsetFromMouseY = -35;
		if (rotationOffsetFromMouseY > 80)
			rotationOffsetFromMouseY = 80;
		if (Mathf.Abs(rotationOffsetFromMouseX) > .1f && mouseHeld == false)
		{
			CatchMouse();
		}


	}

	/// <summary>
	/// Rotates the player object to match the camera's Y angle
	/// </summary>
	void PlayerYToCameraY ()
	{
		if (rotationOffsetFromMouseX != 0) {
			followTarget.transform.Rotate (0, rotationOffsetFromMouseX * 2, 0);
			rotationOffsetFromMouseX = 0;
		}
	}

	/// <summary>
	/// Hides the mouse cursor and locks it in place
	/// </summary>
	void CatchMouse ()
	{
		// This uses an outside library called CursorControl.
		//  -It enables you to lock the mouse position on a global level, something Unity cannot do by default
		savedMousePos = CursorControl.GetGlobalCursorPos();

		// Hides the mouse
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		// Sets a flag for conditional behaviors aroudn the script
		mouseHeld = true;
	}

	/// <summary>
	/// Unhides the mouse cursor and enables free movement
	/// </summary>
	void ReleaseMouse(){
		// This uses an outside library called CursorControl
		//  -
		CursorControl.SetGlobalCursorPos(savedMousePos);

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		// Sets a flag for conditional behaviors aroudn the script
		mouseHeld = false;
	}

	/// <summary>
	/// Matches the camera's transforms with the follow target's transforms.
	/// </summary>
	void MatchFollowTargetTransforms(){
		transform.position = followTarget.transform.position;
			transform.rotation = followTarget.transform.rotation;
	}

	/// <summary>
	/// Sets the follow target for the camera.
	/// </summary>
	/// <param name="target">Target.</param>
	public void SetFollowTarget (GameObject target)
	{
		followTarget = target;
	}

	/// <summary>
	/// Clears the follow target.
	/// </summary>
	public void ClearFollowTarget ()
	{
		followTarget = null;
	}

	public void SetFLookSettings()
    {
		if (dialogMode) return;
		followDistance = fLookDistance;
		followHeight = fLookHeight;
    }
	public void SetDialogLookSettings(Vector3 cameraPosition, Vector3 cameraLookAtPosition)
    {
		dialogMode = true;
		followTarget = null;
		transform.position = cameraPosition;
		transform.LookAt(cameraLookAtPosition);
    }
	public void SetStandardSettings()
    {
		dialogMode = false;
		followDistance = originalFollowDistance;
		followHeight = originalFollowHeight;
		followTarget = MyroController.instance.gameObject;
    }
}
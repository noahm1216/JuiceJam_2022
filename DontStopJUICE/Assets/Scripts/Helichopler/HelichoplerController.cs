using System.Collections.Generic;
using UnityEngine;

public class HelichoplerController : MonoBehaviour {
	enum MouseDir { LEFT, RIGHT, UP, DOWN }

    [SerializeField] GameObject rotor;

	Vector3 prevMousePos;
	Vector3 mouseVelocity;


	bool firstFrame = true;

	[SerializeField] float checkInterval = 0.1f;
	[SerializeField] float minMoveAmount = 0.1f;
	float lastCheckTime;

	bool isMoving;
	int circlesInARow;

	MouseDir currentDirection;
	List<MouseDir> previousDirections = new List<MouseDir>();

	MouseDir[] validCW = {
		MouseDir.RIGHT,
		MouseDir.DOWN,
		MouseDir.LEFT,
		MouseDir.UP,
	};

	// detect when mouse move dir goes from right->down->left->up cycle

	void Update() {
		Vector3 mousePos = Input.mousePosition;

		if (firstFrame) {
			firstFrame = false;
			prevMousePos = mousePos;
			return;
		}

		if (Time.time > lastCheckTime + checkInterval) {
			lastCheckTime = Time.time;

			Vector3 mouseDir = mousePos - prevMousePos;

			float moveAmount = mouseDir.magnitude;
			mouseDir = mouseDir.normalized;

			if (moveAmount > minMoveAmount) {
				isMoving = true;

				if (Mathf.Abs(mouseDir.x) - Mathf.Abs(mouseDir.y) > 0) {
					// moving on x axis
					if (mouseDir.x > 0) currentDirection = MouseDir.RIGHT;
					else currentDirection = MouseDir.LEFT;
				} else {
					// moving on y axis
					if (mouseDir.y > 0) currentDirection = MouseDir.UP;
					else currentDirection = MouseDir.DOWN;
				}

				bool valid = VerifyDirection(currentDirection);
				if (valid) {
					if (previousDirections.Count == 0) {
						previousDirections.Insert(0, currentDirection);
					} else {
						if (currentDirection != previousDirections[0]) circlesInARow++;

						if (currentDirection != previousDirections[0]) {
							previousDirections.Insert(0, currentDirection);
							if (previousDirections.Count > 2) previousDirections.RemoveAt(previousDirections.Count - 1); // do we need to remove?
						}
					}
				} else {
					ResetCircularMovement();
				}
			} else {
				ResetCircularMovement();
			}

			print(circlesInARow);

			prevMousePos = mousePos;
		}
	}

	void ResetCircularMovement() {
		previousDirections.Clear();
		isMoving = false;
		circlesInARow = 0;
	}

	bool VerifyDirection(MouseDir dir) {
		if (previousDirections.Count < 2) return true; // not enough to deny yet
		if (previousDirections[0] == dir) return true; // if we are still moving in the same direction, its fine

		for (int i=0; i<validCW.Length; i++) {
			if (validCW[i] == previousDirections[0]) {
				int leftIndex = i - 1;
				if (leftIndex < 0) leftIndex = validCW.Length - 1;

				int rightIndex = i + 1;
				if (rightIndex == validCW.Length) rightIndex = 0;

				// now we check left and right

				if (validCW[leftIndex] == currentDirection) {
					// check for CCW
					if (validCW[rightIndex] == previousDirections[1]) {
						return true;
					} else {
						return false;
					}
				} 
				//else if (validCW[rightIndex] == currentDirection) {
				//	// check for CW
				//	if (validCW[leftIndex] == previousDirections[1]) return true;
				//	else return false;
				//}
			}
		} return false;
	}

	void FixedUpdate() {
        rotor.transform.Rotate(transform.up, 1f);
    }
}

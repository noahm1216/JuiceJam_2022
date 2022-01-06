using System.Collections.Generic;
using UnityEngine;

public class OverheadRotor : MonoBehaviour {
	// r = 5 meters, m = 3 kg
	[SerializeField] Vector3 r = new Vector3(5f, 0f, 0f);
	[SerializeField] float bladeHeight = 0.2f;
	[SerializeField] float m = 3f;
	// I_of_rotor = 4*m*r^2 / 3 = 1
	[SerializeField] float I = 1f;

	[SerializeField] Vector3 a;
	[SerializeField] Vector3 w;
	[SerializeField] Vector3 theta;

	// forces, with respect to the edge of a rotor at point r
	List<Vector3> forces = new List<Vector3>();

	[SerializeField] Vector3 airResistanceForce;
	[SerializeField] float airDensity = 1.225f;
	[SerializeField] float dragCoefficient = 1f;
	[SerializeField] float crossSectionalSurfaceArea;

	void FixedUpdate() {
		crossSectionalSurfaceArea = r.x * bladeHeight;
		//Vector3 velocitySquared = Vector3.Dot(w, w) * -w.normalized;
		//print("vel squared: " + velocitySquared);
		//airResistanceForce = 0.5f * airDensity * dragCoefficient * crossSectionalSurfaceArea * velocitySquared;
		//print("air " + airResistanceForce);
		//forces.Add(airResistanceForce);
		airResistanceForce = 0.5f * airDensity * dragCoefficient * crossSectionalSurfaceArea * new Vector3(0f, 0f, -1f) * w.magnitude;
		print(airResistanceForce);
		forces.Add(airResistanceForce);

		// a = sum(T) = sum((r x F) / I) = sum(r x F)
		a = Vector3.zero;
		for (int i=0; i<forces.Count; i++) {
			a += Vector3.Cross(r, forces[i]) / I;
	 	} forces.Clear();

		print("acc: " + a);

		// w = a * dt
		w += a * Time.fixedDeltaTime;

		// theta = w * dt
		theta = w * Time.fixedDeltaTime;

		transform.Rotate(theta);
	}

	public void AddForce(Vector3 force) {
		forces.Add(force);
	}
}

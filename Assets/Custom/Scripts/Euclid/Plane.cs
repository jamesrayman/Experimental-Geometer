using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	public class Plane : Figure {
		// a point on the plane
		public Vector3 p { get; }
		public Vector3 normal { get; }

		public Plane (Vector3 p, Vector3 normal) {
			this.p = p;
			this.normal = normal;

			if (!Mathf.Approximately(this.normal.sqrMagnitude, 1)) {
				this.normal = this.normal.normalized;
			}
		}
	}
}

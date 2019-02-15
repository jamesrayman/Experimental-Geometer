using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	public class Circle : Figure {
		public Vector3 center { get; }
		public float radius { get; }

		// the normal to the plane of the circle
		public Vector3 normal;

		public Circle (Vector3 center, float radius, Vector3 normal) {
			this.center = center;
			this.radius = radius;
			this.normal = normal;

			if (!Mathf.Approximately(this.normal.sqrMagnitude, 1)) {
				this.normal = this.normal.normalized;
			}
		}
	}
}

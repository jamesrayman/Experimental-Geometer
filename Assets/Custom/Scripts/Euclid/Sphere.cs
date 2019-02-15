using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	public class Sphere : Figure {
		public Vector3 center { get; }
		public float radius { get; }

		public Sphere (Vector3 center, float radius) {
			this.center = center;
			this.radius = radius;
		}
	}
}

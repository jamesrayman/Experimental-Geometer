using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	public class Line : Figure {
		// a point on the line
		public Vector3 p { get; }
		public Vector3 slope { get; }

		public Line (Vector3 p, Vector3 slope) {
			this.p = p;
			this.slope = slope;

			if (!Mathf.Approximately(this.slope.sqrMagnitude, 1)) {
				this.slope = this.slope.normalized;
			}
		}
	}
}

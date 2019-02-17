using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	public class Point : Figure {
		// the point itself
		public Vector3 p { get; }

		public Point (Vector3 p) {
			this.p = p;
		}

        public Point(float x, float y, float z) {
            p = new Vector3(x, y, z);
        }

        public override string ToString() {
            return p.ToString();
        }
	}
}

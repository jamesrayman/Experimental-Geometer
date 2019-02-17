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

			if (!Util.Approximately(this.normal.sqrMagnitude, 1)) {
				this.normal = this.normal.normalized;
			}
		}

        public override Figure PointOn() {
            return new Point(Vector3.Cross(normal, Util.RandomVector()) + p);
        }

        public override string ToString() {
            return "Plane " + p.ToString() + " " + normal.ToString();
        }

        // static methods
        public static bool operator ==(Plane a, Plane b) {
            return Util.Approximately(Vector3.Cross(a.normal, b.normal), Vector3.zero) && Util.Approximately(Vector3.Dot(a.p - b.p, a.normal), 0);
        }
        public static bool operator !=(Plane a, Plane b) {
            return !(a == b);
        }
        public override bool Equals(object obj) {
            return obj is Plane && this == obj as Plane;
        }
        public override int GetHashCode() {
            return p.GetHashCode() + normal.GetHashCode();
        }
    }
}

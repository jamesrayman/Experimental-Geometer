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

			if (!Util.Approximately(this.normal.sqrMagnitude, 1)) {
				this.normal = this.normal.normalized;
			}
		}

        public override Figure PointOn() {
            Vector3 deviation = Vector3.Cross(normal, Util.RandomVector()).normalized;
            if (deviation == Vector3.zero) return PointOn();
            return new Point(deviation * radius + center);
        }

        public override Figure Center() {
            return new Point(center);
        }

        public override string ToString() {
            return "Circle " + center.ToString() + " " + radius + " " + normal.ToString();
        }

        // static methods
        public static bool operator ==(Circle a, Circle b) {
            return Util.Approximately(a.center, b.center) && Util.Approximately(a.radius, b.radius) && Util.Approximately(Vector3.Cross(a.normal, b.normal), Vector3.zero);
        }
        public static bool operator !=(Circle a, Circle b) {
            return !(a == b);
        }
        public override bool Equals(object obj) {
            return obj is Circle && this == obj as Circle;
        }
        public override int GetHashCode() {
            return center.GetHashCode() + radius.GetHashCode() + normal.GetHashCode();
        }
    }
}

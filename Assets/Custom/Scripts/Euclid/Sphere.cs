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

        public Sphere (Vector3 center, Vector3 p) {
            this.center = center;
            this.radius = (p - center).magnitude;
        }

        public override Figure PointOn () {
            return new Point(Random.onUnitSphere * radius + center);
        }

        public override Figure Center() {
            return new Point(center);
        }

        public override string ToString() {
            return "Sphere " + center.ToString() + " " + radius;
        }

        // static methods
        public static bool operator ==(Sphere a, Sphere b) {
            return Util.Approximately(a.center, b.center) && Util.Approximately(a.radius, b.radius);
        }
        public static bool operator !=(Sphere a, Sphere b) {
            return !(a == b);
        }
        public override bool Equals(object obj) {
            return obj is Sphere && this == obj as Sphere;
        }
        public override int GetHashCode() {
            return center.GetHashCode() + radius.GetHashCode();
        }
    }
}

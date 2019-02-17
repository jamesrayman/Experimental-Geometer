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

        public override List<Figure> Intersection(Point point) {
            return point.Intersection(this);
        }
        public override List<Figure> Intersection(Line line) {
            return line.Intersection(this);
        }
        public override List<Figure> Intersection(Circle circle) {
            if (this == new Plane(circle.center, circle.normal))
                return new List<Figure> { circle };
            if (Util.Approximately(Vector3.Cross(normal, circle.normal), Vector3.zero))
                return new List<Figure>();
            Line line = (this.Intersection(new Plane(circle.center, circle.normal))[0] as Line);
            return line.Intersection(circle);
        }
        public override List<Figure> Intersection(Plane plane) {
            if (this == plane) return new List<Figure> { this };
            if (Util.Approximately(Vector3.Cross(normal, plane.normal), Vector3.zero))
                return new List<Figure>();
            Vector3 slope = Vector3.Cross(normal, plane.normal);
            Line line = ConstructLine(new Point(p), PointOn()) as Line;
            List<Figure> inter = line.Intersection(plane);
            return new List<Figure> { new Line((inter[0] as Point).p, slope) };
        }
        public override List<Figure> Intersection(Sphere sphere) {
            Line line = new Line(sphere.center, normal);
            Vector3 c = (line.Intersection(this)[0] as Point).p;
            float d = (c - sphere.center).magnitude;
            if (Util.Approximately(d, sphere.radius)) {
                return new List<Figure> { new Point(c) };
            }
            if (d > sphere.radius)
                return new List<Figure>();
            Circle cir = new Circle(c, Mathf.Sqrt(sphere.radius * sphere.radius - d * d), normal);
            return new List<Figure> { cir };
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

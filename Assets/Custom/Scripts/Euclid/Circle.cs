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

        public override List<Figure> Intersection(Point point) {
            return point.Intersection(this);
        }
        public override List<Figure> Intersection(Line line) {
            return line.Intersection(this);
        }
        public override List<Figure> Intersection(Circle circle) {
            List<Figure> sphereIntersects = (new Sphere(circle.center, circle.radius)).Intersection(new Sphere(center, radius));
            if (sphereIntersects.Count == 0) {
                return new List<Figure>();
            }
            if (sphereIntersects[0] is Point) {
                Point p = sphereIntersects[0] as Point;
                if (Util.Approximately(Vector3.Dot(p.p - center, normal), 0) && Util.Approximately(Vector3.Dot(p.p - circle.center, circle.normal), 0))
                    return sphereIntersects;
                else
                    return new List<Figure>();
            } else {
                List<Figure> intersects = (new Plane(circle.center, circle.normal)).Intersection(sphereIntersects[0]);
                List<Figure> res = new List<Figure>();
                foreach (Figure fig in intersects) {
                    Point p = fig as Point;
                    if (Util.Approximately(Vector3.Dot(p.p - center, normal), 0))
                        res.Add(p);
                }
                return res;
            }
        }
        public override List<Figure> Intersection(Plane plane) {
            return plane.Intersection(this);
        }
        public override List<Figure> Intersection(Sphere sphere) {
            return sphere.Intersection(this);
        }

        public override Figure Binormal(Point point) {
            return new Point(center).Binormal(point);
        }
        public override Figure Binormal(Line line) {
            return new Point(center).Binormal(line);
        }
        public override Figure Binormal(Plane plane) {
            return new Point(center).Binormal(plane);
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

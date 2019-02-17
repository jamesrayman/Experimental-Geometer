using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
    public class Point : Figure {
        // the point itself
        public Vector3 p { get; }

        public Point(Vector3 p) {
            this.p = p;
        }
        public Point(float x, float y, float z) {
            this.p = new Vector3(x, y, z);
        }

        public override Figure PointOn() {
            return this;
        }

        public override List<Figure> Intersection(Point point) {
            if (this == point)
                return new List<Figure> { this };
            return new List<Figure>();
        }
        public override List<Figure> Intersection(Line line) {
            if (Util.Approximately(Vector3.Cross(line.p - p, line.slope), Vector3.zero))
                return new List<Figure> { this };
            return new List<Figure>();
        }
        public override List<Figure> Intersection(Circle circle) {
            if (Util.Approximately(Vector3.Dot(circle.center - p, circle.normal), 0) &&
                Util.Approximately((circle.center - p).magnitude, circle.radius))
                return new List<Figure> { this };
            return new List<Figure>();
        }
        public override List<Figure> Intersection(Plane plane) {
            if (Util.Approximately(Vector3.Dot(plane.p - p, plane.normal), 0))
                return new List<Figure> { this };
            return new List<Figure>();
        }
        public override List<Figure> Intersection(Sphere sphere) {
            if (Util.Approximately((sphere.center - p).magnitude,sphere.radius))
                return new List<Figure> { this };
            return new List<Figure> ();
        }


        public override Figure Binormal(Point point) {
            return Figure.ConstructLine(this, point);
        }
        public override Figure Binormal(Line line) {
            if (Util.Approximately(Vector3.Cross(p - line.p, line.slope), Vector3.zero))
                return new Null();
            float u = Vector3.Dot(p - line.p, line.slope) / (line.slope).sqrMagnitude;
            return ConstructLine(new Point(p), new Point(line.p + u * line.slope));
        }
        public override Figure Binormal(Plane plane) {
            return new Plane (this, plane.normal);
        }


        public override string ToString() {
            return "Point " + p.ToString();
        }

        // static methods
        public static implicit operator Vector3(Point point) {
            return point.p;
        }
        public static bool operator ==(Point a, Point b) {
            return Util.Approximately(a.p, b.p);
        }
        public static bool operator !=(Point a, Point b) {
            return !(a == b);
        }
        public override bool Equals(object obj) {
            return obj is Point && this == obj as Point;
        }
        public override int GetHashCode() {
            return p.GetHashCode();
        }
    }
}

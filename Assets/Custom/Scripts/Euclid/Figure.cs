using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	// parent class for Point, Line, etc.
	// non-construction data type in Construction
	public class Figure {
        public Dictionary<string, object> properties;
        public Figure() {
            properties = new Dictionary<string, object>();
        }

        public virtual List<Figure> Intersection(Figure f) {
            if (f is Null) {
                return Intersection(f as Null);
            }
            if (f is Point) {
                return Intersection(f as Point);
            }
            if (f is Line) {
                return Intersection(f as Line);
            }
            if (f is Circle) {
                return Intersection(f as Circle);
            }
            if (f is Plane) {
                return Intersection(f as Plane);
            }
            if (f is Sphere) {
                return Intersection(f as Sphere);
            }
            if (f is Space) {
                return Intersection(f as Space);
            }
            return new List<Figure>();
        }

        public virtual List<Figure> Intersection (Null n) {
            return new List<Figure>();
        }
        public virtual List<Figure> Intersection(Point point) {
            return new List<Figure>();
        }
        public virtual List<Figure> Intersection(Line line) {
            return new List<Figure>();
        }
        public virtual List<Figure> Intersection(Circle circle) {
            return new List<Figure>();
        }
        public virtual List<Figure> Intersection(Plane plane) {
            return new List<Figure>();
        }
        public virtual List<Figure> Intersection(Sphere sphere) {
            return new List<Figure>();
        }
        public virtual List<Figure> Intersection(Space space) {
            return new List<Figure>() { this };
        }

        public virtual Figure Binormal(Figure f) {
            if (f is Null) {
                return Binormal(f as Null);
            }
            if (f is Point) {
                return Binormal(f as Point);
            }
            if (f is Line) {
                return Binormal(f as Line);
            }
            if (f is Circle) {
                return Binormal(f as Circle);
            }
            if (f is Plane) {
                return Binormal(f as Plane);
            }
            if (f is Sphere) {
                return Binormal(f as Sphere);
            }
            if (f is Space) {
                return Binormal(f as Space);
            }
            return new Null();
        }

        public virtual Figure Binormal(Null n) {
            return new Null();
        }
        public virtual Figure Binormal(Point point) {
            return new Null();
        }
        public virtual Figure Binormal(Line line) {
            return new Null();
        }
        public virtual Figure Binormal(Circle circle) {
            // TODO: this is only the case if [this] and [circle] are coplanar
            return Binormal(new Point(circle.center));
        }
        public virtual Figure Binormal(Plane plane) {
            return new Null();
        }
        public virtual Figure Binormal(Sphere sphere) {
            return Binormal(new Point(sphere.center));
        }
        public virtual Figure Binormal(Space space) {
            return new Null();
        }

        public virtual Figure PointOn() {
            return new Null();
        }

        public virtual Figure Center() {
            return new Null();
        }

        // the base constuctions
        public static Figure ConstructPoint(float x, float y, float z) {
            return new Point(x, y, z);
        }
		
		// Construct a plane given three points on the plane, or null if the three points are collinear
		public static Figure ConstructPlane (Figure fAlpha, Figure fBeta, Figure fGamma) {
			if (fAlpha is Point && fBeta is Point && fGamma is Point) {
                var a = fAlpha as Point;
                var b = fBeta as Point;
                var c = fGamma as Point;

                Vector3 x = a.p - b.p;
                Vector3 y = c.p - b.p;
                Vector3 normal = Vector3.Cross(x, y);

                if (normal == Vector3.zero)
                    return new Null();

                return new Plane(a.p, normal);
            }
            return new Null();
		}
        
		public static Figure ConstructSphere (Figure fCenter, Figure fP) {
            if (fCenter is Point && fP is Point) {
                var center = fCenter as Point;
                var p = fP as Point;
                return new Sphere(center, p);
            }
            return new Null();
        }
		
		public static List<Figure> Intersection (Figure fAlpha, Figure fBeta) {
            return fAlpha.Intersection(fBeta);
		}
		
		public static Figure PointOn (Figure fAlpha) {
            return fAlpha.PointOn();
		}
		
		// constructions which could be implemented using the base constructions,
		// but are written in C# for conveniences
		
		// Construct the line which is normal to both alpha and beta, or null if no or multiple such lines exist
		public static Figure Binormal (Figure fAlpha, Figure fBeta) {
			return fAlpha.Binormal(fBeta);
		}
		
		// Construct a line given two points, or null if the points are not distinct
		public static Figure ConstructLine (Figure fAlpha, Figure fBeta) {
			if (fAlpha is Point && fBeta is Point) {
                var a = fAlpha as Point;
                var b = fBeta as Point;

                return new Line(a, a.p - b.p);
            }
            return new Null();
		}
		
		// Construct the center of a sphere or a circle, or null if alpha is not a sphere or a circle
		public static Figure Center (Figure fAlpha) {
            return fAlpha.Center();
		}
	}
}

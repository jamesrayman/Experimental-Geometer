using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	// parent class for Point, Line, etc.
	// non-construction data type in Construction
	public class Figure {
        public Dictionary<string, string> renderSpecs;

		// the base constuctions
		
		// Construct a plane given three points on the plane, or null if the three points are collinear
		public static Figure ConstructPlane (Figure fAlpha, Figure fBeta, Figure fGamma) {
			if (fAlpha is Point && fBeta is Point && fGamma is Point)
            {
                Point a = fAlpha as Point;
                Point b = fBeta as Point;
                Point c = fGamma as Point;

                Vector3 x = a.p - b.p;
                Vector3 y = c.p - b.p;
                Vector3 normal = Vector3.Cross(x, y).normalized;

                if (normal == Vector3.zero)
                    return new Null();

                return new Plane(a.p, normal);
            }
            return new Null();
		}
        
		public static Figure ConstructSphere (Figure fCenter, Figure fP) {
            if (fCenter is Point && fP is Point)
            {
                Point center = fCenter as Point;
                Point p = fP as Point;
                //return new Sphere(center, p);
                return new Null();
            }
            return new Null();
        }
		
		public static List<Figure> Intersection (Figure alpha, Figure beta) {
			return new List<Figure>();
		}
		
		public static Figure PointOn (Figure alpha) {
			return new Null();
		}
		
		// constructions which could be implemented using the base constructions,
		// but are written in C# for conveniences
		
		// Construct the line which is normal to both alpha and beta, or null if no or multiple such lines exist
		public static Figure Binormal (Figure alpha, Figure beta) {
			return new Null();
		}
		
		// Construct a line given two points, or null if the points are not distinct
		public static Figure ConstructLine (Figure alpha, Figure beta) {
			return new Null();
		}
		
		// Construct the center of a sphere or a circle, or null if alpha is not a sphere or a circle
		public static Figure Center (Figure alpha) {
			return new Null();
		}
		
		
	}
}
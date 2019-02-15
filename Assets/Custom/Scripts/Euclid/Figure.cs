using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	// parent class for Point, Line, etc.
	// non-construction data type in Construction
	public class Figure {
		// the base constuctions
		
		// Construct a plane given three points on the plane, or null if the three points are collinear
		public static Figure ConstructPlane (Figure alpha, Figure beta, Figure gamma) {
			if (alpha is Point && beta is Point && gamma is Point)
            {

            }
            return new Null();
		}
		
		public static Figure ConstructSphere (Figure center, Figure p) {
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
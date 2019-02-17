using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	// the set of all points in 3d space
	public class Space : Figure {
		public Space () {
			
		}

        public override Figure PointOn() {
            return new Point(Util.RandomVector());
        }

        
        public override List<Figure> Intersection(Point point) {
            return new List<Figure>() { point };
        }
        public override List<Figure> Intersection(Line line) {
            return new List<Figure>() { line };
        }
        public override List<Figure> Intersection(Circle circle) {
            return new List<Figure>() { circle };
        }
        public override List<Figure> Intersection(Plane plane) {
            return new List<Figure>() { plane };
        }
        public override List<Figure> Intersection(Sphere sphere) {
            return new List<Figure>() { sphere };
        }

        public override string ToString() {
            return "Space";
        }

        // Center and Binormal are always return null

        // static methods
        public static bool operator ==(Space a, Space b) {
            return true;
        }
        public static bool operator !=(Space a, Space b) {
            return !(a == b);
        }
        public override bool Equals(object obj) {
            return obj is Space && this == obj as Space;
        }
        public override int GetHashCode() {
            return 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
	// the empty set
	public class Null : Figure {
		public Null () {

        }

        public override string ToString() {
            return "Null";
        }

        // Center, Binormal, PointOn, and Intersection are always null

        // static methods
        public static bool operator ==(Null a, Null b) {
            return true;
        }
        public static bool operator !=(Null a, Null b) {
            return !(a == b);
        }
        public override bool Equals(object obj) {
            return obj is Null && this == obj as Null;
        }
        public override int GetHashCode() {
            return 0;
        }
    }
}

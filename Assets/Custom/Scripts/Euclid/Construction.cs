using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Euclid {
    /* Construction class to handle parsing and outputting constructions based on the diagram files.
     */
    public class Construction {
        private SyntaxNode root;
        private string fileName;

        // Constructs a new construction from file
        public Construction(string fileName) {
            this.fileName = fileName;
            ReadFromFile();
        }

        // Read the construction from the Euclid file
        private void ReadFromFile() {
        }

        // Write all statements to file
        private void WriteToFile() {
            StreamWriter sw = File.CreateText(fileName);
        }

        private List<Figure> RunConstruction() {

        }

        private class SyntaxNode {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderMaster : MonoBehaviour {
    List<Euclid.Figure> figures;
    public GameObject pointPrefab;
    public GameObject linePrefab;
    public GameObject spherePrefab;
    public GameObject planePrefab;
    public GameObject circlePrefab;

    public Euclid.Construction diagram;

    private void Start() {
        diagram = new Euclid.Construction("Assets/Custom/Scripts/Constructions/test.euclid");

        Render();
    }

    public void Render () {
        figures = diagram.Execute();

        for (int i = transform.childCount-1; i > -1; i--) {
            Destroy(transform.GetChild(i));
        }
        foreach (Euclid.Figure f in figures) {
            if (f is Euclid.Point) {
                var point = f as Euclid.Point;
                GameObject g = Instantiate<GameObject>(pointPrefab);
                g.transform.parent = transform;
                g.SetActive(true);
                g.transform.SetPositionAndRotation(point.p, Quaternion.identity);
            }
            if (f is Euclid.Line) {
                var line = f as Euclid.Line;
                GameObject g = Instantiate<GameObject>(linePrefab);
                g.transform.parent = transform;
                g.SetActive(true);
                var render = g.GetComponent<LineRenderer>();
                Vector3[] v = new Vector3[] { line.p + 1000 * line.slope, line.p - 1000 * line.slope };

                if (line.properties.ContainsKey("start")) {
                    v[0] = (line.properties["start"] as Euclid.Point).p;
                }
                if (line.properties.ContainsKey("end")) {
                    v[1] = (line.properties["end"] as Euclid.Point).p;
                }

                render.positionCount = 2;
                render.SetPositions(v);
            }

            if (f is Euclid.Sphere) {
                var sphere = f as Euclid.Sphere;
                GameObject g = Instantiate<GameObject>(spherePrefab);
                g.transform.parent = transform;
                g.SetActive(true);
                g.transform.SetPositionAndRotation(sphere.center, Quaternion.identity);
                g.transform.localScale = Vector3.one * sphere.radius;
            }

            if (f is Euclid.Plane) {
                var plane = f as Euclid.Plane;
                GameObject g = Instantiate<GameObject>(planePrefab);
                g.transform.parent = transform;
                g.SetActive(true);
                g.transform.SetPositionAndRotation(plane.p, Quaternion.identity);
                g.transform.LookAt(plane.normal);
            }

            if (f is Euclid.Circle) {
                var circle = f as Euclid.Circle;
                GameObject g = Instantiate<GameObject>(circlePrefab);
                g.transform.parent = transform;
                g.SetActive(true);
                var render = g.GetComponent<LineRenderer>();
                Vector3[] v = new Vector3[60];
                for (int i = 0; i < 60; i++) {
                    v[i] = circle.center + Quaternion.Euler(90, i*6, 0) * circle.normal * circle.radius;
                }
                render.loop = true;
                render.positionCount = 60;
                render.SetPositions(v);
            }

        }
    }
}

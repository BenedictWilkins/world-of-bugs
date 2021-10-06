using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCubes : MonoBehaviour {

    public int seed = 1;
    public GameObject cubePrefab;
    public int cubes;
    public Vector2 size_range; 
    public Vector2 position_range;

    private List<GameObject> _cubes = new List<GameObject>();

    void Start() {
        Random.InitState(seed);
        Shader shader = Shader.Find("Standard");

        for (int i = 0; i < cubes; i++) {
            GameObject cube = Instantiate(cubePrefab, RandomPosition(position_range), Quaternion.identity, cubePrefab.transform.parent);
            cube.transform.localScale = RandomSize(size_range);
            cube.transform.position += new Vector3(0f, cube.transform.localScale[1] / 2, 0f);

            MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
            Material material = new Material(shader);
            material.color = RandomColour();
            renderer.material = material;
            
            UpDown updown = cube.GetComponent<UpDown>();
            updown.offset = Random.Range(0f,10f);

            _cubes.Add(cube);
        }
    }

    Color RandomColour() {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    Vector3 RandomSize(Vector2 range) { 
        return new Vector3(Random.Range(range.x, range.y), Random.Range(range.x, range.y), Random.Range(range.x, range.y)); 
    }

    Vector3 RandomPosition(Vector2 range) { 
        float rx = Random.Range(range.x, range.y);
        float ry = Random.Range(range.x, range.y);

        float a = Random.Range(0,  2 * Mathf.PI);
        float x = Mathf.Cos(a) * rx;
        float y = Mathf.Sin(a) * ry;
 
        return new Vector3(x, 0, y); 
    }
}

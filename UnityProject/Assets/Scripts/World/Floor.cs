using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    public GameObject p_tile;
    public Vector2 num_tiles;
    private GameObject[][] tiles;

    // Start is called before the first frame update
    void Start()
    {   
        tiles = new GameObject[(int)num_tiles[0]][];
        Vector3 tile_bounds = p_tile.GetComponent<Collider>().bounds.size;
        Vector2 tile_size = new Vector2(tile_bounds[0], tile_bounds[2]);

        Vector2 offset = new Vector2(num_tiles[0] * tile_size[0], num_tiles[1] * tile_size[1]);
        offset = offset / 2 - tile_size / 2;

        for(int i = 0; i < num_tiles[0]; i++) {
            for(int j = 0; j < num_tiles[1]; j++) {
                GameObject tile = Instantiate(p_tile, new Vector3(offset[0] - i * tile_size[0], 0, offset[1] - j * tile_size[1]), Quaternion.identity);
                tile.transform.parent = gameObject.transform;
            }
        }
      

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

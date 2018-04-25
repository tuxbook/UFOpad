using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoPart : MonoBehaviour 
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;

    public Material material;
    public float angle = 10;
    public float radius = 3;

	// Use this for initialization
	void Start () 
    {
        gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        meshRenderer.material = material;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new List<Vector3>();

        // Start with center
        vertices.Add(new Vector3(0, 0, 0));

        var x = radius * Mathf.Sin(angle);
        var y = radius * Mathf.Cos(angle);

        // And add 2 more points
        vertices.Add(new Vector3(0, 0, radius));
        vertices.Add(new Vector3(x, 0, y));

        mesh.vertices = vertices.ToArray();

        triangles = new List<int>();
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);

        mesh.triangles = triangles.ToArray();

        //
        // Create a collision mesh (it's the same data as the rendered mesh)
        //
        Mesh collisionMesh = new Mesh();
        collisionMesh.vertices = vertices.ToArray();
        collisionMesh.triangles = triangles.ToArray();

        //MeshCollider meshCollider = gameObj.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = collisionMesh;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}

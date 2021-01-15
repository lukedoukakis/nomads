﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkData
{
 
    public Vector2 coord;
    public bool loaded;
    public int randomState;

    public GameObject chunk;
    public GameObject trees;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Mesh mesh;

    public float[,] TerrainMap;
    public float[,] CliffMap;
    public bool[,] TreeMap;
    public bool[,] ShoreRockMap;

    public ChunkData(Vector2 _coord)
    {
        coord = _coord;
        loaded = false;
    }

    public void init(GameObject obj)
    {
        randomState = (int)(coord.x + coord.y * 10f);
        chunk = GameObject.Instantiate(obj);
        trees = new GameObject();
        trees.transform.SetParent(chunk.transform);
        meshRenderer = chunk.GetComponent<MeshRenderer>();
        meshFilter = chunk.GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = mesh;

        loaded = true;
    }

    public void Deload()
    {
        mesh.Clear();
        GameObject g = chunk;
        chunk = null;
        GameObject.Destroy(g);
        loaded = false;
        foreach (Transform child in trees.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

}
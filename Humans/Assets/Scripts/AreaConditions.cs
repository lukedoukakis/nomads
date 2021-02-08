﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to contain data about a sample of the area around a certain point
public class AreaConditions
{

    static readonly int AreaSize = 5;

    static ChunkData OriginChunk;
    static int OriginX;
    static int OriginZ;

    public static float Height;
    public static float Temperature;
    public static float Wetness;
    public static float FreshWater;

    // for instantiations
    public float height;
    public float temperature;
    public float wetness;
    public float freshWater;


    //public float woodiness;
    // FUTURE?: Resource[] resources

    // return an instantiation of AreaConditions
    public AreaConditions(Vector3 position)
    {

        GetAreaConditions(position);
        SampleConditions();
        height = Height;
        temperature = Temperature;
        wetness = Wetness;
        freshWater = FreshWater;

    }

    // get the environmental conditions of an area around the given position
    public static void GetAreaConditions(Vector3 position)
    {
        ChunkData chunk = ChunkGenerator.GetChunk(position);
        Vector2 position_chunkSpace = ChunkGenerator.ToChunkSpace(position);

        int x = (int)(ChunkGenerator.chunkSize * Mathf.Abs((position_chunkSpace.x - chunk.coord.x)));
        int z = (int)(ChunkGenerator.chunkSize * Mathf.Abs((position_chunkSpace.y - chunk.coord.y)));
        //Debug.Log(new Vector2(x, z).ToString());

        OriginChunk = chunk;
        OriginX = x;
        OriginZ = z;

        SampleConditions();

    }


    // sample the conditions in a square around originX and originZ
    static void SampleConditions()
    {

        int chunkSize = ChunkGenerator.chunkSize;

        // intermediate variables
        ChunkData cd;
        Vector2 chunkCoord;
        int overflowX, overflowZ;
        int sampleX, sampleZ;

        float hgt = 0;
        float tmp = 0;
        float wet = 0;
        float fw = 0;

        for (int z = OriginZ - AreaSize; z < OriginZ + AreaSize; z++)
        {
            // determine overflowZ
            if (z >= 0)
            {
                if (z >= chunkSize + 1)
                {
                    overflowZ = 1;
                }
                else
                {
                    overflowZ = 0;
                }
            }
            else
            {
                overflowZ = -1;
            }

            // determine sampleZ
            if(overflowZ != 0)
            {
                if (overflowZ == -1) { sampleZ = (chunkSize + 1) + z; }
                else { sampleZ = z - (chunkSize + 1); }
            }
            else
            {
                sampleZ = z;
            }

            for (int x = OriginX - AreaSize; x < OriginX + AreaSize; x++)
            {
                // determine overflowX
                if (x >= 0)
                {
                    if (x >= chunkSize + 1)
                    {
                        overflowX = 1;
                    }
                    else
                    {
                        overflowX = 0;
                    }
                }
                else
                {
                    overflowX = -1;
                }

                // determine sampleX
                if(overflowX != 0)
                {
                    if (overflowX == -1) { sampleX = (chunkSize + 1) + x; }
                    else { sampleX = x - (chunkSize + 1); }
                }
                else
                {
                    sampleX = x;
                }

                // determine chunk to sample from
                if(overflowX != 0 || overflowZ != 0)
                {
                    chunkCoord = OriginChunk.coord + new Vector2(overflowX, overflowZ);
                    cd = ChunkGenerator.GetChunk(chunkCoord);
                }
                else
                {
                    cd = OriginChunk;
                }

                // add data samples to pool
                //Debug.Log(new Vector2(sampleX, sampleZ).ToString());
                hgt += cd.HeightMap[sampleX, sampleZ];
                tmp += cd.TemperatureMap[sampleX, sampleZ];
                wet += cd.WetnessMap[sampleX, sampleZ];
                fw += cd.FreshWaterMap[sampleX, sampleZ];
            }
        }

        float divisor = Mathf.Pow(AreaSize * 2, 2);
        Height = hgt / divisor;
        Temperature = tmp / divisor;
        Wetness = wet / divisor;
        FreshWater = fw / divisor;
        
    }


    
}
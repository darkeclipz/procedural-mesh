using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {
    
    public static float [,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        var noiseMap = new float[mapWidth, mapHeight];
        var prng = new System.Random(seed);
        var octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(scale <= 0) {
            scale = 0.001f;
        }

        // Used to normalize the noise map.
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Used to center the texture.
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        // Generate the noise map
        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++) {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x + offset.x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y + offset.y;

                    float perlinValue = 2 * Mathf.PerlinNoise(sampleX, sampleY) - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                
                if(noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                }

                if(noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalize the noise map.
        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {     
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

}

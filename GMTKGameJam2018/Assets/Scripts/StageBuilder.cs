using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageBuilder : MonoBehaviour
{
    public Transform[] rooms;
    float nextRoom;
    public CameraController cameraController;
    private float offSet;
    public Transform chip;
    public Transform enemy;

    private static int width = 15;
    private static int height = 20;
    private float enemyMin = 10.0f;
    private float enemyMax = 40.0f;
    private float nextEnemy;
        

	// Use this for initialization
	void Start ()
    {
        nextRoom = transform.position.x;
        offSet = transform.position.x - cameraController.transform.position.x;
        nextEnemy = TimeKeeper.GetTime() + Random.Range(enemyMin, enemyMax);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(cameraController.transform.position.x + offSet, transform.position.y, transform.position.z);
		if (transform.position.x > nextRoom)
        {
            nextRoom = transform.position.x + 30;
            SpawnSetPiece();
        }

        if (TimeKeeper.GetTime() > nextEnemy)
        {
            Instantiate(enemy, new Vector3(transform.position.x, transform.position.y - 10, 0), Quaternion.identity);
            nextEnemy = TimeKeeper.GetTime() + Random.Range(enemyMin, enemyMax);
        }
	}

    private void SpawnChips()
    {
        var totalFormations = Random.Range(2, 4) + Random.Range(2, 4);
        List<ChipMatrix> formations = new List<ChipMatrix>();
        for (int i = 0; i < totalFormations; i++)
        {
            formations.Add(matrices[Random.Range(0, matrices.Count)]);
        }

        var fullMatrices = formations.Select(x => x.GetForCoordinates(Random.Range(0, width), Random.Range(0, height)));

        bool[,] finalMatrix = fullMatrices.Aggregate((a, b) => FoldMatrices(a, b));

        for (int i = -14; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var relativeI = Math.Abs(i);
                var relativeJ = Math.Abs(j);

                if (finalMatrix[relativeI, relativeJ])
                    InstantiateChipAtLocation(i, j);
            }
        }
    }

    private void InstantiateChipAtLocation(int x, int y)
    {
        Instantiate(chip, new Vector3(transform.position.x + x, transform.position.y + y, 0), Quaternion.identity);
    }

    private void SpawnSetPiece()
    {
        var pick = Random.Range(0, rooms.Length);
        Instantiate(rooms[pick], new Vector3(transform.position.x, 19, 0), Quaternion.identity);
    }

    private static bool[,] FoldMatrices(bool[,] a, bool[,] b)
    {
        var matrixLengthX = a.GetLength(0);
        var matrixLengthY = a.GetLength(1);
        bool[,] toReturn = new bool[matrixLengthX, matrixLengthY];
        for (int i = 0; i < matrixLengthX; i++)
        {
            for (int j = 0; j < matrixLengthY; j++)
            {
                toReturn[i, j] = a[i, j] || b[i, j];
            }
        }

        return toReturn;
    }

    private class ChipMatrix
    {
        private bool[,] matrix;

        public bool [,] GetForCoordinates(int x, int y)
        {
            var toReturn = new bool[width, height];
            var matrixLengthX = matrix.GetLength(0);
            var matrixLengthY = matrix.GetLength(1);
            for (int i = 0; i < matrixLengthX; i++)
            {
                if (i + x >= width)
                    break;

                for (int j = 0; j < matrixLengthY; j++)
                {
                    if (j + y >= height)
                        break;

                    toReturn[i + x, j + y] = matrix[i, j];
                }
            }

            return toReturn; 
        }

        public ChipMatrix(bool[,] matrix)
        {
            this.matrix = matrix;
        }
    }

    private static List<ChipMatrix> matrices = new List<ChipMatrix>()
    {
        new ChipMatrix(new bool[6, 2] { { true, true }, { true, true },  { true, true }, { true, true }, { true, true }, { true, true }}),
        new ChipMatrix(new bool[2, 2] { { true, true }, { true, true } }),
        new ChipMatrix(new bool[2, 6] { { true, true, true, true, true, true }, { true, true, true, true, true, true } }),
        new ChipMatrix(new bool[4, 4] { { true, true, true, true }, { true, true, true, true }, { true, true, true, true }, { true, true, true, true } }),
        new ChipMatrix(new bool[10, 4] 
        {
            { false, false, true, true },
            { false, false, true, true },
            { false, false, true, true },
            { true, true, true, true },
            { true, true, false, false },
            { true, true, false, false },
            { true, true, true, true },
            { false, false, true, true },
            { false, false, true, true },
            { false, false, true, true },
        }),
    };
}

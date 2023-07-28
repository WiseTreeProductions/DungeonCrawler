using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    public int positionNumber;

    public int row;
    public int column;

    public bool occupied;

    public List<Position> adjacentPositions = new List<Position>();

    public GameObject characterInSlot;

}

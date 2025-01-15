using System.Collections;
using System.Collections;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public int rows = 5;
    public int cols = 5;
    public float nodeSize = 1.0f;
    public GameObject actorPrefab;
    public GameObject pathMarkerPrefab;
    public GameObject goalMarkerPrefab;

    [Header("Coordenadas del actor (configurables)")]
    public Vector2Int start = new Vector2Int(0, 0);
    public Vector2Int goal = new Vector2Int(4, 4);

    private Vector3[,] nodes;
    private GameObject actor;

    void Start()
    {
        ValidateCoordinates();
        GenerateGrid();
        PlaceGoalMarker(goal);
        actor = Instantiate(actorPrefab, nodes[start.x, start.y], Quaternion.identity);
        StartCoroutine(MoveActor(start, goal));
    }

    void GenerateGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        nodes = new Vector3[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                nodes[i, j] = new Vector3(j * nodeSize, 0, i * nodeSize);
                GameObject nodeVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                nodeVisual.transform.position = nodes[i, j];
                nodeVisual.transform.localScale = Vector3.one * 0.2f;
                nodeVisual.transform.parent = transform;
            }
        }
    }

    IEnumerator MoveActor(Vector2Int start, Vector2Int goal)
    {
        Vector2Int currentPos = start;
        yield return new WaitForSeconds(1f);

        while (currentPos != goal)
        {
            PlacePathMarker(currentPos);

            if (currentPos.x < goal.x) currentPos.x++;
            else if (currentPos.x > goal.x) currentPos.x--;

            if (currentPos.y < goal.y) currentPos.y++;
            else if (currentPos.y > goal.y) currentPos.y--;

            actor.transform.position = nodes[currentPos.x, currentPos.y];
            yield return new WaitForSeconds(0.5f);
        }

        PlacePathMarker(goal);
        Debug.Log("El actor ha llegado a la meta!");
    }

    void PlacePathMarker(Vector2Int position)
    {
        Instantiate(pathMarkerPrefab, nodes[position.x, position.y], Quaternion.identity);
    }

    void PlaceGoalMarker(Vector2Int goalPosition)
    {
        Instantiate(goalMarkerPrefab, nodes[goalPosition.x, goalPosition.y], Quaternion.identity);
    }

    void ValidateCoordinates()
    {
        start.x = Mathf.Clamp(start.x, 0, rows - 1);
        start.y = Mathf.Clamp(start.y, 0, cols - 1);
        goal.x = Mathf.Clamp(goal.x, 0, rows - 1);
        goal.y = Mathf.Clamp(goal.y, 0, cols - 1);
    }
}

using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoleMovement : MonoBehaviour
{
    [Header ("Hole Mesh")]
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;

    [Header("Hole Vertices Radius")] 
    [SerializeField] private Vector3 moveLimits;
    [SerializeField] private float radius;
    [SerializeField] private Transform holeCenter;
    [SerializeField] private Transform rotatingCircle;
    
    [Space]
    [SerializeField] private float moveSpeed;

    private Mesh mesh;
    private List<int> holeVertices;
    private List<Vector3> offsets;
    private int holeVerticesCount;

    private float x, y;
    private Vector3 touch, targetPosition;

    private void Start()
    {
        RotateCircleAnim();
        
        GameManager.isMoving = false;
        GameManager.isGameOver = false; 
        
        holeVertices = new List<int>();
        offsets = new List<Vector3>();

        mesh = meshFilter.mesh;
        FindHoleVertices();
    }

    private void RotateCircleAnim()
    {
        rotatingCircle
            .DORotate(new Vector3(90f, 0f, -90f), .2f)
            .SetEase(Ease.Linear)
            .From(new Vector3(90f, 0f, 0f))
            .SetLoops(-1, LoopType.Incremental);
    }
    private void Update()
    {
        MouseMovement();
    }

    private void MouseMovement()
    {
        GameManager.isMoving = Input.GetMouseButton(0);
        if (!GameManager.isGameOver && GameManager.isMoving)
        {
            MoveHole();                     // move hole center
            UpdateHoleVerticesPosition();   // update hole vertices
        }
    }

    private void MobileTouchMovement()
    {
        GameManager.isMoving = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;
        if (!GameManager.isGameOver && GameManager.isMoving)
        {
            MoveHole();                     // move hole center
            UpdateHoleVerticesPosition();   // update hole vertices
        }
    }
    
    private void MoveHole()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        
        touch = Vector3.Lerp(
            holeCenter.position, 
            holeCenter.position + new Vector3(x, 0f, y),
            moveSpeed * Time.deltaTime
        );

        targetPosition = new Vector3(
            Mathf.Clamp(touch.x, -moveLimits.x, moveLimits.x),
            touch.y,
            Mathf.Clamp(touch.z, -moveLimits.y, moveLimits.y)
        );
        
        holeCenter.position = targetPosition;
    }
    
    private void UpdateHoleVerticesPosition()
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < holeVerticesCount; i++)
        {
            vertices[holeVertices[i]] = holeCenter.position + offsets[i];
        }
        // update mesh
        mesh.vertices = vertices;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    private void FindHoleVertices()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distance = Vector3.Distance(holeCenter.position, mesh.vertices[i]);
            if (distance < radius)
            {
                holeVertices.Add(i);
                offsets.Add(mesh.vertices[i] - holeCenter.position);
            }
        }

        holeVerticesCount = holeVertices.Count;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(holeCenter.position, radius);
    }
}

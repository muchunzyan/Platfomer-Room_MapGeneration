using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Island
{
    [RequireComponent(typeof(MeshFilter))]
    public class IslandGenerator : MonoBehaviour
    {
        [SerializeField] private int islandXSize, islandZSize;

        private MeshCollider _islandMeshCollider;
        private Mesh _islandMesh;
        private List<Vector3> _islandVertices = new List<Vector3>();
        private List<int> _islandTriangles = new List<int>();

        [SerializeField] private GameObject waterGo;
        [SerializeField] private int waterXSize, waterZSize;
        private MeshCollider _waterMeshCollider;
        private Mesh _waterMesh;
        private List<Vector3> _waterVertices = new List<Vector3>();
        private List<int> _waterTriangles = new List<int>();

        [SerializeField] private float borderSpreadDistance;

        private void Start()
        {
            _islandMeshCollider = GetComponent<MeshCollider>();
            _waterMeshCollider = waterGo.GetComponent<MeshCollider>();
            GenerateIslandAndWater();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene("Scenes/Island");
            }
        }

        private void GenerateIslandAndWater()
        {
            _islandMesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _islandMesh;
            
            _waterMesh = new Mesh();
            waterGo.GetComponent<MeshFilter>().mesh = _waterMesh;

            var lakeX = Random.Range(0, 3);
            var lakeZ = Random.Range(0, 3);
            CreateShape(lakeX, lakeZ);

            _islandMeshCollider.sharedMesh = _islandMesh;
            _waterMeshCollider.sharedMesh = _waterMesh;
        }

        private void CreateShape(int lakeX, int lakeZ)
        {
            // GenerateVertsForIsland();
            // GenerateTrianglesForIsland(lakeX, lakeZ);
            // GenerateVertsForWater();
            // GenerateTrianglesForWater();
            //
            // UpdateMeshes();
            // RemoveRedundantVerts(lakeX, lakeZ);
            
            GenerateVertsForIsland();
            MoveBorders();
            GenerateTrianglesForIsland(lakeX, lakeZ);
            GenerateVertsForWater();
            GenerateTrianglesForWater();
            
            UpdateMeshes();
            // RemoveRedundantVerts(lakeX, lakeZ);
        }

        private void GenerateVertsForIsland()
        {
            // генерация точек для острова
            for (var z = 0; z < islandZSize; z++)
            {
                for (var x = 0; x < islandXSize; x++)
                {
                    _islandVertices.Add(new Vector3(x + 0, 0, z + 0));
                    _islandVertices.Add(new Vector3(x + 0, 0, z + 1));
                    _islandVertices.Add(new Vector3(x + 1, 0, z + 0));
                    _islandVertices.Add(new Vector3(x + 1, 0, z + 1));
                }
            }
        }

        private void MoveBorders()
        {
            // движение границ
            
            for (var i = 0; i < _islandVertices.Count; i++)
            {
                if (_islandVertices[i].z == 0)
                {
                    var spread = Random.Range(-borderSpreadDistance, 0);
                    _islandVertices[i] = new Vector3(
                        _islandVertices[i].x,
                        _islandVertices[i].y,
                        _islandVertices[i].z + spread);
                } else if (_islandVertices[i].x == 0)
                {
                    var spread = Random.Range(-borderSpreadDistance, 0);
                    _islandVertices[i] = new Vector3(
                        _islandVertices[i].x + spread,
                        _islandVertices[i].y,
                        _islandVertices[i].z);
                }
            }
        }

        private void GenerateTrianglesForIsland(int lakeX, int lakeZ)
        {
            // генерация триугольников для острова
            for (var i = 0; i < islandZSize; i++)
            {
                for (var j = 0; j < islandXSize; j++)
                {
                    if (lakeX * islandXSize / 3 <= j && j <= lakeX * islandXSize / 3 + islandXSize / 3 && 
                        lakeZ * islandZSize / 3 <= i && i <= lakeZ * islandZSize / 3 + islandZSize / 3)
                    {
                        continue;
                    }
                    _islandTriangles.Add(i * islandXSize * 4 + j * 4 + 0);
                    _islandTriangles.Add(i * islandXSize * 4 + j * 4 + 1);
                    _islandTriangles.Add(i * islandXSize * 4 + j * 4 + 2);
                    
                    _islandTriangles.Add(i * islandXSize * 4 + j * 4 + 1);
                    _islandTriangles.Add(i * islandXSize * 4 + j * 4 + 3);
                    _islandTriangles.Add(i * islandXSize * 4 + j * 4 + 2);
                }
            }
        }

        private void GenerateVertsForWater()
        {
            // генерация точек для воды
            for (var z = 0; z < waterZSize; z++)
            {
                for (var x = 0; x < waterXSize; x++)
                {
                    _waterVertices.Add(new Vector3(-(waterXSize - islandXSize) / 2 + x + 0, -1,
                        -(waterZSize - islandZSize) / 2 + z + 0));
                    _waterVertices.Add(new Vector3(-(waterXSize - islandXSize) / 2 + x + 0, -1,
                        -(waterZSize - islandZSize) / 2 + z + 1));
                    _waterVertices.Add(new Vector3(-(waterXSize - islandXSize) / 2 + x + 1, -1,
                        -(waterZSize - islandZSize) / 2 + z + 0));
                    _waterVertices.Add(new Vector3(-(waterXSize - islandXSize) / 2 + x + 1, -1,
                        -(waterZSize - islandZSize) / 2 + z + 1));
                }
            }
        }

        private void GenerateTrianglesForWater()
        {
            // генерация триугольников для моря
            for (var i = 0; i < waterZSize; i++)
            {
                for (var j = 0; j < waterXSize; j++)
                {
                    _waterTriangles.Add(i * waterXSize * 4 + j * 4 + 0);
                    _waterTriangles.Add(i * waterXSize * 4 + j * 4 + 1);
                    _waterTriangles.Add(i * waterXSize * 4 + j * 4 + 2);
                    
                    _waterTriangles.Add(i * waterXSize * 4 + j * 4 + 1);
                    _waterTriangles.Add(i * waterXSize * 4 + j * 4 + 3);
                    _waterTriangles.Add(i * waterXSize * 4 + j * 4 + 2);
                }
            }
        }

        private void RemoveRedundantVerts(int lakeX, int lakeZ)
        {
            // удаление лишних точек для острова
            var islandVertNumber = 0;
            while (islandVertNumber < _islandVertices.Count)
            {
                var v = _islandVertices[islandVertNumber];
                var islandPieceXSize = islandXSize / 3;
                var islandPieceZSize = islandZSize / 3;
                
                if (lakeX > 0 && lakeZ > 0)
                {
                    if (lakeX * islandPieceXSize < v.x && v.x <= lakeX * islandPieceXSize + islandPieceXSize &&
                        lakeZ * islandPieceZSize < v.z && v.z <= lakeZ * islandPieceZSize + islandPieceZSize)
                    {
                        _islandVertices.Remove(v);
                    }
                    else
                    {
                        islandVertNumber++;
                    }
                }
                else if (lakeX > 0 && lakeZ == 0)
                {
                    if (lakeX * islandPieceXSize < v.x && v.x <= lakeX * islandPieceXSize + islandPieceXSize &&
                        lakeZ * islandPieceZSize <= v.z && v.z <= lakeZ * islandPieceZSize + islandPieceZSize)
                    {
                        _islandVertices.Remove(v);
                    }
                    else
                    {
                        islandVertNumber++;
                    }
                }
                else if (lakeX == 0 && lakeZ > 0)
                {
                    if (lakeX * islandPieceXSize <= v.x && v.x <= lakeX * islandPieceXSize + islandPieceXSize &&
                        lakeZ * islandPieceZSize < v.z && v.z <= lakeZ * islandPieceZSize + islandPieceZSize)
                    {
                        _islandVertices.Remove(v);
                    }
                    else
                    {
                        islandVertNumber++;
                    }
                }
                else if (lakeX == 0 && lakeZ == 0)
                {
                    if (lakeX * islandPieceXSize <= v.x && v.x <= lakeX * islandPieceXSize + islandPieceXSize &&
                        lakeZ * islandPieceZSize <= v.z && v.z <= lakeZ * islandPieceZSize + islandPieceZSize)
                    {
                        _islandVertices.Remove(v);
                    }
                    else
                    {
                        islandVertNumber++;
                    }
                }
            }
        }

        private void UpdateMeshes()
        {
            _islandMesh.Clear();
            _islandMesh.vertices = _islandVertices.ToArray();
            _islandMesh.triangles = _islandTriangles.ToArray();
            _islandMesh.RecalculateNormals();
            
            _waterMesh.Clear();
            _waterMesh.vertices = _waterVertices.ToArray();
            _waterMesh.triangles = _waterTriangles.ToArray();
            _waterMesh.RecalculateNormals();
        }

        private void OnDrawGizmos()
        {
            foreach (var v in _islandVertices)
            {
                Gizmos.DrawSphere(v, 0.1f);
            }
        }
    }
}

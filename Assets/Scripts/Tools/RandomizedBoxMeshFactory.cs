using System.Linq;
using UnityEngine;

namespace Tools
{
    public class RandomizedBoxMeshFactory
    {
        private float _randomOffset = .2f;
        
        private readonly Vector3[] _vertices = new Vector3[]
        {
            new Vector3(-1, -1, 1),
            new Vector3(1, -1, 1),
            new Vector3(1, 1, 1),
            new Vector3(-1, 1, 1),
            new Vector3(-1, -1, -1),
            new Vector3(1, -1, -1),
            new Vector3(1, 1, -1),
            new Vector3(-1, 1, -1),
        };
        private readonly int[] _triangles = new int[]
        {
            0, 2, 1,
            0, 3, 2,
            4, 5, 6,
            4, 6, 7,
            0, 7, 3,
            0, 4, 7,
            1, 2, 6,
            1, 6, 5,
            3, 7, 6,
            3, 6, 2,
            0, 1, 5,
            0, 5, 4
        };
        private readonly Vector2[] _uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),

            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };
        
        public Mesh CreateMesh(float size)
        {
            var vertices = _vertices.Select(v => v * size).ToArray();
            var randomOffset = _randomOffset * size;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += new Vector3(
                    Random.Range(-randomOffset, randomOffset),
                    Random.Range(-randomOffset, randomOffset),
                    Random.Range(-randomOffset, randomOffset)
                );
            }
            
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = _triangles;
            mesh.uv = _uv;
            //mesh.RecalculateNormals();

            return mesh;
        }
    }
}
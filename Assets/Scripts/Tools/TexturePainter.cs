using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Tools
{
    [RequireComponent(typeof(Renderer), typeof(Collider))]
    public class TexturePainter : MonoBehaviour
    {
        private RenderTexture _renderTexture;
        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _renderTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
            var material = new Material(GetComponent<Renderer>().material);
            material.SetTexture("_MainTex", _renderTexture);
            GetComponent<Renderer>().material = material;
            Clear();
        }

        private void Clear()
        {
            RenderTexture.active = _renderTexture;
            Texture2D tempTexture =
                new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
            for (int i = 0; i < _renderTexture.width; i++)
            {
                for (int j = 0; j < _renderTexture.height; j++)
                {
                    tempTexture.SetPixel(i, j, Color.white);
                }
            }
            tempTexture.Apply();
            Graphics.Blit(tempTexture, _renderTexture);
            RenderTexture.active = null;
        }

        public void Paint(Vector3 pos, Vector3 direction, Texture2D texture)
        {
            if(!_collider.Raycast(new Ray(pos + direction*-1f, direction), out var hit, 10f))
                return;

            var uv = hit.textureCoord;
            RenderTexture.active = _renderTexture;
            Texture2D tempTexture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
            tempTexture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            tempTexture.Apply();

            int x = (int)(uv.x * _renderTexture.width) - texture.width / 2;
            int y = (int)(uv.y * _renderTexture.height) - texture.height / 2;

            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    if (x + i >= 0 && x + i < _renderTexture.width && y + j >= 0 && y + j < _renderTexture.height)
                    {
                        Color brushColor = texture.GetPixel(i, j);
                        if (brushColor.a > 0)
                        {
                            tempTexture.SetPixel(x + i, y + j, brushColor);
                        }
                    }
                }
            }
            
            tempTexture.Apply();
            Graphics.Blit(tempTexture, _renderTexture);
            RenderTexture.active = null;
        }
    }
}
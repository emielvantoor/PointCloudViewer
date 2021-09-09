using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class PointCloudRenderer : MonoBehaviour
{
    private Texture2D _texColor;
    private Texture2D _texPosScale;
    private VisualEffect _vfx;

    private float _particleSize = 1f;

    private bool _toUpdate;
    private uint _particleCount = 0;

    private string _input;

    private readonly PointCloudReader _pointCloudReader = new PointCloudReader();

    public void Start()
    {
        _vfx = GetComponent<VisualEffect>();
    }

    public void OpenPcd()
    {
        if (string.IsNullOrWhiteSpace(_input) || !File.Exists(_input))
            return;

        var pointCloud = _pointCloudReader.Read(_input);

        var positions = pointCloud.Select(p => p.Location).ToArray();
        var colors = new Color[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            colors[i] = Color.red;
        }

        SetParticles(positions, colors);
    }

    public void OnInputTextChanged(string input)
    {
        _input = input;
    }

    private void SetParticles(Vector3[] positions, Color[] colors)
    {
        const int NORMAL_RESOLUTION = 2048;
        const int BIG_RESOLUTION = 4096;

        var resolution = positions.Length > NORMAL_RESOLUTION * NORMAL_RESOLUTION
            ? BIG_RESOLUTION
            : NORMAL_RESOLUTION;

        _texColor = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false);
        _texPosScale = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false);

        var texWidth = _texColor.width;
        var texHeight = _texColor.height;

        var particleSize = _particleSize;

        for (int y = 0; y < texHeight; y++)
        {
            for (int x = 0; x < texWidth; x++)
            {
                var index = x + y * texWidth;

                if (index < positions.Length)
                {
                    _texColor.SetPixel(x, y, colors[index]);


                    var color = new Color(positions[index].x, positions[index].y, positions[index].z, _particleSize);
                    _texPosScale.SetPixel(x, y, color);
                }
                else
                {
                    _texPosScale.SetPixel(x, y, new Color(0,0,0));
                }
            }
        }

        _texColor.Apply();
        _texPosScale.Apply();

        _particleCount = (uint)positions.Length;

        _vfx.Reinit();
        _vfx.SetUInt(Shader.PropertyToID("ParticleCount"), _particleCount);
        _vfx.SetTexture(Shader.PropertyToID("TexColor"), _texColor);
        _vfx.SetTexture(Shader.PropertyToID("TexPosScale"), _texPosScale);
        _vfx.SetUInt(Shader.PropertyToID("Resolution"), (uint)_texPosScale.width);
    }
}
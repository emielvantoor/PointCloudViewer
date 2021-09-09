using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PointCloudVisualizer : MonoBehaviour
{
    private readonly PointCloudReader _pointCloudReader = new PointCloudReader();
    private GameObject _parent;
    private PointCloud _pointCloud;
    private Coroutine _renderRoutine;

    private string _input;

    [SerializeField]
    private Transform _spawnPosition;

    [SerializeField]
    private float _pointSize = 5;

    [SerializeField]
    private float _scaleSize = 100;

    [SerializeField]
    private Material _material;

    // Start is called before the first frame update
    public void OpenPcd()
    {
        if (string.IsNullOrWhiteSpace(_input) || !File.Exists(_input))
            return;

        if (_parent != null)
        {
            Destroy(_parent);
        }

        var pointCloud = _pointCloudReader.Read(_input);
        // var parentTransform = new GameObject().GetComponent<Transform>();

        _pointCloud = pointCloud;
        // foreach (var point in pointCloud)
        // {
        //     var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //     cube.transform.parent = parentTransform;
        //     cube.transform.position = point.Location;
        //     cube.transform.localScale = Vector3.one * _pointSize;
        //     cube.GetComponent<Renderer>().sharedMaterial = _material;
        //     // cube.AddComponent<Rigidbody>();
        // }
        //
        //
        //
        // parentTransform.position = _spawnPosition.position + new Vector3(0,pointCloud.Height / 100, 0);
        // parentTransform.localScale = Vector3.one / 100;
        // parentTransform.localEulerAngles = new Vector3(83, 90, 90);
        // _parent = parentTransform.gameObject;

        _renderRoutine = StartCoroutine(DrawPoint());
    }

    private IEnumerator DrawPoint()
    {
        var parentTransform = new GameObject().GetComponent<Transform>();

        for (int i = 0; i < _pointCloud.Count; i++)
        {
            var point = _pointCloud[i];
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = parentTransform;
            cube.transform.position = point.Location / _scaleSize;
            cube.transform.localScale = (Vector3.one * _pointSize) / _scaleSize;
            cube.GetComponent<Renderer>().sharedMaterial = _material;

            if (i % 100 == 0)
                yield return new WaitForEndOfFrame();
        }

        parentTransform.position = _spawnPosition.position + new Vector3(0, _pointCloud.Height / _scaleSize, 0);
        // parentTransform.localScale = Vector3.one / _scaleSize;
        parentTransform.localEulerAngles = new Vector3(83, 90, 90);
        _parent = parentTransform.gameObject;

        yield return null;
    }

    public void OnInputTextChanged(string input)
    {
        _input = input;
    }
}
using System.Linq;
using UnityEngine;

public class Pen : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private int penSize;
    [SerializeField] private float tipHeight;
    [SerializeField] private LayerMask layerMask;

    private Renderer _renderer;
    private Color[] _colors;
    private RaycastHit _touch;
    private Paper _paper;
    private Vector2 _touchPos;
    private Vector2 _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;

    private void Start()
    {
        _renderer = tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
    }

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if(Physics.Raycast(tip.position, transform.up, out _touch, tipHeight, layerMask))
        {
            if (_touch.transform.CompareTag("Paper"))
            {
                if(_paper == null)
                {
                    _paper = _touch.transform.GetComponent<Paper>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _paper.textureSize.x - (penSize / 2));
                var y = (int)(_touchPos.y * _paper.textureSize.y - (penSize / 2));

                if (y < 0 || y > _paper.textureSize.y || x < 0 || x > _paper.textureSize.x) return;

                if (_touchedLastFrame)
                {
                    _paper.texture.SetPixels(x, y, penSize, penSize, _colors);

                    for (float f = 0.01f; f < 1.00f; f+= 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _paper.texture.SetPixels(lerpX, lerpY, penSize, penSize, _colors);

                    }

                    transform.rotation =  _lastTouchRot;

                    _paper.texture.Apply();
                    _paper.isDone = true;
                }

                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }

        _paper = null;
        _touchedLastFrame = false;
    }
}

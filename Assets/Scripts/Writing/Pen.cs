using System.Linq;
using UnityEngine;

public class Pen : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize;
    [SerializeField] private float _tipHeight;

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
        _renderer = _tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
    }

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if(Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Paper"))
            {
                if(_paper == null)
                {
                    _paper = _touch.transform.GetComponent<Paper>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _paper.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _paper.textureSize.y - (_penSize / 2));

                if (y < 0 || y > _paper.textureSize.y || x < 0 || x > _paper.textureSize.x) return;

                if (_touchedLastFrame)
                {
                    _paper.texture.SetPixels(x, y, _penSize, _penSize, _colors);

                    for (float f = 0.01f; f < 1.00f; f+= 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _paper.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);

                    }

                    transform.rotation = _lastTouchRot;

                    _paper.texture.Apply();
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

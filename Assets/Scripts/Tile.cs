using UnityEngine;
using UnityEngine.UI;

namespace PathFinding
{
    public class Tile
    {
        public TileGrid Grid { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }
        public int Weight { get; set; }
        public int Cost { get; set; }
        public Tile PrevTile { get; set; }

        private GameObject _gameObject;
        private SpriteRenderer _spriteRenderer;
        private Text _textComponent;

        public Tile(TileGrid grid, int row, int col, int weight)
        {
            Grid = grid;
            Row = row;
            Col = col;
            Weight = weight;
        }

        public void InitGameObject(Transform parent, GameObject prefab)
        {
            _gameObject = GameObject.Instantiate(prefab);
            _gameObject.name = $"Tile({Row}, {Col})";
            _gameObject.transform.parent = parent;
            _gameObject.transform.localPosition = new Vector3(Col, -Row, 0.0f);
            _gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            _spriteRenderer = _gameObject.GetComponent<SpriteRenderer>();
            _textComponent = _gameObject.GetComponentInChildren<Text>();
        }

        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        public void SetText(string text)
        {
            _textComponent.text = text;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(Col, Row);
        }
    }
}

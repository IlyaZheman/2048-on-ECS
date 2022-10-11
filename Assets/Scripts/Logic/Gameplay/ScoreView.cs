using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Gameplay
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI tmp;
        [SerializeField] private Color[] colors = new Color[11];

        public void SetScore(int value)
        {
            SetColor(value);
            SetText(value);
        }
        
        private void SetColor(int value)
        {
            image.DOColor(colors[value], 0.1f);
        }

        private void SetText(int value)
        {
            tmp.color = value <= 2 ? Color.black : Color.white;
            tmp.text = $"{Mathf.Pow(2, value)}";
        }
    }
}
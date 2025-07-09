using TMPro;
using UnityEngine;

namespace Code.Gameplay.UI
{
    public class BusinessScreenUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _balanceText;

        [field: SerializeField] public RectTransform BusinessElementContainer { get; private set; }

        public void SetBalanceText(string text)
            => _balanceText.text = text;
    }
}
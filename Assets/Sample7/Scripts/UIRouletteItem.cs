using TMPro;
using UnityEngine;

namespace Sample7.Scripts
{
 
    public class UIRouletteItem: MonoBehaviour
    {
        public RectTransform imgSelected_RectTransform;
        public TextMeshProUGUI textIndex_TextMeshProUGUI;
        
        public int _gridId;
        public int _index;

        public void SetSelectState(bool state)
        {
            imgSelected_RectTransform.gameObject.SetActive(state);
        }

        public void InitData(int i)
        {
            _gridId = i;
            _index = i;
            textIndex_TextMeshProUGUI.text = (i).ToString();
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic
{
    
    public class RoundTurntableItem : MonoBehaviour
    {
        public int _index;
        public    TextMeshProUGUI _text;
        public void InitData(int index)
        {
            _index = index;
            _text.text = index.ToString();
        }
    }
}
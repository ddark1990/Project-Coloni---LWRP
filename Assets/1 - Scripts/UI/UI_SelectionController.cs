using ProjectColoni;
using UnityEngine;

namespace ProjectColoni
{
    public class UI_SelectionController : MonoBehaviour
    {
        private SelectionManager _selectionManager;

        [SerializeField] private GameObject canvas;
    
        private void Start()
        {
            _selectionManager = SelectionManager.Instance;
        }

        private void Update()
        {
            canvas.SetActive(_selectionManager.currentlySelectedObject != null);
        }
    }
}

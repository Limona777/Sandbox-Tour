using UnityEngine;

public class SavePanelUI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        Destroy(gameObject);
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DamagePopUpGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static DamagePopUpGenerator current;
    public GameObject prefab;
    InputAction TestAction;


    private void Awake()
    {  
        current = this;
       TestAction = InputSystem.actions.FindAction("Test");
    }


    // Update is called once per frame
    private void Update()
    {
        if (TestAction.WasPressedThisFrame())
        {
            CreatePopUp(Vector3.one, Random.Range(0,1000).ToString(), Color.yellow);
        }
    }

    public void CreatePopUp(Vector3 position, string text, Color color)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;

        //Destroy timer
        Destroy(popup, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestTap : MonoBehaviour, ITappable
{
    //[SerializeField]
    //private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTap(GameObject obj)
    {
        Debug.Log("ZA WARUDO " + obj.name);
        obj.transform.rotation = Quaternion.Euler(-20, -30, 154);
       // text.text = obj.name;
    }

    void OnTap(TapEventArgs args)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

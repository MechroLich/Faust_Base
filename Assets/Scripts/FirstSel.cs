using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject FTPno = GameObject.Find("Button name (Skills)");
        if (FTPno)
        {
            EventSystem.current.SetSelectedGameObject(FTPno);
        }
    }
}

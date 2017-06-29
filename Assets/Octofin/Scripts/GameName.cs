using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Octofin.Core.Utility;

public class GameName : MonoBehaviour {

    public TMP_Text gameNameText;
	
	void Start () {
        gameNameText.text = "Octofin - Version " + Game.Version + " " + Game.Build;
	}

}

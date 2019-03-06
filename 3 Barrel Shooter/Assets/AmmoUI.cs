using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
	public Image image;

	public void SetFill(float fill){
		image.fillAmount = fill;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RadialProgressBar : MonoBehaviour
{
    public Transform LoadingBar;
    public Text TextIndicator;
    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;
            TextIndicator.text = ((int)currentAmount).ToString();
        }
        else
        {
            TextIndicator.text = "Finished";
        }
        LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

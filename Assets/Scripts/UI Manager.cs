using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    void Awake()
    {
        instance = this;
    }
    public TMP_Text overheatedtext;
    public Slider weaponTempSlider;
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}

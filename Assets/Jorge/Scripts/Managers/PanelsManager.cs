using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PanelsManager : MonoBehaviour
{
   public static PanelsManager THIS;

   public Image shieldRefill;
   public Text carriedItemsText;

   void Awake(){
    THIS = this;
   }
    void Start()
    {
        Update_CarriedItems(0);
    }

   
    void Update()
    {
        
    }

    public void Actions_Start(){
        SceneManager.LoadScene(1);
    }

    public void Actions_Exit(){
        Application.Quit();
    }

    public void Update_ShieldSlider(float newValue, float maxValue){

        float currentValue = Mathf.Clamp(newValue, 0, maxValue);
        shieldRefill.fillAmount = currentValue / maxValue; 
    }

    public void Update_CarriedItems(int itemsCount){
        carriedItemsText.text = itemsCount.ToString();
    }
}

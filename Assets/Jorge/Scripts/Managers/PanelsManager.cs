using UnityEngine;
using UnityEngine.SceneManagement;


public class PanelsManager : MonoBehaviour
{
   public static PanelsManager THIS;

   void Awake(){
    THIS = this;
   }
    void Start()
    {
        
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
}

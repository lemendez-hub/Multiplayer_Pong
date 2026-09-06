using UnityEngine;

public class ResManager : MonoBehaviour
{
    // Variables
    public int width;
    public int height;
    
    public void SetWidth(int newWidth){
        width = newWidth;
    }
    
    public void SetHeight(int newHeight){
        height = newHeight;
    }
    public void setResolution(){
        Screen.SetResolution(width, height, false); // Sets the screen resolution to the specified width and height, with Fullscreen mode set to false
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorRed : MonoBehaviour
{
    private Texture2D myPicture;

    void Awake()
    {
        myPicture = new Texture2D((int)(Screen.width), (int)(Screen.height));
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myPicture);
        MakeItRed();
    }

    void MakeItRed()
    {
        for (int x = 0; x < myPicture.width; x++)
        {
           for (int y = 0; y < myPicture.height; y++)
           {
               Color color = Color.red;
               myPicture.SetPixel(x, y, color);
           }
       }
       myPicture.Apply();
    }
}

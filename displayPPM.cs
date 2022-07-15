using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayPPM : MonoBehavior
{
    private Texture2D myPicture;

    void Awake()
    {
        myPicture = new Texture2D((int)(Screen.width), (int)(Screen.height));
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myPicture);
        ReadInFile();
        displayImage();
    }

    void ReadInFile()
    {
        //with this method you read the file all at once, if the file is huge, this would not be ideal
        string allText = System.IO.File.ReadAllText(path);
        string replaceText = Regex.Replace(allText, "[^a-zA-Z0-9% ._]", " "); //replace all non-word characters with spaces 
        string[] entries = replaceText.Split (new string[] {" "},System.StringSplitOptions.RemoveEmptyEntries);
        //split by spaces, not including empty strings

        int width = int.Parse(entries [1]);// these should be global variables…
        int height = int.Parse(entries[2]);

        myPicture = new Texture2D((int)(width), (int)(height)); //global
        int normalize = int.Parse(text3[3]);
    }

    void displayImage()
    {
        for (int x = 0; x < myPicture.width; x++)
        {
            for (int y = 0; y < myPicture.height; y++)
            {
                Color color = myPicture.GetPixel(width, height);
                myPicture.SetPixel(x, y, color);
            }
        }
        myPicture.Apply();
    }
}
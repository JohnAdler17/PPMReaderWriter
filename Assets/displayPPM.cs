using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class displayPPM : MonoBehaviour
{
    private Texture2D myPicture;

    public InputField Field; // variable that keeps track of what is typed into the "open file" text box

    public InputField Save; // variable that keeps track of what is typed into the "save file" text box

    string[] entries;
    int width;
    int height;
    float normalize;
    int myX;
    int myY;
    int curr;

    void Awake()
    {
        myPicture = new Texture2D((int)(Screen.width), (int)(Screen.height));
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, myPicture.width, myPicture.height), myPicture);
    }

    public void saveInput()
    {
        string path;
        path = Save.text;
        string fullPath = @"C: \Users\nmnm2\Downloads\sampleinputs\" + path;
        using (StreamWriter writetext = new StreamWriter(fullPath))
        {
            for (int x = 0; x < entries.Length; x++)
            {
                writetext.WriteLine(entries[x]);
            }
        }

    }

    public void readInput()
    {
        string filePath;
        filePath = Field.text;
        display(filePath);
    }

    public void display(string filePath)
    {
        //with this method you read the file all at once, if the file is huge, this would not be ideal
        string allText = System.IO.File.ReadAllText(@"C: \Users\nmnm2\Downloads\sampleinputs\" + filePath);
        string replaceText = Regex.Replace(allText, "[^a-zA-Z0-9% ._]", " "); //replace all non-word characters with spaces 
        entries = replaceText.Split (new string[] {" "},System.StringSplitOptions.RemoveEmptyEntries);
        //split by spaces, not including empty strings

        width = int.Parse(entries[1]); // these should be global variables…
        height = int.Parse(entries[2]);

        myPicture = new Texture2D((int)(width), (int)(height)); //global
        normalize = int.Parse(entries[3]);

        myX = 0;
        myY = height;

        for (int x = 4; x < entries.Length; x += 3)
        {
            Color color = new Color(float.Parse(entries[x]) / normalize, float.Parse(entries[x + 1]) / normalize, float.Parse(entries[x + 2]) / normalize);
            myPicture.SetPixel(myX, myY, color);
            
            if (myX == width)
            {
                myX = 0;
                myY--;
            }
            myX++;
        }
        myPicture.Apply();
    }

    public void negate_red()
    {
        // changes just the red colors to their "negative": Ex. 0 -> 255, 100 -> 155, etc.
        myX = 0;
        myY = height;
        
        myPicture = new Texture2D((int)(width), (int)(height));
        
        float temp;

        for (int x = 4; x < entries.Length; x += 3)
        {
            temp = float.Parse(entries[x]);
            temp = 255 - temp;
            float R = temp / normalize;
            curr = (int)Mathf.Round(R * normalize);
            entries[x] = curr.ToString();

            temp = float.Parse(entries[x + 1]);
            float G = temp / normalize;
            curr = (int)Mathf.Round(G * normalize);
            entries[x + 1] = curr.ToString();

            temp = float.Parse(entries[x + 2]);
            float B = temp / normalize;
            curr = (int)Mathf.Round(B * normalize);
            entries[x + 2] = curr.ToString();

            Color newColor = getColor(R, G, B);
            if (myX == width)
            {
                myX = 0;
                myY--;
            }
            myX++;
            makeImage(newColor, myPicture, myX, myY);
        }
    }

    public void flip_horizontal()
    {
        // flips the picture horizontally -> preserve RGB order
        myX = width;
        myY = height;
        
        myPicture = new Texture2D((int)(width), (int)(height));
        
        float temp;

        for (int x = 4; x < entries.Length; x += 3)
        {
            temp = float.Parse(entries[x]);
            float R = temp / normalize;
            curr = (int)Mathf.Round(R * normalize);
            entries[x] = curr.ToString();

            temp = float.Parse(entries[x + 1]);
            float G = temp / normalize;
            curr = (int)Mathf.Round(G * normalize);
            entries[x + 1] = curr.ToString();

            temp = float.Parse(entries[x + 2]);
            float B = temp / normalize;
            curr = (int)Mathf.Round(B * normalize);
            entries[x + 2] = curr.ToString();

            Color newColor = getColor(R, G, B);
            if (myX == 0)
            {
                myX = width;
                myY--;
            }
            myX--;
            makeImage(newColor, myPicture, myX, myY);
        }

    }

    public void grey_scale()
    {
        // changes the picture into a grey scale image -> done by averaging the values of all three color numbers for a pixel, the red, green, and blue, and replacing them all by that average.
        myX = 0;
        myY = height;

        for (int x = 4; x < entries.Length; x += 3)
        {
            float initialR = float.Parse(entries[x]);
            float initialG = float.Parse(entries[x + 1]);
            float initialB = float.Parse(entries[x + 2]);
            float average = (initialR + initialG + initialB) / 3;
            float R = average / normalize;
            float G = average / normalize;
            float B = average / normalize;
            Color newColor = getColor(R, G, B);
            if (myX == width)
            {
                myX = 0;
                myY--;
            }
            myX++;
            makeImage(newColor, myPicture, myX, myY);
        }
    }

    public void flatten_red()
    {
        // makes the red value 0
        myX = 0;
        myY = height;
        
        myPicture = new Texture2D((int)(width), (int)(height));
        
        float temp;

        for (int x = 4; x < entries.Length; x += 3)
        {
            
            temp = float.Parse(entries[x]);
            float R = 0;
            curr = (int)Mathf.Round(R * normalize);
            entries[x] = curr.ToString();

            temp = float.Parse(entries[x + 1]);
            float G = temp / normalize;
            curr = (int)Mathf.Round(G * normalize);
            entries[x + 1] = curr.ToString();

            temp = float.Parse(entries[x + 2]);
            float B = temp / normalize;
            curr = (int)Mathf.Round(B * normalize);
            entries[x + 2] = curr.ToString();

            Color newColor = getColor(R, G, B);
            if (myX == width)
            {
                myY--;
                myX = 0;
            }
            myX++;
            makeImage(newColor, myPicture, myX, myY);
        }
    }

    public void negate_green()
    {
        // makes green negative
        myX = 0;
        myY = height;
        
        myPicture = new Texture2D((int)(width), (int)(height));
        
        float temp;

        for (int x = 4; x < entries.Length; x += 3)
        {
            temp = float.Parse(entries[x]);
            float R = temp / normalize;
            curr = (int)Mathf.Round(R * normalize);
            entries[x] = curr.ToString();

            temp = float.Parse(entries[x + 1]);
            temp = 255 - temp;
            float G = temp / normalize;
            curr = (int)Mathf.Round(G * normalize);
            entries[x + 1] = curr.ToString();

            temp = float.Parse(entries[x + 2]);
            float B = temp / normalize;
            curr = (int)Mathf.Round(B * normalize);
            entries[x + 2] = curr.ToString();

            Color newColor = getColor(R, G, B);
            if (myX == width)
            {
                myX = 0;
                myY--;
            }
            myX++;
            makeImage(newColor, myPicture, myX, myY);
        }
    }

    public void negate_blue()
    {
        // makes blue negative
        myX = 0;
        myY = height;
        
        myPicture = new Texture2D((int)(width), (int)(height));
        
        float temp;

        for (int x = 4; x < entries.Length; x += 3)
        {
            
            temp = float.Parse(entries[x]);
            float R = temp / normalize;
            curr = (int)Mathf.Round(R * normalize);
            entries[x] = curr.ToString();

            temp = float.Parse(entries[x + 1]);
            float G = temp / normalize;
            curr = (int)Mathf.Round(G * normalize);
            entries[x + 1] = curr.ToString();

            temp = float.Parse(entries[x + 2]);
            temp = 255 - temp;
            float B = temp / normalize;
            curr = (int)Mathf.Round(B * normalize);
            entries[x + 2] = curr.ToString();

            Color newColor = getColor(R, G, B);
            if (myX == width)
            {
                myX = 0;
                myY--;
            }
            myX++;
            makeImage(newColor, myPicture, myX, myY);
        }
    }

    public void flatten_green()
    {
        // makes the green value 0
        myX = 0;
        myY = height;
        
        myPicture = new Texture2D((int)(width), (int)(height));
        
        float temp;

        for (int x = 4; x < entries.Length; x += 3)
        {

            temp = float.Parse(entries[x]);
            float R = temp / normalize;
            curr = (int)Mathf.Round(R * normalize);
            entries[x] = curr.ToString();

            temp = float.Parse(entries[x + 1]);
            float G = 0;
            curr = (int)Mathf.Round(G * normalize);
            entries[x + 1] = curr.ToString();

            temp = float.Parse(entries[x + 2]);
            float B = temp / normalize;
            curr = (int)Mathf.Round(B * normalize);
            entries[x + 2] = curr.ToString();

            Color newColor = getColor(R, G, B);
            if (myX == width)
            {
                myY--;
                myX = 0;
            }
            myX++;
            makeImage(newColor, myPicture, myX, myY);
        }
    }

    public void flatten_blue()
    {
        // makes the blue value 0
        myX = 0;
        myY = height;
        
        myPicture = new Texture2D((int)(width), (int)(height));
        
        float temp;

        for (int x = 4; x < entries.Length; x += 3)
            {

            temp = float.Parse(entries[x]);
            float R = temp / normalize;
            curr = (int)Mathf.Round(R * normalize);
            entries[x] = curr.ToString();

            temp = float.Parse(entries[x + 1]);
            float G = temp / normalize;
            curr = (int)Mathf.Round(G * normalize);
            entries[x + 1] = curr.ToString();

            temp = float.Parse(entries[x + 2]);
            float B = 0;
            curr = (int)Mathf.Round(B * normalize);
            entries[x + 2] = curr.ToString();

            Color newColor = getColor(R, G, B);
                if (myX == width)
                {
                    myY--;
                    myX = 0;
                }
                myX++;
                makeImage(newColor, myPicture, myX, myY);
            }
    }

    void makeImage(Color pixelColor, Texture2D myPicture, int rows, int cols)
    {
        myPicture.SetPixel(rows, cols, pixelColor);
        myPicture.Apply();
    }

    Color getColor (float a, float b, float c)
    {
        Color pixelColor = new Color(a, b, c);
        return pixelColor;
    }
}
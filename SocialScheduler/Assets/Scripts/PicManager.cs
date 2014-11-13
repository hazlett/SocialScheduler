using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
public class PicManager : MonoBehaviour {

    private string directory;
    private List<Texture2D> pictures;
    private Vector2 scroll;
    private float totalHeight;
    private float maxWidth;
    private int scale = 150;
	void Awake () {
        directory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"/SocialSchedulerPics";
        RefreshPicList();
        scroll = new Vector2();
	}

    private void RefreshPicList()
    {
        pictures = new List<Texture2D>();
        string[] files = Directory.GetFiles(directory);
        foreach (string file in files)
        {
            if (file.EndsWith(".bmp", true, System.Globalization.CultureInfo.CurrentCulture) ||
            (file.EndsWith(".jpg", true, System.Globalization.CultureInfo.CurrentCulture)) ||
            (file.EndsWith(".png", true, System.Globalization.CultureInfo.CurrentCulture)))
            {
                LoadPicture(file);
            }
            else
            {
                Debug.Log("The file " + file + " is the wrong format");
            }
        }
        totalHeight = Mathf.Ceil(pictures.Count / 4) * 150.0f;
        maxWidth = 150.0f * 4;
    }
    private void LoadPicture(string file)
    {
        byte[] bytes = File.ReadAllBytes(file);
        Texture2D texture = new Texture2D(32, 32);
        if (texture.LoadImage(bytes))
        {
            totalHeight += texture.height;
            if (maxWidth < texture.width)
            {
                maxWidth = texture.width;
            }
            pictures.Add(texture);
        }
        else
        {
            Debug.Log("Loading file " + file + " failed");
        }
    }
    void OnGUI()
    {
        scroll = GUI.BeginScrollView(new Rect(Screen.width * 0.5f, 0.0f, Screen.width * 0.5f, Screen.height), scroll, new Rect(Screen.width * 0.5f, Screen.height, maxWidth, totalHeight));
        float y = 0.0f;
        int count = 0;
        foreach(Texture2D texture in pictures)
        {
            Texture2D thumbnail = ScaleTexture(texture, 150, 150);
            if (GUI.Button(new Rect(Screen.width * 0.5f + ((count % 4) * 150.0f), Screen.height + y, thumbnail.width, thumbnail.height), thumbnail))
            {
                Debug.Log(texture.width);
            }
            count++;
            if ((count % 4) == 0)
            {
                y += scale;
            }
        }
        GUI.EndScrollView();
    }
    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
}

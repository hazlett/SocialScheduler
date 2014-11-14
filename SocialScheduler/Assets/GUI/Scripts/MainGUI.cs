using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainGUI : MonoBehaviour {

    public PicManager picManager;
    private Vector2 scroll;
    private float totalHeight, maxWidth, addButtonWidth = 250.0f, addButtonHeight = 50.0f, space = 25.0f;
    internal static string[] networks = { "FB", "t", "i" };
    private List<Scheduler> schedules;
    void Start()
    {
        scroll = new Vector2();
        schedules = new List<Scheduler>();
        totalHeight = 0;
        maxWidth = 0;
        totalHeight += addButtonHeight;
        maxWidth += addButtonWidth;
    }

    void OnGUI()
    {
        scroll = GUI.BeginScrollView(new Rect(0.0f, 0.0f, Screen.width * 0.5f, Screen.height), scroll, new Rect(0.0f, 0.0f, maxWidth, totalHeight));
        ScrollView();
        GUI.EndScrollView();
    }

    private void ScrollView()
    {
        if (GUI.Button(new Rect(0, 0, addButtonWidth, addButtonHeight), "ADD SCHEDULER"))
        {
            AddNewScheduler();
        }
        foreach(Scheduler scheduler in schedules)
        {
            scheduler.Draw();
        }
    }

    private void AddNewScheduler()
    {
        totalHeight += space;
        Scheduler schedule = new Scheduler(totalHeight, picManager);
        Vector2 size = schedule.GetSize();
        if (size.x > maxWidth)
        {
            maxWidth = size.x;
        }
        totalHeight += size.y;
        schedules.Add(schedule);
    }

    private class Scheduler
    {
        private float left, top, width, height;
        private float textBoxHeight = 150.0f, textBoxWidth = 350.0f, networkHeight = 50.0f, networkWidth = 350.0f, scheduleButtonWidth = 150.0f, scheduleButtonHeight = 50.0f, thumbnailSize = 150.0f;
        private Rect rect;
        private Rect textField, thumbnail, button;
        private string text;
        private Texture2D picture;
        private bool[] networks = { false, false, false };
        private Rect[] networkRects;
        PicManager picManager;
        public Scheduler(float currentHeight, PicManager picManager)
        {
            this.picManager = picManager;
            top = currentHeight;
            left = 25.0f;
            height = textBoxHeight + 5.0f + networkHeight;
            width = textBoxWidth + scheduleButtonWidth;
            rect = new Rect(left, top, width, height);
            textField = new Rect(left, top, textBoxWidth, textBoxHeight);
            thumbnail = new Rect(left + textBoxWidth, top, thumbnailSize, thumbnailSize);
            text = "";
            picture = null;
            networkRects = new Rect[networks.Length];
            for (int i = 0; i < networks.Length; i++)
            {
                networkRects[i] = new Rect(left + (i * 50), top + 5.0f + textBoxHeight, 50.0f, 50.0f);
            }
            button = new Rect(left + networkWidth, top + textBoxHeight + 5.0f, scheduleButtonWidth, scheduleButtonHeight);
        }
        public Vector2 GetSize()
        {
            return new Vector2(width + left, height);
        }
        public void Draw()
        {
            text = GUI.TextArea(textField, text);
            if (GUI.Button(thumbnail, picture))
            {
                picture = picManager.GetSelectedPicture();
            }
            for (int i = 0; i < networks.Length; i++)
            {
                networks[i] = GUI.Toggle(networkRects[i], networks[i], MainGUI.networks[i]);
            }
            if (GUI.Button(button, "POST"))
            {
                Post();
            }
        }
        private void Post()
        {
            Debug.Log("POSTING");
        }
    }
}

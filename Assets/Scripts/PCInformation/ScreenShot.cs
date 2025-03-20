using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public Camera screenshotCamera;
    public int width = 1920;
    public int height = 1080;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CaptureScreenshot();
        }
    }
    public void CaptureScreenshot()
    {
        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        screenshotCamera.targetTexture = rt;
        screenshotCamera.Render();

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.ARGB32, false);
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Screenshot.png", bytes);
        Debug.Log("Screenshot saved!");

        Destroy(screenshot);
    }
}

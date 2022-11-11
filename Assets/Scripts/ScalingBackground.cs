using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingBackground : MonoBehaviour
{
    public Camera mainCam;
    public GameObject backgroundImage;
    // Start is called before the first frame update
    void Start()
    {
        Scale_BG_To_Screen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Scale_BG_To_Screen() {
        // Get the device screen aspect ratio
        print("Devices Screen Size (W/H): " + Screen.width.ToString() + "/" + Screen.height.ToString());

        float scrHeight = Screen.height;
        float scrWidth = Screen.width;
        float DEVICE_SCREEN_RATIO = scrWidth / scrHeight;

        print("DEVICE_SCREEN_RATIO: " + DEVICE_SCREEN_RATIO.ToString());

        // Set main camera's ratio to be equal to the aspect ratio
        mainCam.aspect = DEVICE_SCREEN_RATIO;

        // Scale background image to fit camera size
        float camHeight = 100.0f * mainCam.orthographicSize * 2.0f;
        float camWidth = camHeight * DEVICE_SCREEN_RATIO;
        print("camHeight " + camHeight.ToString());
        print("camWidth " + camWidth.ToString());

        // Get background image size
        SpriteRenderer backgroundImg = backgroundImage.GetComponent<SpriteRenderer>();
        float bgImgH = backgroundImg.sprite.rect.height;
        float bgImgW = backgroundImg.sprite.rect.width;

        print("bgImgH " + bgImgH.ToString());
        print("bgImgW " + bgImgW.ToString());

        // Calculate ratio for scaling background
        float bgImg_scale_ratio_Height = camHeight / bgImgH;
        float bgImg_scale_ratio_Width = camWidth / bgImgW;

        backgroundImage.transform.localScale = new Vector3(bgImg_scale_ratio_Width, bgImg_scale_ratio_Height, 1);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenTextureApplier : MonoBehaviour
{
    public int wid = 16;
    public int hei = 16;
    public int scale = 16;
    public float highGrainScale = 1;
    public float lowGrainScale = 1;
    public float _2dNoiseGrain = 1 / 5f;


    public bool vertical = false;

    public Color c1 = new Color(139 / 255f, 69 / 255f, 19 / 255f);
    public Color c2 = new Color(200 / 255f, 199 / 255f, 137 / 255f);


    // Start is called before the first frame update
    public void apply()
    {
        var texture = WoodenTexture.getLines(vertical, wid, hei, scale,
            c1, c2,
            highGrainScale, lowGrainScale, _2dNoiseGrain);

        var rend = GetComponent<Renderer>();
        var tempMaterial = new Material(rend.sharedMaterial);
        tempMaterial.mainTexture = texture;
        rend.sharedMaterial = tempMaterial;
    }
}

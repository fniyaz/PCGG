using UnityEngine;

public class Skybox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox.mainTexture
            = CloudsTexture.run(
                (10, 10),
                (10, 10),
                (1, 1),
                Color.black,
                Color.white,
                0
            )[0];
    }

    // Update is called once per frame
    void Update()
    {
    }
}
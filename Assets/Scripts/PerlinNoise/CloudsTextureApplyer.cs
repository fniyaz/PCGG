using UnityEngine;
using UnityEngine.SceneManagement;


public class CloudsTextureApplyer : MonoBehaviour
{
    private Renderer renderer;
    private Texture2D[] texture;

    public float threshhold = 0.5f;
    public Color c1;
    public Color c2;
    
    // Start is called before the first frame update
    void Start()
    {
        texture = CloudsTexture.run((10, 10), (10, 10), (9, 30), c1, c2, threshhold);

        renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = texture[0];
        renderer.material.EnableKeyword("_EMISSION");
        renderer.material.SetTexture(Emission, texture[0]);
    }

    private int frame = 0;
    private int tex = 0;
    private static readonly int Emission = Shader.PropertyToID("_EMISSION");

    void FixedUpdate()
    {
        frame++;
        if (frame > 3)
        {
            frame = 0;
            tex++;
            if (tex >= texture.GetLength(0))
                tex = 0;

            renderer.material.mainTexture = texture[tex];
            renderer.material.SetTexture(Emission, texture[tex]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FlashLight2D : MonoBehaviour {

    [HideInInspector]
    public float light_intensity = 200f;     
    [HideInInspector]
    public float light_range = 2f;          
    [HideInInspector]
    public Gradient ray_gradient = new Gradient();         
    [HideInInspector]
    public float ray_width = 0.05f;           
    [HideInInspector]
    public float ray_step = 0.05f;            
    [HideInInspector]
    public int raysNumber = 20;            
    [HideInInspector]
    public float distance = 10f;           
    [HideInInspector, SerializeField]
    public Material material;
    [HideInInspector]
    public bool debugLight = false;        
    [HideInInspector]
    public bool drawRays = true;           
    [HideInInspector]
    public bool enableLight = true;        

    [SerializeField]
    private List<GameObject> _laserScaner = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _lights = new List<GameObject>();
    [SerializeField]
    public Shader shader;

    private void OnEnable()
    {
        CreateObject();
    }

    public void CreateObject()
    {      
        CreateRays();
        CreateLights();

    }

    void Update()
    {
        CastRays();
    }

    private void CreateRays()
    {
        DeleteRays();

        for (ushort i = 0; i < raysNumber; i++)
        {
            GameObject laser = new GameObject("Flashlight ray "+ i.ToString());
            laser.transform.parent = this.transform;
            laser.AddComponent<LineRenderer>();
            laser.hideFlags = HideFlags.DontSave;
            _laserScaner.Add(laser);
        }
    }

    private void CreateLights()
    {
        DeleteLights();

        GameObject light = new GameObject("Flashlight light ");
        light.transform.parent = this.transform;
        light.AddComponent<Light>();
        light.hideFlags = HideFlags.DontSave;
        _lights.Add(light);
    }

    private void DeleteRays()
    {
        foreach (GameObject laser in _laserScaner)
        {
            if (!Application.isPlaying)
            {
                DestroyImmediate(laser);
            }
            else
                Destroy(laser);
        }
        _laserScaner.Clear();
    }

    private void DeleteLights()
    {
        foreach (GameObject light in _lights)
        {
            if (!Application.isPlaying)
            {
                DestroyImmediate(light);
            }
            else
                Destroy(light);
        }
        _lights.Clear();
    }


    public void CastRays()
    {
        Vector2 position = transform.position;

        uint i = 0;

        foreach(GameObject obj in _laserScaner)
        {
            if (obj)
            {
                float y = i <= raysNumber / 2f ? i * ray_step : (i - raysNumber / 2) * -ray_step;
                float calcDistance = Mathf.Sqrt(Mathf.Pow(distance, 2f) + Mathf.Pow(y, 2f));
                float deg = Mathf.Atan2(y, distance) * Mathf.Rad2Deg;
                Vector2 direction = RotateVector(transform.TransformDirection(Vector2.right), deg);

                LineRenderer line;

                line = obj.GetComponent<LineRenderer>();

                if(line)
                    DrawRay(ref line, position, direction, calcDistance, i == 0 ? true : false);
            }
            i++;
        }

    }



    private void DrawRay(ref LineRenderer line, Vector2 position, Vector2 direction, float distance, bool lightPos)
    {
        RaycastHit2D hitRayCast = new RaycastHit2D();
        hitRayCast = Physics2D.Raycast(position, direction, distance);

        if (debugLight)
            Debug.DrawRay(position, direction * distance, Color.blue);

        line.enabled = false;

        Light light = _lights[0].GetComponent<Light>();
        if (lightPos)
            light.enabled = false;

        line.positionCount = 2;
        Vector3[] positions = new Vector3[2];
        positions[0] = position;
        positions[0].z = -.01f;
        positions[1] = position + direction.normalized * distance;
        positions[1].z = -.01f;

        if (hitRayCast.collider)
        {
            if (debugLight)
                Debug.DrawRay(position, direction * hitRayCast.distance, Color.red);

            positions[1] = hitRayCast.point;

            if (enableLight)
            {
                if (lightPos)
                {
                    light.enabled = true;
                    light.range = light_range;
                    light.transform.position = new Vector3(hitRayCast.point.x, hitRayCast.point.y, -1f);
                    light.type = LightType.Point;
                    light.intensity = light_intensity;
                    if(ray_gradient.colorKeys.Length > 0)
                        light.color = ray_gradient.colorKeys[1].color;
                    light.renderMode = LightRenderMode.ForceVertex;
                }
            }

        }

        if (drawRays)
        {
            line.enabled = true;
            line.SetPositions(positions);
            DrawRayLine(ref line);
        }
    }



    public void DrawRayLine(ref LineRenderer line)
    {
        line.widthMultiplier = ray_width;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.material = material;
        line.colorGradient = ray_gradient;
    }

    public Vector2 RotateVector(Vector2 v, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * v;
    }

  
    public void DestroyObject()
    {
        DeleteRays();
        DeleteLights();

    }

    private void OnDisable()
    {
        DestroyObject();
    }

}





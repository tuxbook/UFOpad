using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;

public class UfoPad : MonoBehaviour 
{
    List<GameObject> parts;

    public Material baseMaterial;

    public float radius = 3;
    public float segments = 15;

    public Color paintColor = new Color(0, 1, 0);
    public List<Color> paintColors = new List<Color>();
    int currentPaintColor = 0;

    public string ufoAddress = "172.20.10.2";

    bool firstTime = true;

	// Use this for initialization
	void Start () 
    {
        if (baseMaterial == null)
            throw new MissingReferenceException("No baseMaterial attached to UfoPad");
        if (paintColors.Count < 1)
            throw new MissingReferenceException("Set some paint colors first");

        paintColor = paintColors[0];
        
        parts = new List<GameObject>();

        var angle = 2f * Mathf.PI / (float)segments;

        for (var i = 0; i < segments; i++)
        {
            var part = new GameObject("part" + i);
            var ufoPart = part.AddComponent<UfoPart>();
            part.transform.Rotate(new Vector3(0, 1, 0), Mathf.Rad2Deg * i * angle);
            parts.Add(part);
            ufoPart.material = new Material(baseMaterial);
            ufoPart.angle = angle;
            ufoPart.radius = radius;
        }

        //GuessUfoAddress();

        Debug.Log("UFO Address: " + ufoAddress);
	}
	
	// Update is called once per frame
	void Update() 
    {
        if (Input.GetMouseButton(0)) 
        {
           RaycastHit hit;
           if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) 
            {
                var part = hit.collider.gameObject.GetComponent<UfoPart>();
                if (part != null)
                {
                    if (!part.GetComponent<MeshRenderer>().material.color.Equals(paintColor))
                    {
                        part.GetComponent<MeshRenderer>().material.color = paintColor;
                        StartCoroutine(CallUfo());
                    }
                }
           }
        }
        if (Input.GetMouseButtonUp(0))
        {
            // Choose random color for next time
            ChooseNextPaintColor();
        }

        if (firstTime)
        {
            StartCoroutine(CallUfo());
            firstTime = false;
        }
	}

    void ChooseNextPaintColor()
    {
        //var choice = (int)(Random.value * (float)paintColors.Count);
        //paintColor = paintColors[choice];

        currentPaintColor++;
        if (currentPaintColor >= paintColors.Count)
            currentPaintColor = 0;
        paintColor = paintColors[currentPaintColor];
    }

    IEnumerator CallUfo()
    {
        var address = "http://" + ufoAddress + "/api?top_init=1&bottom_init=1";

        var colors1 = new List<Color>(); // 4 colors
        var colors2 = new List<Color>(); // 3 colors
        var colors3 = new List<Color>(); // 4 colors
        var colors4 = new List<Color>(); // 4 colors

        for (var i = 0; i < parts.Count; i++)
        {
            var partColor = parts[i].GetComponent<MeshRenderer>().material.color;

            var c = ColorUtility.ToHtmlStringRGB(partColor).ToLower();

            address += string.Format("&top={0}|{1}|{2}", i, 1, c);
            address += string.Format("&bottom={0}|{1}|{2}", i, 1, c);

            if (i >= 0 && i < 4)
                colors1.Add(partColor);
            else if (i >= 4 && i < 7)
                colors2.Add(partColor);
            else if (i >= 7 && i < 11)
                colors3.Add(partColor);
            else
                colors4.Add(partColor);
        }

        var c1 = AverageColors(colors1);
        var c2 = AverageColors(colors2);
        var c3 = AverageColors(colors3);
        var c4 = AverageColors(colors4);

        address += string.Format("&logo={0}|{1}|{2}|{3}", c2, c3, c4, c1);

        Debug.Log(address);

        UnityWebRequest www = UnityWebRequest.Get(address);
        yield return www.Send();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(address);
        }
    }

    string AverageColors(List<Color> colors)
    {
        if (colors.Count < 1)
            return "000000";
        if (colors.Count < 2)
            return ColorUtility.ToHtmlStringRGB(colors[0]).ToLower();

        var lastColor = colors[0];

        for (int i = 1; i < colors.Count; i++)
        {
            var thisColor = colors[i];
            lastColor = new Color(Mathf.Sqrt(lastColor.r * thisColor.r),
                                  Mathf.Sqrt(lastColor.g * thisColor.g),
                                  Mathf.Sqrt(lastColor.b * thisColor.b));
        }

        return ColorUtility.ToHtmlStringRGB(lastColor).ToLower();
    }

    void GuessUfoAddress()
    {
        var myIP = GetIP();
        var components = myIP.Split('.');
        if (components.Length != 4)
            throw new UnityException("Invalid IP address: " + myIP);
        var lastComponent = components[components.Length - 1];
        int last = int.Parse(lastComponent);
        last++;
        var address = string.Format("{0}.{1}.{2}.{3}", components[0], components[1], components[2], last);
        ufoAddress = address;
    }

    string GetIP()
    {
        var hostName = System.Net.Dns.GetHostName();
        var ipEntry = System.Net.Dns.GetHostEntry(hostName);
        var address = ipEntry.AddressList;

        for (var i = 0; 0 < address.Length; i++)
        {
            Debug.Log("address: " + address[i]);
        }

        if (address.Length < 1)
            throw new UnityException("Didn't get any IP addresses in GetIP()");
        return address[0].ToString();
    }
}

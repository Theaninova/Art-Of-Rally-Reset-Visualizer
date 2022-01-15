# Art-Of-Rally-Reset-Visualizer
A mod for Art Of Rally that visualizes waypoints and reset zones.

## Usage

Key Binds
* `F8`: Toggle no-go zones (**on by default**)
* `F9`: Toggle waypoint visualizers
* `F10`: Turn of all visualizers

## Changes

The following changes have been applied to the `Assembly-CSharp.dll`

### `OutOfBoundsManager`

*Added fields*

```cs
private List<GameObject> Q_WaypointVisualizers;
private List<GameObject> Q_NoGoVisualizers;
```

*Modified Methods*

```cs
public void Start()
{
  this.Q_WaypointVisualizers = new List<GameObject>();
  this.Q_NoGoVisualizers = new List<GameObject>();
  
  try
	{
	  resets = GameObject.Find("ResetZones").transform;
	}
	catch
	{
	}
  
  if (resets != null)
	{
		foreach (object obj in resets)
		{
			SphereCollider resetObj = ((Transform)obj).gameObject.GetComponent<SphereCollider>();
			GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.Q_NoGoVisualizers.Add(gameObject2);
			gameObject2.transform.position = resetObj.transform.position;
			gameObject2.transform.localScale = new Vector3(resetObj.radius * 2f, resetObj.radius * 2f, resetObj.radius * 2f);
			((Collider)gameObject2.GetComponent(typeof(Collider))).isTrigger = true;
			MeshRenderer meshRenderer = gameObject2.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
			meshRenderer.receiveShadows = false;
			meshRenderer.material.SetInt("_SrcBlend", 1);
			meshRenderer.material.SetInt("_DstBlend", 10);
			meshRenderer.material.SetInt("_ZWrite", 0);
			meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
			meshRenderer.material.DisableKeyword("_ALPHABLEND_ON");
			meshRenderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			meshRenderer.material.renderQueue = 3000;
			meshRenderer.material.SetFloat("_Mode", 3f);
			meshRenderer.material.color = new Color(1f, 0f, 0f, 0.7f);
			meshRenderer.material.EnableKeyword("_SpecularHighlights");
			meshRenderer.material.EnableKeyword("_GlossyReflections");
			meshRenderer.material.SetFloat("_SpecularHighlights", 0f);
			meshRenderer.material.SetFloat("_GlossyReflections", 0f);
		}
	}
  
  ...
    else if (transform2.gameObject.name.Length > 5 && transform2.name.Contains("Waypoint"))
		{
      this.WaypointList.Add(transform2.position);
			GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject3.SetActive(false);
			gameObject3.transform.position = transform2.position;
			gameObject3.transform.localScale = new Vector3(34f, 34f, 34f);
			((Collider)gameObject3.GetComponent(typeof(Collider))).isTrigger = true;
			MeshRenderer meshRenderer2 = gameObject3.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
			meshRenderer2.receiveShadows = false;
			meshRenderer2.material.SetInt("_SrcBlend", 1);
			meshRenderer2.material.SetInt("_DstBlend", 10);
			meshRenderer2.material.SetInt("_ZWrite", 0);
			meshRenderer2.material.DisableKeyword("_ALPHATEST_ON");
			meshRenderer2.material.DisableKeyword("_ALPHABLEND_ON");
			meshRenderer2.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			meshRenderer2.material.renderQueue = 3000;
			meshRenderer2.material.SetFloat("_Mode", 3f);
			meshRenderer2.material.color = new Color(1f, 0.8f, 0.8f, 0.1f);
			meshRenderer2.material.EnableKeyword("_SpecularHighlights");
			meshRenderer2.material.EnableKeyword("_GlossyReflections");
			meshRenderer2.material.SetFloat("_SpecularHighlights", 0f);
			meshRenderer2.material.SetFloat("_GlossyReflections", 0f);
  ...
  
  if (!this.isForwardStage)
	{
		this.WaypointList.Reverse();
    // this is not used for now
    this.Q_WaypointVisualizers.Reverse();
```

*Added Methods*

```cs
	private void OnGUI()
	{
		if (Event.current.Equals(Event.KeyboardEvent(KeyCode.F9.ToString())))
		{
			foreach (GameObject gameObject in this.Q_WaypointVisualizers)
			{
				gameObject.SetActive(!gameObject.active);
			}
		}
		if (Event.current.Equals(Event.KeyboardEvent(KeyCode.F8.ToString())))
		{
			foreach (GameObject gameObject2 in this.Q_NoGoVisualizers)
			{
				gameObject2.SetActive(!gameObject2.active);
			}
		}
		if (Event.current.Equals(Event.KeyboardEvent(KeyCode.F10.ToString())))
		{
			foreach (GameObject gameObject3 in this.Q_WaypointVisualizers)
			{
				gameObject3.SetActive(false);
			}
			foreach (GameObject gameObject4 in this.Q_NoGoVisualizers)
			{
				gameObject4.SetActive(false);
			}
		}
	}
```

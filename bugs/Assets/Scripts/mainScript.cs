using UnityEngine;
using System.Collections;

public class mainScript : MonoBehaviour {
	public int bugCount=10;
	public GameObject bugPrefab;
	private ArrayList bugs=new ArrayList();
	private ArrayList beacons=new ArrayList();
	// Use this for initialization
	void Start () {
		beacons=new ArrayList();
		foreach(GameObject zone in GameObject.FindGameObjectsWithTag("zone")){
			Beacon b=new Beacon(zone);
			beacons.Add (b);
		}
		for(int count=0;count<bugCount;count++){
			GameObject bug=(GameObject)Instantiate(bugPrefab,new Vector3(0f,.5f,1.25f) ,Quaternion.identity);
			bugScript script=(bugScript)bug.GetComponent("bugScript");
			script.setBeacons(beacons);
			bugs.Add (bug);
			
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
public class Beacon{
	public float radius=3f;
	public string label="";
	public GameObject gameObject=null;
	public Transform transform=null;
	public Beacon(GameObject go){
		gameObject=go;
		beaconScript script=(beaconScript)gameObject.GetComponent("beaconScript");
		this.transform=gameObject.transform;
		radius=script.influenceRadius;
		label=script.label;
		
	}
	public string toString(){
		return label;
	}
}
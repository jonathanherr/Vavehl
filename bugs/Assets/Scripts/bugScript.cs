using UnityEngine;
using System.Collections;

public enum state{hungry,thirsty,tired,safe,wandering};

public class Bug{
	state currentState;
	Transform transform;
	float speed;
	float turnFreq;
	float fov=160f;
	float sightRadius=5f;
	ArrayList worldBeacons;
	Beacon currentBeacon=null;
	System.Collections.Generic.Dictionary<string,StateMachine> stateMachines=null;
	
	public state getState(){
		return currentState;
	}
	public Beacon getBeacon(){
		return currentBeacon;
	}
	//ctor
	public Bug(Transform transform,float speed,float turnFreq){
		stateMachines=new System.Collections.Generic.Dictionary<string,StateMachine>();
		currentState=state.wandering;
		this.transform=transform;
		this.speed=speed;
		this.turnFreq=turnFreq;
		//create bug state machine - TODO: load these from XML
		stateMachines["idle"]=createIdleStateMachine();
		stateMachines["action"]=createActionStateMachine();
	}
	public StateMachine createActionStateMachine(){
		StateMachine stateMachine=new StateMachine("action");
		return stateMachine;
	}
	public StateMachine createIdleStateMachine(){
		StateMachine stateMachine = new StateMachine("idle");
		Node startNode=new Node("Home");
		stateMachine.setStartNode(startNode);
		Node hunger=startNode.addNeighbor("Hunger",.5f);
		Node thirst=startNode.addNeighbor("Thirst",.5f);
		hunger.addNeighbor(thirst);
		thirst.addNeighbor(startNode);
		
		return stateMachine;
	}
	public void setBeacons(ArrayList beacons){
		worldBeacons=beacons;
	}
	//set random state
	public void switchState(){
		if(Random.Range (0.0f,1.0f)<.15f){
			currentState=(state)Random.Range (0,4);
		}
	}
	public void logState(){
		switch (currentState) {
		case state.hungry:
				Debug.Log("hungry");
				break;
		case state.safe:
				Debug.Log ("safe");
				break;
		case state.thirsty:
				Debug.Log("thirsty");
				break;
		case state.wandering:
				Debug.Log ("wandering");
				break;
		case state.tired:
				Debug.Log ("tired");
				break;
		default:
		break;
		}
	}
	public void sense(){
		
		//logState ();
		foreach(Beacon beacon in worldBeacons){
			if (Vector3.Angle(transform.forward, transform.position - beacon.transform.position) < fov/2) {
    			if(Vector3.Distance(transform.position,beacon.transform.position)<=sightRadius && Vector3.Distance(transform.position,beacon.transform.position)<=beacon.radius){
					Debug.Log("I see " + beacon.label);
					
					(beacon.gameObject.GetComponent ("Halo") as Behaviour).enabled=true;
					if(currentBeacon!=null && currentBeacon!=beacon)
						(currentBeacon.gameObject.GetComponent("Halo") as Behaviour).enabled=false;
					currentBeacon=beacon;
				}
				else if(currentBeacon!=null){
					(currentBeacon.gameObject.GetComponent("Halo") as Behaviour).enabled=false;
					currentBeacon=null;
				}
			}
		}
	}
	public void turn(Vector3 direction,float degrees){
		this.transform.RotateAround(this.transform.position,direction,degrees);
	}
	public void randomTurn(){
		this.transform.rotation=Random.rotation;
	}
	public void move(){
		
		this.transform.rigidbody.AddRelativeForce(new Vector3(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f))*this.speed);
	}
}
class EdgeComparer: System.Collections.Generic.IComparer<Edge>
{
	public int Compare(Edge e1, Edge e2){
		return e1.value.CompareTo(e2.value);
	}
}
public class Node{
		string name="";
		System.Collections.Generic.List<Edge> edges=null;
		public string getLabel(){
			return name;
		}
		public System.Collections.Generic.List<Edge> getEdges(){
			return edges;
		}
		public Node(string label){
			this.name=label;
			edges=new System.Collections.Generic.List<Edge>();
			
		}
		
		public Node addNeighbor(string label){
			return addNeighbor(label,1.0f); 
		}
		public Node addNeighbor(string label, float transitionProb){
			Node n=new Node(label);
			addNeighbor(n,transitionProb);
			return n;
		}
		public Node addNeighbor(Node node){
			return addNeighbor(node,1.0f);
		}
		public Node addNeighbor(Node node,float transitionProb){
			Edge edge=new Edge(this,node,transitionProb);
			edges.Add(edge);
			return node;
		}
}

/**
 * Edge in a statemachine DAG. Contains To/From nodes and an optional value for the edge. 
 **/
public class Edge{
	Node from;
	Node to;
	public float value;
	
	public Edge(Node from, Node to, float value){
		this.from=from;
		this.to=to;
		this.value=value;
	}
	public Node getToNode(){
		return to;
	}
	public Node getFromNode(){
		return from;
	}
	
}

public class StateMachine{
	string name="";
	Node currentNode;
	public StateMachine(string label){
		this.name=label;
	}	
	public void setStartNode(Node node){
		currentNode=node;
	}
	public Node next(){
		if(currentNode.getEdges().Count>1){
			float curProb=0.0f;
			Node nextNode=null;
			float prob=Random.Range(0.0f,1.0f);
			float total=0.0f;
			foreach (Edge edge in currentNode.getEdges())
			{	
				total+=edge.value;
				if(prob<=total){
					nextNode=edge.getToNode();
					break; //once we've found our node, quit the loop. 
				}
			}
			return nextNode;
		}
		else{
			return currentNode.getEdges()[0].getToNode();
		}
	}
	public Node walk(Node node){
		string tree=node.getLabel()+"\n";
		foreach(Edge edge in node.getEdges()){
			walk (edge.getToNode);
		}
	}
	public string toString(){
		
	}
}
public class bugScript : MonoBehaviour {
	public float speed=10.0f;
	public float turnFreq=.5f;
	ArrayList worldBeacons;
	Bug bug;
	public void setBeacons(ArrayList beacons){

		worldBeacons=beacons;
	}
	// Use this for initialization
	void Start () {
		bug=new Bug(transform,speed,turnFreq);
		bug.setBeacons(worldBeacons);
	}
	
	// Update is called once per frame
	void Update () {
		bug.sense ();
		if(bug.getState()==state.wandering){
			float choice=Random.Range(0.0f,1.0f);
			
			if(choice>turnFreq){
				bug.move ();
			}
			else{
				bug.randomTurn ();
			}
		}
		else if(bug.getState()==state.hungry){
			
		}
	}
}

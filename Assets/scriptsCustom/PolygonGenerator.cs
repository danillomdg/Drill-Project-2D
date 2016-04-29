using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

[System.Serializable]
public class PolygonGenerator : MonoBehaviour {
	public Text debugger;
	public bool editorMode;
	public static PolygonGenerator porigon; 
	public List<PolygonGenerator> savedTerrains = new List<PolygonGenerator>();
	//public static List<GameEditorData> savedPatterns = new List<GameEditorData>();
	
	public GameCamera cam;

	public List<Vector3> newVertices = new List<Vector3>();
	public List<int> newTriangles = new List<int>();
	public List<Vector2> newUV = new List<Vector2>();
	
	public List<Vector3> colVertices = new List<Vector3>();
	public List<int> colTriangles = new List<int>();

	public List<TerrainElement> Minerals = new List<TerrainElement>();

	

	private int colCount;
	public float PosState = 1;

	private Mesh mesh;
	private MeshCollider col;
	
	private float tUnit = 0.25f;
	private Vector2 graminha = new Vector2 (0, 0);
	private Vector2 hole = new Vector2 (3, 0);
	private Vector2 hole2 = new Vector2 (2, 0);
	private Vector2 tStone = new Vector2 (3, 3);
	private Vector2 tGrass = new Vector2 (3, 1);

	private Vector2 sRock = new Vector2 (3, 2);


	private Vector2 Gold = new Vector2 (0, 3);
	private Vector2 silver = new Vector2 (1, 3);
	private Vector2 bronze = new Vector2 (2, 3);
	private Vector2 diamond = new Vector2 (1, 2);
	private Vector2 iron = new Vector2 (2, 2);

	private Vector2 repairKit = new Vector2 (1, 1);
	private Vector2 fuelTank = new Vector2 (2, 1);

		
	
	public byte[,] blocks;
	private int squareCount;
	//public Vector2 update=new Vector2 (0, -1);
	public bool update = false;
	public bool terrainUpgradeFinished = true; 
	public Vector2 update2 = new Vector2(-1,-1);
	public int  SizeX = 96; //default: 96
	public int SizeY = 128;

	public int  EditorSizeX = 20; //default: 96
	public int EditorSizeY = 22;

	private int timesUpgraded = 0;
	private int TerrainUpgradeY = 20;

	private float colliderChangeZ = 1;

	public WWW Peleja;

	void Awake() {
			porigon = this;
		print ("porigonized");
	}

	// Use this for initialization
	void Start () {
		if (editorMode == false)
		{
		blocks=new byte[SizeX,SizeY];
		mesh = GetComponent<MeshFilter> ().mesh;
		col = GetComponent<MeshCollider> ();

		Minerals.Add(TerrainElement.CreateInstance(3,"gold",0.65f,400,70));
		Minerals.Add(TerrainElement.CreateInstance(4,"silver",0.55f,250,70));
		Minerals.Add(TerrainElement.CreateInstance(5,"bronze",0.0f,150,200));
		Minerals.Add(TerrainElement.CreateInstance(6,"diamond",0.80f,200,70));
		Minerals.Add(TerrainElement.CreateInstance(7,"iron",0.25f,600,200));  



			//StartCoroutine(initialize());
		GenTerrain(blocks, SizeX,SizeY);
		BuildMesh();
		UpdateMesh();

		}
		else 
		{
			blocks=new byte[EditorSizeX,EditorSizeY];
			mesh = GetComponent<MeshFilter> ().mesh;
			col = GetComponent<MeshCollider> ();
			GenEditorTerrain(blocks, EditorSizeX,EditorSizeY);
			BuildMesh();
			UpdateMesh();

		}
	}


	
	void Update(){

		if(update2.x != -1){
			BuildBlocks((int)update2.x,(int)update2.y * -1,7);
			//BuildMesh();
			UpdateMesh();
			update2.x = -1;
		}
	
	}

	public IEnumerator initialize()
	{
		string path = "file:///"+Application.streamingAssetsPath + "/savedPatterns.gd";
		string uin = debugger.text;
		debugger.text = uin+" On Windows"+"\n" ;
		if(Application.platform == RuntimePlatform.Android)
		{
		
			path =  "jar:file://" + Application.dataPath + "/assets/"+"savedPatterns.gd";
			//path= Application.streamingAssetsPath + "/savedPatterns.gd";
			debugger.text = uin+" On Android"+"\n" ;
		}
		//string path = "jar:file://" + Application.dataPath + "!/assets/";
		Peleja = new WWW(path);
		while(!Peleja.isDone)
			yield return null;
		GenTerrain(blocks, SizeX,SizeY);
		BuildMesh();
		UpdateMesh();
	}




	int NoiseInt (int x, int y, float scale, float mag, float exp){
		
		return (int) (Mathf.Pow ((Mathf.PerlinNoise(x/scale,y/scale)*mag),(exp) ));
		
		
	}

	void GenEditorTerrain(byte[,] blocks, int x, int y)
	{
		for(int px=0;px<blocks.GetLength(0);px++)
		{
			for(int py=2;py<blocks.GetLength(1);py++)
			{
				blocks[px, py]=1;
			}
			
		}
		
	}
	

	public static List<GameEditorData> ByteArrayToObject(byte[] arrBytes)
	{
		using (MemoryStream memStream = new MemoryStream())
		{
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write(arrBytes, 0, arrBytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			List<GameEditorData> obj = (List<GameEditorData>)binForm.Deserialize(memStream);
			return obj;
		}
	}



	public void PlaceManualMade (bool namae, string name, int numba, int x, int y)
	{


		GEDContainer ola = new GEDContainer();
		var configFile = Resources.Load("savedPatterns") as TextAsset;
		var serializer = new XmlSerializer(typeof(GEDContainer));
		using(var reader = new System.IO.StringReader(configFile.text))
		{
			ola =  serializer.Deserialize(reader) as GEDContainer;
		}
		print ("Oi, eu sou o "+ola.savedPatterns[0].Name);
		debugger.text = debugger.text+ola.savedPatterns[0].Name+"\n" ;



				if (namae == true)
				{
			for (int i =0; i < ola.savedPatterns.Count; i++)
					{
				if (ola.savedPatterns[i].Name == name)
					PlacePattern (ola.savedPatterns[i].blocks, x, y);
					}
				}
				else 
				{
			if (ola.savedPatterns.Count -1 <= numba)
				PlacePattern (ola.savedPatterns[numba].blocks, x, y);
				}

		
	}

	private void PlacePattern (byte[][] Pattern, int x, int y)
	
	{
		print ("Pattern x: "+Pattern.Length);
		print ("Pattern Y: "+Pattern[0].Length);
		for (int xi = 0; xi < Pattern.Length; xi ++)
		{
			for (int yi = 0; yi < Pattern[0].Length; yi ++)
			{
				blocks[x + xi, y + yi] = Pattern[xi][yi];
			}
	}


	}

	public void EditorUpdateMesh()
	{
		BuildMesh();
		UpdateMesh();
	}
	
	void GenTerrain(byte[,] blocks, int x, int y){
	//	blocks=new byte[x,y];    // original: 96, 128


		
		for(int px=0;px<blocks.GetLength(0);px++){
			int stone= NoiseInt(px,0, 80,15,1);
			stone+= NoiseInt(px,0, 50,30,1);
			stone+= NoiseInt(px,0, 10,10,1);
			stone+=75;
			
			
			int dirt = NoiseInt(px,0, 100f,35,1);
			dirt+= NoiseInt(px,100, 50,30,1);
			dirt+=75;
			
			
			for(int py=2;py<blocks.GetLength(1);py++){
				if (blocks[px,py]!=5 ){
					if(py<blocks.GetLength(1)){
					blocks[px, py]=1;
					
					if(NoiseInt(px,py,12,16,1)>10){  //dirt spots
						blocks[px,py]=2;
						
					}
					
					
				} else if(py<dirt) {
					blocks[px,py]=2;
				}
				

				}
			}
		}

		for (int x1 = 0; x1 <=200;x1++)
		{
			Vector2 Coords = randomiza(0, (blocks.GetLength(0)/2) ,3, blocks.GetLength(1)/2-10,21); 
			//	Vector2 Coords = randomiza(0, (blocks.GetLength(0)/2) ,0, (blocks.GetLength(1)/2),3);
			
			EscalaCubo(blocks,Coords,21,-1);
		}


		generateMinerals(blocks,true,0,0);


		for (int x1 = 0; x1 <=50;x1++)
		{
			Vector2 Coords = randomiza(0, (blocks.GetLength(0)/2) ,3, blocks.GetLength(1)/2-10,3); 
			//	Vector2 Coords = randomiza(0, (blocks.GetLength(0)/2) ,0, (blocks.GetLength(1)/2),3);
			
			EscalaCubo(blocks,Coords,20,-1);
			EscalaCubo(blocks,new Vector2(Coords.x, Coords.y -2),20,-1);
		}


		for (int x1 = 0; x1 <=50;x1++)
		{
			Vector2 Coords = randomiza(0, (blocks.GetLength(0)/2) ,3, blocks.GetLength(1)/2-10,21); 
			//	Vector2 Coords = randomiza(0, (blocks.GetLength(0)/2) ,0, (blocks.GetLength(1)/2),3);
			
			EscalaCubo(blocks,Coords,51,-1);
		}

		for (int x1 = 0; x1 <=50;x1++)
		{
			Vector2 Coords = randomiza(0, (blocks.GetLength(0)/2) ,3, blocks.GetLength(1)/2-10,21); 
			//	Vector2 Coords = randomiza(0, (blocks.GetLength(0)/2) ,0, (blocks.GetLength(1)/2),3);
			
			EscalaCubo(blocks,Coords,52,-1);
		}
		
		

		//GENERATE TinnyCaves
		for (int x1 = 0; x1 <=660;x1++) //default: 440
		{

			Vector2 X1 = new Vector2(3,5);
			Vector2 Y2 = new Vector2(-4,4);
			generateCaves(blocks,X1,Y2,5,49);
			
		}


		for (int x1 = 0; x1 <=660;x1++) //default: 440
		{
//			Vector2 X1 = new Vector2(4,12);
//			Vector2 Y2 = new Vector2(-8,8);

			Vector2 X1 = new Vector2(4,12);
			Vector2 Y2 = new Vector2(-8,8);
			generateCaves(blocks,X1,Y2,50,0);
		
		}

//CRIA O CHAO DI PREDA

//		for(int py2=blocks.GetLength(1)-20;py2<blocks.GetLength(1)-15;py2++){
//		for(int px2=0;px2<blocks.GetLength(0);px2++){
//
//					blocks[px2,py2]=21;
//			}}

		//MUDA O NUMERO DA GRAMINHA
		for (int px2=0; px2 <blocks.GetLength(0);px2++)
			blocks[px2,1]=0;

		//blocks[0,0]=21;
		for (int i = 0; i < blocks.GetLength (0); i++)
			blocks[i,blocks.GetLength(1)-1] = 1;

		PlaceManualMade(true, "Onikiri", 0, 3, 5);
		
	}
	

	void BuildMesh(){
		for(int px=0;px<blocks.GetLength(0);px++){
			for(int py=0;py<blocks.GetLength(1);py++){
				
				if(blocks[px,py]!=0 && blocks[px,py]!=20  ){ 
					
					GenCollider(px,py);
					if(blocks[px,py]==1 ){
															
							GenSquare(px,py,tStone );
					
							}
					 else if(blocks[px,py]==2){
						GenSquare(px,py,tGrass );
					}

					else if(blocks[px,py]==3){
						GenSquare(px,py,Gold  );
					}
					else if(blocks[px,py]==4){
						GenSquare(px,py,silver );
					}
					else if(blocks[px,py]==5){
						GenSquare(px,py,bronze );
					}
					else if(blocks[px,py]==6){
						GenSquare(px,py,diamond );
					}
					else if(blocks[px,py]==7){
						GenSquare(px,py,iron );
					}
					//STATIC ROCKS:


					else if  (blocks[px,py]==21) 
						GenSquare(px,py, sRock );

//					else if  (blocks[px,py]==50) 
//						GenSquare(px,py, sRock );

					else if  (blocks[px,py]==51) 
						GenSquare(px,py, repairKit );

					else if  (blocks[px,py]==52) 
						GenSquare(px,py, fuelTank );

					
					
				}
				else	if  ((py > 1 && py < blocks.GetLength (1)-1) || blocks[px,py]==20 ) GenSquare(px,py, hole );

				 if  (py == 1) GenSquare(px,py, graminha );
			}
		}
	}

 public void testUpgradeMesh()
	{
		StartCoroutine(UpgradeMesh(true,TerrainUpgradeY));
		//UpgradeMesh(true,20); 
		print ("passou daqui");
	}


	public IEnumerator UpgradeMesh(bool y, int value)
	{
		terrainUpgradeFinished = false;
		if (y == true)
		{
			byte[,] oldBlocks = blocks;
			byte[,] addBlocks = new byte[blocks.GetLength(0),value];
			byte[,] newBlocks = new byte[blocks.GetLength(0),blocks.GetLength(1) + value];

			//INICIO DA INSANIDADE



			float divisor = (float)value / (float)SizeY;
			
			for(int px=0;px<addBlocks.GetLength(0);px++){
				int stone= NoiseInt(px,0, 80,15,1);
				stone+= NoiseInt(px,0, 50,30,1);
				stone+= NoiseInt(px,0, 10,10,1);
				stone+=75;

				int dirt = NoiseInt(px,0, 100f,35,1);
				dirt+= NoiseInt(px,100, 50,30,1);
				dirt+=75;
				
				for(int py=0;py<addBlocks.GetLength(1);py++){
					if (addBlocks[px,py]!=5 ){
						if(py<addBlocks.GetLength(1)){
							addBlocks[px, py]=1;
							
							if(NoiseInt(px,py,12,16,1)>10){  //dirt spots
								addBlocks[px,py]=2;
								
							}
									
						} else if(py<dirt) {
							addBlocks[px,py]=2;
						}
					}

				}
				print ("aptempt x: "+px+ " y : ");
				yield return null;
			}

			int Eufemismo = 20;
			
			for (int x1 = 0; x1 <=Mathf.RoundToInt(200* divisor);x1++)
			{
				print ("Dividar "+Mathf.RoundToInt(200* divisor));
				Vector2 Coords = randomiza(0, (addBlocks.GetLength(0)/2) ,3, addBlocks.GetLength(1)/2,21); 
				
				EscalaCubo(addBlocks,Coords,21,-1);
				if (x1 % Eufemismo == 0)
				yield return null;
			}
			
			
			generateMinerals(addBlocks,true,oldBlocks.GetLength (1),addBlocks.GetLength (1));
			
			
			for (int x1 = 0; x1 <=Mathf.RoundToInt(50* divisor);x1++)
			{
				Vector2 Coords = randomiza(0, (addBlocks.GetLength(0)/2) ,3, addBlocks.GetLength(1)/2,3); 

				
				EscalaCubo(addBlocks,Coords,20,-1);
				EscalaCubo(addBlocks,new Vector2(Coords.x, Coords.y -2),20,-1);
				if (x1 % Eufemismo == 0)
				yield return null;
			}
			
			
			
			
			for (int x1 = 0; x1 <=Mathf.RoundToInt(660* divisor);x1++) //default: 440
			{

				
				Vector2 X1 = new Vector2(4,12);
				Vector2 Y2 = new Vector2(-8,8);
				generateCaves(addBlocks,X1,Y2,0,0);
				if (x1 % Eufemismo == 0)
				yield return null;
				
			}

			for (int i = 0; i < addBlocks.GetLength (0); i++)
				addBlocks[i,addBlocks.GetLength(1)-1] = 1;


			//FIM DA INSANIDADE


			for (int i = 0;i< oldBlocks.GetLength(0);i++)
				{
				for (int j = 0;j< oldBlocks.GetLength(1);j++)
					{
					newBlocks[i,j] = oldBlocks[i,j];
					}
				}

			for (int i = 0;i< addBlocks.GetLength(0);i++)
			{
				for (int j = 0;j< addBlocks.GetLength(1);j++)
				{
					newBlocks[i,j+oldBlocks.GetLength(1)] = addBlocks[i,j];
				}
			}

			blocks = newBlocks;
			SizeY = newBlocks.GetLength(1);
			print (blocks.GetLength(1));
			update2 = new Vector2(Mathf.RoundToInt(cam.transform.position.x),Mathf.RoundToInt(cam.transform.position.y-transform.position.y));


		//	BuildMesh();
		//	UpdateMesh();

		}
		terrainUpgradeFinished = true;
		timesUpgraded += 1;
		print ("Terrain Upgrade number #"+timesUpgraded+": Success!");
		yield return true;

	}


	void BuildBlocks(int x, int y, int size){



		for(int px=x-size;px<x+size+1;px++){
			for(int py=y-size;py<y+size+1;py++){
				if (px >=0 && px < blocks.GetLength(0) && py >=0 && py <blocks.GetLength(1))
				{


					if(blocks[px,py]!=0 && blocks[px,py]!=20 && blocks[px,py]!=22 ){  
					
					GenCollider(px,py);

					if(blocks[px,py]==1){
						GenSquare(px,py,tStone );		
					}


					else if(blocks[px,py]==2){
						GenSquare(px,py,tGrass );
					}
					
					else if(blocks[px,py]==3){
						GenSquare(px,py,Gold  );
					}
					else if(blocks[px,py]==4){
						GenSquare(px,py,silver );
					}
					else if(blocks[px,py]==5){
						GenSquare(px,py,bronze );
					}
					else if(blocks[px,py]==6){
						GenSquare(px,py,diamond );
					}
					else if(blocks[px,py]==7){
						GenSquare(px,py,iron );
					}
						//STATIC ROCKS:
						else if  (blocks[px,py]==21) 
							GenSquare(px,py, sRock );

//					else if  (blocks[px,py]==50) 
//						GenSquare(px,py, sRock );

					else if  (blocks[px,py]==51) 
							GenSquare(px,py, repairKit );

					else if  (blocks[px,py]==52) 
						GenSquare(px,py, fuelTank );
					
				}
					else	if  (py > 1 && py < blocks.GetLength (1)-1 || blocks[px,py]==20 || blocks[px,py]==22 ) GenSquare(px,py, hole );
					if  (py == 1) GenSquare(px,py, graminha );
				}
			}

		}
	
	}
	

	byte Block (int x, int y){
		
		if(x==-1 || x==blocks.GetLength(0) || y==-1 || y==blocks.GetLength(1)){
			return (byte)1;
		}
		
		return blocks[x,y];
	}
	
	void GenCollider(int x, int y){
		
		y = y*-1;
		//Top
		if(Block(x,-y-1)==0 || Block(x,-y-1)==20 || Block(x,-y-1)==22){

			colVertices.Add( new Vector3 (x  ,  y  , colliderChangeZ));
			colVertices.Add( new Vector3 (x + 1 ,  y  , colliderChangeZ));
			colVertices.Add( new Vector3 (x + 1 ,  y , 0 ));
			colVertices.Add( new Vector3 (x  ,  y  , 0 ));
			
			ColliderTriangles();
			
			colCount++;
		}


		//bot
		if(Block(x,-y+1)==0 || Block(x,-y+1)==20 || Block(x,-y+1)==22){
			colVertices.Add( new Vector3 (x  ,  y -1 , 0));
			colVertices.Add( new Vector3 (x + 1 ,  y -1 , 0));
			colVertices.Add( new Vector3 (x + 1 ,  y -1 , colliderChangeZ ));
			colVertices.Add( new Vector3 (x  ,  y -1 , colliderChangeZ ));
			
			ColliderTriangles();
			colCount++;
		}
		
		//left
		if(Block(x-1,-y)==0 || Block(x-1,-y)==20 || Block(x-1,-y)==22){
			colVertices.Add( new Vector3 (x  ,  y -1 , colliderChangeZ));
			colVertices.Add( new Vector3 (x  ,  y  , colliderChangeZ));
			colVertices.Add( new Vector3 (x  ,  y  , 0 ));
			colVertices.Add( new Vector3 (x  ,  y -1 , 0 ));
			
			ColliderTriangles();
			
			colCount++;
		}
		
		//right
		if(Block(x+1,-y)==0 || Block(x+1,-y)==20 || Block(x+1,-y)==22){
			colVertices.Add( new Vector3 (x +1 ,  y  , colliderChangeZ));
			colVertices.Add( new Vector3 (x +1 ,  y -1 , colliderChangeZ));
			colVertices.Add( new Vector3 (x +1 ,  y -1 , 0 ));
			colVertices.Add( new Vector3 (x +1 ,  y  , 0 ));
			
			ColliderTriangles();
			
			colCount++;
		}
		
	}
	
	void ColliderTriangles(){
		colTriangles.Add(colCount*4);
		colTriangles.Add((colCount*4)+1);
		colTriangles.Add((colCount*4)+3);
		colTriangles.Add((colCount*4)+1);
		colTriangles.Add((colCount*4)+2);
		colTriangles.Add((colCount*4)+3);
	}


	
	void GenSquare(int x, int y, Vector2 texture){
		y = y*-1;

		newVertices.Add( new Vector3 (x  ,  y  , 0 ));
		newVertices.Add( new Vector3 (x + 1 ,  y  , 0 ));
		newVertices.Add( new Vector3 (x + 1 ,  y-1 , 0 ));
		newVertices.Add( new Vector3 (x  ,  y-1 , 0 ));
	
		
		newTriangles.Add(squareCount*4);
		newTriangles.Add((squareCount*4)+1);
		newTriangles.Add((squareCount*4)+3);
		newTriangles.Add((squareCount*4)+1);
		newTriangles.Add((squareCount*4)+2);
		newTriangles.Add((squareCount*4)+3);

		y = y*-1;

		if (texture == hole) {

			float xaux1 = 0;
			float yaux1 = 0;

			int xaux2 = 1;
			int yaux2 = 1;

			for  (int x1 = -1 ; x1 <2; x1++)
			{
				if ((x+x1) > 0  && (x+x1) < blocks.GetLength(0))
				{
					if (blocks[x+x1,y]  != 0 && blocks[x+x1,y]  != 20 && blocks[x+x1,y]  != 22)
						xaux1 += (x1+2);
				}
			}
			for  (int y1 = -1 ; y1 <2; y1++)
			{
				if ((y+y1) > 0 && (y+y1)  < blocks.GetLength(1))
				{
					if (blocks[x,y-y1] != 0 && blocks[x,y-y1] != 20 && blocks[x,y-y1] != 22)
						yaux1 += (y1+2);
				}
				
			}
			if (xaux1 != 0) xaux1 = xaux1-2;
			if (yaux1 != 0) yaux1 = yaux1-2;

			if (xaux1 == 2){
				texture = hole2;
				xaux1 = 1;
				xaux2 = -1;
				if (yaux1 == -1 || yaux1 == 1)
					yaux1 = yaux1 * 0.83f;
			
			}



			if (yaux1 == 2){
				texture = hole2;
				yaux1 = 1;
				yaux2 = -1;


			}


			if (yaux1 == 4 && xaux1 == 4)
		
			{
				newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y + tUnit));
				newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
				newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y));
				newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y));
			}
			else
			{

			newUV.Add(new Vector2 (tUnit * texture.x + tUnit * 0.25f  + xaux1 * xaux2 * tUnit * 0.25f, tUnit * texture.y + tUnit * 0.75f +  yaux1  * tUnit * 0.25f));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit * 0.75f  + xaux1 * tUnit * 0.25f, tUnit * texture.y + tUnit * 0.75f + yaux1  * tUnit * 0.25f));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit * 0.75f + xaux1 * tUnit * 0.25f, tUnit * texture.y + tUnit * 0.25f + yaux1 * yaux2 * tUnit * 0.25f));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit * 0.25f + xaux1 * xaux2 * tUnit * 0.25f, tUnit * texture.y + tUnit * 0.25f + yaux1 * yaux2 * tUnit * 0.25f));
			}

		}

		else{


			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
			newUV.Add(new Vector2 (tUnit * texture.x + tUnit, tUnit * texture.y));
			newUV.Add(new Vector2 (tUnit * texture.x, tUnit * texture.y));

		}


		squareCount++;
		
	}

	void UpdateMesh () {
		mesh.Clear ();
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.Optimize ();
		mesh.RecalculateNormals ();
		
		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();
		squareCount=0;
		
		Mesh newMesh = new Mesh();
		newMesh.vertices = colVertices.ToArray();
		newMesh.triangles = colTriangles.ToArray();
		col.sharedMesh= newMesh;


		colVertices.Clear();
		colTriangles.Clear();
		colCount=0;
	}



	Vector2 randomiza (float Xmin, float Xmax,float Ymin, float Ymax, int type){
		int x1 = Mathf.FloorToInt(Random.Range(Xmin,Xmax)) * 2;
		int y1 =  Mathf.FloorToInt(Random.Range(Ymin,Ymax)) * 2;
		byte t1 = (byte) type; 
		if (x1 % 2 == 0 && y1 % 2 == 0){
			return new Vector2(x1,y1);

		}
			else  
		{randomiza(Xmin, Xmax,Ymin, Ymax,type);
			return new Vector2(0,0);}
	}


	void EscalaCubo (byte [,] blocks, Vector2 coords,int type, int antitype ){
		byte t1 = (byte) type; 
		byte t2 = (byte) antitype; 

		if (coords.x > 0 && coords.x < blocks.GetLength(0) && coords.y > 0 && coords.y < blocks.GetLength(1)){
					if (blocks[Mathf.FloorToInt(coords.x)   ,   Mathf.FloorToInt(coords.y)]   != t2)
						blocks[Mathf.FloorToInt(coords.x)   ,   Mathf.FloorToInt(coords.y)]   =   t1;
				}

	}

	float Dado10lados() {
		float num = Random.Range(0,10);
		return num;
	}

	void generateMinerals(byte[,] blocks,bool fixedSize,int StartSize, int PlussSize)
	{
		for (int i =0; i< Minerals.Count; i++)
		{
			for (int j = 0; j< Minerals[i].quantity;j++)
				{
				int randPos = Random.Range(2,SizeY+PlussSize-1);
				int barreira = Mathf.FloorToInt((SizeY+PlussSize-1) * Minerals[i].rareness);
				Vector2 gotIt = new Vector2(Random.Range(1,SizeX),randPos - StartSize);
					if (randPos < barreira)
						{

							if (Random.Range(0,barreira-randPos) == 0)
								{
						EscalaCubo(blocks,gotIt,Minerals[i].ID,-1);
								}
							else 
							{
							if (fixedSize)	j = j-1;
							}
						}
				else 
				
					EscalaCubo(blocks,gotIt,Minerals[i].ID,-1);

				
			}
		}
	}


	void generateCaves(byte[,] blocks,Vector2 MaxMinX1, Vector2 MaxMinY2, int MinPosY, int MaxPosY  ){
		{
			if (MaxPosY < MinPosY || MaxPosY <= 0 )
				MaxPosY = blocks.GetLength(1)-1;
			Vector2 Coords = randomiza(0, (blocks.GetLength(0)) ,MinPosY/2,MaxPosY/2,3); 

			int RandX1 = Random.Range(Mathf.FloorToInt(MaxMinX1.x),Mathf.FloorToInt(MaxMinX1.y));
			int RandY1 = Random.Range(0,RandX1/2);

						for (int x = 0; x<RandX1; x++){

							//CREATE MONSTERSPAWNER
							if (x==0)
							{
								if (Coords.y > 30)
								{
									EscalaCubo(blocks,new Vector2(Coords.x+x,Coords.y),22,-1);
								}
								else
								{
									EscalaCubo(blocks,new Vector2(Coords.x+x,Coords.y),0,-1);
								}
								
							}
							else
							{
								EscalaCubo(blocks,new Vector2(Coords.x+x,Coords.y),0,-1);
							}
						}
					
			int RandY2 = Random.Range((int)MaxMinY2.x,(int)MaxMinY2.y);
			for (int y = 0; y<System.Math.Abs(RandY2); y++){
				if(Coords.y+y*Mathf.Sign(RandY2) > 2) 
					EscalaCubo(blocks,new Vector2(Coords.x+RandY1,Coords.y+y*Mathf.Sign(RandY2)),0,-1);
			}
			
		}


	}

	public void SizeRequested(GameObject Objectum)
	{
		Vector2 saizo = new Vector2(SizeX,SizeY);
		Objectum.SendMessage("GetSize",saizo);
	}


}



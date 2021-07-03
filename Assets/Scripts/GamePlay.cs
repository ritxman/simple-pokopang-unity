using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlay : MonoBehaviour {

	public Text textReady;
	public Text textScore;
	public Text textTimer;
	public Text textAddScore;
	public Text textScoreResult;
	private int counterReady;
	private bool gameStart;
	private bool isClicked;
	private int time;
	private int score;
	private int scoreAdd;
	private int totalScoreAdd;
	public GameObject Menu;
	public GameObject pauseGame;
	public GameObject restartGame;
	public GameObject backToMainMenu;
	public GameObject gameOver;
	public GameObject textNoMoves;
	private GameObject [] imageBoard = new GameObject[65];
	private int [] shapeType = new int[65];
	private bool [] shapeIsSelected = new bool[65];
	private bool [] shapeCheckedMove = new bool[65];
	private int [] totalShapesMatch = new int[65];
	private int randomShape;
	private int shapeSelectedIndex;
	private int tempShapeSelectedIndex;
	private int totalMatch;
	private bool isFall;
	Color colorTimeWarning;
	Color colorNormal;
	Color shapeSelectedColor;
	Color shapeNormalColor;
	Color shapeDestroy;
	private int indexBarisCheckFall;
	private int indexKolomCheckFall;
	private bool isMoveAvailable;
	private int isAnotherFall;
	//untuk animasi text:
	private int counterTextAddScore;
	// Use this for initialization
	void Start () {
		shapeSelectedIndex = -1;
		tempShapeSelectedIndex = -1;
		Camera.main.aspect = 1980f / 1080f;
		if(Application.loadedLevelName == "mainMenu"){
			
		}else{
			colorTimeWarning.r = 255;
			colorTimeWarning.g = 0;
			colorTimeWarning.b = 0;
			colorTimeWarning.a = 1;
			colorNormal.r = 255;
			colorNormal.g = 255;
			colorNormal.b = 255;
			colorNormal.a = 1;
			shapeSelectedColor.r = 255;
			shapeSelectedColor.g = 255;
			shapeSelectedColor.b = 255;
			shapeSelectedColor.a = 0.35f;
			shapeNormalColor.r = 255;
			shapeNormalColor.g = 255;
			shapeNormalColor.b = 255;
			shapeNormalColor.a = 1f;
			shapeDestroy.a = 255;
			shapeDestroy.g = 255;
			shapeDestroy.b = 255;
			shapeDestroy.a = 0f;
			totalScoreAdd = 0;
			indexBarisCheckFall = 0;
			indexKolomCheckFall = 0;
			gameStart = false;
			isClicked = false;
			isFall = false;
			isAnotherFall = 0;
			scoreAdd = 0;
			isMoveAvailable = false;
			counterReady = 0;
			time = 90;
			score = 0;
			counterTextAddScore = -1;
			totalMatch = 0;
			for(int i=0; i<64; i++){
				imageBoard[i] = GameObject.Find("Board ("+i+")");
				shapeType[i] = 0;
				shapeIsSelected[i] = false;
				shapeCheckedMove[i] = false;
				totalShapesMatch[i] = 1;
			}
			textScore.text = "Score: "+score;
			textTimer.text = "Time: "+time;
			textAddScore.text = "";
			textScoreResult.text = "";
			Menu.SetActive(false);
			fillBoard();
			checkMoves();
			StartCoroutine(readyAnimation());
			StartCoroutine(checkFall());
			StartCoroutine(animasiText());
		}
	}
	public void StartGame(){
		Application.LoadLevel("PuzzleGame");
	}
	public void ExitGame(){
		Application.Quit();
	}
	public void RestartGame(){
		if(isAnotherFall == 0){
			for(int i=0; i<64; i++){
				imageBoard[i].GetComponent<Image>().sprite = null;
			}
			restartGame.SetActive(true);
			gameStart = false;
		}
	}
	public void PauseGame(){
		if(isAnotherFall == 0){
			for(int i=0; i<64; i++){
				imageBoard[i].GetComponent<Image>().sprite = null;
			}
			pauseGame.SetActive(true);
			gameStart = false;
		}
	}
	public void BackToGame(){
		for(int i=0; i<64; i++){
			imageBoard[i].GetComponent<Image>().sprite = GameObject.Find(""+shapeType[i]).GetComponent<Image>().sprite;
		}
		pauseGame.SetActive(false);
		restartGame.SetActive(false);
		backToMainMenu.SetActive(false);
		gameStart = true;
	}
	public void BackToMainMenu(){
		if(isAnotherFall == 0){
			for(int i=0; i<64; i++){
				imageBoard[i].GetComponent<Image>().sprite = null;
			}
			backToMainMenu.SetActive(true);
			gameStart = false;
		}
	}
	public void yesBackToMainMenu(){
		Application.LoadLevel("mainMenu");
	}
	public void GameOver(){
		gameOver.SetActive(true);
		textScoreResult.text = "Score: "+score;
	}
	public void fillBoard(){
		do{
			for(int i=0; i<64; i++){
				addShape(i);
			}
			checkMoves();
		}while(isMoveAvailable == false);
	}
	public void addShape(int index){
		randomShape = Random.Range(1,5);
		shapeType[index] = randomShape;
		imageBoard[index].GetComponent<Image>().sprite = GameObject.Find(""+randomShape).GetComponent<Image>().sprite;
		imageBoard[index].GetComponent<Image>().color = shapeNormalColor;
	}
	public void shapeSelected(int index){
		shapeSelectedIndex = index;
	}
	public void shapeReleased(int index){
		
	}
	public void checkMoves(){
		isMoveAvailable = false;
		for(int i=0; i<64; i++){
			shapeCheckedMove[i] = false;
			totalShapesMatch[i] = 1;
		}
		//cek dari bawah ke atas
		for(int i=0; i<=56; i=i+8){
			for(int j=i+7; j>=i; j--){
				if(shapeCheckedMove[j] == false){
					addTotalShapeMatch(j,1);
				}
			}
		}
		for(int i=0; i<64; i++){
			shapeCheckedMove[i] = false;
			totalShapesMatch[i] = 1;
		}
		
		//cek dari atas ke bawah
		for(int i=56; i>=0; i=i-8){
			for(int j=i; j<=i+7; j++){
				if(shapeCheckedMove[j] == false){
					addTotalShapeMatch(j,1);
				}
			}
		}
		//no moves
		if(isMoveAvailable == false){
			textNoMoves.SetActive(true);
			gameStart = false;
			Invoke("addShapeAfterNoMoves",3f);
		}
	}
	public void addShapeAfterNoMoves(){
		fillBoard();
		if(time > 0)gameStart = true;
		textNoMoves.SetActive(false);
	}
	public void addTotalShapeMatch(int index, int score){
		if(index < 0 || index > 63 || shapeCheckedMove[index] == true){
			return;
		}
		shapeCheckedMove[index] = true;
		totalShapesMatch[index] = score;
		if(score >= 3)isMoveAvailable = true;
		//cek atas
		if(index % 8 != 0){
			if(shapeType[index]==shapeType[index-1]){
				addTotalShapeMatch(index-1,score+1);
			}
		}
		//cek bawah
		if(index != 7 && index != 15 && index != 23 && index != 31 && index != 39 && index != 47 && index != 55 && index != 63){
			if(shapeType[index]==shapeType[index+1]){
				addTotalShapeMatch(index+1,score+1);
			}
		}
		//cek kiri
		if(index > 7){
			if(shapeType[index]==shapeType[index-8]){
				addTotalShapeMatch(index-8,score+1);
			}
		}
		//cek kanan
		if(index < 56){
			if(shapeType[index]==shapeType[index+8]){
				addTotalShapeMatch(index+8,score+1);
			}
		}
	}
	public void resetShape(){
		for(int i=0; i<64; i++){
			imageBoard[i].GetComponent<Image>().color = shapeNormalColor;
			shapeIsSelected[i] = false;
		}
		totalMatch = 0;
		tempShapeSelectedIndex = -1;
	}
	public void checkDestroy(){
		if(totalMatch >= 3 && time > 0){
			totalScoreAdd = 0;
			for(int i=0; i<64; i++){
				if(shapeIsSelected[i] == true){
					shapeType[i] = 0;
					//rumus pertambahan score
					scoreAdd = (50 * totalMatch);
					totalScoreAdd = totalScoreAdd + scoreAdd;
					score = score + scoreAdd;
					totalMatch--;
					imageBoard[i].GetComponent<Image>().color = shapeDestroy;
					shapeIsSelected[i] = false;
				}
			}
			counterTextAddScore = 0;
			textScore.text = "Score: "+score;
			isAnotherFall = 1;
			totalMatch = 0;
			tempShapeSelectedIndex = -1;
		}else{
			resetShape();
		}
	}
	IEnumerator animasiText(){
		while(true){
			if(counterTextAddScore != -1){
				counterTextAddScore++;
				textAddScore.text = "+"+totalScoreAdd;
				if(counterTextAddScore == 15){
					textAddScore.text = "";
					counterTextAddScore = -1;
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
	IEnumerator checkFall(){
		while(true){
			if(isAnotherFall == 1){
				isAnotherFall = 2;
			}
			for(indexKolomCheckFall=0; indexKolomCheckFall<=56; indexKolomCheckFall=indexKolomCheckFall+8){
				isFall = false;
				for(indexBarisCheckFall=indexKolomCheckFall+7; indexBarisCheckFall>=indexKolomCheckFall; indexBarisCheckFall--){
					if(shapeType[indexBarisCheckFall] == 0){
						isAnotherFall = 1;
						isFall = true;
					}
					if(isFall){
						if(shapeType[indexBarisCheckFall] != 0){
							shapeType[indexBarisCheckFall+1] = shapeType[indexBarisCheckFall];
							shapeType[indexBarisCheckFall] = 0;
							imageBoard[indexBarisCheckFall+1].GetComponent<Image>().sprite = imageBoard[indexBarisCheckFall].GetComponent<Image>().sprite;
							imageBoard[indexBarisCheckFall+1].GetComponent<Image>().color = shapeNormalColor;
							imageBoard[indexBarisCheckFall].GetComponent<Image>().color = shapeDestroy;
							break;
						}
					}
					if(indexBarisCheckFall == indexKolomCheckFall){
						if(shapeType[indexBarisCheckFall] == 0){
							addShape(indexBarisCheckFall);
						}
					}
				}
			}
			if(isAnotherFall == 2){
				isAnotherFall = 0;
				checkMoves();
			}
			yield return new WaitForSeconds(0.05f);
		}
	}
	IEnumerator timerGame(){
		while(true){
			if(gameStart){
				if(time > 0){
					time--;
					if(time <= 20){
						textTimer.GetComponent<Text>().color = colorTimeWarning;
					}else{
						textTimer.GetComponent<Text>().color = colorNormal;
					}
					textTimer.text = "Time: "+time;
				}
				if(time == 0){
					gameStart = false;
					GameOver();
				}
			}
			yield return new WaitForSeconds(1f);
		}
	}
	IEnumerator readyAnimation(){
		while(true){
			counterReady++;
			if(counterReady == 1){
				textReady.text = "READY";
			}else if(counterReady == 2){
				textReady.text = "GO";
			}else if(counterReady == 3){
				textReady.gameObject.SetActive(false);
				StartCoroutine(timerGame());
				Menu.SetActive(true);
				gameStart = true;
			}
			yield return new WaitForSeconds(2f);
		}
	}
	// Update is called once per frame
	void Update () {
		if(gameStart == true){
			if(Input.GetMouseButtonDown(0)){
				isClicked = true;
			}
			if(Input.GetMouseButtonUp(0)){
				isClicked = false;
				checkDestroy();
			}
			if(Input.GetMouseButton(0)){
				if(isClicked){
					if(shapeSelectedIndex >= 0 && shapeSelectedIndex <=63){
						//pointer memilih shape pertama
						if(tempShapeSelectedIndex == -1){
							tempShapeSelectedIndex = shapeSelectedIndex;
							imageBoard[shapeSelectedIndex].GetComponent<Image>().color = shapeSelectedColor;
							shapeIsSelected[shapeSelectedIndex] = true;
							totalMatch++;
						}else{
							//pointer memilih shape yang berdekatan
							if(shapeSelectedIndex != tempShapeSelectedIndex){
								if(shapeSelectedIndex == tempShapeSelectedIndex + 1 || shapeSelectedIndex == tempShapeSelectedIndex - 1 ||
									shapeSelectedIndex == tempShapeSelectedIndex + 8 || shapeSelectedIndex == tempShapeSelectedIndex - 8){
									if(shapeType[tempShapeSelectedIndex] == shapeType[shapeSelectedIndex] && shapeIsSelected[shapeSelectedIndex] == false){
										tempShapeSelectedIndex = shapeSelectedIndex;
										imageBoard[shapeSelectedIndex].GetComponent<Image>().color = shapeSelectedColor;
										shapeIsSelected[shapeSelectedIndex] = true;
										totalMatch++;
									}else{
										isClicked = false;
										resetShape();
									}
								}else{
									isClicked = false;
									resetShape();
								}
							}
						}
					}else{
						isClicked = false;
						resetShape();
					}
				}
			}
		}
	}
}

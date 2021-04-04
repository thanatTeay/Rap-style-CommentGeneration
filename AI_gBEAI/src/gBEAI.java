import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Timer;
import java.util.TimerTask;
import java.util.Vector;
import java.util.logging.Logger;

import aiinterface.CommandCenter;
import enumerate.Action;
import enumerate.State;
import aiinterface.AIInterface;
import mcts.AiData;
import mcts.MCTS;
import mcts.Node;
import mcts.Prediction;
import mcts.TheIO;
import parameter.FixParameter;
import simulator.Simulator;
import struct.CharacterData;
import struct.FrameData;
import struct.GameData;
import struct.Key;
import struct.MotionData;
import struct.AttackData;

public class gBEAI implements AIInterface {

	private Simulator simulator;
	private Key key;
	private CommandCenter commandCenter;
	private boolean playerNumber;

	/** à¸£à¸…à¸¢à¸„à¸¢à¸‡à¸£à¸†à¸¥â€œà¸¢à¸Œà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸ŽFrameData */
	private FrameData frameData;

	/** à¸£à¸…à¸¢à¸„à¸¢à¸‡à¸£à¸†à¸¥â€œà¸¢à¸Œà¸£à¸ƒà¹‚â‚¬ï¿½à¸«ï¿½à¸£à¸ƒà¹‚â‚¬ï¿½à¸¥Â FRAME_AHEADà¸£à¸…à¸«ï¿½à¹‚â‚¬Â à¸£à¸‰à¹�à¸Ÿà¸�à¹‚â‚¬à¸†à¸£à¸ƒà¹‚â‚¬ï¿½à¸¥â€™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¥à¸˜FrameData */
	private FrameData simulatorAheadFrameData;

	/** à¸£à¸ˆà¹‚â‚¬à¸�à¸¢à¸Šà¸£à¸…à¸«ï¿½à¹‚â‚¬Â à¸£à¸ƒà¹�à¸Ÿà¸�à¸¥â€™à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸ƒà¹�à¸Ÿà¸�à¸«ï¿½à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸…à¹‚â‚¬à¸†à¸¢à¸ˆà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸† */
	private LinkedList<Action> myActions;

	/** à¸£à¸‡à¹‚â‚¬à¸šà¸¢à¸˜à¸£à¸†à¹‚â‚¬à¸�à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¥â€™à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸ƒà¹�à¸Ÿà¸�à¸«ï¿½à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸…à¹‚â‚¬à¸†à¸¢à¸ˆà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸† */
	private LinkedList<Action> oppActions;

	/** à¸£à¸ˆà¹‚â‚¬à¸�à¸¢à¸Šà¸£à¸…à¸«ï¿½à¹‚â‚¬Â à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸†à¸¦â€™à¹‚â‚¬à¸†à¸£à¸…à¸¢Â à¸¢à¸‘ */
	private CharacterData myCharacter;

	/** à¸£à¸‡à¹‚â‚¬à¸šà¸¢à¸˜à¸£à¸†à¹‚â‚¬à¸�à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸†à¸¦â€™à¹‚â‚¬à¸†à¸£à¸…à¸¢Â à¸¢à¸‘ */
	private CharacterData oppCharacter;

	private Action[] actionAir;

	private Action[] actionGround;

	/** STAND_D_DF_FCà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸…à¹‚â‚¬à¸šà¸¥à¸žà¸£à¸‰à¹�à¸Ÿà¸�à¸¢à¸Ÿà¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸‡à¹‚â‚¬ï¿½à¸¢à¸ˆà¸£à¸ƒà¸¦â€™à¹‚â‚¬à¸‚à¸£à¸ƒà¸¦â€™à¸¢à¸‰à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸� */
	private boolean isFcFirst = true;

	/** à¸£à¸†à¹‚â‚¬à¸‚à¸¢à¸•à¸£à¸ƒà¹�à¸Ÿà¸�à¸¥â€™STAND_D_DF_FCà¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸„à¸¢à¸�à¸¢à¸Ÿà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸ƒà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸†à¸£à¸ƒà¹�à¸Ÿà¸�à¹�à¸Ÿà¸�à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‰à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬Â à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸™ */
	private boolean canFC = true;

	/** STAND_D_DF_FCà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸…à¹‚â‚¬à¸šà¸¥à¸žà¸£à¸‰à¹�à¸Ÿà¸�à¸¢à¸Ÿà¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸†à¹‚ï¿½à¸‚à¹‚â‚¬ï¿½à¸£à¸‰à¹‚â‚¬â€œà¹‚â‚¬ï¿½à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸ˆà¸¢à¸ˆà¸«ï¿½à¸£à¸†à¸¢à¸˜à¸¢à¸Œà¸£à¸ƒà¹�à¸Ÿà¸�à¹‚ï¿½à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™ */
	private long firstFcTime;

	private ArrayList<MotionData> myMotion;

	private ArrayList<MotionData> oppMotion;

	private Action spSkill;

	private Node rootNode;

	private MCTS mcts;
	
	private AttackData myAttackData;
	
	Logger logger;

	//private double beta;

	//private int[] resultHpDiff;

	//private int target = 0;
	
	//TTS text generator	
	int gameState; // initialize game state, 0 start, 1 early game, 2 mid game, 3 near end game, 4 end game, 5 specific mode
	int AIState; // 0 highlight, 1 mcts, 2 harmless
	String textFromAI, previousText, nameC;
	public String textFromCheering;
	String myCurrentMove;	
	String opponentPreviousMove;
	String P1, theWinner;
	String P2;
	int myCurrentMoveDamage;
	int myCurrentMoveDamageMax;
	private LinkedList<String> tempOpponentActionList;	
	boolean isSpeaking;//check if it's speaking
	float balancednessFitness;
	//TTSSkillMap_Male ttsSkillMap_Zen;
	TTSSkillMap_ZEN ttsSkillMap_Zen;
	TTSSkillMap_LUD ttsSkillMap_Lud;
	TTSSkillMap_Common ttsSkillMap_common;
	String opponentActionPath;
	String opponentCurrentAction;
	String opponentPreviousAction;
		//UKI Map
		private Map<String, String> ukiSkillMap;
		private Map<String, Integer> realToUkiMap;	
		int getmyHp,getoppHp;
		int deltaHp;//P1 Hp - P2 Hp
		float pdaEvalMin;
		float pdaEvalMax;
		float distanceMin;	
		float distanceMax;
		float current;
		float actionValue;
		String path1 = "C:\\Users\\maili\\Desktop\\texttospeechAPG\\textfile\\";
		String path2 = "D:\\FTGexp\\F\\";
		boolean cheering,writing;
		int count =0, k=0, p2_gotDamaged=0, p1_gotDamaged=0, round=1, roundP1won = 0, roundP2won = 0;
		String printPlayerWin;
		Timer timer;
	    TimerTask task = new TimerTask() {
	    	public void run() {
	    		count++;
	    		//writing = true;
	    		System.out.println("timing!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
	    		if(count == 5) {
	    			count = 0;
	    		}
	    	}
	    };
	    
	public void start()
	{
		timer = new Timer();
		timer.scheduleAtFixedRate(task, 1000, 1000);
		
	}
	public void stop()
	{
		timer.cancel();
		
	}
		
	private void exportTextFile()
		{
			
			if(cheering)//checkCheering
			{
				try {
					BufferedWriter writer =
	                    new BufferedWriter(new FileWriter(path1+"Cheering.txt"));

					writer.write(textFromCheering);
					System.out.println(textFromCheering);
					System.out.println(count);
					writer.close();          
					cheering = false;
	          
					} catch (IOException e) {
						e.printStackTrace();
						cheering = false;
				}			
			}
		
			if(playerNumber)//P1 action
			{
				try {
			           BufferedWriter writer =
			                  new BufferedWriter(new FileWriter(path1+"P1Comment.txt"));

			           writer.write(textFromAI);
			           System.out.println("Player1: "+textFromAI);
			           System.out.println(count);
			           writer.close();       
			           writing = false;
			           
			          
			       } catch (IOException e) {
			           e.printStackTrace();
			       }
					//Print HP
				
				try {
					BufferedWriter writer =
			                  new BufferedWriter(new FileWriter(path2+"p1HP.txt"));
					
					 writer.write(String.valueOf(getmyHp));
					 writer.close();
			         System.out.println("Player1 HP: "+getmyHp);
				}catch (IOException e){
					e.printStackTrace();
				}
				
				
				
				
				}
			else if(!playerNumber)//P2 action
			{
				try {
			           BufferedWriter writer =
			                   new BufferedWriter(new FileWriter(path1+"P2Comment.txt"));

			           writer.write(textFromAI);
			           System.out.println("Player2: "+textFromAI);
			           System.out.println(count);
			           writer.close();          
			           writing = false;
			          
			       } catch (IOException e) {
			           e.printStackTrace();
			       }
				
				try {
					BufferedWriter writer =
			                  new BufferedWriter(new FileWriter(path2+"p2HP.txt"));
					
					 writer.write(String.valueOf(getmyHp));
			         System.out.println("Player1 HP: "+getmyHp);
			         writer.close();
				}catch (IOException e){
					e.printStackTrace();
				}
			}
				//textFromAI = ttsSkillMap_Zen.generateCheerUpCommentary();
			
			try {
				BufferedWriter writer =
		                  new BufferedWriter(new FileWriter(path2+"roundEnd.txt"));
				
				 writer.write(" ");
				 writer.close();
			}catch (IOException e){
				e.printStackTrace();
			}
				isSpeaking = false;
				count=0;
				
		}
			/*else {
				count++;
				isSpeaking = true;
			}*/
			
			
		
	
	private void getmyCurrentMoveInformation() {
		//TODO
//		if (opponentPreviousMove != ttsSkillMap.getActionRealName(this.frameData.getCharacter(!playerNumber).getAction().name())) {
//			opponentPreviousMove = myCurrentMove;
		//if(playerNumber)	
		//{
			myCurrentMove = TTSSkillMap_Common.getActionRealName(this.frameData.getCharacter(playerNumber).getAction().name());

		//}
		/*else if(!playerNumber) {
			myCurrentMove = TTSSkillMap_Common.getActionRealName(this.frameData.getCharacter(!playerNumber).getAction().name());

		}*/
			
			//Ishii CoG 2019
//			if (myCurrentMove == "Ultimate Hadouken"){
//				actionValue = 1.0f;
//			} else if (myCurrentMove == "Super Uppercut"){
//				actionValue = 0.5f;
//			} else if (myCurrentMove == "Slide Kick"){
//				actionValue = 0.25f;
//			} else if (myCurrentMove == "Super Hadouken"){
//				actionValue = 0.125f;		
//			} else {
//				actionValue = 0.0f;
//			}

			if (isSpeaking == true) {
				if (!myCurrentMove.contains("Default"))
				{		
					tempOpponentActionList.add(myCurrentMove);
					myCurrentMoveDamage = myMotion.get(this.frameData.getCharacter(playerNumber).getAction().ordinal()).getAttackHitDamage();
					if (myCurrentMoveDamage > myCurrentMoveDamageMax) 
					{
						myCurrentMoveDamageMax = myCurrentMoveDamage;
						
					}
				
				}
			} else {
				if (tempOpponentActionList.size() != 0) {
					myCurrentMove = tempOpponentActionList.get(TTSSkillMap_Common.getRandomNumber(tempOpponentActionList.size()));
					
//					current = (float) myCurrentMoveDamageMax / (float) myMotion.get(Action.STAND_D_DF_FC.ordinal()).getAttackHitDamage();
//					//System.out.println("Max=" + (float) myCurrentMoveDamageMax + "current" + (float) myMotion.get(Action.STAND_D_DF_FC.ordinal()).getAttackHitDamage());
					myCurrentMoveDamage = myMotion.get(Action.STAND_D_DF_FC.ordinal()).getAttackHitDamage() / myMotion.get(Action.STAND_D_DF_FC.ordinal()).getAttackHitDamage();
					tempOpponentActionList.clear();
					myCurrentMoveDamageMax = 0;
				}
			}			
		}

	@Override
	public void close() {
		// TODO à¸£à¸ˆà¹‚â‚¬à¸�à¸¢à¸Šà¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸‡à¹‚â‚¬ï¿½à¸¥à¸˜à¸£à¸†à¸«ï¿½à¹�à¸Ÿà¸�à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¸¥â€™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¥à¸˜à¸£à¸ƒà¸¦â€™à¸¢à¸�à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸�à¸£à¸ƒà¸¦â€™à¸¦â€™à¸£à¸ƒà¸¦â€™à¹‚â‚¬à¸�à¸£à¸ƒà¸¦â€™à¸¢à¸›à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸™à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸Ÿà¸£à¸ƒà¸¦â€™à¹‚â‚¬â€œ
		textFromAI = TTSSkillMap_Common.generateEndCommentary();
		
		if(roundP1won > roundP2won)
		{
			 P1 = "WIN";
			 P2 = "LOSE";
			 theWinner = "The winner of this game is Zen";
		}
		else if(roundP1won == roundP2won)
		{
			 P1 = "DRAW";
			 P2 = "DRAW";
			 theWinner = "Draw";

		}
		else if(roundP1won < roundP2won)
		{
			 P1 = "LOSE";
			 P2 = "WIN";
			 theWinner = "The winner of this game is Lud";
		}
		//System.out.println("end");
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"TotalP1Win.txt"));
			
			 writer.write(String.valueOf(P1+"\n"+roundP1won));
			 writer.close();
		}catch (IOException e){
			e.printStackTrace();
		}
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"TotalP2Win.txt"));
			
			 writer.write(String.valueOf(P2+"\n"+roundP2won));
			 writer.close();
		}catch (IOException e){
			e.printStackTrace();
		}
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"p1HP.txt"));
			
			 writer.write(String.valueOf("0"));
			 writer.close();
		}catch (IOException e){
			e.printStackTrace();
		}
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"p2HP.txt"));
			
			 writer.write(String.valueOf("0"));
			 writer.close();
		}catch (IOException e){
			e.printStackTrace();
		}
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"theWinner.txt"));
			
			 writer.write(theWinner);
			 writer.close();
		}catch (IOException e){
			e.printStackTrace();
		}
		
		
		System.out.println(textFromAI);
		stop();
		//count = 200;
		exportTextFile();

	}

	@Override
	public void getInformation(FrameData frameData, boolean isControl) {
		this.frameData = frameData;
		this.commandCenter.setFrameData(this.frameData, playerNumber);
		this.myCharacter = this.frameData.getCharacter(playerNumber);
		this.oppCharacter = this.frameData.getCharacter(!playerNumber);
	}

	@Override
	public int initialize(GameData gameData, boolean playerNumber) {
		setupBesfAI(playerNumber);
		//--------------
		this.playerNumber = playerNumber;
		
		TTSSkillMap_Common.prepareComment(playerNumber,"Zen","Lud");
		
		
		this.key = new Key();
		this.frameData = new FrameData();
		this.commandCenter = new CommandCenter();

		this.myActions = new LinkedList<Action>();
		this.oppActions = new LinkedList<Action>();

		this.simulator = gameData.getSimulator();
		this.myMotion = gameData.getMotionData(playerNumber);
//		this.myMotion = gameData.getMyMotion(playerNumber);
		this.oppMotion = gameData.getMotionData(!playerNumber);
//		this.oppMotion = gameData.getOpponentMotion(playerNumber);
		this.opponentPreviousMove = "Default";
		this.tempOpponentActionList = new LinkedList<String>();
	/*	this.beta = 0;
		this.resultHpDiff = new int[3];
		Arrays.fill(resultHpDiff, 0);*/

		// à¸£à¸…à¹�à¸Ÿà¸�à¹‚â‚¬ï¿½à¸£à¸‡à¸¢à¸ˆà¸¢à¸Žà¸£à¸‰à¸¢Â à¹‚â‚¬à¸†à¸£à¸‡à¹‚â‚¬à¸šà¸¢à¸Žà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸…à¸«ï¿½à¹�à¸Ÿà¸�à¸£à¸†à¸¥â€œà¸¥à¸˜à¸£à¸…à¸¥â€™à¹‚â‚¬â€œ
		setPerformAction();
		count=0;
		getmyHp = 0;
		getoppHp = 0;
		deltaHp = 0;
		roundP1won = 0;
		roundP2won = 0;
		start();
		isSpeaking = false;
		cheering = false;
		ttsSkillMap_Lud = new TTSSkillMap_LUD();
		ttsSkillMap_Zen = new TTSSkillMap_ZEN();
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"roundEnd.txt"));
			
			 writer.write(" ");
			 writer.close(); 
	         
		}catch (IOException e){
			e.printStackTrace();
		}
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"TotalP1Win.txt"));
			
			 writer.write(" ");
			 writer.close(); 
	         
		}catch (IOException e){
			e.printStackTrace();
		}
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"TotalP2Win.txt"));
			
			 writer.write(" ");
			 writer.close(); 
	         
		}catch (IOException e){
			e.printStackTrace();
		}
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"theWinner.txt"));
			
			 writer.write(" ");
			 writer.close();
		}catch (IOException e){
			e.printStackTrace();
		}
		textFromAI = TTSSkillMap_Common.generateBeginCommentary();
		exportTextFile() ;
		
		return 0;
	}

	@Override
	public void processing() {

		
		if (canProcessing()) {
			 getmyHp = myCharacter.getHp();
			 getoppHp = oppCharacter.getHp();
			 
			//System.out.print("Process!!!!!!");
			getmyCurrentMoveInformation();
			if(count == 3)
			{
				if(playerNumber)
				{
					cheering = true;
				}
				
//				if(playerNumber) // P1 condition
//				{
//					checkHp = getmyHp - getoppHp;
//					getDamage1 = myMotion.get(this.frameData.getCharacter(playerNumber).getAction().ordinal()).getAttackHitDamage();
//					getDamage2 = myMotion.get(this.frameData.getCharacter(!playerNumber).getAction().ordinal()).getAttackHitDamage();
//					if(checkHp < -60)
//					{
//						textFromCheering = ttsSkillMap_Zen.generateCheerUpCommentaryLose();
//					}else if(checkHp > 60) {
//						textFromCheering = ttsSkillMap_Zen.generateCheerUpCommentaryWin();
//					}else {
//						textFromCheering = ttsSkillMap_Zen.generateCheerUpCommentaryDraw();
//					}
//					
//					cheering = true;		
//					
//					
//					//System.out.println("getDamage1 = "+getDamage1);
//					//System.out.println("getDamage2 = "+getDamage2);
//					textFromAI = ttsSkillMap_Zen.generateNormalCommentary(myCurrentMove);	
//
//					
//				}
//				else if(!playerNumber)
//				{
//					checkHp = getmyHp - getoppHp;
//					getDamage1 = myMotion.get(this.frameData.getCharacter(playerNumber).getAction().ordinal()).getAttackHitDamage();
//					getDamage2 = myMotion.get(this.frameData.getCharacter(!playerNumber).getAction().ordinal()).getAttackHitDamage();
//					
//					textFromAI = ttsSkillMap_Lud.generateNormalCommentary(myCurrentMove);	
//
//				}
				
				
				genComment();
				
				exportTextFile() ;
				
				
			}
			
			/*else if(count == 2){
				isSpeaking = true;
			}
			else {
				isSpeaking = false;
			}*/
			
			
			// à¸£à¸ƒà¸¦â€™à¹‚â‚¬à¸‚à¸£à¸ƒà¸¦â€™à¸¢à¸‰à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸�à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‹à¸£à¸ƒà¹‚â‚¬ï¿½à¸«ï¿½à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸ƒà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸†à¸£à¸„à¸¢à¸šà¸«ï¿½à¸£à¸†à¸¢à¸˜à¸¢à¸Œà¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚ï¿½à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸™à¸£à¸‰à¹�à¸Ÿà¸�à¸¢à¸˜à¸£à¸†à¸¥Â à¸¥à¸ž
			if (FixParameter.PREDICT_FLAG) {
				if (oppMotion.get(oppCharacter.getAction().ordinal()).getFrameNumber() == oppCharacter
						.getRemainingFrame()) {
					Prediction.getInstance().countOppAction(this.frameData,oppCharacter, commandCenter);
				}
			}

			if (commandCenter.getSkillFlag()) {
				key = commandCenter.getSkillKey();
			} else {
				key.empty();
				commandCenter.skillCancel();

				aheadFrame(); // à¸£à¸‰à¹�à¸Ÿà¸�à¹‚â‚¬à¸†à¸£à¸ƒà¹‚â‚¬ï¿½à¸¥â€™à¸£à¸ƒà¸¦â€™à¹‚â‚¬à¸‚à¸£à¸ƒà¸¦â€™à¸¢à¸Œà¸£à¸ƒà¸¦â€™à¸¢à¸œà¸£à¸ƒà¸¦â€™à¸¢Â à¸£à¸…à¸«ï¿½à¹‚â‚¬Â à¸£à¸‰à¹‚ï¿½à¸Œà¸¢à¸’à¸£à¸ƒà¹‚â‚¬ï¿½à¹�à¸Ÿà¸�à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™

				// à¸£à¸ƒà¸¦â€™à¹‚â‚¬à¸‚à¸£à¸ƒà¸¦â€™à¸¢à¸‰à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸�à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‹à¸£à¸ƒà¹‚â‚¬ï¿½à¸«ï¿½à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸ƒà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸†à¸£à¸…à¹‚â‚¬à¸šà¸¥à¸žà¸£à¸‰à¹�à¸Ÿà¸�à¸¢à¸Ÿà¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚ï¿½à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‰à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬Â à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸™à¸£à¸‰à¹�à¸Ÿà¸�à¸¢à¸˜à¸£à¸†à¸¥Â à¸¥à¸ž
				if (FixParameter.AVOID_FLAG) {
					String enemyAction = this.frameData.getCharacter(!playerNumber).getAction().name();
					int enemyEnergy = this.frameData.getCharacter(!playerNumber).getEnergy();

					if (enemyAction.equals("STAND_D_DF_FC")) {
						canFC = true;
						isFcFirst = true;
					}

					if (enemyEnergy >= 150 && canFC) {
						if (isFcFirst) {
							firstFcTime = frameData.getRemainingTime();
							isFcFirst = false;
						}
						if (firstFcTime - frameData.getRemainingTime() >= FixParameter.AVOIDANCE_TIME) {
							canFC = false;
							isFcFirst = true;
						} else {
							commandCenter.commandCall("STAND_D_DB_BA");
							return;
						}
					}
				}

				if (FixParameter.PREDICT_FLAG) {
					Prediction.getInstance().getInfomation(); // à¸£à¸…à¹‚â‚¬à¸šà¸¥à¸žà¸£à¸†à¹‚â‚¬à¸‚à¸¢à¸�à¸£à¸‰à¸¢Â à¹‚â‚¬Â à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‡à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸�à¸£à¸ƒà¸¦â€™à¸¢à¸œà¸£à¸ƒà¸¦â€™à¸«ï¿½
				}

				// MCTSà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‹à¸£à¸ƒà¹‚â‚¬ï¿½à¸«ï¿½à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸†à¸¢à¸‘à¸¢à¸šà¸£à¸…à¸¢à¸Žà¸¥à¸�
				Action bestAction = Action.STAND_D_DB_BA;
				mctsPrepare(); // MCTSà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸„à¸¢à¸˜à¹‚â‚¬à¸™à¸£à¸†à¸¢à¸šà¹‚â‚¬â€œà¸£à¸…à¹‚â‚¬ï¿½à¹‚ï¿½à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬Â 

				bestAction = mcts.runMcts(); // MCTSà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸…à¸¢à¸Žà¸¥à¸˜à¸£à¸ˆà¸¢à¸�à¸¥â€™
				commandCenter.commandCall(bestAction.name()); // MCTSà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‡à¸£à¸‰à¹�à¸Ÿà¸�à¸¢à¸˜à¸£à¸†à¸¥Â à¸¥à¸žà¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¸¥â€™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¥à¸˜à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸…à¸¢à¸Žà¸¥à¸˜à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚ï¿½à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™

				if (FixParameter.DEBUG_MODE) {
					mcts.printNode(rootNode);
				}
			}
		} else {
			canFC = true;
			isFcFirst = true;
		}
	}
	

	public void genComment() {
		String comment = "";
		boolean isP1 = playerNumber;
		TTSSkillMap_Common.isP1 = playerNumber;
//		System.out.println(TTSSkillMap_Common.isP1 + "ssssss");
		TTSSkillMap_Common.beai = this;
		deltaHp = getmyHp - getoppHp;
		p2_gotDamaged = myMotion.get(this.frameData.getCharacter(isP1).getAction().ordinal()).getAttackHitDamage();
		p1_gotDamaged = myMotion.get(this.frameData.getCharacter(!isP1).getAction().ordinal()).getAttackHitDamage();

		TTSSkillMap_Common.setCharacter("KKK", "MMM");
		
		if(playerNumber) // P1 condition
		{
			if(deltaHp > 60) {
				textFromCheering = TTSSkillMap_Common.generateCheerUpCommentaryWin();
			}
			else if(deltaHp < -60)
			{
				textFromCheering = TTSSkillMap_Common.generateCheerUpCommentaryLose();
			}else {
				textFromCheering = TTSSkillMap_Common.generateCheerUpCommentaryDraw();
			}	
			textFromAI = TTSSkillMap_Common.generateNormalCommentary(myCurrentMove);	
		}
		else 
		{
			textFromAI = TTSSkillMap_Common.generateNormalCommentary(myCurrentMove);	
		}
		//System.out.println("getDamage1 = "+getDamage1);
		//System.out.println("getDamage2 = "+getDamage2);
		
		
	}
	
	
	
//
//	public String genComment() {
//		String comment = "";
//		Boolean isP1 = playerNumber;
//		deltaHp = getmyHp - getoppHp;
//		p2_gotDamaged = myMotion.get(this.frameData.getCharacter(isP1).getAction().ordinal()).getAttackHitDamage();
//		p1_gotDamaged = myMotion.get(this.frameData.getCharacter(!isP1).getAction().ordinal()).getAttackHitDamage();
//		if(isP1) // P1 condition
//		{
//			if(deltaHp > 60) {
//				comment = ttsSkillMap_Zen.generateCheerUpCommentaryWin();
//			}
//			else if(deltaHp < -60)
//			{
//				comment = ttsSkillMap_Zen.generateCheerUpCommentaryLose();
//			}else {
//				comment = ttsSkillMap_Zen.generateCheerUpCommentaryDraw();
//			}	
//			comment = ttsSkillMap_Zen.generateNormalCommentary(myCurrentMove);	
//		}
//		else if(!isP1)
//		{
//			
//			comment = ttsSkillMap_Lud.generateNormalCommentary(myCurrentMove);	
//		}
//		//System.out.println("getDamage1 = "+getDamage1);
//		//System.out.println("getDamage2 = "+getDamage2);
//		return comment;
//		
//	}

	@Override
	public void roundEnd(int x, int y, int frame) {
		
		/*this.resultHpDiff[frameData.getRound()] = this.playerNumber ? x - y : y - x;

		double targetHP = 0;
		int i = 0;
		for (i = 0; i <= frameData.getRound() && frameData.getRound() != 2; i++) {
			targetHP += resultHpDiff[i];
		}

		if (frameData.getRound() != 2) {
			this.beta = ((double) targetHP / i) / FixParameter.TANH_SCALE;

			//System.out.println("target: " + targetHP + " beta " + beta);
		}*/

		/*if (frameData.getRound() != 2) {
			this.beta = ((double) target) / FixParameter.TANH_SCALE;

			//System.out.println("target: " + target + " beta " + beta);
		}*/
		deltaHp = x - y;
		if(deltaHp > 0)
		{
			printPlayerWin = "ROUND "+(round+1)+" \n P1's won in Round "+round;
			round++;
			roundP1won++;
		}
		if(deltaHp < 0)
		{
			printPlayerWin = "ROUND "+(round+1)+" \n P2's won in Round "+round;
			round++;
			roundP2won++;
		}
		try {
			BufferedWriter writer =
	                  new BufferedWriter(new FileWriter(path2+"roundEnd.txt"));
			
			 writer.write(printPlayerWin);
			 writer.close(); 
	         
		}catch (IOException e){
			e.printStackTrace();
		}
		count = 0;
		if(logData) {exportData();}
	}

	/**
	 * MCTSà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸„à¸¢à¸˜à¹‚â‚¬à¸™à¸£à¸†à¸¢à¸šà¹‚â‚¬â€œà¸£à¸…à¹‚â‚¬ï¿½à¹‚ï¿½à¸‚ <br>
	 * à¸£à¸‰à¹�à¸Ÿà¸�à¹‚â‚¬à¸†à¸£à¸ƒà¹‚â‚¬ï¿½à¸¥â€™à¸£à¸ƒà¸¦â€™à¹‚â‚¬à¸‚à¸£à¸ƒà¸¦â€™à¸¢à¸Œà¸£à¸ƒà¸¦â€™à¸¢à¸œà¸£à¸ƒà¸¦â€™à¸¢Â à¸£à¸…à¸«ï¿½à¹‚â‚¬Â à¸£à¸‰à¹‚ï¿½à¸Œà¸¢à¸’à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸žà¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸šà¸£à¸ƒà¹�à¸Ÿà¸�à¸¥à¸˜FrameDataà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸…à¹�à¸Ÿà¸�à¹‚â‚¬â€œà¸£à¸…à¸¢à¸žà¹‚â‚¬â€�à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Šà¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‰à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬Â 
	 */
	public void mctsPrepare() {
		setMyAction();
		setOppAction();
		rootNode = new Node(null);
		mcts = new MCTS(rootNode, simulatorAheadFrameData, simulator, myCharacter.getHp(), oppCharacter.getHp(),
				myActions, oppActions, playerNumber, filePath);
		mcts.createNode(rootNode);
		if(logData) {logData();}
	}

	/** à¸£à¸ˆà¹‚â‚¬à¸�à¸¢à¸Šà¸£à¸ˆà¸¢à¸šà¸¢à¸‹à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸…à¹�à¸Ÿà¸�à¸¢à¸�à¸£à¸ˆà¸¦â€™à¸¢à¸�à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Šà¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸›à¸£à¸ƒà¸¦â€™à¸¦â€™à¸£à¸ƒà¸¦â€™à¸«ï¿½à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚ï¿½à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™ */
	public void setMyAction() {
		myActions.clear();

		int energy = myCharacter.getEnergy();

		if (myCharacter.getState() == State.AIR) {
			for (int i = 0; i < actionAir.length; i++) {
				if (Math.abs(myMotion.get(Action.valueOf(actionAir[i].name()).ordinal())
						.getAttackStartAddEnergy()) <= energy) {
					myActions.add(actionAir[i]);
				}
			}
		} else {
			if (ultimateOK &&
					Math.abs(
					myMotion.get(Action.valueOf(spSkill.name()).ordinal()).getAttackStartAddEnergy()) <= energy) {
				myActions.add(spSkill);
			}

			for (int i = 0; i < actionGround.length; i++) {
				if (Math.abs(myMotion.get(Action.valueOf(actionGround[i].name()).ordinal())
						.getAttackStartAddEnergy()) <= energy) {
					myActions.add(actionGround[i]);
				}
			}
		}

	}

	/** à¸£à¸‡à¹‚â‚¬à¸šà¸¢à¸˜à¸£à¸†à¹‚â‚¬à¸�à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸…à¹�à¸Ÿà¸�à¸¢à¸�à¸£à¸ˆà¸¦â€™à¸¢à¸�à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Šà¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸›à¸£à¸ƒà¸¦â€™à¸¦â€™à¸£à¸ƒà¸¦â€™à¸«ï¿½à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚ï¿½à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™ */
	public void setOppAction() {
		oppActions.clear();

		int energy = oppCharacter.getEnergy();

		if (oppCharacter.getState() == State.AIR) {
			for (int i = 0; i < actionAir.length; i++) {
				if (Math.abs(oppMotion.get(Action.valueOf(actionAir[i].name()).ordinal())
						.getAttackStartAddEnergy()) <= energy) {
					oppActions.add(actionAir[i]);
				}
			}
		} else {
			if (Math.abs(oppMotion.get(Action.valueOf(spSkill.name()).ordinal())
					.getAttackStartAddEnergy()) <= energy) {
				oppActions.add(spSkill);
			}

			for (int i = 0; i < actionGround.length; i++) {
				if (Math.abs(oppMotion.get(Action.valueOf(actionGround[i].name()).ordinal())
						.getAttackStartAddEnergy()) <= energy) {
					oppActions.add(actionGround[i]);
				}
			}
		}
	}

	/** à¸£à¸‰à¹�à¸Ÿà¸�à¹‚â‚¬à¸†à¸£à¸ƒà¹‚â‚¬ï¿½à¸¥â€™à¸£à¸ƒà¸¦â€™à¹‚â‚¬à¸‚à¸£à¸ƒà¸¦â€™à¸¢à¸Œà¸£à¸ƒà¸¦â€™à¸¢à¸œà¸£à¸ƒà¸¦â€™à¸¢Â à¸£à¸…à¸«ï¿½à¹‚â‚¬Â à¸£à¸‰à¹‚ï¿½à¸Œà¸¢à¸’à¸£à¸ƒà¹‚â‚¬ï¿½à¹�à¸Ÿà¸�à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™ */
	private void aheadFrame() {
		simulatorAheadFrameData = simulator.simulate(this.frameData, playerNumber, null, null,
				FixParameter.FRAME_AHEAD);
		myCharacter = simulatorAheadFrameData.getCharacter(playerNumber);
		oppCharacter = simulatorAheadFrameData.getCharacter(!playerNumber);
	}

	/** à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸�à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸—à¸£à¸ƒà¸¦â€™à¸¢à¸‡à¸£à¸ƒà¸¦â€™à¸¢à¸“à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸‰à¹‚â‚¬à¸†à¹�à¸Ÿà¸�à¸£à¸…à¸«ï¿½à¹‚â‚¬â€�à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸Žà¸£à¸…à¸«ï¿½à¹�à¸Ÿà¸�à¸£à¸†à¸¥â€œà¸¥à¸˜à¸£à¸…à¸¥â€™à¹‚â‚¬â€œ */
	private void setPerformAction() {
		actionAir = new Action[] { Action.AIR_GUARD, Action.AIR_A, Action.AIR_B, Action.AIR_DA, Action.AIR_DB,
				Action.AIR_FA, Action.AIR_FB, Action.AIR_UA, Action.AIR_UB, Action.AIR_D_DF_FA, Action.AIR_D_DF_FB,
				Action.AIR_F_D_DFA, Action.AIR_F_D_DFB, Action.AIR_D_DB_BA, Action.AIR_D_DB_BB };
		actionGround = new Action[] { Action.STAND_D_DB_BA, Action.BACK_STEP, Action.FORWARD_WALK, Action.DASH,
				Action.JUMP, Action.FOR_JUMP, Action.BACK_JUMP, Action.STAND_GUARD, Action.CROUCH_GUARD, Action.THROW_A,
				Action.THROW_B, Action.STAND_A, Action.STAND_B, Action.CROUCH_A, Action.CROUCH_B, Action.STAND_FA,
				Action.STAND_FB, Action.CROUCH_FA, Action.CROUCH_FB, Action.STAND_D_DF_FA, Action.STAND_D_DF_FB,
				Action.STAND_F_D_DFA, Action.STAND_F_D_DFB, Action.STAND_D_DB_BB };
		spSkill = Action.STAND_D_DF_FC;
	}

	/**
	 * AIà¸£à¸ƒà¹�à¸Ÿà¸�à¸¥â€™à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‡à¸£à¸ƒà¹�à¸Ÿà¸�à¹�à¸Ÿà¸�à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‰à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬Â à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸™à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬ï¿½à¸£à¸…à¸«ï¿½à¸¢à¸„à¸£à¸…à¸«ï¿½à¸¢à¸…à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚ï¿½à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™
	 *
	 * @return AIà¸£à¸ƒà¹�à¸Ÿà¸�à¸¥â€™à¸£à¸ˆà¸¢à¸�à¸¥â€™à¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‡à¸£à¸ƒà¹�à¸Ÿà¸�à¹�à¸Ÿà¸�à¸£à¸ƒà¹‚â‚¬ï¿½à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¢à¸‰à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬Â à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸™
	 */
	public boolean canProcessing() {
		return !frameData.getEmptyFlag() && frameData.getRemainingTime() > 0;
	}

	@Override
	public Key input() {
		// TODO à¸£à¸ˆà¹‚â‚¬à¸�à¸¢à¸Šà¸£à¸…à¹‚â‚¬à¸™à¹‚â‚¬à¸‚à¸£à¸‡à¹‚â‚¬ï¿½à¸¥à¸˜à¸£à¸†à¸«ï¿½à¹�à¸Ÿà¸�à¸£à¸ƒà¹�à¸Ÿà¸�à¹‚â‚¬à¸‚à¸£à¸ƒà¹‚â‚¬ï¿½à¸¥â€™à¸£à¸ƒà¹�à¸Ÿà¸�à¸¥à¸˜à¸£à¸ƒà¸¦â€™à¸¢à¸�à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸�à¸£à¸ƒà¸¦â€™à¸¦â€™à¸£à¸ƒà¸¦â€™à¹‚â‚¬à¸�à¸£à¸ƒà¸¦â€™à¸¢à¸›à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸™à¸£à¸ƒà¹‚â‚¬ï¿½à¸¢à¸Ÿà¸£à¸ƒà¸¦â€™à¹‚â‚¬â€œ
		return key;
	}


	//BesfAI============================================
	Boolean testMODE = false;
	Boolean ultimateOK = true;
	Boolean logData = false;
	
	public String filePath = "";
	public String dataPath = "data/aiData/gBEAI/";
	
	public void setupBesfAI(boolean playerNumber) {
		if(!testMODE) {
			//use file paths that match Sound Capture (specified in path_.txt)
			try {
				System.out.println("Load Setting"); 
				if(playerNumber) { filePath = TheIO.readFile(dataPath + "path1.txt"); }
				else { filePath = TheIO.readFile(dataPath + "path2.txt"); }
				System.out.print("FILE: " + filePath);
			}
			catch(Exception ex) {
				System.out.print("File Path: " + ex.toString()); 
			}
		}
		else {
			String p = dataPath + "P1.txt";
			String p2 = dataPath + "P2.txt";
			if(playerNumber) { filePath = p; }
			else { filePath = p2; }
		}
	}


	//=================
	List<AiData> list_data = new ArrayList<AiData>();
	void logData() {
		int t = mcts.getTimeLeftSecond();
		if(firstRecord) {
			AiData d  = mcts.getData();
			list_data.add(d);
			firstRecord = false;
		}
		else if(lastRecord) {
			AiData d  = mcts.getData();
			list_data.remove(list_data.size() - 1);
			list_data.add(d);
		}
		else if(t <= nextData) {
			AiData d  = mcts.getData();
			list_data.add(d);
			nextData -= interval;
			if(nextData <= 0) {
				lastRecord = true;
			}
		}
//		System.out.println("log " + list_data.size()); 
	}
	
	int nextData = 59400;
	int interval = 600;
	Boolean lastRecord = false;
	Boolean firstRecord = true;
		
	void exportData(){
		try {
			SimpleDateFormat formatter = new SimpleDateFormat("yyyyMMdd HHmmss");  
		    Date date = new Date();  
		    String filename = formatter.format(date);
		    if(playerNumber) { filename += " P1"; }
		    else { filename += " P2"; }
		    filename += ".csv";
			String path = "data/aiData/gBEAI/log/" + filename;
			PrintWriter writer = new PrintWriter(path, "UTF-8");
			//
			//writer.println("time,hp_my,hp_opp,alpha,dB");
			for(AiData d : list_data){
				String s = "";
				s += d.time + "," + d.myHP + "," + d.oppHP + "," + d.f + "," + d.dB;
				//s += d.myHP + "," + d.oppHP;
				writer.println(s);
			}
			writer.close();
			System.out.print("Export " + filename); 
		}
		catch(Exception ex){
			System.out.print(ex.toString()); 
		}
	}
	
}
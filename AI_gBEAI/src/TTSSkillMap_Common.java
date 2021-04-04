
import java.util.HashMap;
import java.util.Map;
import java.util.Random;

import dataloader.BalFitnessDataLoader;

public class  TTSSkillMap_Common {
	static Map<String , String > skillMap = new HashMap<String , String >(){{
		
		put("STAND_D_DB_BA", "Flying crop");
//		put("BACK_STEP", "Back step");
//		put("FORWARD_WALK", "Step forward");
		put("DASH", "Lean forward");
		put("STAND_GUARD", "Guard");
		put("CROUCH_GUARD", "Guard");
		put("THROW_A", "Throw");
		put("THROW_B", "Great Throw");
		put("STAND_A", "Punch");
		put("STAND_B", "Kick");
		put("CROUCH_A", "Low Punch");
		put("CROUCH_B", "Low Kick");
		put("STAND_FA", "Heavy Punch");
		put("STAND_FB", "Heavy Kick");
		put("CROUCH_FA", "Low Heavy Punch");
		put("CROUCH_FB", "Low Heavy Kick");
		put("STAND_D_DF_FA", "Hadouken");
		put("STAND_D_DF_FB", "Super Hadouken");	
		put("STAND_F_D_DFA", "Uppercut");
		put("STAND_F_D_DFB", "Super Uppercut");
		put("STAND_D_DB_BB", "Slide Kick");
		put("STAND_D_DF_FC", "Ultimate Hadouken");	
	}};
	
	//  extracted Commentary
	
	static public gBEAI beai;
	
	static public boolean isP1 = true;
	static public String  myName = "Player1";
	static public String  oppName = "Player2";

	static void setCharacter(String myName0, String OppName0) {
		myName = myName0;
		oppName = OppName0;
	}
	
	
	static String getMyName(){
		return myName;
	}
	
	static String getOppName(){
		return oppName;
	}
	
	static String beginCommentary[] = {
	};
	
	static String beginCommentaryCn[] = {			
		};
	
	static String endCommentary[] = {
	};
	
	static String generalCommentary[] = {
	};
	
	static String forwardActionInstruction[] = {
	};
	
	static String actionInstruction[] = {};
	static String actionForwardPositiveCommentary[] = {
			};
	
	static String actionForwardNegativeCommentary[] = {	};
				
	static String actionBackwardPositiveCommentary[] = {			};
	
	static String actionBackwardNegativeCommentary[] = {
	};
	
	static String healthForwardQuestion[] = {};
	static String healthBackwardQuestion[] = {};	
	static String healthPositiveCommentary[] = {};	
	static String healthNegativeCommentary[] = {};	
	static String cheerUpCommentaryWin[] = {};
	static String cheerUpCommentaryLose[] = {};
	static String cheerUpCommentarySame[] = {};
	
	
	static String[] replaceName(String[] sinput) {
		String[] soutput = new String[sinput.length];
		for(int i = 0; i < sinput.length; i++) {
			soutput[i] = sinput[i].replace("PLAYER1", myName).replace("PLAYER2", oppName);
			
		}
		return soutput;
	}
	
	static void prepareComment(Boolean isP10, String myName0, String OppName0) {
		isP1 = isP10;
		if(isP1){ setCharacter("Zen","Lud"); }
		else { setCharacter("Lud","Zen"); }
		

		beginCommentary = replaceName(new String[] {
				"Hi everybody, welcome to the match!",
				"Alright, welcome everybody!",
				"Welcome! Let's see a match between PLAYER1 and PLAYER2",
				"Welcome to the match",
				"Welcome to PLAYER1 versus PLAYER2",
		});
		endCommentary = replaceName(new String[] {
				"It is a nice match",
				"It is a wonderful match",
				"See you again in the next match",
				"Thank you for watching",
		});
		
		generalCommentary = replaceName(new String[] {
				"They are trying to attack each other",
				"It would be a match of the century!",
				"I have never seen it before",
				"So attention to these two players",
				"They try to play slowly in this match",
				"Let's keep attention to these players",
				"They are trying to predict each other",
				"Haven't really seen this kind of challenging game so far",
		});
		healthForwardQuestion = replaceName(new String[] {
				"There's a button there,",
				"It's good to try to employ",
				"It seems possible to",
		});		
		healthBackwardQuestion = replaceName(new String[] {
				", that PLAYER1 can rely on being pretty safe to hit",
				" to get PLAYER2 punished",
				", and knock PLAYER2 down",
		});
		healthPositiveCommentary = replaceName(new String[] {
				"Yeah, PLAYER1 successfully punished PLAYER2!",
				"Oh, I can't believe it! Such a pretty move!",
				"Wow, Nice!",
				"Hoo, nice timing!",
				"That's fantastic!",
				"Oh, that worked!",
				"Excellent!",
				"Wonderful!",
				"Good game",
				"Nice shot",
				"Nice move",
				"Good",
				"That shot!!",
				"Those moves are nice",
				"PLAYER1 moves're great!!",
		});
		healthNegativeCommentary = replaceName(new String[] {
				"PLAYER1 is at a dangerous point",
				"PLAYER1 likes being in that range ",
				"Hoo, what a bad timing,",
				"PLAYER1 could have been knocked down",
				"What is PLAYER1 doing!!",
				"That's a bad move",
				"Oh, no. Not that",
				"That's not a good idea",
		});
		cheerUpCommentaryWin = replaceName(new String[] {
				"Go Go Go, PLAYER1, Go Go!!!",
				"We want more! We want more!",
				"Go go go",
				"PLAYER1 gonna win",
				"Attack! Attack!",
		});
		cheerUpCommentaryLose = replaceName(new String[] {
				"Don't give up PLAYER1. Try again",
				"Don't give up! Come back PLAYER1",
				"Fight PLAYER1 Fight!!",
				"You have not been defeated yet, do not give up, just fight it",
				"C'mon, defense",
		});
		cheerUpCommentarySame = replaceName(new String[] {
				"They are the stars!",
				"We want more! We want more!",
				"Attack! Attack!",
		});
			actionForwardPositiveCommentary = replaceName(new String[] {
					"PLAYER1 used",
					"For an opportunity, PLAYER1 used",
					"PLAYER1 continues to use",
					"Nice time to use",
					"That's common to use",
					"That's very nice to use",
					"PLAYER1 knew when to use",
					"PLAYER1 is pressing his opponent by using",
					"Hit PLAYER2 by ",
					"For a chance, PLAYER1 used",
					"How skillfully, PLAYER1 used",
					"PLAYER1 released a powerful ",
			});
			
			actionBackwardPositiveCommentary = replaceName(new String[] {
					". Excellent for that",
					". That is a good move!",
					". Wow, that happened really quick",
					", really a good one",
					" that PLAYER2 should be very careful",
					". That'll be a great deal",
					". Some damage here",
					" that should tell PLAYER2 to quit this game",
					". It's a good deal of damage",
					". That's a nice shot",
					". That's perfect",
					". PLAYER2 gotta be punished!",
					". PLAYER2 will lose for sure!",
					". Nice!",
			});

			
			
			
			
//			
//			actionForwardNegativeCommentary = replaceName(new String[] {
//			});
//						
//			actionBackwardNegativeCommentary = replaceName(new String[] {
//			});
//		
//
//				
//		
//		forwardActionInstruction = new String[]{
//			getMyName() + " may",
//			getMyName() + " should",
//			getMyName() + " can"
//		};

		
//		actionInstruction = new String[]{
//				"step back your foot for guarding",
//				"move forward to get close to the opponent",
//				"lean backward to move backward",
//				"lean forward for dashing",
//				"get away from your opponent",
//				"step back and jump to jump backward",
//				"use left punch to execute lights punch",
//				"use right punch to execute heavy punch",
//				"crouch to make the character crouch",
//				"step back and crouch to guard while crouching",
//				"crouch and left punch to execute weak punch while crouching",
//				"crouch and right punch to execute weak uppercut while crouching",
//				"use left Kick for executing light kick while crouching",
//				"use right kick to execute heavy kick while crouching",
//				"use two-handed punch while the opponent is close to throw the opponent",
//				"step back and two-handed punch while the opponent is close to heavily throw the opponent"
//				//"right kick to execute heavy kick while crouching.",
//				//"This skill can take the opponent down",
//				//"Two-handed punch while the opponent is close to throw the opponent",
//				//"Step back the right foot and two-handed punch while the opponent is close to heavily throw the opponent",
//				//"Right swing from back to front to execute sliding attack. This skill can take the opponent down",
//				//"This skill can take the opponent down",
//				//"Right-handed knifehand strike (karate chop) to execute forward flying attack",
//				//"Do Hadouken on your right side to execute projectile attack"
////				23	Right Step Back for guarding	Step back your right foot for guarding
////				22	He Right Step Forward for walking forward	Step forward your right foot for walking forward
////				21	Lean Backward Left punch to execute weak punchfor moving backward	Lean backward to move backward
////				20	Lean Forward for dashing	Lean forward for dashing
////				19	Jump	Jump to make the character jump
////				18	Right Step Back and Jump for jumping backward	Step back the right foot and jump to jump backward
////				17	Right Step Front and Jump for jumping forward	Step front the right foot and jump to jump forward
////				16	Left Punch for weak punching	
////				15	Right Punch for heavy punching	Right punch to execute heavy punch
////				14	Left Knee Strike for weak kicking	Raise up the left knee to execute weak kick
////				13	Right Knee Strike for heavy kicking	Raise up the right knee to execute heavy kick
////				12	Crouch	Crouch to make the character crouch
////				11	Right Step Back and Crouch for guarding while crouching	Step back the right foot and crouch to guard while crouching
////				10	Crouch and Left Punch for punching while crouching	Crouch and left punch to execute weak punch while crouching
////				9	Crouch and Right Punch for throwing uppercut while crouching	Crouch and right punch to execute weak uppercut while crouching
////				8	Left Kick for executing weak kick while crouching	Left kick to execute weak kick while crouching
////				7	Right Kick for executing heavy kick while crouching	Right kick to execute heavy kick while crouching. This skill can take the opponent down.
////				6	Two-handed Punch for throwing the opponent.	Two-handed punch while the opponent is close to throw the opponent.
////				5	Right Step Back and Two-handed Punch for throwing the opponent heavily.	Step back the right foot and two-handed punch while the opponent is close to heavily throw the opponent
////				4	Right Swing for executing sliding attack.	Right swing from back to front to execute sliding attack. This skill can take the opponent down.
////				3	Left Uppercut for throwing uppercut	Left uppercut to execute heavy uppercut. This skill can take the opponent down.
////				2	Right Knifehand Strike for executing forward flying attack.	Right-handed knifehand strike (karate chop) to execute forward flying attack.
////				1	Hadouken for executing projectile attack.	Do Hadouken on your right side to execute projectile attack.
//		};
//		
		
		// health PDA 
		
		
	}
	
	
	
	
	//=============================================================================================
	
	public static String  getActionRealName(String  skillCode) {
		return skillMap.getOrDefault(skillCode, "Default");
	}
		
	public static String  generateNormalCommentary(String  actionRealName) {
		if (actionRealName == "Default") {
			if(Math.abs(beai.deltaHp) > 60 && beai.p2_gotDamaged > 0) {	
				return generateHealthCommentary(beai.myCurrentMove, isP1); //healthPositive
			}
			else if(Math.abs(beai.deltaHp) > 60 && beai.p1_gotDamaged > 0) {	//healthNegative
				return generateHealthCommentary(beai.myCurrentMove, isP1);
			}
			else {
				//diff less than 60 OR noone did damage
				return generalCommentary[getRandomNumber(generalCommentary.length)];	//generalComment
			}	
		} 
		else {
			if (Math.abs(beai.deltaHp) > 60)
			{
				return Skillbased_healthCommentary(actionRealName)	;		
			}
			else
			{
				return Skillbased_generalCommentary(actionRealName);
			}
		}

	}
	
	
	public static String  Skillbased_healthCommentary(String  actionRealName) {
		return healthForwardQuestion[getRandomNumber(healthForwardQuestion.length)] + " " + actionRealName + healthBackwardQuestion[getRandomNumber(healthBackwardQuestion.length)] + ".";	
		}
	
	public static String  Skillbased_generalCommentary(String  actionRealName) {
		return actionForwardPositiveCommentary[getRandomNumber(actionForwardPositiveCommentary.length)] + " " + actionRealName + actionBackwardPositiveCommentary[getRandomNumber(actionBackwardPositiveCommentary.length)] + ".";			
		}
	
	
	public static String  generateHealthQuestion(String  recommendedActionRealName) {
		return healthForwardQuestion[getRandomNumber(healthForwardQuestion.length)] + " " + recommendedActionRealName + healthBackwardQuestion[getRandomNumber(healthBackwardQuestion.length)];
	}
	//TODO
	public static String  generateActionInstruction(String  recommendedActionRealName) {
		return forwardActionInstruction[getRandomNumber(forwardActionInstruction.length)] + " " +actionInstruction[getRandomNumber(actionInstruction.length)];
	}
	
	public static String  generateHealthCommentary(String  actionRealName, boolean judgement) {
		if (judgement) {	
				return healthPositiveCommentary[getRandomNumber(healthPositiveCommentary.length)];			
		} else {
				return healthNegativeCommentary[getRandomNumber(healthNegativeCommentary.length)];				
		}
	}
	
	public static String  generateBeginCommentary(){
		return beginCommentary[getRandomNumber(beginCommentary.length)];
//		return beginCommentaryCn[getRandomNumber(beginCommentary.length)];
	}
	
	public static String  generateCheerUpCommentaryWin(){
		return cheerUpCommentaryWin[getRandomNumber(cheerUpCommentaryWin.length)];

	}
	
	public static String  generateCheerUpCommentaryLose(){
		return cheerUpCommentaryLose[getRandomNumber(cheerUpCommentaryLose.length)];

	}

	public static String generateCheerUpCommentaryDraw(){
		return cheerUpCommentarySame[getRandomNumber(cheerUpCommentarySame.length)];

	}
	
	public static String  generateEndCommentary(){
		return endCommentary[getRandomNumber(endCommentary.length)];
	}
	
	
	/**
	 * random number generator
	 * 
	 * @param range
	 * @return random number within range
	 */
	public static int getRandomNumber(int range) {
		Random random = new Random();
		return random.nextInt(range);
	}
}
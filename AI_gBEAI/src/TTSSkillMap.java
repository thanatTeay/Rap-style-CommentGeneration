
import java.util.HashMap;
import java.util.Map;
import java.util.Random;

import dataloader.BalFitnessDataLoader;

public class TTSSkillMap {
	
	private Map<String, String> skillMap;
	//  extracted Commentary
	
	String beginCommentary[] = {
			"Hi everybody, welcome to the match!",
			"Alright, welcome everybody!",
			"Welcome! Let's see a match between Garnet and Zen",
			"Welcome to the match",
			"Welcome to Garnet versus Zen"

	};
	
	String beginCommentaryCn[] = {
			
		};
	
	String endCommentary[] = {
		"Thank you for watching",
		"It is a nice match",
		"See you again in the next match",
		"It is a wonderful match"
		
	};
	
	String chat[] = {
			"They are trying to attack each other",
			"It would be a match of the century!",
			"I have never seen it before",
			"So attention to these two players",
			"Let's keep attention to these players",
			"They are trying to predict each other",
			"Haven't really seen this kind of challenging game so far"

	};
	
	String forwardActionInstruction[] = {
		"Garnet may",
		"Garnet should",
		"Garnet can"
	};
	
	String actionInstruction[] = {
			"step back your foot for guarding",
			"move forward to get close to the opponent",
			"lean backward to move backward",
			"lean forward for dashing",
			"get away from your opponent",
			"step back and jump to jump backward",
			"use left punch to execute lights punch",
			"use right punch to execute heavy punch",
			"crouch to make the character crouch",
			"step back and crouch to guard while crouching",
			"crouch and left punch to execute weak punch while crouching",
			"crouch and right punch to execute weak uppercut while crouching",
			"use left Kick for executing light kick while crouching",
			"use right kick to execute heavy kick while crouching",
			"use two-handed punch while the opponent is close to throw the opponent",
			"step back and two-handed punch while the opponent is close to heavily throw the opponent"
			//"right kick to execute heavy kick while crouching.",
			//"This skill can take the opponent down",
			//"Two-handed punch while the opponent is close to throw the opponent",
			//"Step back the right foot and two-handed punch while the opponent is close to heavily throw the opponent",
			//"Right swing from back to front to execute sliding attack. This skill can take the opponent down",
			//"This skill can take the opponent down",
			//"Right-handed knifehand strike (karate chop) to execute forward flying attack",
			//"Do Hadouken on your right side to execute projectile attack"

			
			
			
			
//			23	Right Step Back for guarding	Step back your right foot for guarding
//			22	She Right Step Forward for walking forward	Step forward your right foot for walking forward
//			21	Lean Backward Left punch to execute weak punchfor moving backward	Lean backward to move backward
//			20	Lean Forward for dashing	Lean forward for dashing
//			19	Jump	Jump to make the character jump
//			18	Right Step Back and Jump for jumping backward	Step back the right foot and jump to jump backward
//			17	Right Step Front and Jump for jumping forward	Step front the right foot and jump to jump forward
//			16	Left Punch for weak punching	
//			15	Right Punch for heavy punching	Right punch to execute heavy punch
//			14	Left Knee Strike for weak kicking	Raise up the left knee to execute weak kick
//			13	Right Knee Strike for heavy kicking	Raise up the right knee to execute heavy kick
//			12	Crouch	Crouch to make the character crouch
//			11	Right Step Back and Crouch for guarding while crouching	Step back the right foot and crouch to guard while crouching
//			10	Crouch and Left Punch for punching while crouching	Crouch and left punch to execute weak punch while crouching
//			9	Crouch and Right Punch for throwing uppercut while crouching	Crouch and right punch to execute weak uppercut while crouching
//			8	Left Kick for executing weak kick while crouching	Left kick to execute weak kick while crouching
//			7	Right Kick for executing heavy kick while crouching	Right kick to execute heavy kick while crouching. This skill can take the opponent down.
//			6	Two-handed Punch for throwing the opponent.	Two-handed punch while the opponent is close to throw the opponent.
//			5	Right Step Back and Two-handed Punch for throwing the opponent heavily.	Step back the right foot and two-handed punch while the opponent is close to heavily throw the opponent
//			4	Right Swing for executing sliding attack.	Right swing from back to front to execute sliding attack. This skill can take the opponent down.
//			3	Left Uppercut for throwing uppercut	Left uppercut to execute heavy uppercut. This skill can take the opponent down.
//			2	Right Knifehand Strike for executing forward flying attack.	Right-handed knifehand strike (karate chop) to execute forward flying attack.
//			1	Hadouken for executing projectile attack.	Do Hadouken on your right side to execute projectile attack.
	};

// only describe player 1 now, Garnet vs Zen
	String actionForwardPositiveCommentary[] = {
			"She used",
			"Garnet used",
			"For an opportunity, she used",
			"she continues to use",
			"Nice time to use",
			"That's common to use",
			//"Oh My God and just like that showing you exactly what can happen with",
			"That's very nice to use",
			"She knew when to use",
			"She is pressing her opponent by using",
			"Hit him by",
			"For a chance, she used",
			"How skillfully, Garnet used",
			"She released a powerful"
			
			
			
			/*"She",
			"I have never seen that before",
			"So threatening she",
			"Garnet",
			"For an opportunity she",
			"she continues to",
			//"Oh My God and just like that showing you exactly what can happen with",
			"Nice thing for",
			"That's common that",
			"That's very crucial",
			"What a fast",
			"She knew is that to",
			"She could turn on his head if that",
			"She just knows the",
			"It's always not enough to",
			"She used",
			"You would think even given",
			"She is pressing it out pump",
			"She's so powerful releasing",
			"Hit him by",
			"Again that "*/

			};
	
	String actionForwardNegativeCommentary[] = {
	};
				
	String actionBackwardPositiveCommentary[] = {
			". Excellent for that",
			". That is a good move!",
			". Wow, that happened really quick",
			", really a good one",
			"that Zen should be very careful",
			". That'll be a great deal",
			". Some damage here",
			"that should tell him to quit this game",
			". It's a good deal of damage",
			". That's a nice shot",
			". That's perfect",
			". He gotta be punished!",
			". ZEN will lose for sure!",
			". Nice!"
			
			
			
			/*"excellent for that",
			", that is a good move",
			", really wants to get in there",
			"for real baby",
			"actually, gave him always here",
			"be a trigger right now already for nice hit",
			//", what a fast under funky invasiveness he read the board on the box",
			", wow that happens really quick",
			"punish that point",
			"really a good one",
			//", that's really important to go",
			"that Zen should be very careful",
			"all of us should applause for him",
			//", answers back immediately reflect around",
			", she is gotta be punished",
			"always not going to be a son",
			"it could go to different",
			"Oh that'll be a great deal",
			", some damage here",
			"for a bit of comeback",
			"that to tell him should quit this game",
			"always has to respect it",
			"good flow",
			"counted"*/

			};
	
	String actionBackwardNegativeCommentary[] = {
	};
	
	// health PDA 
	
	String healthForwardQuestion[] = {
			"There's a button there,",
			"It's good to try to employ",
			"It seems possible to"
		
	};
	
	String healthBackwardQuestion[] = {
			". that you can rely on being pretty safe to hit",
			"to get him punished",
			", and knock him down"

	};
	
	String healthPositiveCommentary[] = {
			"Yeah, she successfully punished him!",
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
			"Her moves're great!!"

		
	};
	
	String healthNegativeCommentary[] = {
			"She is at a dangerous point",
			"She likes being in that range",	
			"Hoo, what a bad timing",
			"she could have been knocked down",
			"What is she doing!!",
			"That's a bad move",
			"Oh, no. Not that",
			"Oh, no. Not that"

	};
	
	String cheerUpCommentary[] = {
			"Go Go Go, Garnet, Go Go!!!",
			"Don't give up Garnet. Try again",	
			"Don't give up! Come back Garnet",
			"Fight Garnet Fight!!",
			"You have not been defeated yet, do not give up, just fight it",
			"Go Go Go, Zen, Go Go!!!",
			"Don't give up Zen. Try again",	
			"Don't give up! Come back Zen",
			"Fight Zen Fight!!",
			"He gonna win",
			"She gonna win",
			"They are the stars!",
			"Attack! Attack!",
			"C'mon, defense",
			"We want more! We want more!"

	};
	public TTSSkillMap() {
		skillMap = new HashMap<String, String>();
		
		skillMap.put("STAND_D_DB_BA", "Flying crop");
//		skillMap.put("BACK_STEP", "Back step");
//		skillMap.put("FORWARD_WALK", "Step forward");
//		skillMap.put("DASH", "Lean forward");
		skillMap.put("STAND_GUARD", "Guard");
		skillMap.put("CROUCH_GUARD", "Guard");
		skillMap.put("THROW_A", "Throw");
		skillMap.put("THROW_B", "Great Throw");
		skillMap.put("STAND_A", "Punch");
		skillMap.put("STAND_B", "Kick");
		skillMap.put("CROUCH_A", "Low Punch");
		skillMap.put("CROUCH_B", "Low Kick");
		skillMap.put("STAND_FA", "Heavy Punch");
		skillMap.put("STAND_FB", "Heavy Kick");
		skillMap.put("CROUCH_FA", "Low Heavy Punch");
		skillMap.put("CROUCH_FB", "Low Heavy Kick");
		skillMap.put("STAND_D_DF_FA", "Hadouken");
		skillMap.put("STAND_D_DF_FB", "Super Hadouken");	
		skillMap.put("STAND_F_D_DFA", "Uppercut");
		skillMap.put("STAND_F_D_DFB", "Super Uppercut");
		skillMap.put("STAND_D_DB_BB", "Slide Kick");
		skillMap.put("STAND_D_DF_FC", "Ultimate Hadouken");	
	}
	
	/**
	 * transfer action code into real action name in natural language
	 */
	public String getActionRealName(String skillCode) {
		return skillMap.getOrDefault(skillCode, "Default");
	}
		
	/**
	 * natural language processing using real action name, just prototype for it
	 * TODO
	 * @return complete Commentary
	 */
	
//	public String generateNormalCommentary(String actionRealName, int maxVarId) {
	public String generateNormalCommentary(String actionRealName) {
		if (actionRealName == "Default") {
//			if (getRandomNumber(100) < 30) {
				return chat[getRandomNumber(chat.length)];				
//			} else {
//				//TODO add improve variability
//				return forwardActionInstruction[getRandomNumber(forwardActionInstruction.length)] + " " + actionInstruction[maxVarId];
//			}

		} else {
			return actionForwardPositiveCommentary[getRandomNumber(actionForwardPositiveCommentary.length)] + " " + actionRealName  + actionBackwardPositiveCommentary[getRandomNumber(actionBackwardPositiveCommentary.length)] + ".";			
		}

	}
	
	public String generateHealthQuestion(String recommendedActionRealName) {
		return healthForwardQuestion[getRandomNumber(healthForwardQuestion.length)] + " " + recommendedActionRealName  + healthBackwardQuestion[getRandomNumber(healthBackwardQuestion.length)];
	}
	//TODO
	public String generateActionInstruction(String recommendedActionRealName) {
		return forwardActionInstruction[getRandomNumber(forwardActionInstruction.length)] + " " +actionInstruction[getRandomNumber(actionInstruction.length)];
	}
	
	public String generateHealthCommentary(String actionRealName, boolean judgement) {
		if (actionRealName == "Default") {
			if (judgement) {	
				return healthPositiveCommentary[getRandomNumber(healthPositiveCommentary.length)];			
			} else {
				return healthNegativeCommentary[getRandomNumber(healthNegativeCommentary.length)];				
			}
		} else {
			return actionForwardPositiveCommentary[getRandomNumber(actionForwardPositiveCommentary.length)] + " " + actionRealName + actionBackwardPositiveCommentary[getRandomNumber(actionBackwardPositiveCommentary.length)] + ".";
		}
		
	}
	
	public String generateBeginCommentary(){
		return beginCommentary[getRandomNumber(beginCommentary.length)];
//		return beginCommentaryCn[getRandomNumber(beginCommentary.length)];
	}
	
	public String generateCheerUpCommentary(){
		return cheerUpCommentary[getRandomNumber(cheerUpCommentary.length)];

	}
	
	
	public String generateEndCommentary(){
		return endCommentary[getRandomNumber(endCommentary.length)];
	}
	
	
	/**
	 * random number generator
	 * 
	 * @param range
	 * @return random number within range
	 */
	public int getRandomNumber(int range) {
		Random random = new Random();
		return random.nextInt(range);
	}
}
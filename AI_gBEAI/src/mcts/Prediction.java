package mcts;

import java.security.SecureRandom;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;
import java.util.Random;

import aiinterface.CommandCenter;
import enumerate.Action;
import struct.CharacterData;
import struct.FrameData;

/**
 * ç›¸æ‰‹ã�®Actionã‚’äºˆæ¸¬ã�™ã‚‹ã�Ÿã‚�ã�®ã‚·ãƒ³ã‚°ãƒ«ãƒˆãƒ³ã�ªã‚¯ãƒ©ã‚¹ <br>
 * è¿‘è·�é›¢ãƒ»ä¸­è·�é›¢ãƒ»é•·è·�é›¢ã��ã‚Œã�žã‚Œã�«ã�Šã�„ã�¦ã�®æ•µã�®Actionã�®å›žæ•°ã‚’è¨˜éŒ²ã�—ã�¦ã�Šã��
 *
 * @author Taichi Miyazaki modified by Makoto Ishihara
 */
public class Prediction {
	private static final Prediction instance = new Prediction();

	private ArrayList<HashMap<Action, Integer>> actionList;
	private Random rnd;

	/** è¿‘è·�é›¢ */
	private static final int SHORT_DISTANCE = 110;

	/** é•·è·�é›¢ */
	private static final int LONG_DISTANCE = 160;

	public static Prediction getInstance() {
		return instance;
	}

	private Prediction() {
		actionList = new ArrayList<>();
		for (int i = 0; i < 3; i++) {
			actionList.add(new HashMap<Action, Integer>());
		}
		rnd = new SecureRandom();
	}

	public void countOppAction(FrameData fd, CharacterData oppCharacter, CommandCenter commandCenter) {
		Action action = oppCharacter.getAction();

		int distance = Math.abs(fd.getDistanceX());
		if (distance <= SHORT_DISTANCE) {
			addActionList(action, 0);
		} else if (distance > SHORT_DISTANCE && distance <= LONG_DISTANCE) {
			addActionList(action, 1);
		} else {
			addActionList(action, 2);
		}
	}

	public void addActionList(Action action, int index) {
		if (!containsStatusAction(action)) {
			int count = actionList.get(index).containsKey(action) ? actionList.get(index).get(action) : 0;
			actionList.get(index).put(action, count + 1);
		}
	}

	/**
	 * äºˆæ¸¬ã�—ã�Ÿæ•µã�®æ¬¡ã�®Actionã‚’è¿”ã�™
	 *
	 * @return äºˆæ¸¬ã�—ã�Ÿæ•µã�®æ¬¡ã�®Action
	 */
	public Action predict(int distance, LinkedList<Action> oppActions) {
		int index = 1;
		if (distance <= SHORT_DISTANCE) {
			index = 0;
		} else if (distance > SHORT_DISTANCE && distance <= LONG_DISTANCE) {
			index = 1;
		} else {
			index = 2;
		}
		return getAction(index, oppActions);
	}

	public Action getAction(int index, LinkedList<Action> oppActions) {
		return getActionRoulette(index, oppActions);
	}

	public Action getActionMost(int index) {
		List<Map.Entry<Action, Integer>> entries = new ArrayList<Map.Entry<Action, Integer>>(
				actionList.get(index).entrySet());
		Collections.sort(entries, new Comparator<Map.Entry<Action, Integer>>() {

			@Override
			public int compare(Entry<Action, Integer> entry1, Entry<Action, Integer> entry2) {
				return ((Integer) entry2.getValue()).compareTo((Integer) entry1.getValue());
			}
		});

		Action action = Action.STAND_B;
		if (entries.size() > 0) {
			action = entries.get(0).getKey();
		}

		return action;
	}

	public Action getActionRoulette(int index, LinkedList<Action> oppActions) {
		List<Map.Entry<Action, Integer>> entries = new ArrayList<Map.Entry<Action, Integer>>(
				actionList.get(index).entrySet());
		Collections.sort(entries, new Comparator<Map.Entry<Action, Integer>>() {

			@Override
			public int compare(Entry<Action, Integer> entry1, Entry<Action, Integer> entry2) {
				return ((Integer) entry2.getValue()).compareTo((Integer) entry1.getValue());
			}
		});

		Action action = null;
		if (entries.size() > 0) {
			int max = 0;
			for (Entry<Action, Integer> s : entries) {
				max += s.getValue();
			}

			int randomTmp = rnd.nextInt(max);
			for (Entry<Action, Integer> s : entries) {
				randomTmp -= s.getValue();
				if (randomTmp < 0) {
					action = s.getKey();
					break;
				}
			}
		} else {
			action = oppActions.get(rnd.nextInt(oppActions.size()));
		}

		return action;
	}

	public void getInfomation() {

		for (int index = 0; index < 3; index++) {
			List<Map.Entry<Action, Integer>> entries = new ArrayList<Map.Entry<Action, Integer>>(
					actionList.get(index).entrySet());
			Collections.sort(entries, new Comparator<Map.Entry<Action, Integer>>() {

				@Override
				public int compare(Entry<Action, Integer> entry1, Entry<Action, Integer> entry2) {
					return ((Integer) entry2.getValue()).compareTo((Integer) entry1.getValue());
				}
			});
		}
	}

	/**
	 * è¡Œå‹•ã�«çŠ¶æ…‹(æ”»æ’ƒã€�ç§»å‹•ä»¥å¤–)ã�Œå�«ã�¾ã‚Œã�¦ã�„ã‚‹ã�‹ã‚’ãƒ�ã‚§ãƒƒã‚¯ <br>
	 *
	 * @return true çŠ¶æ…‹ã�Œå�«ã�¾ã‚Œã�¦ã�„ã‚‹ false çŠ¶æ…‹ã�Œå�«ã�¾ã‚Œã�¦ã�„ã�ªã�„
	 */
	private boolean containsStatusAction(Action action) {
		if (action != null) {
			if (action.name().equals("STAND") || action.name().equals("CROUCH") || action.name().equals("AIR")
					|| action.name().equals("RISE") || action.name().equals("LANDING") || action.name().equals("DOWN")
					|| action.name().equals("STAND_GUARD_RECOV") || action.name().equals("CROUCH_GUARD_RECOV")
					|| action.name().equals("AIR_GUARD_RECOV") || action.name().equals("STAND_RECOV")
					|| action.name().equals("CROUCH_RECOV") || action.name().equals("AIR_RECOV")
					|| action.name().equals("CHANGE_DOWN") || action.name().equals("THROW_HIT")
					|| action.name().equals("THROW_SUFFER")) {
				return true;
			} else {
				return false;
			}
		}

		return false;
	}
}

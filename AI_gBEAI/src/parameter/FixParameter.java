package parameter;

public class FixParameter {
	//////////// MCTS関連のパラメータ///////////////

	/** UCTの実行時間 */
	public static final long UCT_TIME = 165 * 100000;

	/** UCB1の定数Cの値 */
	public static final double UCB_C = 0.42;

	/** 探索する木の深さ */
	public static final int UCT_TREE_DEPTH = 3;

	/** ノードを生成する閾値 */
	public static final int UCT_CREATE_NODE_THRESHOULD = 7;

	/** シミュレーションを行う時間 */
	public static final int SIMULATION_TIME = 60;

	/** フレームの調整用時間 */
	public static final int FRAME_AHEAD = 14;

	/** Tanh式のスケール */
	public static final double TANH_SCALE = 30;

	/** デバッグモードであるかどうか。trueの場合、様々なログが出力される */
	public static final boolean DEBUG_MODE = false;

	/////////// 各提案手法に関するパラメータ//////////////

	/** 回避行動を続ける時間 */
	public static final int AVOIDANCE_TIME = 5;

	/////////// 各提案手法を組み込むかどうかを管理するフラグ//////////////

	/** 通常MCTSか手加減MCTSかを管理するフラグ */
	public static final boolean STRONG_FLAG = false;

	/** 自然さを考慮するかどうかを管理するフラグ */
	public static final boolean MIX_FLAG = true;

	/** 評価値を正規化するかどうかを管理するフラグ */
	public static final boolean NORMALIZED_FLAG = true;

	/** 予測をするかどうかを管理するフラグ */
	public static final boolean PREDICT_FLAG = true;

	/** 回避行動をするかどうかを管理するフラグ */
	public static final boolean AVOID_FLAG = true;

}

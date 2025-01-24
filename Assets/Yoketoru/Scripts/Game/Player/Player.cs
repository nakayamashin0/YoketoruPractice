using UnityEngine.Events;
using UnityEngine;

public class Player : MonoBehaviour, IGameStateListener
{
    enum State
    {
        None = -1, // 初期状態
        Play, // 進行状態
        Miss, // ミス状態
        Clear, // クリア状態
        Reset, // リセット
    }

    SimpleState<State> state = new(State.None);

    // 座標を記録するための変数
    private Vector3 initialPosition;

    // 回転を記録するための変数
    private Vector3 initialRotation;

    public UnityEvent<IGameStateListener> GameStateListenerDestroyed { get; private set; } = new();

    /// <summary>
    /// 初期化処理（Awake）
    /// </summary>
    private void Awake() 
    {
        // 座標を記録
        initialPosition = transform.position;

        // 回転を記録
        initialRotation = transform.Find("Pivot").eulerAngles;
    }

    /// <summary>
    /// フレーム更新
    /// </summary>
    private void Update()
    {
        UpdateState();
    }

    /// <summary>
    /// 物理処理のための固定更新
    /// </summary>
    void FixedUpdate()
    {
        InitState();
        FixedUpdateState();
    }

    void OnDestroy()
    {
        // オブジェクトを消すときに、必ずInvokeする
        GameStateListenerDestroyed.Invoke(this);
    }

    /// <summary>
    /// 状態の初期化処理
    /// </summary>
    void InitState()
    {
        if (!state.ChangeState())
        {
            return;
        }

        switch (state.CurrentState)
        {
            case State.Play:
                Debug.Log($"操作と移動開始");
                break;

            case State.Miss:
                Debug.Log($"ミスの演出。なければ消す");
                break;

            case State.Clear:
                Debug.Log($"クリア演出。なければ消す");
                break;

            case State.Reset:
                // Reset状態になったときに座標と回転を元に戻す
                Debug.Log($"座標と向きを、Awakeで記録したものに戻す");

                // 座標と回転を初期状態に戻す
                transform.position = initialPosition;
                transform.Find("Pivot").eulerAngles = initialRotation;
                break;
        }
    }

    /// <summary>
    /// 状態のフレーム更新
    /// </summary>
    void UpdateState()
    {
        switch (state.CurrentState)
        {
            case State.Play:
                break;
        }
    }

    /// <summary>
    /// 状態の物理更新
    /// </summary>
    void FixedUpdateState()
    {
        switch (state.CurrentState)
        {
            case State.Play:
                break;
        }
    }

    public void OnReset()
    {
        state.SetNextState(State.Reset);  // Reset状態に遷移
    }

    public void OnGameStart()
    {
        state.SetNextState(State.Play);  // Play状態に遷移
    }

    public void OnGameOver()
    {
        state.SetNextState(State.Miss);  // Miss状態に遷移
    }

    public void OnClear()
    {
        state.SetNextState(State.Clear);  // Clear状態に遷移
    }
}

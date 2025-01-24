using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーを制御するクラス。
/// </summary>
public class Player : MonoBehaviour, IGameStateListener
{
    enum State
    {
        None = -1,
        Play,
        Miss,
        Clear,
        Reset,
    }

    SimpleState<State> state = new(State.None);

    public UnityEvent<IGameStateListener> GameStateListenerDestroyed { get; private set; } = new();

    /// <summary>
    /// フレーム更新
    /// </summary>
    private void Update()
    {
        UpdateState();
    }

    /// <summary>
    /// 物理処理ための固定更新
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

                // TODO: 動作を確認したら、消す
                transform.Find("Pivot").eulerAngles
                    = new Vector3(0, 0, -45);
                transform.Translate(new Vector3(1, 1, 0));
                break;

            case State.Miss:
                Debug.Log($"ミスの演出。なければ消す");
                break;

            case State.Clear:
                Debug.Log($"クリア演出。なければ消す");
                break;

            case State.Reset:
                Debug.Log($"座標と向きを、Awakeで記録したものに戻す");
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
        state.SetNextState(State.Reset);
    }

    public void OnGameStart()
    {
        state.SetNextState(State.Play);
    }

    public void OnGameOver()
    {
        state.SetNextState(State.Miss);
    }

    public void OnClear()
    {
        state.SetNextState(State.Clear);
    }
}

using Mirror;
using System;
using UnityEngine.Events;

public class PlayerScore : NetworkBehaviour
{
    [SyncVar]
    public int score = 0;

    public UnityEvent<string> NewScoreTextEvent = new(); // не стал сильно париться, просто в префабе добавил листенер на текст UI

    public void ChangeScore(bool plus)
    {

        if (plus)
            score++;
        else
            score = Math.Clamp(score - 1, 0, int.MaxValue);

        ChangeScoreTarget(score);

    }

    [TargetRpc]
    private void ChangeScoreTarget(int score)
    {
        NewScoreTextEvent.Invoke(score.ToString());
    }
}

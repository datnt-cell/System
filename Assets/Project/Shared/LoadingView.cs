using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class LoadingView : MonoBehaviour
{
    [SerializeField] private GameObject shield;

    private CancellationTokenSource _cts;

    public void SetShield(bool active)
    {
        if (active)
        {
            ActivateShield().Forget();
        }
        else
        {
            DeactivateShield();
        }
    }

    private async UniTaskVoid ActivateShield()
    {
        ResetCTS();

        var destroyToken = this.GetCancellationTokenOnDestroy();
        var linked = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, destroyToken);

        shield.SetActive(true);

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: linked.Token);
            shield.SetActive(false);
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            linked.Dispose();
        }
    }

    private void DeactivateShield()
    {
        _cts?.Cancel();
        shield.SetActive(false);
    }

    private void ResetCTS()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
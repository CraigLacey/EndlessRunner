using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

/// <summary>
/// WebRequestUtils adds helpers for UnityWebRequests.
/// </summary>
public static class WebRequestUtils
{
    // Helper function to await UnityWebRequest
    public static Task SendWebRequestAsync(UnityWebRequest request)
    {
        var tcs = new TaskCompletionSource<bool>();

        request.SendWebRequest().completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.Success || request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                tcs.SetResult(true);
            }
            else
            {
                tcs.SetException(new Exception($"UnityWebRequest failed: {request.error}"));
            }
        };

        return tcs.Task;
    }
}

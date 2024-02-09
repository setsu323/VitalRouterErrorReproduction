using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VitalRouter;

[Routes]
public partial class ErrorReproduction : MonoBehaviour
{
    private ICommandPublisher _commandPublisher;

    private void Start()
    {
        _commandPublisher = Router.Default;
        MapTo(Router.Default);
        
        CallCommand();
    }

    private async void CallCommand()
    {
        _commandPublisher.PublishAsync(new OneSecondCommand()).Forget();
        await UniTask.NextFrame();
        
        //This will be expected to end after 30 seconds.
        //But this process actually will end after 1 second.
        _commandPublisher.PublishAsync(new ThirtySecondsCommand()).Forget();
    }
    
    public async UniTask On(OneSecondCommand command)
    {
        //Fast process
        await UniTask.Delay(TimeSpan.FromSeconds(1));
    }
    
    public async UniTask On(ThirtySecondsCommand command)
    {
        //Slow process
        await UniTask.Delay(TimeSpan.FromSeconds(30));
    }
}

public struct OneSecondCommand : ICommand{}
public struct ThirtySecondsCommand : ICommand{}
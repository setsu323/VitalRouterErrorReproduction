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
        //This task will not be finished.
        _commandPublisher.PublishAsync(new OneSecondCommand()).Forget();
        await UniTask.NextFrame();
        _commandPublisher.PublishAsync(new ToSecondsCommand()).Forget();
    }
    
    public async UniTask On(OneSecondCommand command)
    {
        //Fast process
        await UniTask.Delay(TimeSpan.FromSeconds(1));
    }
    
    public async UniTask On(ToSecondsCommand command)
    {
        //Slow process
        await UniTask.Delay(TimeSpan.FromSeconds(2));
    }
}

public struct OneSecondCommand : ICommand{}
public struct ToSecondsCommand : ICommand{}
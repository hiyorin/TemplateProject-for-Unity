using System;
using NUnit.Framework;
using SocialGame.Dialog;
using SocialGame.Internal.Dialog;
using UnityEngine;
using Zenject;
using UniRx;
using Moq;

[TestFixture]
public sealed class DialogModelTest : ZenjectUnitTestFixture
{
    private class MockDialog : MonoBehaviour, IDialog
    {
        public readonly Subject<Unit> Next = new Subject<Unit>();

        public readonly Subject<Unit> Prev = new Subject<Unit>();

        public IObservable<Unit> OnOpenAsObservable(float defaultDuration) { return Observable.ReturnUnit(); }

        public IObservable<Unit> OnCloseAsObservable(float defaultDuration) { return Observable.ReturnUnit(); }

        public IObservable<Unit> OnStartAsObservable(object param) { return Observable.ReturnUnit(); }

        public IObservable<Unit> OnResumeAsObservable(object param) { return Observable.ReturnUnit(); }

        public IObservable<RequestDialog> OnNextAsObservable() { return Next.Select(_ => new RequestDialog(DialogType.Sample, null)); }

        public IObservable<object> OnPreviousAsObservable() { return Prev.Select(_ => string.Empty); }
    }

    private readonly Subject<RequestDialog> _open = new Subject<RequestDialog>();

    private CompositeDisposable _disposables;

    private MockDialog _dialog;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        Container.BindInterfacesAndSelfTo<DialogModel>().AsSingle();

        var factoryMock = new Mock<IDialogFactory>();
        _dialog = new GameObject(string.Empty).AddComponent<MockDialog>();
        factoryMock.Setup(x => x.Spawn(It.IsAny<DialogType>())).Returns(_dialog.gameObject);
        Container.BindInstance(factoryMock.Object).AsSingle();

        var intentMock = new Mock<IDialogIntent>();
        intentMock.Setup(x => x.OnOpenAsObservable()).Returns(_open);
        Container.BindInstance(intentMock.Object).AsSingle();

        Container.ResolveAll<IInitializable>().ForEach(x => x.Initialize());
        _disposables = new CompositeDisposable();
    }

    [TearDown]
    public override void Teardown()
    {
        _disposables.Dispose();
        Container.ResolveAll<IDisposable>().ForEach(x => x.Dispose());
        base.Teardown();
    }

    [Test]
    public void Add()
    {
        var call = false;
        Container
            .Resolve<IDialogModel>()
            .OnAddAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        _open.OnNext(new RequestDialog(DialogType.Sample, string.Empty));
        Assert.IsTrue(call);
    }

    [Test]
    public void Open()
    {
        _open.OnNext(new RequestDialog(DialogType.Sample, string.Empty));
        var call = false;
        Container
            .Resolve<IDialogModel>()
            .OnOpenAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);

        Assert.IsTrue(call);
    }

    [Test]
    public void Close()
    {
        _open.OnNext(new RequestDialog(DialogType.Sample, string.Empty));
        _dialog.Prev.OnNext(Unit.Default);
        var call = false;
        Container
            .Resolve<IDialogModel>()
            .OnCloseAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        Assert.IsTrue(call);
    }
}
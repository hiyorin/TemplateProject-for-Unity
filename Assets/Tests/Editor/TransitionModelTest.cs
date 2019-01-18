using System;
using NUnit.Framework;
using SocialGame.Transition;
using SocialGame.Internal.Transition;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Async;
using Moq;

[TestFixture]
public sealed class TransitionModelTest : ZenjectUnitTestFixture
{
    private class MockTransition : MonoBehaviour, ITransition
    {
        public async UniTask OnTransIn(float defaultDuration) {}

        public async UniTask OnTransOut(float defaultDuration) {}
    }

    private readonly Subject<TransMode> _in = new Subject<TransMode>();

    private readonly Subject<Unit> _out = new Subject<Unit>();

    private CompositeDisposable _disposables;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        Container.BindInterfacesAndSelfTo<TransitionModel>().AsSingle();

        var factoryMock = new Mock<ITransitionFactory>();
        factoryMock.Setup(x => x.Create(It.IsAny<TransMode>())).Returns(new GameObject(string.Empty).AddComponent<MockTransition>().gameObject);
        Container.BindInstance(factoryMock.Object).AsSingle();

        var intentMock = new Mock<ITransitionIntent>();
        intentMock.Setup(x => x.OnTransInAsObservable()).Returns(_in);
        intentMock.Setup(x => x.OnTransOutAsObservable()).Returns(_out);
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
            .Resolve<ITransitionModel>()
            .OnAddAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        _in.OnNext(TransMode.BlackFade);
        Assert.IsTrue(call);
    }

    [Test]
    public void In()
    {
        var call = false;
        Container
            .Resolve<ITransitionModel>()
            .OnTransInCompleteAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        _in.OnNext(TransMode.BlackFade);
        Assert.IsTrue(call);
    }

    [Test]
    public void Out()
    {
        _in.OnNext(TransMode.BlackFade);
        var call = false;
        Container
            .Resolve<ITransitionModel>()
            .OnTransOutCompleteAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        _out.OnNext(Unit.Default);
        Assert.IsTrue(call);
    }
}
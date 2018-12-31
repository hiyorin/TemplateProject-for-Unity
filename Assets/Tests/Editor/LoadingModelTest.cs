using System;
using NUnit.Framework;
using SocialGame.Loading;
using SocialGame.Internal.Loading;
using UnityEngine;
using Zenject;
using UniRx;
using Moq;

[TestFixture]
public sealed class LoadingModelTest : ZenjectUnitTestFixture
{
    private class MockLoading : MonoBehaviour, ILoading
    {
        public IObservable<Unit> OnShowAsObservable(float defaultDuration) { return Observable.ReturnUnit(); }

        public IObservable<Unit> OnHideAsObservable(float defaultDuration) { return Observable.ReturnUnit(); }
    }

    private readonly Subject<LoadingType> _onShow = new Subject<LoadingType>();

    private readonly Subject<Unit> _onHide = new Subject<Unit>();

    private CompositeDisposable _disposables;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        Container.BindInterfacesAndSelfTo<LoadingModel>().AsSingle();

        var factoryMock = new Mock<ILoadingFactory>();
        factoryMock.Setup(x => x.Create(It.IsAny<LoadingType>())).Returns(new GameObject(string.Empty).AddComponent<MockLoading>().gameObject);
        Container.BindInstance(factoryMock.Object).AsSingle();

        var intentMock = new Mock<ILoadingIntent>();
        intentMock.Setup(x => x.OnShowAsObservable()).Returns(_onShow);
        intentMock.Setup(x => x.OnHideAsObservable()).Returns(_onHide);
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
            .Resolve<ILoadingModel>()
            .OnAddAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        _onShow.OnNext(LoadingType.Sample);
        Assert.IsTrue(call);
    }

    [Test]
    public void Show()
    {
        _onShow.OnNext(LoadingType.Sample);
        var call = false;
        Container
            .Resolve<ILoadingModel>()
            .OnShowAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        Assert.IsTrue(call);
    }

    [Test]
    public void Hide()
    {
        var call = false;
        Container
            .Resolve<ILoadingModel>()
            .OnHideAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        _onHide.OnNext(Unit.Default);
        Assert.IsTrue(call);
    }
}

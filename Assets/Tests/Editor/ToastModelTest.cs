using System;
using NUnit.Framework;
using SocialGame.Toast;
using SocialGame.Internal.Toast;
using UnityEngine;
using Zenject;
using UniRx;
using Moq;

[TestFixture]
public sealed class ToastModelTest : ZenjectUnitTestFixture
{
    private class MockToast : MonoBehaviour, IToast
    {
        public IObservable<Unit> OnOpenAsObservable(object param)
        {
            return Observable.ReturnUnit();
        }

        public IObservable<Unit> OnCloseAsObservable()
        {
            return Observable.ReturnUnit();
        }
    }

    private readonly Subject<RequestToast> _open = new Subject<RequestToast>();

    private CompositeDisposable _disposables;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        Container.BindInterfacesAndSelfTo<ToastModel>().AsSingle();
        Container.BindInterfacesAndSelfTo<ToastSettings>().AsSingle();

        var factoryMock = new Mock<IToastFactory>();
        factoryMock.Setup(x => x.Create(It.IsAny<ToastType>())).Returns(new GameObject("").AddComponent<MockToast>().gameObject);
        Container.BindInstance(factoryMock.Object).AsSingle();

        var intentMock = new Mock<IToastIntent>();
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
            .Resolve<IToastModel>()
            .OnAddAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        _open.OnNext(new RequestToast(ToastType.Sample, string.Empty));
        Assert.IsTrue(call);
    }

    [Test]
    public void Open()
    {
        _open.OnNext(new RequestToast(ToastType.Sample, string.Empty));
        var call = false;
        Container
            .Resolve<IToastModel>()
            .OnOpenAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables); 
        Assert.IsTrue(call);
    }

    [Test]
    public void Close()
    {
        var call = false;
        Container
            .Resolve<IToastModel>()
            .OnCloseAsObservable()
            .Subscribe(_ => call = true)
            .AddTo(_disposables);
        Assert.IsTrue(call);
    }
}

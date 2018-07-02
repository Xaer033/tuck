using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using GhostGen;

[CreateAssetMenu(menuName = "Mr.Tuck/InjectorContext")]
public class InjectorContext : ScriptableObjectInstaller<InjectorContext>
{
    public override void InstallBindings()
    {
        //Container.Bind<IFoo>().To<Foo>()
        Container.Bind<IStateFactory>().To<TuckStateFactory>().AsSingle();
        //Container.Bind<IViewFactory<UIView>>().To<ViewFactory>().AsSingle();
    }
}

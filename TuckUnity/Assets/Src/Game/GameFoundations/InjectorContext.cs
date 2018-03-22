using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using GhostGen;

public class InjectorContext : MonoInstaller<InjectorContext>
{
    public override void InstallBindings()
    {
        //Container.Bind<IFoo>().To<Foo>()
        Container.Bind<IStateFactory>().To<TuckStateFactory>().AsTransient();
    }
}

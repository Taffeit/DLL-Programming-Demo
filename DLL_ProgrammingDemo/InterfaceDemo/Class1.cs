using System;

namespace InterfaceDemo
{
    /// <summary>
    ///     每个插件都需要一个接口，因为插件模块在面向对象编程中本质上就属于多态的应用。
    ///     我们需要创建一个所有人都可以用的接口，然后要使用接口的类均引用依赖项该接口即可
    /// </summary>
    public interface I_a
    {
        string Get();
    }

}

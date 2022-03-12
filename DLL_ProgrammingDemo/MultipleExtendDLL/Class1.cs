using System;
using InterfaceDemo;

namespace MultipleExtendDLL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}

//插件A
namespace MultipleExtendDLL2
{
    /// <summary>
    ///     在进行编译时，一定要将插件的主要出口设为Public，不然类和方法均不会被标为exported，也就是无法被Assembly识别到
    /// </summary>
    public class PluginA : InterfaceDemo.I_a
    {
        public string Get()
        {
            return "插件A";
        }
    }
}

//插件B
namespace MultipleExtendDLL
{
    /// <summary>
    ///     当方法编程完成后，将文件进行编译，也就是Visual中的生成，生成后会在对应的资源文件夹中显示两个以上的DLL，
    ///     一个是必定会有的接口InterfaceDemo2的DLL，另外几个就是我们的插件DLL，一般名字为解决方案的名字+.DLL。
    ///     将这些DLL均移动到可被PluginLoadingDemo中的Plugins文件夹中(该文件夹为我自行创建，没有可以自行创建)，
    ///     接下来请移步至PluginLoadingDemo内查看教程
    /// </summary>
    public class PluginB : InterfaceDemo.I_a
    {
        public string Get()
        {
            return "插件B";
        }
    }
}
using System;

namespace PluginLoadingDemo
{
    /// <summary>
    ///     <p><h>插件导入实例</h></p>
    ///         <p>从根目录下的Plugins目录中导入所有有继承引用接口InterfaceDemo2.I_a的DLL<br/>
    ///         该原理基本是使用Reflection中的Assembly加载需要导入的DLL的类框架，之后
    ///         再尝试将获取到的I_a暴露接口赋值给I_a的实例，调用实现</p>
    ///         查看步骤为：<br/>
    ///             InterfaceDemo -> MultipleExtendDLL -> PluginLoadingDemo
    /// </summary>
    /// <author>Taffeit</author>
    /// <version>1.0</version>
    /// <CreateDate>2022.3.12</CreateDate>
    public partial class MainFrame
    {
        /// <summary>
        ///     <p><h>插件导入步骤</h></p>
        /// </summary>
        public void FullSequence()
        {
            //确认文件夹中插件的位置
            //其实还可以添加一部选取单个插件的方法，但是我懒得写了就直接取第一个Dll了
            string PluginPath = this.LocatePlugins()[1];

            //确认自身对象的完整程序集名称
            string SelfTypeFullName = this.GetSelfTypeFullName();

            //以程序集名称创建类框架
            System.Reflection.Assembly SelfReflectionTool = this.LoadClassFrameByFullName(SelfTypeFullName);
            //以文件物理路径捕捉目标文件框架
            System.Reflection.Assembly DirReflectionTool = this.LoadClassFrameByPtah(PluginPath);

            //获取所有以存在外露的外露方法，若是不存在则返回null，外露即明确标识为public的类的方法
            System.Type[] DLLDefinedTypes = this.GetFrameExportedTypes(DirReflectionTool);
            System.Type[] SelfDefinedTypes = this.GetFrameExportedTypes(SelfReflectionTool);

            //遍历其外露的方法
            foreach (System.Type For_1_Tem_DefType in SelfDefinedTypes)
            {
                //用其对应接口的程序集创建实例对象并且执行
                if (For_1_Tem_DefType.GetInterface("I_a") != null)
                {
                    InterfaceDemo.I_a For_1_If_TemInstance = this.CreateInstance(For_1_Tem_DefType);
                    //用接口方法输出
                    System.Console.WriteLine(For_1_If_TemInstance.Get());
                }
            }

            System.Console.WriteLine("done");
            System.Console.ReadLine();
        }

        /// <summary>
        /// <p><h>返回插件目录下所有的插件名称</h></p>
        ///         将从根目录下的Plugins下载导出所有插件的名称<br/>
        ///         格式例：<br/>
        ///             "F:\\Program\\C#\\DLLDemo\\Solution1\\PluginLoadingDemo\\bin\\Debug\\Plugins\\Demo.dll"
        /// </summary>
        /// <returns>Plugins目录中所有的DLL文件的完整地址字符串</returns>
        private string[] LocatePlugins()
        {
            string MainProgramPath = System.AppDomain.CurrentDomain.BaseDirectory;
            MainProgramPath = System.IO.Path.Combine(MainProgramPath, "Plugins");
            string[] PluginsCollection = System.IO.Directory.GetFiles(MainProgramPath, "*.dll");

            return PluginsCollection;
        }

        /// <summary>
        /// <p><h>获取自身对象的完整程序集名称</h></p>
        ///         格式例：<br>
        ///             "PluginLoadingDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        /// </summary>
        /// <returns>自身对象的完整程序集名称</returns>
        private string GetSelfTypeFullName()
        {
            System.Type SelfType = new MainFrame().GetType();
            return SelfType.Assembly.FullName;
        }

        /// <summary>
        ///     <p><h>使用完整程序集载入类框架</h></p>
        /// </summary>
        /// <example>
        ///     System.Reflection.Assembly Tool = LoadClassFrameByFullName(
        ///         "PluginLoadingDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
        /// </example>
        /// <example>
        ///     System.Reflection.Assembly Tool = LoadClassFrameByFullName(this.GetSelfTypeFullName());
        /// </example>
        /// <param name="aClassFullName">一段类的完整程序集名称</param>
        /// <returns>
        ///     返回：成功-载入成功的类框架；异常-null
        /// </returns>
        private System.Reflection.Assembly LoadClassFrameByFullName(string aClassFullName)
        {
            System.Reflection.Assembly Tem_ClassFrame = null;

            try
            {
                Tem_ClassFrame = System.Reflection.Assembly.Load(aClassFullName);
            }
            catch (System.ArgumentNullException error)
            {
                //暂时还没想好要throw什么，所以就直接return null了，感觉没什么必要
                return null;
            }

            return Tem_ClassFrame;
        }

        /// <summary>
        ///     <p><h>使用完整路径载入类框架</h></p>
        ///             推荐从Plugins文件夹中载入DLL作为类框架
        /// </summary>
        /// <example>
        ///     System.Reflection.Assembly Tool = LoadClassFrameByFullName(
        ///         "F:\\Program\\C#\\DLLDemo\\Solution1\\PluginLoadingDemo\\bin\\Debug\\Plugins\\Demo.dll");
        /// </example>
        /// <example>
        ///     System.Reflection.Assembly Tool = LoadClassFrameByFullName(this.LocatePlugins()[0]);
        /// </example>
        /// <param name="aClassFullName">一段完整的DLL路径</param>
        /// <returns>
        ///     返回：成功-载入成功的类框架；异常-null
        /// </returns>
        private System.Reflection.Assembly LoadClassFrameByPtah(string aFileFullPath)
        {
            System.Reflection.Assembly Tem_ClassFrame = null;

            try
            {
                Tem_ClassFrame = System.Reflection.Assembly.LoadFrom(aFileFullPath);
            }
            catch (System.ArgumentNullException error)
            {
                //暂时还没想好要throw什么，所以就直接return null了，感觉没什么必要
                return null;
            }

            return Tem_ClassFrame;
        }

        /// <summary>
        ///     <p><h>加载类框架中所有外露方法</h></p>
        ///             外露大概是指所有被设为Publie的方法
        /// </summary>
        /// <example>
        ///     System.Type[] Tool = GetFrameExportedTypes(
        ///         LoadClassFrameByPtah(***));
        /// </example>
        /// <param name="aClassFullName">一个成功被加载的类框架</param>
        /// <returns>
        ///     返回：成功-框架中所有外露的方法；异常-null
        /// </returns>
        private System.Type[] GetFrameExportedTypes(System.Reflection.Assembly aAssemblyFrame)
        {
            System.Type[] Tem_ClassTypes = null;

            try
            {
                Tem_ClassTypes = aAssemblyFrame.GetExportedTypes();
            }
            catch (System.ArgumentNullException error)
            {
                //暂时还没想好要throw什么，所以就直接return null了，感觉没什么必要
                return null;
            }

            return Tem_ClassTypes;
        }

        /// <summary>
        ///     <p><h>以<c>InterfaceDemo2.I_a</c>为类型创建对应对象实例</h></p>
        ///             需要对应着<c>InterfaceDemo2.I_a</c>接口的程序集
        /// </summary>
        /// <param name="aClassFullName">包含着InterfaceDemo2.I_a接口的程序集</param>
        /// <returns>
        ///     返回：为InterfaceDemo2.I_a为框架的对象
        /// </returns>
        private InterfaceDemo.I_a CreateInstance(System.Type aType)
        {
            return (InterfaceDemo.I_a)System.Activator.CreateInstance(aType);
        }

        static void Main(string[] args)
        {
            new MainFrame().FullSequence();
        }
    }
}

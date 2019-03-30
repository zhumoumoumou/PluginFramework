using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using PluginFramework.Interface;

namespace PluginFramework.Manager
{
    /// <summary>
    /// 表示如何寻找指定工作目录所含的插件。
    /// </summary>
    [Flags]
    public enum DirectoryManageOption : byte
    {
        /// <summary>
        /// 搜索文件夹下所有文件。
        /// </summary>
        All,
        /// <summary>
        /// 仅搜索根目录。
        /// </summary>
        Root,
        /// <summary>
        /// 已按文件夹分类，搜索工作目录下所有目录的根目录。
        /// </summary>
        Classified
    };


    /// <summary>
    /// 插件管理器。
    /// </summary>
    public class BasicManager
    {
        /// <summary>
        /// 所有托管的插件。
        /// </summary>
        public List<ManagedPluginItem> Plugins { get; private set; }

        /// <summary>
        /// 所有已注册的互斥类型。
        /// </summary>
        public List<string> RegistedMutexCategories { get; private set; }

        /// <summary>
        /// 获取所有的可加载项。
        /// </summary>
        public IEnumerable<ManagedPluginItem> 
            LoadList { get { return GetComponents<ILoadable>(); } }
        
        /// <summary>
        /// 当前管理器的工作目录。
        /// </summary>
        public string WorkDirectory { get; set; }

        /// <summary>
        /// 当执行目录扫描方法时的扫描参数。
        /// </summary>
        public DirectoryManageOption ScanOption { get; set; }

        public BasicManager()
        {
            Plugins = new List<ManagedPluginItem>();
            RegistedMutexCategories = new List<string>();
        }

        #region GetComponents
        /// <summary>
        /// 获取所有类型为T、继承自T或者实现了接口T的<see cref="ManagedPluginItem"/>。
        /// </summary>
        /// <typeparam name="T">源类型。</typeparam>
        /// <returns>所有类型为T、继承自T或者实现了接口T的
        /// <see cref="ManagedPluginItem"/>。</returns>
        public IEnumerable<ManagedPluginItem> GetComponents<T>()
        {
            var query = from plugin in Plugins
                        where plugin.Plugin is T
                        select plugin;
            return query;
        }

        /// <summary>
        /// 获取所有类型为T、继承自T或者实现了接口T的
        /// <see cref="ManagedPluginItem"/>，并按照过滤器规则过滤。
        /// </summary>
        /// <typeparam name="T">源类型。</typeparam>
        /// <param name="filter">过滤器</param>
        /// <returns>所有类型为T、继承自T或者实现了接口T，并按照filter过滤的
        /// <see cref="ManagedPluginItem"/>。</returns>
        public IEnumerable<ManagedPluginItem> GetComponents<T>(Predicate<T> filter)
        {
            var query = from plugin in Plugins
                        where plugin.Plugin is T && filter((T)plugin.Plugin)
                        select plugin;
            return query;
        }
        #endregion

        #region Scanner
        /// <summary>
        /// 对指定目录按照<see cref="ScanOption"/>参数进行扫描。
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"/>
        public virtual IEnumerable<string> DirectoryPluginScan(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException("工作目录未正确设置。");
            }
            List<string> files = new List<string>();

            if (this.ScanOption == 0)
            {
                // TODO: 递归查找所有的dll
                files = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories).ToList();
            }
            else
            {
                if (((int)this.ScanOption & 0x01) != 0)
                {
                    // 查找根目录中所有的dll
                    files = Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly).ToList();
                }

                if (((int)this.ScanOption & 0x02) != 0)
                {
                    // 列出所有的子文件夹
                    var paths = Directory.GetDirectories(dir);
                    foreach (var path in paths)
                    {
                        string[] dllFiles = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);
                        foreach (var item in dllFiles)
                        {
                            files.Add(item);
                        }
                    }
                }
            }
            return files;
        }
        
        /// <summary>
        ///  扫描工作目录下的dll文件。
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> WorkDirectoryDllFilesScan()
        {
            return DirectoryPluginScan(WorkDirectory);
        }
        
        #endregion
        /// <summary>
        /// 从给定路径数组加载所有的<see cref="Assembly"/>
        /// </summary>
        /// <param name="dllFiles">待加载的所有路径。</param>
        /// <returns>返回所有已加载的Assembly。</returns>
        /// <exception cref="FileNotFoundException"/>
        public virtual IEnumerable<Assembly> LoadAssembliesFromDlls(IEnumerable<string> dllFiles)
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var item in dllFiles)
            {
                if (!File.Exists(item))
                {
                    throw new FileNotFoundException("指定的dll文件并不存在。", item);
                }

                try
                {
                    Assembly assembly = Assembly.LoadFile(item);
                    assemblies.Add(assembly);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return assemblies.AsEnumerable();
        }

        /// <summary>
        /// 表示互斥类型列表是否已经被当前<see cref="BasicManager"/>注册。
        /// </summary>
        /// <param name="mutexCategory"></param>
        /// <returns>若返回true表示尚未注册。</returns>
        public bool IsMutexCategoryRegisted(IEnumerable<string> mutexCategory)
        {
            return !(mutexCategory.Intersect(this.RegistedMutexCategories).Count() == 0);
        }

        /// <summary>
        /// 判断当前类型的MutexCategory是否已被注册，若没有注册，则创建相应的
        /// <see cref="ManagedPluginItem"/>并将其添加到
        /// <see cref="RegistedMutexCategories"/>列表中。
        /// </summary>
        /// <param name="type">要添加管理的类型。</param>
        /// <exception cref="ArgumentException"/>
        public virtual void AppendPluginManagedItem(Type type)
        {
            if (type.GetInterfaces().Contains(typeof(IManageable)))
            {
                var mutex = ManagedPluginItem.GetTypeMutexCategories(type);

                if (mutex.Intersect(this.RegistedMutexCategories).Count() == 0)
                {
                    RegistedMutexCategories = RegistedMutexCategories.Concat(mutex).ToList();
                    Plugins.Add(ManagedPluginItem.CreateManagedPluginItem(type));
                }
                else
                {

                    if (OnMutexPluginConflict == null)
                    {
                        throw new ArgumentException("已有互斥类型注册。");
                    }
                    else
                    {
                        OnMutexPluginConflict.Invoke(type);
                    }
                }
            }
        }

        /// <summary>
        /// 按照一定的顺序开始加载dll文件，并将其中的插件载入。
        /// </summary>
        public void DllLoadingChain()
        {
            var dllPaths = this.WorkDirectoryDllFilesScan();
            var assemblies = this.LoadAssembliesFromDlls(dllPaths);

            foreach (var ass in assemblies)
            {
                var types = ass.GetTypes();
                foreach (var type in types)
                {
                    this.AppendPluginManagedItem(type);
                }
            }
        }
        /// <summary>
        /// 当检测到冲突时引发事件。若该事件未受任何订阅，<see cref="AppendPluginManagedItem(Type)"/>方法将抛出<see cref="ArgumentException"/>。
        /// </summary>
        public event Action<Type> OnMutexPluginConflict;

        /// <summary>
        /// 用于处理不同接口响应的一般策略。
        /// </summary>
        public event Action<Type> InterfaceViewStratagy;
    }
}

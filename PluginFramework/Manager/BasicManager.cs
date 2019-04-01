using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using PluginFramework.Model;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

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
    [ComVisible(true)]
    public static class BasicManager
    {
        /// <summary>
        /// 所有托管的插件。
        /// </summary>
        public static ObservableCollection<Extension> Plugins { get; private set; }
        
        /// <summary>
        /// 当前管理器的工作目录。
        /// </summary>
        public static string WorkDirectory { get; set; }

        /// <summary>
        /// 当执行目录扫描方法时的扫描参数。
        /// </summary>
        public static DirectoryManageOption ScanOption { get; set; }

        static BasicManager()
        {
            Plugins = new ObservableCollection<Extension>();
        }

        #region GetComponents
        /// <summary>
        /// 获取所有类型为T、继承自T或者实现了接口T的对象。
        /// </summary>
        /// <typeparam name="T">源类型。</typeparam>
        /// <returns>所有类型为T、继承自T或者实现了接口T的对象。</returns>
        public static IEnumerable<T> GetComponents<T>()
        {
            var query = from plugin in Plugins
                        where plugin is T
                        select (T)(object)plugin;
            return query;
        }

        /// <summary>
        /// 获取所有类型为T、继承自T或者实现了接口T的对象
        /// ，并按照过滤器规则过滤。
        /// </summary>
        /// <typeparam name="T">源类型。</typeparam>
        /// <param name="filter">过滤器。</param>
        /// <returns>所有类型为T、继承自T或者实现了接口T，并按照filter过滤的对象。</returns>
        public static IEnumerable<T> GetComponents<T>(Predicate<T> filter)
        {
            var query = from plugin in Plugins
                        where plugin is T && filter((T)(object)plugin)
                        select (T)(object)plugin;
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
        public static IEnumerable<string> DirectoryPluginScan(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException("工作目录未正确设置。");
            }
            List<string> files = new List<string>();

            if (ScanOption == 0)
            {
                // TODO: 递归查找所有的dll
                files = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories).ToList();
            }
            else
            {
                if (((int)ScanOption & 0x01) != 0)
                {
                    // 查找根目录中所有的dll
                    files = Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly).ToList();
                }

                if (((int)ScanOption & 0x02) != 0)
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
        public static IEnumerable<string> WorkDirectoryDllFilesScan()
        {
            return DirectoryPluginScan(WorkDirectory);
        }
        
        #endregion
        /// <summary>
        /// 从给定路径数组加载所有的<see cref="Assembly"/>。
        /// </summary>
        /// <param name="dllFiles">待加载的所有路径。</param>
        /// <returns>返回所有已加载的Assembly。</returns>
        /// <exception cref="FileNotFoundException"/>
        public static IEnumerable<Assembly> LoadAssembliesFromDlls(IEnumerable<string> dllFiles)
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
            return assemblies;
        }
    }
}

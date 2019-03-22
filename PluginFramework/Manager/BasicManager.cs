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
        public List<ManagedPluginItem> Plugins { get; private set; }

        /// <summary>
        /// 获取所有的可加载项。
        /// </summary>
        public IEnumerable<ManagedPluginItem> 
            LoadList { get { return GetComponents<ILoadable>(); } }
        
        /// <summary>
        /// 当前管理器的工作目录。
        /// </summary>
        public string WorkDirectory { get; set; }
        
        public DirectoryManageOption ScanOption { get; set; }

        public BasicManager()
        {
            Plugins = new List<ManagedPluginItem>();
        }

        #region GetComponents
        public IEnumerable<ManagedPluginItem> GetComponents<T>()
        {
            var query = from plugin in Plugins
                        where plugin.Plugin is T
                        select plugin;
            return query;
        }

        public IEnumerable<ManagedPluginItem> GetComponents<T>(Predicate<T> predicate)
        {
            var query = from plugin in Plugins
                        where plugin.Plugin is T && predicate((T)plugin.Plugin)
                        select plugin;
            return query;
        }
        #endregion

        #region Scanner

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
        
        public virtual IEnumerable<string> WorkDirectoryPluginScan()
        {
            return DirectoryPluginScan(WorkDirectory);
        }
        
        #endregion
        
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

        public virtual void AppendPluginManagedItems(IEnumerable<Assembly> assemblies)
        {
            foreach (var ass in assemblies)
            {
                var types = ass.GetTypes();
                foreach (var type in types)
                {
                    if (type.GetInterfaces().Contains(typeof(IManageable)))
                    {
                        Plugins.Add(ManagedPluginItem.CreateManagedPluginItem(type));
                    }
                }
            }
        }


        public virtual void PluginRegistStratagy()
        {
            OnPluginRegist?.Invoke();
            PluginRegisted?.Invoke();
        }

        public event Action OnPluginRegist;

        public event Action PluginRegisted;

    }

}

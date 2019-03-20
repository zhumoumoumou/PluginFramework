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
        private List<object> plugins;

        /// <summary>
        /// 获取所有的可加载项。
        /// </summary>
        public IEnumerable<ILoadable> LoadList { get { return GetComponents<ILoadable>(); } }
        
        /// <summary>
        /// 当前管理器的工作目录。
        /// </summary>
        public string WorkDirectory { get; set; }
        
        public DirectoryManageOption ScanOption { get; set; }

        public BasicManager()
        {
            plugins = new List<object>();
        }

        #region GetComponents
        public IEnumerable<T> GetComponents<T>()
        {
            var query = from plugin in plugins
                        where plugin is T
                        select (T)plugin;
            return query;
        }

        public IEnumerable<T> GetComponents<T>(Predicate<T> predicate)
        {
            var query = from plugin in plugins
                        where plugin is T && predicate((T)plugin)
                        select (T)plugin;
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

        public virtual IEnumerable<ManagedPluginItem> CreatePluginManagedItems(IEnumerable<Assembly> assemblies)
        {
            List<ManagedPluginItem> managedPluginItems = new List<ManagedPluginItem>();





            return managedPluginItems;
        }


    }

    public class ManagedPluginItem
    {
        /// <summary>
        /// 插件的友好名称。
        /// </summary>
        public string FirendlyName { get; private set; }
        /// <summary>
        /// 插件的开发者备注。
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// 插件的非互斥类目。
        /// </summary>
        public IEnumerable<string> Category { get; private set; }
        /// <summary>
        /// 插件的互斥类目。
        /// </summary>
        public IEnumerable<string> MutexCategory { get; private set; }
        /// <summary>
        /// 插件实现的接口。
        /// </summary>
        public IEnumerable<Type> ImplementedInterfaces { get; private set; }
        /// <summary>
        /// 插件实例。
        /// </summary>
        public object Plugin { get; set; }

        public ManagedPluginItem()
        {
            Category = new List<string>();
            MutexCategory = new List<string>();
            ImplementedInterfaces = new List<Type>();
        }


        #region Operations
        /// <summary>
        /// 判断两插件是否互斥。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns>若不互斥，返回true。</returns>
        public static bool IsTwoItemMutex(ManagedPluginItem item1, ManagedPluginItem item2)
        {
            IEnumerable<string> concatMutexCategory = item1.MutexCategory.Concat(item2.MutexCategory);

            IEnumerable<string> dropDuplicated = concatMutexCategory.Distinct();

            return concatMutexCategory.Count() == dropDuplicated.Count();
        }

        /// <summary>
        /// 判断插件是否不受互斥类目的影响而可以插入。
        /// </summary>
        /// <param name="registedMutexCategory">已记录的互斥类目。</param>
        /// <param name="item">欲判断的新插件。</param>
        /// <returns>若可以插入，则返回true。</returns>
        public static bool CanPluginInsert(IEnumerable<string> registedMutexCategory, ManagedPluginItem item)
        {
            IEnumerable<string> concatMutexCategory = registedMutexCategory.Concat(item.MutexCategory);

            IEnumerable<string> dropDuplicated = concatMutexCategory.Distinct();

            return concatMutexCategory.Count() == dropDuplicated.Count();
        }
        #endregion

        /// <summary>
        /// 从类型的自定义属性中创建<see cref="ManagedPluginItem"/>实例。
        /// </summary>
        /// <param name="targetType">目标类型。</param>
        /// <returns><see cref="ManagedPluginItem"/>的实例</returns>
        public static ManagedPluginItem CreateManagedPluginItemFromAttributes(Type targetType)
        {
            var attributes = targetType.GetCustomAttributes();
            ManagedPluginItem item = new ManagedPluginItem
            {
                Plugin = Activator.CreateInstance(targetType)
            };
            foreach (var attribute in attributes)
            {
                if (attribute is Attribute.PluginInfoAttribute)
                {
                    var info = attribute as Attribute.PluginInfoAttribute;
                    item.FirendlyName = info.FriendlyName;
                    item.Description = info.Description;
                }
                if (attribute is Attribute.PluginCategoryAttribute)
                {
                    var info = attribute as Attribute.PluginCategoryAttribute;
                    if (info.IsMutex)
                    {
                        ((List<string>)item.MutexCategory).Add(info.Category);
                    }
                    else
                    {
                        ((List<string>)item.Category).Add(info.Category);
                    }
                }
            }

            var interfaces = targetType.GetInterfaces();
            foreach (var i in interfaces)
            {
                ((List<Type>)item.ImplementedInterfaces).Add(i);
            }

            return item;
        }
    }
}

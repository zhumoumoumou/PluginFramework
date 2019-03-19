using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual void WorkDirectoryPluginScan()
        {
            if (!Directory.Exists(WorkDirectory))
            {
                throw new DirectoryNotFoundException("工作目录未正确设置。");
            }

            if (this.ScanOption == 0)
            {
                // TODO: 递归查找所有的dll
            }

            if (((int)this.ScanOption & 0x01 ) != 0)
            {
                // 查找根目录中所有的dll
            }

            if (((int)this.ScanOption & 0x02) != 0)
            {
                // 查找所有归类的dll

            }

        }

        /// <summary>
        ///  TODO: 扫描指定目录下是否存在插件。所有的文件夹都需要用文件夹分装。
        /// </summary>
        /// <param name="dir">已设置的插件文件夹。</param>
        protected virtual void PluginScanDirClassified(string dir)
        {
            // 列出所有的子文件夹
            var paths = Directory.GetDirectories(dir);
            foreach (var path in paths)
            {
                string[] dllFiles = Directory.GetFiles(path, "*.dll");
            }
        }
        #endregion

        #region Compose

        #endregion
    }
}

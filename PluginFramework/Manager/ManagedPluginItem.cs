using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Manager
{
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
        /// 插件类型。
        /// </summary>
        public Type PluginType { get; set; }
        /// <summary>
        /// 插件实例。
        /// </summary>
        public object Plugin { get; set; }

        ManagedPluginItem() { }
        
        #region Operations
        /// <summary>
        /// 判断两插件是否互斥。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns>若不互斥，返回true。</returns>
        public static bool IsTwoItemMutex
            (ManagedPluginItem item1, ManagedPluginItem item2)
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
        public static bool CanPluginInsert
            (IEnumerable<string> registedMutexCategory, ManagedPluginItem item)
        {
            IEnumerable<string> concatMutexCategory = registedMutexCategory.Concat(item.MutexCategory);

            IEnumerable<string> dropDuplicated = concatMutexCategory.Distinct();

            return concatMutexCategory.Count() == dropDuplicated.Count();
        }

        /// <summary>
        /// 从类型的自定义属性中创建<see cref="ManagedPluginItem"/>实例。
        /// </summary>
        /// <param name="targetType">目标类型。</param>
        /// <returns><see cref="ManagedPluginItem"/>的实例</returns>
        public static ManagedPluginItem CreateManagedPluginItem(Type targetType)
        {
            var interfaces = targetType.GetInterfaces();

            if (!interfaces.Contains(typeof(Interface.IManageable)))
            {
                throw new ArgumentException(
                    targetType.FullName + 
                    " 并未实现接口 " + 
                    typeof(Interface.IManageable).FullName + 
                    " ，不可进行管理。");
            }

            var attributes = targetType.GetCustomAttributes();
            List<string> category = new List<string>();
            List<string> mutexCategory = new List<string>();
            ManagedPluginItem item = new ManagedPluginItem
            {
                Plugin = Activator.CreateInstance(targetType),
                PluginType = targetType
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
                        mutexCategory.Add(info.Category);
                    }
                    else
                    {
                        category.Add(info.Category);
                    }
                }
            }

            item.Category = category;
            item.MutexCategory = mutexCategory;

            item.ImplementedInterfaces = targetType.GetInterfaces();

            return item;
        }
        
        /// <summary>
        /// 获取类型的所有互斥类型。
        /// </summary>
        /// <param name="targetType">目标类型。</param>
        /// <returns>目标类型所含的所有互斥类型属性。</returns>
        public static IEnumerable<string> GetTypeMutexCategories(Type targetType)
        {
            var query = from att 
                        in targetType.
                        GetCustomAttributes<Attribute.PluginCategoryAttribute>()
                        where att.IsMutex
                        select att.Category;

            return query;
        }

        #endregion

    }
}

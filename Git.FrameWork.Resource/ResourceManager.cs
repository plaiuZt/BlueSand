using Git.Framework.Cache;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Io;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;

namespace Git.Framework.Resource
{

    public class ResourceManager
    {
        private static Git.Framework.Log.Log log = Git.Framework.Log.Log.Instance(typeof(ResourceManager));

        public static CacheEntity GetCacheEntity(string argKey)
        {
            ICache<string, CacheEntity> cache = new Git.Framework.Cache.Cache<string, CacheEntity>();
            return cache.Get(argKey);
        }

        public static CookieEntity GetCookieEntity(string argKey)
        {
            ICache<string, CookieEntity> cache = new Git.Framework.Cache.Cache<string, CookieEntity>();
            if (!argKey.StartsWith("Cookie."))
            {
                argKey = "Cookie." + argKey;
            }
            return cache.Get(argKey);
        }

        public static FileEntity GetFileEntity(string argKey)
        {
            ICache<string, FileEntity> cache = new Git.Framework.Cache.Cache<string, FileEntity>();
            if (!argKey.StartsWith("File."))
            {
                argKey = "File." + argKey;
            }
            return cache.Get(argKey);
        }

        public static FileGroup GetFileGroup(string argKey)
        {
            ICache<string, FileGroup> cache = new Git.Framework.Cache.Cache<string, FileGroup>();
            if (!argKey.StartsWith("FileGroup."))
            {
                argKey = "FileGroup." + argKey;
            }
            return cache.Get(argKey);
        }

        public static FileValidEntity GetFileValidEntity(string argKey)
        {
            ICache<string, FileValidEntity> cache = new Git.Framework.Cache.Cache<string, FileValidEntity>();
            if (!argKey.StartsWith("FileVali."))
            {
                argKey = "FileVali." + argKey;
            }
            return cache.Get(argKey);
        }

        public static MessageEntity GetMessageEntity(string argKey)
        {
            ICache<string, MessageEntity> cache = new Git.Framework.Cache.Cache<string, MessageEntity>();
            if (!argKey.StartsWith("Message."))
            {
                argKey = "Message." + argKey;
            }
            return cache.Get(argKey);
        }

        public static PageEntity GetPageEntity(string argKey)
        {
            ICache<string, PageEntity> cache = new Git.Framework.Cache.Cache<string, PageEntity>();
            if (!argKey.StartsWith("Page."))
            {
                argKey = "Page." + argKey;
            }
            return cache.Get(argKey);
        }

        public static PageEntity GetPageEntityByPath(string argPath)
        {
            Func<PageEntity, bool> func = null;
            ICache<string, PageEntity> cache = new Git.Framework.Cache.Cache<string, PageEntity>();
            PageEntity[] values = cache.GetValues();
            if (values != null)
            {
                if (func == null)
                {
                    func = item => item.PagePath.ToLower() == argPath.ToLower();
                }
                return Enumerable.Where<PageEntity>(values, func).SingleOrDefault<PageEntity>();
            }
            return null;
        }

        public static SeoEntity GetSeoEntity(string argKey)
        {
            ICache<string, SeoEntity> cache = new Git.Framework.Cache.Cache<string, SeoEntity>();
            if (!argKey.StartsWith("Seo."))
            {
                argKey = "Seo." + argKey;
            }
            return cache.Get(argKey);
        }

        public static SettingEntity GetSettingEntity(string argKey)
        {
            ICache<string, SettingEntity> cache = new Git.Framework.Cache.Cache<string, SettingEntity>();
            if (!argKey.StartsWith("Setting."))
            {
                argKey = "Setting." + argKey;
            }
            return cache.Get(argKey);
        }

        private static void Init()
        {
            ICache<string, CacheEntity> cache = new Git.Framework.Cache.Cache<string, CacheEntity>();
            foreach (string str in cache.GetKeys())
            {
                string str2;
                ListenCache cache2;
                if (ConstVariable.GIT_SEO_CACHE_KEY == str)
                {
                    str2 = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_SEO_CACHE_KEY).Dependencies;
                    cache2 = new ListenCache();
                    cache2.ListenCacheFile(new LoadResourceDelegate(ResourceManager.LoadSeoEntity), str2);
                    LoadSeoEntity(null);
                }
                if (ConstVariable.GIT_FILE_CACHE_KEY == str)
                {
                    str2 = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_FILE_CACHE_KEY).Dependencies;
                    cache2 = new ListenCache();
                    cache2.ListenCacheFile(new LoadResourceDelegate(ResourceManager.LoadFile), str2);
                    LoadFile(null);
                }
                if (ConstVariable.GIT_FILEGROUP_CACHE_KEY == str)
                {
                    str2 = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_FILEGROUP_CACHE_KEY).Dependencies;
                    cache2 = new ListenCache();
                    cache2.ListenCacheFile(new LoadResourceDelegate(ResourceManager.LoadFileGroup), str2);
                    LoadFileGroup(null);
                }
                if (ConstVariable.GIT_MESSAGE_CACHE_KEY == str)
                {
                    str2 = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_MESSAGE_CACHE_KEY).Dependencies;
                    cache2 = new ListenCache();
                    cache2.ListenCacheFile(new LoadResourceDelegate(ResourceManager.LoadMessage), str2);
                    LoadMessage(null);
                }
                if (ConstVariable.GIT_SETTING_CACHE_KEY == str)
                {
                    str2 = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_SETTING_CACHE_KEY).Dependencies;
                    cache2 = new ListenCache();
                    cache2.ListenCacheFile(new LoadResourceDelegate(ResourceManager.LoadSettingEntity), str2);
                    LoadSettingEntity(null);
                }
                if (ConstVariable.GIT_COOKIE_CACHE_KEY == str)
                {
                    str2 = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_COOKIE_CACHE_KEY).Dependencies;
                    cache2 = new ListenCache();
                    cache2.ListenCacheFile(new LoadResourceDelegate(ResourceManager.LoadCookieEntity), str2);
                    LoadCookieEntity(null);
                }
                if (ConstVariable.GIT_PAGES_CACHE_KEY == str)
                {
                    str2 = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_PAGES_CACHE_KEY).Dependencies;
                    cache2 = new ListenCache();
                    cache2.ListenCacheFile(new LoadResourceDelegate(ResourceManager.LoadPageEntity), str2);
                    LoadPageEntity(null);
                }
                if (ConstVariable.GIT_FILEVALI_CACHE_KEY == str)
                {
                    str2 = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_FILEVALI_CACHE_KEY).Dependencies;
                    new ListenCache().ListenCacheFile(new LoadResourceDelegate(ResourceManager.LoadFileValiEntity), str2);
                    LoadFileValiEntity(null);
                }
            }
        }

        public static void LoadCache()
        {
            string uri = FileManager.GetRootPath() + ConfigurationManager.AppSettings["CacheConfig"].ToString();
            ICache<string, CacheEntity> cache = new Git.Framework.Cache.Cache<string, CacheEntity>();
            cache.Clear();
            IEnumerable<XElement> enumerable = from item in XDocument.Load(uri).Element("CacheConfig").Elements("cache") select item;
            foreach (XElement element in enumerable)
            {
                CacheEntity argValue = new CacheEntity {
                    Key = element.Attribute("key").Value,
                    AutoLoad = Convert.ToBoolean(element.Attribute("autoLoad").Value),
                    Expires = Convert.ToInt32(element.Attribute("expires").Value),
                    Dependencies = element.Attribute("dependencies").Value
                };
                cache.Insert(argValue.Key, argValue);
            }
            Init();
        }

        public static void LoadCookieEntity(object state)
        {
            Action<FileItem> action1 = null;
            string directoryPath = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_COOKIE_CACHE_KEY).Dependencies;
            ICache<string, CookieEntity> cache = new Git.Framework.Cache.Cache<string, CookieEntity>();
            cache.Clear();
            Action<string> action = delegate (string filePath) {
                IEnumerable<XElement> enumerable = from item in XDocument.Load(filePath).Element("CookieConfig").Elements("cookie") select item;
                foreach (XElement element in enumerable)
                {
                    CookieEntity argValue = new CookieEntity {
                        Key = "Cookie." + element.Attribute("key").Value,
                        Expires = Convert.ToInt64(element.Attribute("expires").Value),
                        IsEncrypt = Convert.ToBoolean(element.Attribute("isEncrypt").Value)
                    };
                    cache.Insert(argValue.Key, argValue);
                }
            };
            if (FileManager.DirectoryExists(directoryPath))
            {
                List<FileItem> fileItems = FileManager.GetFileItems(directoryPath);
                if (!fileItems.IsNullOrEmpty<FileItem>())
                {
                    if (action1 == null)
                    {
                        action1 = item => action(item.FullName);
                    }
                    fileItems.ForEach(action1);
                }
            }
            if (FileManager.FileExists(directoryPath))
            {
                action(directoryPath);
            }
        }

        public static void LoadFile(object state)
        {
            Action<FileItem> action1 = null;
            string directoryPath = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_FILE_CACHE_KEY).Dependencies;
            ICache<string, FileEntity> cache = new Git.Framework.Cache.Cache<string, FileEntity>();
            cache.Clear();
            Action<string> action = delegate (string filePath) {
                IEnumerable<XElement> enumerable = from item in XDocument.Load(filePath).Element("Configs").Elements("file") select item;
                foreach (XElement element in enumerable)
                {
                    FileEntity argValue = new FileEntity {
                        FileName = "File." + element.Attribute("name").Value,
                        FilePath = element.Value,
                        FileType = element.Value.EndsWith(".css") ? EFileType.Css : EFileType.Js
                    };
                    if (element.Attribute("browser").IsNotNull())
                    {
                        argValue.Browser = element.Attribute("browser").Value;
                    }
                    cache.Insert(argValue.FileName, argValue);
                }
            };
            if (FileManager.DirectoryExists(directoryPath))
            {
                List<FileItem> fileItems = FileManager.GetFileItems(directoryPath);
                if (!fileItems.IsNullOrEmpty<FileItem>())
                {
                    if (action1 == null)
                    {
                        action1 = item => action(item.FullName);
                    }
                    fileItems.ForEach(action1);
                }
            }
            if (FileManager.FileExists(directoryPath))
            {
                action(directoryPath);
            }
        }

        public static void LoadFileGroup(object state)
        {
            Action<FileItem> action1 = null;
            string directoryPath = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_FILEGROUP_CACHE_KEY).Dependencies;
            ICache<string, FileGroup> cache = new Git.Framework.Cache.Cache<string, FileGroup>();
            cache.Clear();
            Action<string> action = delegate (string filePath) {
                IEnumerable<XElement> enumerable = from item in XDocument.Load(filePath).Element("Groups").Elements("group") select item;
                foreach (XElement element in enumerable)
                {
                    FileGroup argValue = new FileGroup {
                        Key = "FileGroup." + element.Attribute("name").Value,
                        FileList = new List<FileEntity>(),
                        FileKeys = new List<string>()
                    };
                    foreach (XElement element2 in element.Elements("file"))
                    {
                        argValue.FileKeys.Add(element2.Value);
                        argValue.FileList.Add(GetFileEntity(element2.Value));
                    }
                    cache.Insert(argValue.Key, argValue);
                }
            };
            if (FileManager.DirectoryExists(directoryPath))
            {
                List<FileItem> fileItems = FileManager.GetFileItems(directoryPath);
                if (!fileItems.IsNullOrEmpty<FileItem>())
                {
                    if (action1 == null)
                    {
                        action1 = item => action(item.FullName);
                    }
                    fileItems.ForEach(action1);
                }
            }
            if (FileManager.FileExists(directoryPath))
            {
                action(directoryPath);
            }
        }

        public static void LoadFileValiEntity(object state)
        {
            Action<FileItem> action1 = null;
            string directoryPath = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_FILEVALI_CACHE_KEY).Dependencies;
            ICache<string, FileValidEntity> cache = new Git.Framework.Cache.Cache<string, FileValidEntity>();
            cache.Clear();
            Action<string> action = delegate (string filePath) {
                IEnumerable<XElement> enumerable = from item in XDocument.Load(filePath).Element("File").Elements("SourceType") select item;
                foreach (XElement element in enumerable)
                {
                    action = null;
                    FileValidEntity entity = new FileValidEntity {
                        SourceID = ConvertHelper.ToType<int>(element.Attribute("SourceID").Value, 0),
                        Length = ConvertHelper.ToType<int>(element.Attribute("Length").Value, 0),
                        FileType = element.Attribute("FileType").Value
                    };
                    string targetValue = element.Element("FileSize").Value;
                    string[] collection = null;
                    if (entity.FileSize.IsNullOrEmpty<int[]>())
                    {
                        entity.FileSize = new List<int[]>();
                    }
                    if (!targetValue.IsEmpty())
                    {
                        if (action == null)
                        {
                            action = delegate (string listItem) {
                                int[] item = listItem.Do<string, int[]>(delegate (string value) {
                                    string[] strArray = null;
                                    if (!value.IsEmpty())
                                    {
                                        strArray = value.Split(new char[] { '-' });
                                    }
                                    int[] numArray = new int[2];
                                    if (!(strArray.IsNullOrEmpty<string>() || (strArray.Count<string>() != 2)))
                                    {
                                        numArray[0] = ConvertHelper.ToType<int>(strArray[0], 0);
                                        numArray[1] = ConvertHelper.ToType<int>(strArray[1], 0);
                                    }
                                    return numArray;
                                }, null);
                                entity.FileSize.Add(item);
                            };
                        }
                        targetValue.Split(new char[] { '|' }).ForEach<string>(action);
                    }
                    if (entity.FileExtension.IsNullOrEmpty<string>())
                    {
                        entity.FileExtension = new List<string>();
                    }
                    if (!element.Element("FileExtension").Value.IsEmpty())
                    {
                        collection = element.Element("FileExtension").Value.Split(new char[] { '|' });
                        entity.FileExtension.AddRange(collection);
                    }
                    if (entity.ContentType.IsNullOrEmpty<string>())
                    {
                        entity.ContentType = new List<string>();
                    }
                    if (!element.Element("ContentType").Value.IsEmpty())
                    {
                        collection = element.Element("ContentType").Value.Split(new char[] { '|' });
                        entity.ContentType.AddRange(collection);
                    }
                    entity.FilePath = element.Element("FilePath").Value;
                    cache.Insert("FileVali." + entity.SourceID.ToString(), entity);
                }
            };
            if (FileManager.DirectoryExists(directoryPath))
            {
                List<FileItem> fileItems = FileManager.GetFileItems(directoryPath);
                if (!fileItems.IsNullOrEmpty<FileItem>())
                {
                    if (action1 == null)
                    {
                        action1 = item => action(item.FullName);
                    }
                    fileItems.ForEach(action1);
                }
            }
            if (FileManager.FileExists(directoryPath))
            {
                action(directoryPath);
            }
        }

        public static void LoadMessage(object state)
        {
            Action<FileItem> action1 = null;
            string directoryPath = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_MESSAGE_CACHE_KEY).Dependencies;
            ICache<string, MessageEntity> cache = new Git.Framework.Cache.Cache<string, MessageEntity>();
            cache.Clear();
            Action<string> action = delegate (string filePath) {
                IEnumerable<XElement> enumerable = from item in XDocument.Load(filePath).Element("Messages").Elements("message") select item;
                foreach (XElement element in enumerable)
                {
                    MessageEntity argValue = new MessageEntity {
                        Name = "Message." + element.Attribute("name").Value,
                        Value = element.Value
                    };
                    cache.Insert(argValue.Name, argValue);
                }
            };
            if (FileManager.DirectoryExists(directoryPath))
            {
                List<FileItem> fileItems = FileManager.GetFileItems(directoryPath);
                if (!fileItems.IsNullOrEmpty<FileItem>())
                {
                    if (action1 == null)
                    {
                        action1 = item => action(item.FullName);
                    }
                    fileItems.ForEach(action1);
                }
            }
            if (FileManager.FileExists(directoryPath))
            {
                action(directoryPath);
            }
        }

        public static void LoadPageEntity(object state)
        {
            Action<FileItem> action1 = null;
            string directoryPath = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_PAGES_CACHE_KEY).Dependencies;
            ICache<string, PageEntity> cache = new Git.Framework.Cache.Cache<string, PageEntity>();
            cache.Clear();
            Action<string> action = delegate (string filePath) {
                IEnumerable<XElement> enumerable = from item in XDocument.Load(filePath).Element("PageConfigs").Elements("page") select item;
                foreach (XElement element in enumerable)
                {
                    PageEntity argValue = new PageEntity {
                        Key = "Page." + element.Attribute("name").Value,
                        PagePath = element.Attribute("path").Value,
                        FileGroupList = new List<FileGroup>(),
                        FileGroup = new List<string>()
                    };
                    foreach (XElement element2 in element.Elements("group"))
                    {
                        argValue.FileGroup.Add(element2.Value);
                        argValue.FileGroupList.Add(GetFileGroup(element2.Value));
                    }
                    argValue.SeoKey = element.Element("seo").Value;
                    argValue.SeoEntity = GetSeoEntity(element.Element("seo").Value);
                    cache.Insert(argValue.Key, argValue);
                }
            };
            if (FileManager.DirectoryExists(directoryPath))
            {
                List<FileItem> fileItems = FileManager.GetFileItems(directoryPath);
                if (!fileItems.IsNullOrEmpty<FileItem>())
                {
                    if (action1 == null)
                    {
                        action1 = item => action(item.FullName);
                    }
                    fileItems.ForEach(action1);
                }
            }
            if (FileManager.FileExists(directoryPath))
            {
                action(directoryPath);
            }
        }

        public static void LoadSeoEntity(object state)
        {
            Action<FileItem> action1 = null;
            string directoryPath = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_SEO_CACHE_KEY).Dependencies;
            ICache<string, SeoEntity> cache = new Git.Framework.Cache.Cache<string, SeoEntity>();
            cache.Clear();
            Action<string> action = delegate (string filePath) {
                IEnumerable<XElement> enumerable = from item in XDocument.Load(filePath).Element("PageSEOConfigs").Elements("page") select item;
                foreach (XElement element in enumerable)
                {
                    SeoEntity argValue = new SeoEntity {
                        Key = "Seo." + element.Attribute("name").Value,
                        Title = element.Element("title").Value,
                        KeyWords = element.Element("keywords").Value,
                        Description = element.Element("description").Value
                    };
                    cache.Insert(argValue.Key, argValue);
                }
            };
            if (FileManager.DirectoryExists(directoryPath))
            {
                List<FileItem> fileItems = FileManager.GetFileItems(directoryPath);
                if (!fileItems.IsNullOrEmpty<FileItem>())
                {
                    if (action1 == null)
                    {
                        action1 = item => action(item.FullName);
                    }
                    fileItems.ForEach(action1);
                }
            }
            if (FileManager.FileExists(directoryPath))
            {
                action(directoryPath);
            }
        }

        public static void LoadSettingEntity(object state)
        {
            Action<FileItem> action1 = null;
            string directoryPath = FileManager.GetRootPath() + GetCacheEntity(ConstVariable.GIT_SETTING_CACHE_KEY).Dependencies;
            ICache<string, SettingEntity> cache = new Git.Framework.Cache.Cache<string, SettingEntity>();
            cache.Clear();
            Action<string> action = delegate (string filePath) {
                IEnumerable<XElement> enumerable = from item in XDocument.Load(filePath).Element("Settings").Elements("setting") select item;
                foreach (XElement element in enumerable)
                {
                    SettingEntity argValue = new SettingEntity {
                        Name = "Setting." + element.Attribute("key").Value,
                        Value = element.Attribute("value").Value
                    };
                    cache.Insert(argValue.Name, argValue);
                }
            };
            if (FileManager.DirectoryExists(directoryPath))
            {
                List<FileItem> fileItems = FileManager.GetFileItems(directoryPath);
                if (!fileItems.IsNullOrEmpty<FileItem>())
                {
                    if (action1 == null)
                    {
                        action1 = item => action(item.FullName);
                    }
                    fileItems.ForEach(action1);
                }
            }
            if (FileManager.FileExists(directoryPath))
            {
                action(directoryPath);
            }
        }
    }
}


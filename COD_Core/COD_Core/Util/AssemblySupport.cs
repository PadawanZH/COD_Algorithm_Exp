using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace COD_Base.Util
{
    public class AssemblySupport
    {
        /// <summary>
		/// One element of internal list of assemblies
		/// </summary>
		private class AssemblyItem
        {
            public Assembly assembly;
            public string fullName;
            public FileInfo fileInfo;
        }

        /// <summary>
        /// Internal list of assemblies.
        /// </summary>
        private static List<AssemblyItem> _assemblies;

        /// <summary>
        /// Loads specific assembly into internal list of assemblies.
        /// </summary>
        /// <param name="directory">Directory <c>filename</c> is relative to, or <c>null</c> if <c>filename</c> is absolute or relative to current directory.</param>
        /// <param name="filename">Relative or absolute path to assembly.</param>
        /// <remarks>See <see cref="Utils.GetFileInfo">Utils.GetFileInfo</see> for more info about how
        /// specified file is searched. If file isn't found, method tries to
        /// load assembly from global assembly cache (GAC).</remarks>
        public static Assembly LoadAssembly(string directory, string filename)
        {
            Assembly assembly;

            FileInfo assemblyFileInfo = GetFileInfo(directory, filename);

            // if assemby file exists, try to load it
            if (assemblyFileInfo.Exists)
            {
                assembly = Assembly.LoadFrom(assemblyFileInfo.FullName);
            }
            else
            {
                // if file doesn't exist, try to load assembly from GAC
                try
                {
                    assembly = Assembly.Load(filename);
                }
                catch (Exception e)
                {
                    throw (new Exception("Assembly cannot be loaded (CurrentDirectory='" + Directory.GetCurrentDirectory() + "', Name='" + filename + "')", e));
                }
            }

            // add assembly to list of assemblies only if not already present
            foreach (AssemblyItem assemblyItem in _assemblies)
                if (0 == String.Compare(assemblyItem.fullName, assembly.FullName, true))
                    return assembly;

            AssemblyItem newItem = new AssemblyItem();
            newItem.assembly = assembly;
            newItem.fullName = assembly.FullName;
            newItem.fileInfo = assemblyFileInfo;
            _assemblies.Add(newItem);
            return assembly;
        }


        /// <summary>
        /// Creates new instance of type contained in one previously loaded assembly, or from application context if 
        /// not found.
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <returns>Returns new instance of specified type.</returns>
        /// <remarks>New instance is created with default parameterless constructor,
        /// if such constructor doesn't exists an exception is thrown.</remarks>
        public static object GetNewInstance(string typeName)
        {
            AppDomain.CurrentDomain.AssemblyResolve += FindAssemblies;

            object result;
            Type type = null;

            foreach (AssemblyItem assemblyItem in _assemblies)
            {
                type = assemblyItem.assembly.GetType(typeName, false);
                if (type != null)
                    break;
            }

            if (type == null)
                type = Type.GetType(typeName, false);

            if (type == null)
                throw (new Exception("Class type " + typeName + " not found neither in loaded assemblies nor in application context."));

            // construct new item with default constructor
            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
            if (constructorInfo == null)
                throw (new Exception("Requested class type has no default parameterless constructor."));

            result = constructorInfo.Invoke(null);
            return (result);
        }

        public static List<string> GetClassNameImplementedWithType(Type interfaceType)
        {
            AppDomain.CurrentDomain.AssemblyResolve += FindAssemblies;

            List<string> result = new List<string>();
            Type type = null;

            foreach (AssemblyItem assemblyItem in _assemblies)
            {
                foreach(Type tp in assemblyItem.assembly.ExportedTypes)
                {
                    if (tp.GetInterface(interfaceType.Name) != null){
                        result.Add(tp.FullName);
                    }   
                }
            }
            return result;   
        }

        /// <summary>
        /// 失败时调用，查找同目录下是否有对应的dll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static Assembly FindAssemblies(object sender, ResolveEventArgs args)
        {
            string name = new AssemblyName(args.Name).Name + ".dll";

            FileSystemInfo fi;
            Assembly a = null;

            foreach (AssemblyItem ai in _assemblies)
            {
                fi = new FileInfo(Path.Combine(ai.fileInfo.Directory.FullName, name));
                if (fi.Exists)
                    a = Assembly.LoadFrom(fi.FullName);
                if (a != null)
                    return a;
            }

            return a;
        }

        public static FileInfo GetFileInfo(string relativeDir, string filename)
        {
            string oldDirectory = null;
            if (relativeDir != null)
                oldDirectory = Directory.GetCurrentDirectory();

            try
            {
                if (relativeDir != null)
                    Directory.SetCurrentDirectory(relativeDir);
                return (new FileInfo(filename));
            }
            finally
            {
                if (oldDirectory != null)
                    Directory.SetCurrentDirectory(oldDirectory);
            }
        }

        public static void ReleaseAll()
        {
            _assemblies = new List<AssemblyItem>();
            GC.Collect();
        }

        static AssemblySupport()
        {
            _assemblies = new List<AssemblyItem>();
        }
    }
}


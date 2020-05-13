using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using GoAhead.FPGA;
using System.Reflection;
using System.Collections;
using GoAhead.Commands;
using System.Windows.Forms;
using System.IO;

namespace GoAhead
{
    public class TclDLL
    {
        private const string dll_name = "tcl86t.dll";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate int Tcl_ObjCmdProc(
            IntPtr clientData,
            IntPtr interp,
            int objc,
            IntPtr* argv
        );

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Tcl_CreateObjCommand(
            IntPtr interp,
            [In, MarshalAs(UnmanagedType.LPStr)] string cmdName,
            IntPtr proc,
            IntPtr clientData,
            IntPtr deleteProc
        );

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Tcl_CreateInterp();

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Tcl_Eval(IntPtr interp, string skript);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Tcl_EvalObjEx(IntPtr interp, IntPtr objPtr, int flags);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Tcl_GetObjResult(IntPtr interp);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Tcl_SetObjResult(IntPtr interp, IntPtr objPtr);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Tcl_ResetResult(IntPtr interp);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern char* Tcl_GetStringFromObj(IntPtr tclObj, IntPtr length);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern IntPtr Tcl_GetString(IntPtr obj);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Tcl_NewIntObj(int intValue);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Tcl_NewBooleanObj(int boolValue);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Tcl_NewDoubleObj(double doubleValue);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Tcl_NewStringObj(string bytes, int length);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Tcl_NewListObj(int objc, IntPtr[] objv);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Tcl_NewDictObj();

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Tcl_DictObjPut(IntPtr interp, IntPtr dict, IntPtr key, IntPtr value);

        public static unsafe void Helper_RegisterProc(IntPtr interp, string name, Tcl_ObjCmdProc proc)
        {
            Tcl_CreateObjCommand(
                    interp: interp,
                    cmdName: name,
                    proc: Marshal.GetFunctionPointerForDelegate(proc),
                    clientData: IntPtr.Zero,
                    deleteProc: IntPtr.Zero
            );

            TCL.CommandsList.RegisterExtraCommand(name);
        }
    }

    public class TclInterpreter
    {
        public const int TCL_ERROR = 1;
        public const int TCL_RETURN = 2;
        public const int TCL_BREAK = 3;
        public const int TCL_CONTINUE = 4;
        public const int TCL_WARNING = 5;

        private string error = "";
        public string ErrorMessage
        {
            get
            {
                string res = error;
                error = "";
                return res;
            }

            set { error = value; }
        }

        public IntPtr ptr;
        public Form context;

        public TclInterpreter()
        {
            ptr = TclDLL.Tcl_CreateInterp();
            if (ptr == IntPtr.Zero)
            {
                throw new SystemException("Can not initialize Tcl interpreter");
            }
        }

        public int EvalScript(string script)
        {
            //return TclDLL.Tcl_EvalObjEx(ptr, TclAPI.GetTclObject(script), 0);
            return TclDLL.Tcl_Eval(ptr, script);
        }

        unsafe public string Result
        {
            get
            {
                IntPtr obj = TclDLL.Tcl_GetObjResult(ptr);
                if (obj == IntPtr.Zero)
                {
                    return "";
                }
                else
                {
                    return Marshal.PtrToStringAnsi((IntPtr)TclDLL.Tcl_GetStringFromObj(obj, IntPtr.Zero));
                }
            }
        }
    }

    public static class TclAPI
    {
        private const string r_code = "InternalCode";
        public static Dictionary<uint, Tuple<object, Type>> RegisteredTclStructs = new Dictionary<uint, Tuple<object, Type>>();

        public static void ResetContext()
        {
            RegisteredTclStructs = new Dictionary<uint, Tuple<object, Type>>();
            classes = null;
            singletons = null;
            enums = null;
        }

        private static List<Type> classes = null;
        public static List<Type> Classes
        {
            get
            {
                if (classes == null)
                {
                    classes = new List<Type>();
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("GoAhead")))
                    {
                        classes.AddRange(a.GetTypes());
                    }
                }
                return classes;
            }
        }

        private static List<object> singletons = null;
        public static List<object> Singletons
        {
            get
            {
                if(singletons == null)
                {
                    singletons = new List<object>();
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("GoAhead")))
                    {
                        foreach (Type t in a.GetTypes())
                        {
                            singletons.AddRange(t.GetFields().Where(m => m.Name == "Instance" && !m.FieldType.ContainsGenericParameters)
                                .Select(f => f.GetValue(null)));
                        }
                    }
                }
                return singletons;
            }
        }

        private static List<Type> enums = null;
        public static List<Type> Enums
        {
            get
            {
                if (enums == null)
                {
                    enums = new List<Type>();
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("GoAhead")))
                    {
                        enums.AddRange(a.GetTypes().Where(t => t.IsEnum));
                    }
                }
                return enums;
            }
        }

        public static object GetSingleton(string name)
        {
            var candidates = Singletons.Where(x => x.GetType().Name.Contains(name));
            if (candidates.Count() == 1)
            {
                return candidates.FirstOrDefault();
            }

            return null;
        }

        public static string GetCsString(IntPtr ptr)
        {
            return Marshal.PtrToStringAnsi(TclDLL.Tcl_GetString(ptr));
        }

        public static Tuple<object, Type> GetRegisteredCsObject(IntPtr tclObj)
        {
            string[] str = GetCsString(tclObj).Split(' ');
            for(int i=0; i<str.Length; i++)
            {
                if(str[i] == r_code)
                {
                    if(i+1 < str.Length && uint.TryParse(str[i + 1], out uint code))
                    {
                        if(RegisteredTclStructs.ContainsKey(code))
                        {
                            return RegisteredTclStructs[code];
                        }
                    }
                }
            }

            return null;
        }

        public static Type GetCsTypeFromName(string fullName)
        {
            Type objType = null;

            foreach (Type c in Classes)
            {
                if (c.FullName == fullName)
                {
                    objType = c;
                }
            }

            return objType;
        }

        /// <summary>
        /// Tries to find a registered cs object, if unsuccessful looks for a singleton
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns>null if no object is found</returns>
        public static object FindCsObject(IntPtr pointer)
        {
            object obj = null;

            // Try to find a registered cs object
            var objTuple = GetRegisteredCsObject(pointer);

            // Otherwise try to find a singleton
            if (objTuple == null)
            {
                string name = GetCsString(pointer);
                obj = GetSingleton(name);
            }
            else
            {
                try
                {
                    obj = Convert.ChangeType(objTuple.Item1, objTuple.Item2);
                }
                catch(Exception)
                {
                    return null;
                }
            }

            return obj;
        }

        public static string GetStringLabelForType(Type type)
        {
            if (type == typeof(int))
            {
                return "i:";
            }
            else if (type == typeof(uint))
            {
                return "u:";
            }
            else if (type == typeof(double) || type == typeof(float))
            {
                return "d:";
            }
            else if (type == typeof(bool))
            {
                return "b:";
            }
            else if (type == typeof(string))
            {
                return "s:";
            }
            else if (type.IsEnum)
            {
                return "e:" + type.Name + ".";
            }
            else if (type == typeof(IEnumerable<>))
            {
                return "l:" + GetStringLabelForType(type.GetGenericArguments()[0]);
            }
            else
            {
                return "o:";
            }
        }

        private static object LabeledStr2CsPrimitive(string str)
        {
            try
            {
                if (str.StartsWith("i:"))
                {
                    return Convert.ChangeType(str.Substring(2), typeof(int));
                }
                else if (str.StartsWith("u:"))
                {
                    return Convert.ChangeType(str.Substring(2), typeof(uint));
                }
                else if (str.StartsWith("d:"))
                {
                    return Convert.ChangeType(str.Substring(2), typeof(double));
                }
                else if (str.StartsWith("b:"))
                {
                    return Convert.ChangeType(str.Substring(2), typeof(bool));
                }
                else if (str.StartsWith("s:"))
                {
                    return str.Substring(2);
                }
                else if (str.StartsWith("e:"))
                {
                    str = str.Substring(2);
                    int pos = str.LastIndexOf('.');
                    string enumTypeName = str.Substring(0, pos);
                    string enumConstName = str.Substring(pos + 1);

                    return Enum.Parse(Enums.Where(e => e.Name.Equals(enumTypeName)).FirstOrDefault(), enumConstName);
                }
            }
            catch(Exception) { }

            return null;
        }

        public static object Tcl2Cs(IntPtr ptr, string prefix = "")
        {
            string arg = prefix + GetCsString(ptr);

            try
            {
                if (arg.StartsWith("l:"))
                {
                    string[] elements = arg.Substring(4).Split(' ');
                    List<object> list = new List<object>();
                    for (int i = 0; i < elements.Length; i++)
                    {
                        object p = LabeledStr2CsPrimitive(arg.Substring(2, 2) + elements[i]);
                        if (p == null) throw new ArgumentException();
                        list.Add(p);
                    }
                    return list;
                }
                else if (arg.StartsWith("o:"))
                {
                    string[] elements = arg.Substring(2).Split(' ');
                    for (int i = 0; i < elements.Length; i++)
                    {
                        if (elements[i].Equals(r_code))
                        {
                            uint code = (uint)Convert.ChangeType(elements[i + 1], typeof(uint));
                            return RegisteredTclStructs[code].Item1;
                        }
                    }
                }
                else
                {
                    return LabeledStr2CsPrimitive(arg);
                }
            }
            catch (Exception) { }

            return null;
        }

        public static IntPtr Cs2Tcl(object obj)
        {
            if (obj == null)
            {
                return TclDLL.Tcl_NewStringObj("", 0);
            }
            else if (obj is int i)
            {
                return TclDLL.Tcl_NewIntObj(i);
            }
            else if (obj is uint u)
            {
                return TclDLL.Tcl_NewIntObj((int)u);
            }
            else if (obj is double d)
            {
                return TclDLL.Tcl_NewDoubleObj(d);
            }
            else if (obj is float f)
            {
                return TclDLL.Tcl_NewDoubleObj(f);
            }
            else if (obj is bool b)
            {
                return TclDLL.Tcl_NewBooleanObj(b ? 1 : 0);
            }
            else if (obj is string s)
            {
                return TclDLL.Tcl_NewStringObj(s, s.Length);
            }
            else if (obj.GetType().IsGenericType && 
                (obj.GetType().GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                 obj.GetType().GetGenericTypeDefinition() == typeof(List<>)))
            {
                IEnumerable<object> e = ((IEnumerable)obj).Cast<object>();
                return GetTclList(e);
            }
            else
            {
                return GetTclStruct(obj, Program.mainInterpreter.ptr);
            }
        }

        public static IntPtr GetTclList<T>(IEnumerable<T> enumerable)
        {
            int length = enumerable.Count();
            IntPtr[] ptrs = new IntPtr[length];

            int i = 0;
            foreach(var e in enumerable)
            {
                ptrs[i] = Cs2Tcl(e);
                i++;
            }

            return TclDLL.Tcl_NewListObj(length, ptrs);
        }

        public static IntPtr GetTclStruct(object obj, IntPtr interp)
        {
            var reg = RegisteredTclStructs.Where(s => s.Value.Item1.Equals(obj));
            uint code =
                reg.Count() == 1 ?
                reg.FirstOrDefault().Key :
                (uint) RegisteredTclStructs.Keys.Count;

            IntPtr dic = TclDLL.Tcl_NewDictObj();
            TclDLL.Tcl_DictObjPut(interp, dic, Cs2Tcl("Type"), Cs2Tcl(obj.GetType().ToString()));
            TclDLL.Tcl_DictObjPut(interp, dic, Cs2Tcl("InternalCode"), Cs2Tcl(code));

            // Register cs object if new
            if(!RegisteredTclStructs.ContainsKey(code))
                RegisteredTclStructs.Add(code, new Tuple<object, Type>(obj, obj.GetType()));

            return dic;
        }

        public static bool IsTclPrimitive(Type type)
        {
            return type == typeof(int) ||
                   type == typeof(double) ||
                   type == typeof(float) ||
                   type == typeof(bool) ||
                   type == typeof(string) ||
                   (type.IsGenericType && (
                   type.GetGenericTypeDefinition() == typeof(IEnumerable<int>) ||
                   type.GetGenericTypeDefinition() == typeof(IEnumerable<double>) ||
                   type.GetGenericTypeDefinition() == typeof(IEnumerable<float>) ||
                   type.GetGenericTypeDefinition() == typeof(IEnumerable<bool>) ||
                   type.GetGenericTypeDefinition() == typeof(IEnumerable<string>)));   
        }

        public static IEnumerable<PropertyInfo> GetTclPrimitiveProperties(Type type)
        {
            return type.GetProperties().Where(m => IsTclPrimitive(m.PropertyType));  
        }

        public static IEnumerable<MethodInfo> GetTclPrimitiveMethods(Type type)
        {
            foreach(var method in type.GetMethods())
            {
                bool valid = true;
                foreach(var param in method.GetParameters())
                {
                    if(!IsTclPrimitive(param.ParameterType))
                    {
                        valid = false;
                        break;
                    }
                }
                if(valid)
                {
                    yield return method;
                }
            }
        }

        public static bool IsAPICompatibleMember(MemberInfo member)
        {
            return //member.MemberType == MemberTypes.Field ||
                   member.MemberType == MemberTypes.Property ||
                   member.MemberType == MemberTypes.Method;  
        }
    }
}
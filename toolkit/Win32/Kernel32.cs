﻿//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2010-2012 Garrett Serack and CoApp Contributors. 
//     Contributors can be discovered using the 'git log' command.
//     All rights reserved.
// </copyright>
// <license>
//     The software is licensed under the Apache 2.0 License (the "License")
//     You may not use the software except in compliance with the License. 
// </license>
//-----------------------------------------------------------------------

namespace CoApp.Toolkit.Win32 {
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using Microsoft.Win32.SafeHandles;

    public static class Kernel32 {
#if !COAPP_ENGINE_CORE
        public delegate bool ConsoleHandlerRoutine(ConsoleEvents eventId);
#endif

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SearchPath(
            string lpPath,
            string lpFileName,
            string lpExtension,
            int nBufferLength,
            [MarshalAs(UnmanagedType.LPTStr)]
                StringBuilder lpBuffer,
            out IntPtr lpFilePart);

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern int GlobalAddAtom(string name);

        [DllImport("kernel32.dll")]
        public static extern int GlobalDeleteAtom(int atom);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DeviceIoControl(SafeFileHandle hDevice, ControlCodes dwIoControlCode, IntPtr InBuffer, int nInBufferSize, IntPtr OutBuffer,
            int nOutBufferSize, out int pBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(string name, NativeFileAccess access, FileShare share, IntPtr security, FileMode mode,
            NativeFileAttributesAndFlags flags, IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize,
            IntPtr vaListArguments);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetFileAttributes(string fileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetFileSizeEx(SafeFileHandle handle, out long size);

        [DllImport("kernel32.dll")]
        public static extern FileType GetFileType(SafeFileHandle handle);

        [DllImport("kernel32.dll", EntryPoint = "CreateSymbolicLinkW", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string name);

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BackupRead(SafeFileHandle hFile, ref Win32StreamId pBuffer, int numberOfBytesToRead, out int numberOfBytesRead,
            [MarshalAs(UnmanagedType.Bool)] bool abort, [MarshalAs(UnmanagedType.Bool)] bool processSecurity, ref IntPtr context);

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BackupRead(SafeFileHandle hFile, SafeHGlobalHandle pBuffer, int numberOfBytesToRead, out int numberOfBytesRead,
            [MarshalAs(UnmanagedType.Bool)] bool abort, [MarshalAs(UnmanagedType.Bool)] bool processSecurity, ref IntPtr context);

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BackupSeek(SafeFileHandle hFile, int bytesToSeekLow, int bytesToSeekHigh, out int bytesSeekedLow, out int bytesSeekedHigh,
            ref IntPtr context);

        [DllImport("kernel32.dll", EntryPoint = "MoveFileEx")]
        internal static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, MoveFileFlags dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, out ByHandleFileInformation lpFileInformation);

        [DllImport("kernel32.dll")]
        public static extern Int32 GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

#if !COAPP_ENGINE_CORE
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCtrlHandler(ConsoleHandlerRoutine routine, bool add);
#endif

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(int processId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

#if !COAPP_ENGINE_CORE
        [DllImport("kernel32.dll")]
        public static extern SafeFileHandle GetStdHandle(StandardHandle nStandardHandle);
#endif

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr h);

#if !COAPP_ENGINE_CORE
        [DllImport("kernel32.dll")]
        public static extern Coord GetLargestConsoleWindowSize();

        [DllImport("kernel32.dll")]
        public static extern Coord GetConsoleFontSize(IntPtr hOut, Int32 index);

        [DllImport("kernel32.dll")]
        public static extern bool GetCurrentConsoleFont(IntPtr hOut, bool bMaximumWnd, out ConsoleFontInfo ConsoleCurrentFont);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleActiveScreenBuffer(IntPtr hBuf);

        [DllImport("kernel32.dll")]
        public static extern bool GetConsoleScreenBufferInfo(IntPtr hOut, out ConsoleScreenBufferInfo csbi);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WriteConsoleInput(SafeFileHandle hIn, [MarshalAs(UnmanagedType.LPStruct)] KeyInputRecord r, Int32 count, out Int32 countOut);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool PeekConsoleInput(IntPtr hConsoleInput, [Out] [MarshalAs(UnmanagedType.LPStruct)] FocusInputRecord lpBuffer, Int32 nLength,
            out Int32 lpNumberOfEventsRead);

        [DllImport("kernel32.dll")]
        public static extern bool GetNumberOfConsoleInputEvents(IntPtr hIn, out Int32 num);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateConsoleScreenBuffer(NativeFileAccess dwDesiredAccess, FileShare dwShareMode,
            [MarshalAs(UnmanagedType.LPStruct)] SecurityAttributes lpSecurityAttributes, Int32 dwFlags, IntPtr lpScreenBufferData);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCursorPosition(IntPtr hOut, Coord newPos);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleScreenBufferSize(IntPtr hOut, Coord newSize);

        [DllImport("kernel32.dll")]
        public static extern bool WriteConsole(IntPtr hConsoleOutput, String lpBuffer, Int32 nNumberOfCharsToWrite, out Int32 lpNumberOfCharsWritten,
            IntPtr lpReserved);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateProcessW(IntPtr lpApplicationName, IntPtr lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            Int32 bInheritHandles, Int32 dwCreationFlags, IntPtr lpEnvironment, IntPtr lpCurrentDirectory,
            [MarshalAs(UnmanagedType.LPStruct)] [In] Startupinfo lpStartupInfo, IntPtr lpProcessInformation);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateProcessW(String applicationName, String commandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, Int32 dwCreationFlags, IntPtr lpEnvironment, String lpCurrentDirectory,
            [MarshalAs(UnmanagedType.LPStruct)] [In] Startupinfo lpStartupInfo, [MarshalAs(UnmanagedType.LPStruct)] [In] ProcessInformation lpProcessInformation);

#endif

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateProcessW(String applicationName, String commandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, Int32 dwCreationFlags, IntPtr lpEnvironment, IntPtr lpCurrentDirectory, IntPtr lpStartupInfo, IntPtr lpProcessInformation);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern bool CreateProcessA(String applicationName, String commandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, Int32 dwCreationFlags, IntPtr lpEnvironment, IntPtr lpCurrentDirectory, IntPtr lpStartupInfo, IntPtr lpProcessInformation);

        [DllImport("kernel32.dll")] //, CharSet=CharSet.Unicode
        public static extern IntPtr GetProcAddress(SafeModuleHandle hmod, String name);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern SafeModuleHandle GetModuleHandle(String lpModuleName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, // handle to process
            IntPtr lpBaseAddress, // base	of memory area
            IntPtr lpBuffer, // data buffer
            Int32 nSize, // count of	bytes to write
            out Int32 lpNumberOfBytesWritten // count of bytes	written
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr VirtualAllocEx(SafeProcessHandle processHandle, IntPtr address, SizeT size, AllocationType flAllocationType,
            MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeProcessHandle OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(SafeProcessHandle processHandle, IntPtr lpBaseAddress, byte[] lpBuffer, SizeT nSize,
            ref SizeT lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool FlushInstructionCache(SafeProcessHandle processHandle, IntPtr lpBaseAddress, SizeT dwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeThreadHandle CreateRemoteThread(SafeProcessHandle processHandle, IntPtr lpThreadAttributes, SizeT dwStackSize,
            IntPtr lpStartAddress, IntPtr lpParameter, CreateRemoteThreadFlags creationFlags, out uint lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] SafeProcessHandle processHandle, [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualFreeEx(SafeProcessHandle processHandle, IntPtr address, SizeT size, AllocationType flAllocationType);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeWaitHandle CreateEvent(IntPtr lpSecurityAttributes, bool isManualReset, bool initialState, string name);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeWaitHandle OpenEvent(int desiredAccess, bool inheritHandle, string name);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetEvent(SafeWaitHandle handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ResetEvent(SafeWaitHandle handle);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int WaitForSingleObject(SafeWaitHandle handle, int dwMilliseconds);

#if !COAPP_ENGINE_CORE
        /// <summary>
        ///   Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="lpFileName"> The name of the module. </param>
        /// <param name="hFile"> This parameter is reserved for future use. </param>
        /// <param name="dwFlags"> The action to be taken when loading the module. </param>
        /// <returns> </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        /// <summary>
        ///   Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <param name="hModule"> A handle to the loaded library module. </param>
        /// <returns> If the function succeeds, the return value is nonzero. </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        ///   Enumerates resource types within a binary module.
        /// </summary>
        /// <param name="hModule"> Handle to a module to search. </param>
        /// <param name="lpEnumFunc"> Pointer to the callback function to be called for each enumerated resource type. </param>
        /// <param name="lParam"> Specifies an application-defined value passed to the callback function. </param>
        /// <returns> Returns TRUE if successful; otherwise, FALSE. </returns>
        [DllImport("kernel32.dll", EntryPoint = "EnumResourceTypesW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumResourceTypes(IntPtr hModule, EnumResourceTypesDelegate lpEnumFunc, IntPtr lParam);

        /// <summary>
        ///   An application-defined callback function used with the EnumResourceTypes and EnumResourceTypesEx functions.
        /// </summary>
        /// <param name="hModule"> The handle to the module whose executable file contains the resources for which the types are to be enumerated. </param>
        /// <param name="lpszType"> Pointer to a null-terminated string specifying the type name of the resource for which the type is being enumerated. </param>
        /// <param name="lParam"> Specifies the application-defined parameter passed to the EnumResourceTypes or EnumResourceTypesEx function. </param>
        /// <returns> Returns TRUE if successful; otherwise, FALSE. </returns>
        public delegate bool EnumResourceTypesDelegate(IntPtr hModule, IntPtr lpszType, IntPtr lParam);

        /// <summary>
        ///   Enumerates resources of a specified type within a binary module.
        /// </summary>
        /// <param name="hModule"> Handle to a module to search. </param>
        /// <param name="lpszType"> Pointer to a null-terminated string specifying the type of the resource for which the name is being enumerated. </param>
        /// <param name="lpEnumFunc"> Pointer to the callback function to be called for each enumerated resource name or ID. </param>
        /// <param name="lParam"> Specifies an application-defined value passed to the callback function. </param>
        /// <returns> Returns TRUE if the function succeeds or FALSE if the function does not find a resource of the type specified, or if the function fails for another reason. </returns>
        [DllImport("kernel32.dll", EntryPoint = "EnumResourceNamesW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumResourceNames(IntPtr hModule, IntPtr lpszType, EnumResourceNamesDelegate lpEnumFunc, IntPtr lParam);

        /// <summary>
        ///   An application-defined callback function used with the EnumResourceNames and EnumResourceNamesEx functions.
        /// </summary>
        /// <param name="hModule"> The handle to the module whose executable file contains the resources that are being enumerated. </param>
        /// <param name="lpszType"> Pointer to a null-terminated string specifying the type of resource that is being enumerated. </param>
        /// <param name="lpszName"> Specifies the name of a resource of the type being enumerated. </param>
        /// <param name="lParam"> Specifies the application-defined parameter passed to the EnumResourceNames or EnumResourceNamesEx function. </param>
        /// <returns> Returns TRUE if the function succeeds or FALSE if the function does not find a resource of the type specified, or if the function fails for another reason. </returns>
        public delegate bool EnumResourceNamesDelegate(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam);

        /// <summary>
        ///   Enumerates language-specific resources, of the specified type and name, associated with a binary module.
        /// </summary>
        /// <param name="hModule"> The handle to a module to search. </param>
        /// <param name="lpszType"> Pointer to a null-terminated string specifying the type of resource for which the language is being enumerated. </param>
        /// <param name="lpszName"> Pointer to a null-terminated string specifying the name of the resource for which the language is being enumerated. </param>
        /// <param name="lpEnumFunc"> Pointer to the callback function to be called for each enumerated resource language. </param>
        /// <param name="lParam"> Specifies an application-defined value passed to the callback function. </param>
        /// <returns> Returns TRUE if successful or FALSE otherwise. </returns>
        [DllImport("kernel32.dll", EntryPoint = "EnumResourceLanguagesW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumResourceLanguages(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, EnumResourceLanguagesDelegate lpEnumFunc,
            IntPtr lParam);

        /// <summary>
        ///   An application-defined callback function used with the EnumResourceLanguages and EnumResourceLanguagesEx functions.
        /// </summary>
        /// <param name="hModule"> The handle to the module whose executable file contains the resources for which the languages are being enumerated. </param>
        /// <param name="lpszType"> Pointer to a null-terminated string specifying the type name of the resource for which the language is being enumerated. </param>
        /// <param name="lpszName"> Pointer to a null-terminated string specifying the name of the resource for which the language is being enumerated. </param>
        /// <param name="wIDLanguage"> Specifies the language identifier for the resource for which the language is being enumerated. </param>
        /// <param name="lParam"> Specifies the application-defined parameter passed to the EnumResourceLanguages or EnumResourceLanguagesEx function. </param>
        /// <returns> Returns TRUE if successful or FALSE otherwise. </returns>
        public delegate bool EnumResourceLanguagesDelegate(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, UInt16 wIDLanguage, IntPtr lParam);

        /// <summary>
        ///   Determines the location of the resource with the specified type, name, and language in the specified module.
        /// </summary>
        /// <param name="hModule"> Handle to the module whose executable file contains the resource. </param>
        /// <param name="lpszType"> Pointer to a null-terminated string specifying the type name of the resource. </param>
        /// <param name="lpszName"> Pointer to a null-terminated string specifying the name of the resource. </param>
        /// <param name="wLanguage"> Specifies the language of the resource. </param>
        /// <returns> If the function succeeds, the return value is a handle to the specified resource's information block. </returns>
        [DllImport("kernel32.dll", EntryPoint = "FindResourceExW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr FindResourceEx(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, UInt16 wLanguage);

        /// <summary>
        ///   Locks the specified resource in memory.
        /// </summary>
        /// <param name="hResData"> Handle to the resource to be locked. </param>
        /// <returns> If the loaded resource is locked, the return value is a pointer to the first byte of the resource; otherwise, it is NULL. </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        /// <summary>
        ///   Loads the specified resource into global memory.
        /// </summary>
        /// <param name="hModule"> Handle to the module whose executable file contains the resource. </param>
        /// <param name="hResData"> Handle to the resource to be loaded. </param>
        /// <returns> If the function succeeds, the return value is a handle to the data associated with the resource. </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResData);

        /// <summary>
        ///   Returns the size, in bytes, of the specified resource.
        /// </summary>
        /// <param name="hInstance"> Handle to the module whose executable file contains the resource. </param>
        /// <param name="hResInfo"> Handle to the resource. This handle must be created by using the FindResource or FindResourceEx function. </param>
        /// <returns> If the function succeeds, the return value is the number of bytes in the resource. </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SizeofResource(IntPtr hInstance, IntPtr hResInfo);

        //[DllImport("kernel32.dll", SetLastError = true)]
        //internal static extern bool CloseHandle(IntPtr hHandle);
        /// <summary>
        ///   Closes an open object handle.
        /// </summary>
        /// <param name="hHandle"> A valid handle to an open object. </param>
        /// <returns> If the function succeeds, the return value is nonzero. </returns>
        /// <summary>
        ///   Returns a handle to either a language-neutral portable executable file (LN file) or a language-specific resource file (.mui file) that can be used by the UpdateResource function to add, delete, or replace resources in a binary module.
        /// </summary>
        /// <param name="pFileName"> Pointer to a null-terminated string that specifies the binary file in which to update resources. </param>
        /// <param name="bDeleteExistingResources"> Specifies whether to delete the pFileName parameter's existing resources. </param>
        /// <returns> If the function succeeds, the return value is a handle that can be used by the UpdateResource and EndUpdateResource functions. </returns>
        [DllImport("kernel32.dll", EntryPoint = "BeginUpdateResourceW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr BeginUpdateResource(string pFileName, bool bDeleteExistingResources);

        /// <summary>
        ///   Adds, deletes, or replaces a resource in a portable executable (PE) file. There are some restrictions on resource updates in files that contain Resource Configuration (RC Config) data: language-neutral (LN) files and language-specific resource (.mui) files.
        /// </summary>
        /// <param name="hUpdate"> A module handle returned by the BeginUpdateResource function, referencing the file to be updated. </param>
        /// <param name="lpType"> Pointer to a null-terminated string specifying the resource type to be updated. </param>
        /// <param name="lpName"> Pointer to a null-terminated string specifying the name of the resource to be updated. </param>
        /// <param name="wLanguage"> Specifies the language identifier of the resource to be updated. </param>
        /// <param name="lpData"> Pointer to the resource data to be inserted into the file indicated by hUpdate. </param>
        /// <param name="cbData"> Specifies the size, in bytes, of the resource data at lpData. </param>
        /// <returns> Returns TRUE if successful or FALSE otherwise. </returns>
        [DllImport("kernel32.dll", EntryPoint = "UpdateResourceW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool UpdateResource(IntPtr hUpdate, IntPtr lpType, IntPtr lpName, UInt16 wLanguage, byte[] lpData, UInt32 cbData);

        /// <summary>
        ///   Commits or discards changes made prior to a call to UpdateResource.
        /// </summary>
        /// <param name="hUpdate"> A module handle returned by the BeginUpdateResource function, and used by UpdateResource, referencing the file to be updated. </param>
        /// <param name="fDiscard"> Specifies whether to write the resource updates to the file. If this parameter is TRUE, no changes are made. If it is FALSE, the changes are made: the resource updates will take effect. </param>
        /// <returns> Returns TRUE if the function succeeds; FALSE otherwise. </returns>
        [DllImport("kernel32.dll", EntryPoint = "EndUpdateResourceW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFileMapping(IntPtr hFile, ref SECURITY_ATTRIBUTES lpFileMappingAttributes, PageProtection flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

#endif
    }

    [Flags]
    public enum PageProtection : uint {
        NoAccess = 0x01,
        Readonly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        Guard = 0x100,
        NoCache = 0x200,
        WriteCombine = 0x400,
    }
}
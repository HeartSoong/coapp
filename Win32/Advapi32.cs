﻿//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2011 Garrett Serack . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CoApp.Toolkit.Win32 {
    using System;
    using System.Runtime.InteropServices;

    public static class Advapi32 {
        // Token Specific Access Rights

        public const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        public const UInt32 STANDARD_RIGHTS_READ = 0x00020000;
        public const UInt32 TOKEN_ASSIGN_PRIMARY = 0x0001;
        public const UInt32 TOKEN_DUPLICATE = 0x0002;
        public const UInt32 TOKEN_IMPERSONATE = 0x0004;
        public const UInt32 TOKEN_QUERY = 0x0008;
        public const UInt32 TOKEN_QUERY_SOURCE = 0x0010;
        public const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x0020;
        public const UInt32 TOKEN_ADJUST_GROUPS = 0x0040;
        public const UInt32 TOKEN_ADJUST_DEFAULT = 0x0080;
        public const UInt32 TOKEN_ADJUST_SESSIONID = 0x0100;
        public const UInt32 TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);
        public const UInt32 TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
            TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE |
                TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES |
                    TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT | TOKEN_ADJUST_SESSIONID);


        public const Int32 ERROR_INSUFFICIENT_BUFFER = 122;


        // Integrity Levels

        public const Int32 SECURITY_MANDATORY_UNTRUSTED_RID = 0x00000000;
        public const Int32 SECURITY_MANDATORY_LOW_RID = 0x00001000;
        public const Int32 SECURITY_MANDATORY_MEDIUM_RID = 0x00002000;
        public const Int32 SECURITY_MANDATORY_HIGH_RID = 0x00003000;
        public const Int32 SECURITY_MANDATORY_SYSTEM_RID = 0x00004000;


        /// <summary>
        /// The function opens the access token associated with a process.
        /// </summary>
        /// <param name="hProcess">
        /// A handle to the process whose access token is opened.
        /// </param>
        /// <param name="desiredAccess">
        /// Specifies an access mask that specifies the requested types of 
        /// access to the access token. 
        /// </param>
        /// <param name="hToken">
        /// Outputs a handle that identifies the newly opened access token 
        /// when the function returns.
        /// </param>
        /// <returns></returns>
        [DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr hProcess, UInt32 desiredAccess, out SafeTokenHandle hToken);


        /// <summary>
        /// The function creates a new access token that duplicates one 
        /// already in existence.
        /// </summary>
        /// <param name="ExistingTokenHandle">
        /// A handle to an access token opened with TOKEN_DUPLICATE access.
        /// </param>
        /// <param name="ImpersonationLevel">
        /// Specifies a SECURITY_IMPERSONATION_LEVEL enumerated type that 
        /// supplies the impersonation level of the new token.
        /// </param>
        /// <param name="DuplicateTokenHandle">
        /// Outputs a handle to the duplicate token. 
        /// </param>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken( SafeTokenHandle ExistingTokenHandle,SecurityImpersonationLevel ImpersonationLevel,out SafeTokenHandle DuplicateTokenHandle);


        /// <summary>
        /// The function retrieves a specified type of information about an 
        /// access token. The calling process must have appropriate access 
        /// rights to obtain the information.
        /// </summary>
        /// <param name="hToken">
        /// A handle to an access token from which information is retrieved.
        /// </param>
        /// <param name="tokenInfoClass">
        /// Specifies a value from the TOKEN_INFORMATION_CLASS enumerated 
        /// type to identify the type of information the function retrieves.
        /// </param>
        /// <param name="pTokenInfo">
        /// A pointer to a buffer the function fills with the requested 
        /// information.
        /// </param>
        /// <param name="tokenInfoLength">
        /// Specifies the size, in bytes, of the buffer pointed to by the 
        /// TokenInformation parameter. 
        /// </param>
        /// <param name="returnLength">
        /// A pointer to a variable that receives the number of bytes needed 
        /// for the buffer pointed to by the TokenInformation parameter. 
        /// </param>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetTokenInformation(
            SafeTokenHandle hToken,
            TokenInformationClass tokenInfoClass,
            IntPtr pTokenInfo,
            Int32 tokenInfoLength,
            out Int32 returnLength);


        /// <summary>
        /// Sets the elevation required state for a specified button or 
        /// command link to display an elevated icon. 
        /// </summary>
        public const UInt32 BCM_SETSHIELD = 0x160C;

        /// <summary>
        /// The function returns a pointer to a specified subauthority in a 
        /// security identifier (SID). The subauthority value is a relative 
        /// identifier (RID).
        /// </summary>
        /// <param name="pSid">
        /// A pointer to the SID structure from which a pointer to a 
        /// subauthority is to be returned.
        /// </param>
        /// <param name="nSubAuthority">
        /// Specifies an index value identifying the subauthority array 
        /// element whose address the function will return.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a pointer to the 
        /// specified SID subauthority. To get extended error information, 
        /// call GetLastError. If the function fails, the return value is 
        /// undefined. The function fails if the specified SID structure is 
        /// not valid or if the index value specified by the nSubAuthority 
        /// parameter is out of bounds.
        /// </returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetSidSubAuthority(IntPtr pSid, UInt32 nSubAuthority);
    }
}
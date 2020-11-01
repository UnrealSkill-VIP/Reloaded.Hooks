﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Reloaded.Hooks.Definitions
{
    public interface IFunctionPtr<TDelegate> where TDelegate : Delegate
    {
        /// <summary>
        /// The address of the pointer in memory with which this class was instantiated with.
        /// </summary>
        ulong FunctionPointer { get; }

        /// <summary>
        /// Returns a delegate instance for a function at a specified index of the pointer array.
        /// Only use this if all functions in VTable use same delegate instance.
        /// </summary>
        /// <param name="index">Array index of pointer to function.</param>
        TDelegate this[int index] { get; }

        /// <summary>
        /// Address of the function to which the pointer is currently pointing to.
        /// </summary>
        /// <param name="index">Index of the function.</param>
        IntPtr GetFunctionAddress(int index = 0);

        /// <summary>
        /// Retrieves an delegate instance which can be used to call the function behind the function pointer.
        /// </summary>
        /// <returns>Null if the pointer is zero; else a callable delegate.</returns>
        TDelegate GetDelegate(int index = 0);
    }
}

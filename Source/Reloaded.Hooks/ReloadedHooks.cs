﻿using System;
using System.Runtime.CompilerServices;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X86;
using Reloaded.Hooks.Tools;

namespace Reloaded.Hooks
{
    public class ReloadedHooks : IReloadedHooks
    {
        public static ReloadedHooks Instance { get; } = new ReloadedHooks();

        public IFunction<TFunction> CreateFunction<TFunction>(long address) => new Function<TFunction>(address, this);
        public IHook<TFunction> CreateHook<TFunction>(TFunction function, long functionAddress, int minHookLength = -1) => new Hook<TFunction>(function, functionAddress, minHookLength);
        public unsafe IHook<TFunction> CreateHook<TFunction>(void* targetAddress, long functionAddress, int minHookLength = -1) => new Hook<TFunction>(targetAddress, functionAddress, minHookLength);

        public IntPtr CreateNativeWrapperX86<TFunction>(IntPtr functionAddress, IFunctionAttribute fromFunction) => X86.Wrapper.Create<TFunction>(functionAddress, fromFunction);
        public IntPtr CreateNativeWrapperX64<TFunction>(IntPtr functionAddress, Definitions.X64.IFunctionAttribute fromConvention, Definitions.X64.IFunctionAttribute toConvention) => X64.Wrapper.Create<TFunction>(functionAddress, fromConvention, toConvention);

        public IntPtr CreateWrapper<TFunction>(long functionAddress)
        {
            if (IntPtr.Size == 4)
                return X86.Wrapper.CreatePointer<TFunction>(functionAddress, out _);

            return X64.Wrapper.CreatePointer<TFunction>(functionAddress, out _);
        }

        public TFunction CreateWrapper<TFunction>(long functionAddress, out IntPtr wrapperAddress)
        {
            if (IntPtr.Size == 4)
                return X86.Wrapper.Create<TFunction>(functionAddress, out wrapperAddress);

            return X64.Wrapper.Create<TFunction>(functionAddress, out wrapperAddress);
        }

        public IReverseWrapper<TFunction> CreateReverseWrapper<TFunction>(TFunction function)
        {
            if (IntPtr.Size == 4)
                return new X86.ReverseWrapper<TFunction>(function);

            return new X64.ReverseWrapper<TFunction>(function);
        }

        public IReverseWrapper<TFunction> CreateReverseWrapper<TFunction>(IntPtr function)
        {
            if (IntPtr.Size == 4)
                return new X86.ReverseWrapper<TFunction>(function);

            return new X64.ReverseWrapper<TFunction>(function);
        }

        public IVirtualFunctionTable VirtualFunctionTableFromObject(IntPtr objectAddress, int numberOfMethods) => VirtualFunctionTable.FromObject(objectAddress, numberOfMethods);
        public IVirtualFunctionTable VirtualFunctionTableFromAddress(IntPtr tableAddress, int numberOfMethods) => VirtualFunctionTable.FromAddress(tableAddress, numberOfMethods);
        public IFunctionPtr<TDelegate> CreateFunctionPtr<TDelegate>(ulong functionPointer) where TDelegate : Delegate => new FunctionPtr<TDelegate>(functionPointer);

        public IAsmHook CreateAsmHook(string[] asmCode, long functionAddress, AsmHookBehaviour behaviour = AsmHookBehaviour.ExecuteFirst, int hookLength = -1) => new AsmHook(asmCode, functionAddress, behaviour, hookLength);
        public IAsmHook CreateAsmHook(byte[] asmCode, long functionAddress, AsmHookBehaviour behaviour = AsmHookBehaviour.ExecuteFirst, int hookLength = -1) => new AsmHook(asmCode, functionAddress, behaviour, hookLength);

        #if FEATURE_FUNCTION_POINTERS
        public unsafe IHook<TFunction, TFunctionPointer> CreateHook<TFunction, TFunctionPointer>(void* targetAddress, long functionAddress, int minHookLength = -1) where TFunctionPointer : unmanaged => new Hook<TFunction, TFunctionPointer>(targetAddress, functionAddress, minHookLength);

        public TFunctionPointer CreateWrapperPtr<TFunction, TFunctionPointer>(long functionAddress)
        {
            var ptr = CreateWrapper<TFunction>(functionAddress);
            return Unsafe.As<IntPtr, TFunctionPointer>(ref ptr);
        }
        #endif

        public IReloadedHooksUtilities Utilities { get; } = new ReloadedHooksUtilities();
    }
}

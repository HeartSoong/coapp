﻿namespace CoApp.Toolkit.ImpromptuInterface.Dynamic
{
    using System;
    using System.Dynamic;
    using System.Runtime.Serialization;
    using Optimization;

    /// <summary>
    /// Late bind types from libraries not not at compile type
    /// </summary>
    [Serializable]
    public class ImpromptuLateLibraryType:ImpromptuForwarder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImpromptuLateLibraryType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ImpromptuLateLibraryType(Type type)
            : base(type)
        {

        }  
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ImpromptuLateLibraryType"/> class.
        /// </summary>
        /// <param name="typeName">Qualified Name of the type.</param>
        public ImpromptuLateLibraryType(string typeName)
            : base(Type.GetType(typeName, false))
        {

        }

        /// <summary>
        /// Returns a late bound constructor
        /// </summary>
        /// <value>The late bound constructor</value>
        public dynamic @new
        {
            get { return new ConstuctorFoward((Type)Target); }
        }

        /// <summary>
        /// Forward argument to constructor including named arguments
        /// </summary>
        public class ConstuctorFoward:DynamicObject
        {
            private readonly Type _type;
            internal ConstuctorFoward(Type type)
            {
                _type = type;
            }
            public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
            {
                result = Impromptu.InvokeConstructor(_type, Util.NameArgsIfNecessary(binder.CallInfo, args));
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this Type is available at runtime.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        public bool IsAvailable
        {
            get { return Target != null; }
        }


        protected override object CallTarget
        {
            get
            {
                return InvokeContext.CreateStatic((Type)Target);
            }
        }
    

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the <see cref="ImpromptuForwarder"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public ImpromptuLateLibraryType(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

#endif
    }
}

﻿namespace CoApp.Toolkit.ImpromptuInterface {
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Dynamic;

    public class ActLikeCaster : ImpromptuForwarder {
        private List<Type> _interfaceTypes;

        public override bool TryConvert(ConvertBinder binder, out object result) {
            result = null;

            if (binder.Type.IsInterface) {
                _interfaceTypes.Insert(0, binder.Type);
                result = Impromptu.DynamicActLike(Target, _interfaceTypes.ToArray());
                return true;
            }

            if (binder.Type.IsInstanceOfType(Target)) {
                result = Target;
            }

            return false;
        }

        public ActLikeCaster(object target, IEnumerable<Type> types) : base(target) {
            _interfaceTypes = types.ToList();
        }

#if !SILVERLIGHT

        public ActLikeCaster(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }

#endif
    }
}
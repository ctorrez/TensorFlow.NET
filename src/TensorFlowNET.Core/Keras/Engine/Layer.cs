﻿using System;
using System.Collections.Generic;
using System.Text;
using Tensorflow.Keras.Utils;

namespace Tensorflow.Keras.Engine
{
    /// <summary>
    /// Base layer class.
    /// A layer is a class implementing common neural networks operations, such
    /// as convolution, batch norm, etc. These operations require managing weights,
    /// losses, updates, and inter-layer connectivity.
    /// </summary>
    public class Layer : CheckpointableBase
    {
        /// <summary>
        /// Indicates whether `build` needs to be called upon layer call, to create
        /// the layer's weights.
        /// </summary>
        protected bool built;

        public Tensor __call__(Tensor inputs,
            VariableScope scope = null)
        {
            var input_list = new Tensor[] { inputs };

            // We will attempt to build a TF graph if & only if all inputs are symbolic.
            // This is always the case in graph mode. It can also be the case in eager
            // mode when all inputs can be traced back to `keras.Input()` (when building
            // models using the functional API).
            bool build_graph = tf_utils.are_all_symbolic_tensors(input_list);

            // Handle Keras mask propagation from previous layer to current layer.
            Python.with(new ops.name_scope(_name_scope()), delegate
            {
                if (!built)
                {
                    _maybe_build(inputs);
                }
            });

            throw new NotImplementedException("");
        }

        protected virtual string _name_scope()
        {
            return null;
        }

        protected void _maybe_build(Tensor inputs)
        {
            var input_list = new Tensor[] { inputs };
            build(inputs.getShape());
        }

        protected virtual void build(TensorShape input_shape)
        {

        }
    }
}

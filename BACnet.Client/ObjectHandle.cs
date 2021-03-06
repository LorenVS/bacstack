﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Tagging;
using BACnet.Types;

namespace BACnet.Client
{
    /// <summary>
    /// A handle to a remote object, which can
    /// be used to perform a variety of operations
    /// </summary>
    /// <typeparam name="TObj">The type of the remote object</typeparam>
    public class ObjectHandle<TObj>
    {
        /// <summary>
        /// The client
        /// </summary>
        public Client Client { get; private set; }

        /// <summary>
        /// The device instance
        /// </summary>
        public uint DeviceInstance { get; private set; }

        /// <summary>
        /// The object identifier
        /// </summary>
        public ObjectId ObjectIdentifier { get; private set; }

        /// <summary>
        /// Constructs a new property reader
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="deviceInstance">The device instance</param>
        /// <param name="objectIdentifier">The object identifeir</param>
        public ObjectHandle(Client client, uint deviceInstance, ObjectId objectIdentifier)
        {
            this.Client = client;
            this.DeviceInstance = deviceInstance;
            this.ObjectIdentifier = objectIdentifier;
        }

        /// <summary>
        /// Reads a single property from the object
        /// </summary>
        /// <typeparam name="T">The type of the property to read</typeparam>
        /// <param name="propertyExpr">The property expression</param>
        /// <returns>The property value</returns>
        public async Task<T> ReadPropertyAsync<T>(Expression<Func<TObj, T>> propertyExpr)
        {
            var reference = ObjectHelpers.GetPropertyReference(propertyExpr);
            var request = new ReadPropertyRequest(ObjectIdentifier, reference.PropertyIdentifier, reference.PropertyArrayIndex);
            var ack = await Client.SendRequestAsync<ReadPropertyAck>(
                DeviceInstance,
                request);
            return ack.PropertyValue.As<T>();
        }

        /// <summary>
        /// Reads a single property from the object
        /// </summary>
        /// <typeparam name="T">The type of the property to read</typeparam>
        /// <param name="propertyExpr">The property expression</param>
        /// <returns>The property value</returns>
        public T1 ReadProperty<T1>(
            Expression<Func<TObj, T1>> propertyExpr)
        {
            return ReadPropertyAsync(propertyExpr).Result;
        }


        /// <summary>
        /// Reads a property from the object
        /// </summary>
        /// <typeparam name="T1">The type of property to read</typeparam>
        /// <param name="property1Expr">The property expression</param>
        public async Task<Tuple<T1>> ReadPropertiesAsync<T1>(Expression<Func<TObj, T1>> property1Expr)
        {
            var values = await Client.SendRPMAsync(DeviceInstance, ObjectIdentifier,
                ObjectHelpers.GetPropertyReference(property1Expr));

            return new Tuple<T1>(values[0].As<T1>());
        }

        /// <summary>
        /// Reads a property from the object
        /// </summary>
        /// <typeparam name="T1">The type of property to read</typeparam>
        /// <param name="property1Expr">The property expression</param>
        public Tuple<T1> ReadProperties<T1>(Expression<Func<TObj, T1>> property1Expr)
        {
            return ReadPropertiesAsync(property1Expr).Result;
        }

        /// <summary>
        /// Reads two properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <returns>The two property tuple</returns>
        public async Task<Tuple<T1, T2>> ReadPropertiesAsync<T1, T2>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr)
        {
            var values = await Client.SendRPMAsync(DeviceInstance, ObjectIdentifier,
                ObjectHelpers.GetPropertyReference(property1Expr),
                ObjectHelpers.GetPropertyReference(property2Expr));

            return new Tuple<T1, T2>(
                values[0].As<T1>(),
                values[1].As<T2>());
        }

        /// <summary>
        /// Reads two properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <returns>The two property tuple</returns>
        public Tuple<T1, T2> ReadProperties<T1, T2>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr)
        {
            return ReadPropertiesAsync(property1Expr, property2Expr).Result;
        }


        /// <summary>
        /// Reads three properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <returns>The three property tuple</returns>
        public async Task<Tuple<T1, T2, T3>> ReadPropertiesAsync<T1, T2, T3>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr)
        {
            var values = await Client.SendRPMAsync(DeviceInstance, ObjectIdentifier,
                ObjectHelpers.GetPropertyReference(property1Expr),
                ObjectHelpers.GetPropertyReference(property2Expr),
                ObjectHelpers.GetPropertyReference(property3Expr));

            return new Tuple<T1, T2, T3>(
                values[0].As<T1>(),
                values[1].As<T2>(),
                values[2].As<T3>());
        }

        /// <summary>
        /// Reads three properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <returns>The three property tuple</returns>
        public Tuple<T1, T2, T3> ReadProperties<T1, T2, T3>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr)
        {
            return ReadPropertiesAsync(
                property1Expr,
                property2Expr,
                property3Expr).Result;
        }


        /// <summary>
        /// Reads four properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <typeparam name="T4">The type of the fourth property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <param name="property4Expr">The expression for the fourth property</param>
        /// <returns>The four property tuple</returns>
        public async Task<Tuple<T1, T2, T3, T4>> ReadPropertiesAsync<T1, T2, T3, T4>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr,
            Expression<Func<TObj, T4>> property4Expr)
        {
            var values = await Client.SendRPMAsync(DeviceInstance, ObjectIdentifier,
                ObjectHelpers.GetPropertyReference(property1Expr),
                ObjectHelpers.GetPropertyReference(property2Expr),
                ObjectHelpers.GetPropertyReference(property3Expr),
                ObjectHelpers.GetPropertyReference(property4Expr));

            return new Tuple<T1, T2, T3, T4>(
                values[0].As<T1>(),
                values[1].As<T2>(),
                values[2].As<T3>(),
                values[3].As<T4>());
        }

        /// <summary>
        /// Reads four properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <typeparam name="T4">The type of the fourth property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <param name="property4Expr">The expression for the fourth property</param>
        /// <returns>The four property tuple</returns>
        public Tuple<T1, T2, T3, T4> ReadProperties<T1, T2, T3, T4>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr,
            Expression<Func<TObj, T4>> property4Expr)
        {
            return ReadPropertiesAsync(
                property1Expr,
                property2Expr,
                property3Expr,
                property4Expr).Result;
        }


        /// <summary>
        /// Reads five properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <typeparam name="T4">The type of the fourth property</typeparam>
        /// <typeparam name="T5">The type of the fifth property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <param name="property4Expr">The expression for the fourth property</param>
        /// <param name="property5Expr">The expression for the fifth property</param>
        /// <returns>The five property tuple</returns>
        public async Task<Tuple<T1, T2, T3, T4, T5>> ReadPropertiesAsync<T1, T2, T3, T4, T5>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr,
            Expression<Func<TObj, T4>> property4Expr,
            Expression<Func<TObj, T5>> property5Expr)
        {
            var values = await Client.SendRPMAsync(DeviceInstance, ObjectIdentifier,
                ObjectHelpers.GetPropertyReference(property1Expr),
                ObjectHelpers.GetPropertyReference(property2Expr),
                ObjectHelpers.GetPropertyReference(property3Expr),
                ObjectHelpers.GetPropertyReference(property4Expr),
                ObjectHelpers.GetPropertyReference(property5Expr));

            return new Tuple<T1, T2, T3, T4, T5>(
                values[0].As<T1>(),
                values[1].As<T2>(),
                values[2].As<T3>(),
                values[3].As<T4>(),
                values[4].As<T5>());
        }

        /// <summary>
        /// Reads five properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <typeparam name="T4">The type of the fourth property</typeparam>
        /// <typeparam name="T5">The type of the fifth property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <param name="property4Expr">The expression for the fourth property</param>
        /// <param name="property5Expr">The expression for the fifth property</param>
        /// <returns>The five property tuple</returns>
        public Tuple<T1, T2, T3, T4, T5> ReadProperties<T1, T2, T3, T4, T5>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr,
            Expression<Func<TObj, T4>> property4Expr,
            Expression<Func<TObj, T5>> property5Expr)
        {
            return ReadPropertiesAsync(
                property1Expr,
                property2Expr,
                property3Expr,
                property4Expr,
                property5Expr).Result;
        }

#region ReadPropertiesSafe

        /// <summary>
        /// Reads a property from the object
        /// </summary>
        /// <typeparam name="T1">The type of property to read</typeparam>
        /// <param name="property1Expr">The property expression</param>
        public async Task<Tuple<ErrorOr<T1>>> ReadPropertiesSafeAsync<T1>(Expression<Func<TObj, T1>> property1Expr)
        {
            var results = await Client.SendRPMForReadResultsAsync(DeviceInstance,
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property1Expr));

            return new Tuple<ErrorOr<T1>>(
                Client.FromReadResult<T1>(results[0]));
        }

        /// <summary>
        /// Reads a property from the object
        /// </summary>
        /// <typeparam name="T1">The type of property to read</typeparam>
        /// <param name="property1Expr">The property expression</param>
        public Tuple<ErrorOr<T1>> ReadPropertiesSafe<T1>(Expression<Func<TObj, T1>> property1Expr)
        {
            return ReadPropertiesSafeAsync(property1Expr).Result;
        }

        /// <summary>
        /// Reads two properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <returns>The two property tuple</returns>
        public async Task<Tuple<ErrorOr<T1>, ErrorOr<T2>>> ReadPropertiesSafeAsync<T1, T2>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr)
        {
            var values = await Client.SendRPMForReadResultsAsync(DeviceInstance,
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property1Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property2Expr));

            return new Tuple<ErrorOr<T1>, ErrorOr<T2>>(
                Client.FromReadResult<T1>(values[0]),
                Client.FromReadResult<T2>(values[1]));
        }

        /// <summary>
        /// Reads two properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <returns>The two property tuple</returns>
        public Tuple<ErrorOr<T1>, ErrorOr<T2>> ReadPropertiesSafe<T1, T2>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr)
        {
            return ReadPropertiesSafeAsync(property1Expr, property2Expr).Result;
        }


        /// <summary>
        /// Reads three properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <returns>The three property tuple</returns>
        public async Task<Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>>> ReadPropertiesSafeAsync<T1, T2, T3>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr)
        {
            var values = await Client.SendRPMForReadResultsAsync(DeviceInstance,
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property1Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property2Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property3Expr));

            return new Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>>(
                Client.FromReadResult<T1>(values[0]),
                Client.FromReadResult<T2>(values[1]),
                Client.FromReadResult<T3>(values[2]));
        }

        /// <summary>
        /// Reads three properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <returns>The three property tuple</returns>
        public Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>> ReadPropertiesSafe<T1, T2, T3>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr)
        {
            return ReadPropertiesSafeAsync(property1Expr, property2Expr, property3Expr).Result;
        }


        /// <summary>
        /// Reads four properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <typeparam name="T4">The type of the fourth property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <param name="property4Expr">The expression for the fourth property</param>
        /// <returns>The four property tuple</returns>
        public async Task<Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>, ErrorOr<T4>>> ReadPropertiesSafeAsync<T1, T2, T3, T4>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr,
            Expression<Func<TObj, T4>> property4Expr)
        {
            var values = await Client.SendRPMForReadResultsAsync(DeviceInstance,
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property1Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property2Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property3Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property4Expr));

            return new Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>, ErrorOr<T4>>(
                Client.FromReadResult<T1>(values[0]),
                Client.FromReadResult<T2>(values[1]),
                Client.FromReadResult<T3>(values[2]),
                Client.FromReadResult<T4>(values[3]));
        }

        /// <summary>
        /// Reads four properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <typeparam name="T4">The type of the fourth property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <param name="property4Expr">The expression for the fourth property</param>
        /// <returns>The four property tuple</returns>
        public Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>, ErrorOr<T4>> ReadPropertiesSafe<T1, T2, T3, T4>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr,
            Expression<Func<TObj, T4>> property4Expr)
        {
            return ReadPropertiesSafeAsync(property1Expr, property2Expr, property3Expr, property4Expr).Result;
        }


        /// <summary>
        /// Reads five properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <typeparam name="T4">The type of the fourth property</typeparam>
        /// <typeparam name="T5">The type of the fifth property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <param name="property4Expr">The expression for the fourth property</param>
        /// <param name="property5Expr">The expression for the fifth property</param>
        /// <returns>The five property tuple</returns>
        public async Task<Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>, ErrorOr<T4>, ErrorOr<T5>>> ReadPropertiesSafeAsync<T1, T2, T3, T4, T5>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr,
            Expression<Func<TObj, T4>> property4Expr,
            Expression<Func<TObj, T5>> property5Expr)
        {
            var values = await Client.SendRPMForReadResultsAsync(DeviceInstance,
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property1Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property2Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property3Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property4Expr),
                ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, property5Expr));

            return new Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>, ErrorOr<T4>, ErrorOr<T5>>(
                Client.FromReadResult<T1>(values[0]),
                Client.FromReadResult<T2>(values[1]),
                Client.FromReadResult<T3>(values[2]),
                Client.FromReadResult<T4>(values[3]),
                Client.FromReadResult<T5>(values[4]));
        }

        /// <summary>
        /// Reads five properties from the object
        /// </summary>
        /// <typeparam name="T1">The type of the first property</typeparam>
        /// <typeparam name="T2">The type of the second property</typeparam>
        /// <typeparam name="T3">The type of the third property</typeparam>
        /// <typeparam name="T4">The type of the fourth property</typeparam>
        /// <typeparam name="T5">The type of the fifth property</typeparam>
        /// <param name="property1Expr">The expression for the first property</param>
        /// <param name="property2Expr">The expression for the second property</param>
        /// <param name="property3Expr">The expression for the third property</param>
        /// <param name="property4Expr">The expression for the fourth property</param>
        /// <param name="property5Expr">The expression for the fifth property</param>
        /// <returns>The five property tuple</returns>
        public Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>, ErrorOr<T4>, ErrorOr<T5>> ReadPropertiesSafe<T1, T2, T3, T4, T5>(
            Expression<Func<TObj, T1>> property1Expr,
            Expression<Func<TObj, T2>> property2Expr,
            Expression<Func<TObj, T3>> property3Expr,
            Expression<Func<TObj, T4>> property4Expr,
            Expression<Func<TObj, T5>> property5Expr)
        {
            return ReadPropertiesSafeAsync(
                property1Expr,
                property2Expr,
                property3Expr,
                property4Expr,
                property5Expr).Result;
        }

#endregion

        /// <summary>
        /// Reads an entire range of an array from the object
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="range">The range to read, or None for the entire array</param>
        /// <returns>The resulting array range</returns>
        public async Task<ReadOnlyArray<T>> ReadRangeAsync<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            Option<ReadRangeRequest.RangeType> range = default(Option<ReadRangeRequest.RangeType>))
        {
            var reference = ObjectHelpers.GetPropertyReference(propertyExpr);

            var request = new ReadRangeRequest(
                ObjectIdentifier,
                reference.PropertyIdentifier,
                reference.PropertyArrayIndex,
                range
            );

            var ack = await Client.SendRequestAsync<ReadRangeAck<T>>(DeviceInstance, request);
            return ack.ItemData;
        }

        /// <summary>
        /// Reads an entire range of an array from the object
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="range">The range to read, or None for the entire array</param>
        /// <returns>The resulting array range</returns>
        public ReadOnlyArray<T> ReadRange<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            Option<ReadRangeRequest.RangeType> range = default(Option<ReadRangeRequest.RangeType>))
        {
            return ReadRangeAsync(propertyExpr, range).Result;
        }

        /// <summary>
        /// Reads a range of an array from the object by the array index
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="startIndex">The index to start reading</param>
        /// <param name="count">The number of entries to read</param>
        /// <returns>The resulting array range</returns>
        public Task<ReadOnlyArray<T>> ReadRangeByPositionAsync<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            uint startIndex,
            int count)
        {
            var range = ReadRangeRequest.RangeType.NewByPosition(startIndex, count);
            return ReadRangeAsync(propertyExpr, range);
        }

        /// <summary>
        /// Reads a range of an array from the object by the array index
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="startIndex">The index to start reading</param>
        /// <param name="count">The number of entries to read</param>
        /// <returns>The resulting array range</returns>
        public ReadOnlyArray<T> ReadRangeByPosition<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            uint startIndex,
            int count)
        {
            return ReadRangeByPositionAsync(propertyExpr, startIndex, count).Result;
        }

        /// <summary>
        /// Reads a range of an array from the object by sequence number
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="referenceIndex">The sequence number to start reading at</param>
        /// <param name="count">The number of entries to read</param>
        /// <returns>The resulting array range</returns>
        public Task<ReadOnlyArray<T>> ReadRangeBySequenceNumberAsync<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            uint referenceIndex,
            int count)
        {
            var range = ReadRangeRequest.RangeType.NewBySequenceNumber(referenceIndex, count);
            return ReadRangeAsync(propertyExpr, range);
        }

        /// <summary>
        /// Reads a range of an array from the object by sequence number
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="referenceIndex">The sequence number to start reading at</param>
        /// <param name="count">The number of entries to read</param>
        /// <returns>The resulting array range</returns>
        public ReadOnlyArray<T> ReadRangeBySequenceNumber<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            uint referenceIndex,
            int count)
        {
            return ReadRangeBySequenceNumberAsync(propertyExpr, referenceIndex, count).Result;
        }

        /// <summary>
        /// Reads a range of an array from the object by date time
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="start">The datetime to start reading at</param>
        /// <param name="count">The number of entries to read</param>
        /// <returns>The resulting array range</returns>
        public Task<ReadOnlyArray<T>> ReadRangeByTimeAsync<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            DateAndTime start,
            int count)
        {
            var range = ReadRangeRequest.RangeType.NewByTime(start, count);
            return ReadRangeAsync(propertyExpr, range);
        }

        /// <summary>
        /// Reads a range of an array from the object by date time
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="start">The datetime to start reading at</param>
        /// <param name="count">The number of entries to read</param>
        /// <returns>The resulting array range</returns>
        public ReadOnlyArray<T> ReadRangeByTime<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            DateAndTime start,
            int count)
        {
            return ReadRangeByTimeAsync(propertyExpr, start, count).Result;
        }

        /// <summary>
        /// Reads a range of an array from the object by date time
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="start">The datetime to start reading at</param>
        /// <param name="count">The number of entries to read</param>
        /// <returns>The resulting array range</returns>
        public Task<ReadOnlyArray<T>> ReadRangeByTimeAsync<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            DateTime start,
            int count)
        {
            return ReadRangeByTimeAsync(propertyExpr, DateAndTime.FromDateTime(start), count);
        }

        /// <summary>
        /// Reads a range of an array from the object by date time
        /// </summary>
        /// <typeparam name="T">The element type of the array</typeparam>
        /// <param name="propertyExpr">The expression of the array</param>
        /// <param name="start">The datetime to start reading at</param>
        /// <param name="count">The number of entries to read</param>
        /// <returns>The resulting array range</returns>
        public ReadOnlyArray<T> ReadRangeByTime<T>(
            Expression<Func<TObj, ReadOnlyArray<T>>> propertyExpr,
            DateTime start,
            int count)
        {
            return ReadRangeByTimeAsync(propertyExpr, start, count).Result;
        }
        
        /// <summary>
        /// Writes a property on the object
        /// </summary>
        /// <typeparam name="T">The type of property to write</typeparam>
        /// <param name="propertyExpr">The property expression</param>
        /// <param name="propertyValue">The property value</param>
        public Task WritePropertyAsync<T>(Expression<Func<TObj, T>> propertyExpr, T propertyValue)
        {
            var reference = ObjectHelpers.GetPropertyReference(propertyExpr);

            var request = new WritePropertyRequest(
                ObjectIdentifier,
                reference.PropertyIdentifier,
                reference.PropertyArrayIndex,
                TaggedGenericValue.Encode(propertyValue)
            );

            return Client.SendRequestAsync(DeviceInstance, request);
        }

        /// <summary>
        /// Writes a property on the object
        /// </summary>
        /// <typeparam name="T">The type of property to write</typeparam>
        /// <param name="propertyExpr">The property expression</param>
        /// <param name="propertyValue">The property value</param>
        public void WriteProperty<T>(Expression<Func<TObj, T>> propertyExpr, T propertyValue)
        {
            WritePropertyAsync(propertyExpr, propertyValue).Wait();
        }

    }
}

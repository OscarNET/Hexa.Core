﻿//----------------------------------------------------------------------------------------------
// <copyright file="IEagerLoadRequest.cs" company="HexaSystems Inc">
// Copyright (c) HexaSystems Inc. Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
//-----------------------------------------------------------------------------------------------
namespace Hexa.Core.Domain
{
    using System.Linq;

    public interface IEagerLoadRequest<TQueried, TFetch> : IOrderedQueryable<TQueried>
    {
    }
}
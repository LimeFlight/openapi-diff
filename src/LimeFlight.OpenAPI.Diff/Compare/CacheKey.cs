﻿using System;
using LimeFlight.OpenAPI.Diff.BusinessObjects;

namespace LimeFlight.OpenAPI.Diff.Compare
{
    public class CacheKey : IEquatable<CacheKey>
    {
        private readonly DiffContextBO context;
        private readonly string left;
        private readonly string right;

        public CacheKey(string left, string right, DiffContextBO context)
        {
            this.left = left;
            this.right = right;
            this.context = context;
        }

        public bool Equals(CacheKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return left == other.left && right == other.right && Equals(context, other.context);
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            if (obj == null || GetType() != obj.GetType()) return false;

            var cacheKey = (CacheKey) obj;

            return Equals(cacheKey);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = left != null ? left.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (right != null ? right.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (context != null ? context.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}